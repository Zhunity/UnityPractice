using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Unity.GPUAnimation
{
	public static class CharacterUtility
	{
		/// <summary>
		/// Blob		
		/// n. 	(尤指液体的) 一点，一滴; (颜色的) 一小片，斑点;
		/// vt.弄脏; 弄错
		/// https://gametorrahod.com/everything-about-isharedcomponentdata/
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static BlobAssetReference<BakedAnimationClipSet> CreateClipSet(KeyframeTextureBaker.BakedData data)
		{
			using (var builder = new BlobBuilder(Allocator.Temp))
			{
				ref var root = ref builder.ConstructRoot<BakedAnimationClipSet>();
				var clips = builder.Allocate(data.Animations.Count, ref root.Clips);
				for (int i = 0; i != data.Animations.Count; i++)
					clips[i] = new BakedAnimationClip(data.AnimationTextures, data.Animations[i]);

				return builder.CreateBlobAssetReference<BakedAnimationClipSet>(Allocator.Persistent);
			}
		}

		/// <summary>
		/// 把角色转换成可以使用GPU渲染的关键
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="entity"></param>
		/// <param name="characterRig"></param>
		/// <param name="clips"></param>
		/// <param name="framerate"></param>
		public static void AddCharacterComponents(EntityManager manager, Entity entity, GameObject characterRig, AnimationClip[] clips, float framerate)
		{
			// SkinnedMeshRenderer : the skinned mesh filter
			// SkinnedMeshRenderer.sharedMesh : the mesh used for skinning
			var renderer = characterRig.GetComponentInChildren<SkinnedMeshRenderer>();

			//Debug.Log(renderer.gameObject + "   " + renderer.sharedMesh);    "minion_skeleton"
			var lod = new LodData
			{
				Lod1Mesh = renderer.sharedMesh,
				Lod2Mesh = renderer.sharedMesh,
				Lod3Mesh = renderer.sharedMesh,
				Lod1Distance = 0,
				Lod2Distance = 100,
				Lod3Distance = 10000,
			};

			// validation 生效
			//@TODO: Perform validation that the shader supports GPU Skinning mode
			var bakedData = KeyframeTextureBaker.BakeClips(characterRig, clips, framerate, lod);

			// 利用manager在entity中依次添加animation state, texturecoordinate, rendercharacter
			var animState = default(GPUAnimationState);
			animState.AnimationClipSet = CreateClipSet(bakedData);
			manager.AddComponentData(entity, animState);
			manager.AddComponentData(entity, default(AnimationTextureCoordinate));

			var renderCharacter = new RenderCharacter
			{
				Material = renderer.sharedMaterial,
				AnimationTexture = bakedData.AnimationTextures,
				Mesh = bakedData.NewMesh,
				ReceiveShadows = renderer.receiveShadows,
				CastShadows = renderer.shadowCastingMode
				
			};
			manager.AddSharedComponentData(entity, renderCharacter);
		}
	}
    public class ConvertToGPUCharacter : MonoBehaviour, IConvertGameObjectToEntity
    {
		public AnimationClip[] Clips;
		public float Framerate = 60.0F;
		
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            CharacterUtility.AddCharacterComponents(dstManager, entity, gameObject, Clips, Framerate);
        }
    }
}