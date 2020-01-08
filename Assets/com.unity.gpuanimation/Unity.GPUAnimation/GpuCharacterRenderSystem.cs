using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace Unity.GPUAnimation
{
	/// <summary>
	/// 存储动画片段的持续时间和编号
	/// 存储KeyframeTextureBaker.BakeClips返回的数据
	/// </summary>
	public struct GPUAnimationState : IComponentData
	{
		public float Time;
		public int   AnimationClipIndex;
		
		public BlobAssetReference<BakedAnimationClipSet> AnimationClipSet;
	}
	
	struct AnimationTextureCoordinate : IComponentData
	{
		public float3 Coordinate;
	}
	
	
	public struct BakedAnimationClipSet
	{
		public BlobArray<BakedAnimationClip> Clips;
	}

	/// <summary>
	/// 存储Animation Clip数据在材质中的具体信息
	/// </summary>
	public struct BakedAnimationClip
	{
		/// <summary>
		/// start = (float)clipData.PixelStart / animTextures.Animation0.width
		/// </summary>
		internal float TextureOffset;

		/// <summary>
		/// end - start
		/// </summary>
		internal float TextureRange;

		/// <summary>
		/// animTextures.Animation0.width
		/// Width of the texture in pixels. (Read Only)
		/// 相对应的有个height(Height of the texture in pixels. (Read Only))
		/// 和OneOverPixelOffset一样
		/// </summary>
		internal float TextureWidth;

		/// <summary>
		/// 1.0F / TextureWidth
		/// 和OnePixelOffset一样
		/// </summary>
		internal float OneOverTextureWidth;

		/// <summary>
		/// clipData.Clip.length
		/// </summary>
		public float AnimationLength;

		/// <summary>
		/// clipData.Clip.wrapMode == WrapMode.Loop
		/// </summary>
		public bool  Looping;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="animTextures">width=所有动作帧数的总和 height=骨骼数</param>
		/// <param name="clipData"></param>
		public BakedAnimationClip(AnimationTextures animTextures, KeyframeTextureBaker.AnimationClipData clipData)
		{
			// QUESTION 为什么要倒数？ 
			// GUESS	怕溢出？
			// QUESTION 还有为什么除以width，不是width*height，也不是所有像素，感觉就是为了防溢出
			// 因为width=所有动作帧数量的总和啊，求这个占比才是有道理的
			float start = (float)clipData.PixelStart / animTextures.Animation0.width;		// 开始像素在所有像素中的占比，即开始帧在所有帧中的百分比位置
			float end = (float)clipData.PixelEnd / animTextures.Animation0.width;           // 结束像素在所有像素中的占比，即结束帧在所有帧中的百分比位置

			TextureOffset = start;
			TextureRange = end - start;
			TextureWidth = animTextures.Animation0.width;
			OneOverTextureWidth = 1.0F / TextureWidth;
			AnimationLength = clipData.Clip.length;
			Looping = clipData.Clip.wrapMode == WrapMode.Loop;
			#region data
			//Debug.LogFormat("name:{0}\nTextureOffset:{1}\nTextureRange:{2}\nOnePixelOffset:{3}\nOneOverTextureWidth:{4}\nOneOverPixelOffset:{5}\nAnimationLength:{6}\nTextureWidth:{7}", 
			//		clipData.Clip.name, TextureOffset, TextureRange,		OnePixelOffset,		OneOverTextureWidth,	 OneOverPixelOffset,	AnimationLength,	 TextureWidth);
			//name: Walk
			// TextureOffset:0.02857143
			//TextureRange: 0.9428571
			//OnePixelOffset: 0.02857143
			//OneOverTextureWidth: 0.02857143
			//OneOverPixelOffset:35
			//AnimationLength:0.5333334
			//TextureWidth: 35

			// TextureOffset（因为这是第一个动作，碰巧一样而已） == OnePixelOffset == OneOverTextureWidth
			/// QUESTION：OneOverTextureWidth和OnePixelOffset难道不是同一个东西吗？
			/// TextureWidth = animTextures.Animation0.width;
			/// OneOverTextureWidth = 1.0F / TextureWidth;
			/// float onePixel = 1f / animTextures.Animation0.width;
			/// OnePixelOffset = onePixel;
			/// 
			/// QUESTION：TextureWidth和OneOverPixelOffset有什么区别？
			/// TextureWidth = animTextures.Animation0.width;
			/// float onePixel = 1f / animTextures.Animation0.width;
			///  OnePixelOffset = onePixel;
			///  OneOverPixelOffset = 1.0F / OnePixelOffset;
			///  ANSWER:没区别
			///  已删除
			///  
			#endregion
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="normalizedTime"></param>
		/// <returns>float3(当前时间向下取证的一帧， 向下取整一帧+1， 当前时间和第一的差)</returns>
		public float3 ComputeCoordinate(float normalizedTime)
		{
			float texturePosition = normalizedTime * TextureRange + TextureOffset;
			// QUESTION 这不是向下取整嘛？不就等于0了吗
			// ASWNER 看清楚一点，是乘以所有帧的数量
			float lowerPixelFloor = math.floor(texturePosition * TextureWidth);
			
			float lowerPixelCenter = lowerPixelFloor * OneOverTextureWidth;
			float upperPixelCenter = lowerPixelCenter + OneOverTextureWidth;
			
			float lerpFactor = (texturePosition - lowerPixelCenter) * TextureWidth;
			// Debug.Log(lowerPixelCenter + "   " + upperPixelCenter + "   " + lerpFactor); 0.4857143   0.5142857   0.3135899 这时间挺短的
			return new float3(lowerPixelCenter, upperPixelCenter, lerpFactor);
		}
		
		/// <summary>
		/// 计算当前时间/动画时间，获得一个比0大，比1小的百分比
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public float ComputeNormalizedTime(float time)
		{
			if (Looping)
				return Mathf.Repeat(time, AnimationLength) / AnimationLength;
			else
				// Returns the result of clamping the value x into the interval [a, b], where x, a and b are float values.
				// public static float saturate(float x) { return clamp(x, 0.0f, 1.0f); }

				/// <summary>Returns the result of clamping the value x into the interval [a, b], where x, a and b are float values.</summary>
				/// public static float clamp(float x, float a, float b) { return max(a, min(b, x)); }
				return math.saturate(time / AnimationLength);
		}

	}

	/// <summary>
	/// 准备好Material，Animation Texture、Mesh之后就可以准备绘制
	/// </summary>
	[System.Serializable]
	struct RenderCharacter : ISharedComponentData, IEquatable<RenderCharacter>
	{
		//@TODO: Would be nice if we had BlobAssetReference in shared component data support (Serialize not supported...) 
		public Material                                  Material;
		public AnimationTextures                         AnimationTexture;
		public Mesh                                      Mesh;
		public bool                                      ReceiveShadows;
		public ShadowCastingMode                         CastShadows;
		
		public bool Equals(RenderCharacter other)
		{
			return Material == other.Material && AnimationTexture.Equals(other.AnimationTexture) && Mesh == other.Mesh && ReceiveShadows == other.ReceiveShadows && CastShadows == other.CastShadows;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (ReferenceEquals(Material, null) ? 0 : Material.GetHashCode());
				hashCode = (hashCode * 397) ^ AnimationTexture.GetHashCode();
				hashCode = (hashCode * 397) ^ (ReferenceEquals(Mesh, null) ? 0 : Mesh.GetHashCode());
				return hashCode;
			}
		}
	}

	unsafe public static class NativeExtensionTemp
	{
		/// <summary>
		/// 扩展函数
		///  Reinterpret：重新解释
		///  NativeArray<T>转换成 NativeArray<U>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static NativeArray<U> Reinterpret_Temp<T, U>(this NativeArray<T> array) where U : struct where T : struct
        {
            var tSize = UnsafeUtility.SizeOf<T>();
            var uSize = UnsafeUtility.SizeOf<U>();

             var byteLen = ((long) array.Length) * tSize;
            var uLen = byteLen / uSize;

 #if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (uLen * uSize != byteLen)
            {
                throw new InvalidOperationException($"Types {typeof(T)} (array length {array.Length}) and {typeof(U)} cannot be aliased due to size constraints. The size of the types and lengths involved must line up.");
            }

 #endif
            var ptr = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(array);
            var result = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<U>(ptr, (int) uLen, Allocator.Invalid);

 #if ENABLE_UNITY_COLLECTIONS_CHECKS
            var handle = NativeArrayUnsafeUtility.GetAtomicSafetyHandle(array);
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref result, handle);
#endif

             return result;
        }

	}

	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public class CalculateTextureCoordinateSystem : JobComponentSystem
	{
		[BurstCompile]
		struct CalculateTextureCoordJob : IJobForEach<GPUAnimationState, AnimationTextureCoordinate>
		{
			public void Execute([ReadOnly]ref GPUAnimationState animstate, ref AnimationTextureCoordinate textureCoordinate)
			{
				ref var clips = ref animstate.AnimationClipSet.Value.Clips;
				if ((uint) animstate.AnimationClipIndex < (uint) clips.Length)
				{
					var normalizedTime = clips[animstate.AnimationClipIndex].ComputeNormalizedTime(animstate.Time);
					textureCoordinate.Coordinate = clips[animstate.AnimationClipIndex].ComputeCoordinate(normalizedTime);
				}
				else
				{
					// How to warn???
				}
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			return new CalculateTextureCoordJob().Schedule(this, inputDeps);
		}
	}

	[UpdateInGroup(typeof(PresentationSystemGroup))]
	[UpdateAfter(typeof(CalculateTextureCoordinateSystem))]
	public class GpuCharacterRenderSystem : JobComponentSystem
    {
		// 创建绘制的角色列表
	    private List<RenderCharacter> _Characters = new List<RenderCharacter>();


		/// <summary>
		/// 这个只有Add，没有Remove
		/// </summary>
	    private Dictionary<RenderCharacter, InstancedSkinningDrawer> _Drawers = new Dictionary<RenderCharacter, InstancedSkinningDrawer>();

	    private EntityQuery m_Characters;


	    protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
	        _Characters.Clear();
	        EntityManager.GetAllUniqueSharedComponentData(_Characters);

	        foreach (var character in _Characters)
	        {
		        if (character.Material == null || character.Mesh == null)
			        continue;
		        
		        //@TODO: Currently we never cleanup the _Drawers cache when the last entity with that renderer disappears.
		        InstancedSkinningDrawer drawer;
		        if (!_Drawers.TryGetValue(character, out drawer))
		        {
					// 对要绘制的角色实例化一个Drawer
			        drawer = new InstancedSkinningDrawer(character.Material, character.Mesh, character.AnimationTexture);
			        _Drawers.Add(character, drawer);
		        }

				/// <summary>
				/// Filters this EntityQuery so that it only selects entities with shared component values
				/// matching the values specified by the `sharedComponent1` parameter.
				/// </summary>
				/// <param name="sharedComponent1">The shared component values on which to filter.</param>
				/// <typeparam name="SharedComponent1">The type of shared component. (The type must also be
				/// one of the types used to create the EntityQuery.</typeparam>
				m_Characters.SetFilter(character);

				Profiler.BeginSample("ExtractState");
				// QUESTION 这两个job是什么job？
				// ANSWER 因为会报错，同时下面一个操作是通过job来实现的
				// TODO 了解ToComponentDataArray
				//InvalidOperationException: The previously scheduled job GatherComponentDataJob`1 writes to the NativeArray GatherComponentDataJob`1.Data.ComponentData.You must call JobHandle.Complete() on the job GatherComponentDataJob`1, before you can read from the NativeArray safely.
				//Unity.Collections.LowLevel.Unsafe.AtomicSafetyHandle.CheckReadAndThrowNoEarlyOut(Unity.Collections.LowLevel.Unsafe.AtomicSafetyHandle handle) < 0x19c096328c0 + 0x00052 > in < c254d6bddee24edb8742cd1c78b0db19 >:0
				//Unity.Collections.LowLevel.Unsafe.AtomicSafetyHandle.CheckReadAndThrow(Unity.Collections.LowLevel.Unsafe.AtomicSafetyHandle handle)(at C:/ buildslave / unity / build / Runtime / Export / Jobs / AtomicSafetyHandle.bindings.cs:148)
				//Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr[T](Unity.Collections.NativeArray`1[T] nativeArray)(at C:/ buildslave / unity / build / Runtime / Export / NativeArray / NativeArray.cs:517)
				//UnityEngine.ComputeBuffer.SetData[T](Unity.Collections.NativeArray`1[T] data, System.Int32 nativeBufferStartIndex, System.Int32 computeBufferStartIndex, System.Int32 count)(at C:/ buildslave / unity / build / Runtime / Export / Shaders / ComputeShader.bindings.cs:202)
				//Unity.GPUAnimation.InstancedSkinningDrawer.Draw(Unity.Collections.NativeArray`1[T] TextureCoordinates, Unity.Collections.NativeArray`1[T] ObjectToWorld, UnityEngine.Rendering.ShadowCastingMode shadowCastingMode, System.Boolean receiveShadows)(at Assets / com.unity.gpuanimation / Unity.GPUAnimation / InstancedSkinningDrawer.cs:97)
				//Unity.GPUAnimation.GpuCharacterRenderSystem.OnUpdate(Unity.Jobs.JobHandle inputDeps)(at Assets / com.unity.gpuanimation / Unity.GPUAnimation / GpuCharacterRenderSystem.cs:318)
				//Unity.Entities.JobComponentSystem.InternalUpdate()(at Library / PackageCache / com.unity.entities@0.1.1 - preview / Unity.Entities / ComponentSystem.cs:933)
				//Unity.Entities.ComponentSystemBase.Update()(at Library / PackageCache / com.unity.entities@0.1.1 - preview / Unity.Entities / ComponentSystem.cs:284)
				//Unity.Entities.ComponentSystemGroup.OnUpdate()(at Library / PackageCache / com.unity.entities@0.1.1 - preview / Unity.Entities / ComponentSystemGroup.cs:602)
				//UnityEngine.Debug:LogException(Exception)
				//Unity.Debug:LogException(Exception)(at Library / PackageCache / com.unity.entities@0.1.1 - preview / Unity.Entities / Stubs / Unity / Debug.cs:25)
				//Unity.Entities.ComponentSystemGroup:OnUpdate()(at Library / PackageCache / com.unity.entities@0.1.1 - preview / Unity.Entities / ComponentSystemGroup.cs:606)
				//Unity.Entities.ComponentSystem:InternalUpdate()(at Library / PackageCache / com.unity.entities@0.1.1 - preview / Unity.Entities / ComponentSystem.cs:800)
				//Unity.Entities.ComponentSystemBase:Update()(at Library / PackageCache / com.unity.entities@0.1.1 - preview / Unity.Entities / ComponentSystem.cs:284)
				//Unity.Entities.DummyDelegateWrapper:TriggerUpdate()(at Library / PackageCache / com.unity.entities@0.1.1 - preview / Unity.Entities / ScriptBehaviourUpdateOrder.cs:144)

				JobHandle jobA, jobB;
				// 传输坐标和LocalToWorld矩阵
		        var coords = m_Characters.ToComponentDataArray<AnimationTextureCoordinate>(Allocator.TempJob, out jobA);
		        var localToWorld = m_Characters.ToComponentDataArray<LocalToWorld>(Allocator.TempJob, out jobB);
		        JobHandle.CompleteAll(ref jobA, ref jobB);
		        Profiler.EndSample();
		        
				// 调用Draw()方法
		        //drawer.Draw(coords.Reinterpret_Temp<AnimationTextureCoordinate, float3>(), localToWorld.Reinterpret_Temp<LocalToWorld, float4x4>(), character.CastShadows, character.ReceiveShadows);
		        
		        coords.Dispose();
		        localToWorld.Dispose();
	        }

	        return inputDeps;
        }

        protected override void OnCreate()
        {
	        m_Characters = GetEntityQuery(ComponentType.ReadOnly<RenderCharacter>(), ComponentType.ReadOnly<GPUAnimationState>(), ComponentType.ReadOnly<LocalToWorld>(), ComponentType.ReadOnly<AnimationTextureCoordinate>());
        }

        protected override void OnDestroy()
        {
	        foreach(var drawer in _Drawers.Values)
		        drawer.Dispose();
	        _Drawers = null;
        }
    }
}