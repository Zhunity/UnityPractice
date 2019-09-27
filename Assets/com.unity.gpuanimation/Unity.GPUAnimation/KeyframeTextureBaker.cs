using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = System.Object;


namespace Unity.GPUAnimation
{
	/// <summary>
	/// 存储转换成Texture2D数据的动画片段
	/// 每个动画片段会在定点处进行三次采样
	/// file:///C:/Program%20Files/Unity/Hub/Editor/2019.2.2f1/Editor/Data/Documentation/en/ScriptReference/Texture2D.html
	/// </summary>
	public struct AnimationTextures : IEquatable<AnimationTextures>
	{
		public Texture2D Animation0;
		public Texture2D Animation1;
		public Texture2D Animation2;

		public bool Equals(AnimationTextures other)
		{
			return Animation0 == other.Animation0 && Animation1 == other.Animation1 && Animation2 == other.Animation2;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (ReferenceEquals(Animation0, null) ? 0 : Animation0.GetHashCode());
				hashCode = (hashCode * 397) ^ (ReferenceEquals(Animation1, null) ? 0 : Animation1.GetHashCode());
				hashCode = (hashCode * 397) ^ (ReferenceEquals(Animation2, null) ? 0 : Animation2.GetHashCode());
				return hashCode;
			}
		}
	}

	public static class KeyframeTextureBaker
	{
		

		/// <summary>
		/// 存储转换成Texture2D变量后的动画片段数据和Mesh、LOD、帧率等
		/// </summary>
		public class BakedData
		{
			public AnimationTextures AnimationTextures;
			public Mesh NewMesh;
			public LodData lods;
			public float Framerate;

			public List<AnimationClipData> Animations = new List<AnimationClipData>();

			public Dictionary<string, AnimationClipData> AnimationsDictionary = new Dictionary<string, AnimationClipData>();
		}

		/// <summary>
		/// 存储原始的动画片段和该动画片段在Texture2D中对应起始和终止像素
		/// </summary>
		public class AnimationClipData
		{
			public AnimationClip Clip;
			public int PixelStart;
			public int PixelEnd;
		}

		/// <summary>
		/// IMPORTENT！！！
		/// </summary>
		/// <param name="animationRoot">动画根对象</param>
		/// <param name="animationClips">动画片段数组</param>
		/// <param name="framerate">帧率</param>
		/// <param name="lods">LOD数据</param>
		/// <returns></returns>
		public static BakedData BakeClips(GameObject animationRoot, AnimationClip[] animationClips, float framerate, LodData lods)
		{
			//首先获取动画根对象子对象的SkinMeshRenderer
			var skinRenderers = animationRoot.GetComponentsInChildren<SkinnedMeshRenderer>();
			if (skinRenderers.Length != 1)
				throw new System.ArgumentException("There must be exactly one SkinnedMeshRenderer");

			// @TODO: warning about more than one materials

			// messing 使不整洁的   arbitrary 任意的
			// Before messing about with some arbitrary game object hierarchy.
			// Instantiate the character, but make sure it's inactive so it doesn't trigger any unexpected systems. 
			var wasActive = animationRoot.activeSelf;
			animationRoot.SetActive(false);
			// TODO 感觉这里应该是不需要再实例化的，程序加载模型的时候已经完成了
			var instance = GameObject.Instantiate(animationRoot, Vector3.zero, Quaternion.identity);
			animationRoot.SetActive(wasActive);
			
			instance.transform.localScale = Vector3.one;
			var skinRenderer = instance.GetComponentInChildren<SkinnedMeshRenderer>();

			BakedData bakedData = new BakedData();
			bakedData.NewMesh = CreateMesh(skinRenderer);               // 利用这个skinRenderer作为CreateMesh方法的参数，生成BakedData的NewMesh
			// BakedData的LodData结构体中的mesh成员也使用CreateMesh生成，只不过需要的第二个参数是输入lod的mesh成员
			var lod1Mesh = CreateMesh(skinRenderer, lods.Lod1Mesh);	
			var lod2Mesh = CreateMesh(skinRenderer, lods.Lod2Mesh);
			var lod3Mesh = CreateMesh(skinRenderer, lods.Lod3Mesh);
			bakedData.lods = new LodData(lod1Mesh, lod2Mesh, lod3Mesh, lods.Lod1Distance, lods.Lod2Distance, lods.Lod3Distance);

			bakedData.Framerate = framerate;    // BakedData的Framerate直接使用输入的framerate

			var sampledBoneMatrices = new List<Matrix4x4[,]>();

			int numberOfKeyFrames = 0;

			// 获取所有动画的数据
			for (int i = 0; i < animationClips.Length; i++)
			{
				// 使用SampleAnimationClip方法对每个动画片段采样得到sapledMatrix，然后添加到list中
				var sampledMatrix = SampleAnimationClip(instance, animationClips[i], skinRenderer, bakedData.Framerate);
				sampledBoneMatrices.Add(sampledMatrix);

				// 使用sampledBoneMatrices的维数参数作为关键帧和骨骼的数目统计
				// 所有动画的关键帧数量
				numberOfKeyFrames += sampledMatrix.GetLength(0);
			}

			// 返回列数，即骨头数量
			int numberOfBones = sampledBoneMatrices[0].GetLength(1);

			// QUESTION：为什么都弄三个一样的？
			// 使用骨骼数和关键帧数作为大小创建材质
			// width = 所有动画的关键帧数量, height = 骨头数量
			var tex0 = bakedData.AnimationTextures.Animation0 = new Texture2D(numberOfKeyFrames, numberOfBones, TextureFormat.RGBAFloat, false);

			// https://docs.unity3d.com/ScriptReference/TextureWrapMode.html
			// Corresponds to the settings in a texture inspector. Wrap mode determines how texture is sampled when texture coordinates are outside of the typical 0..1 range. For
			// example, Repeat makes the texture tile, whereas Clamp makes the texture edge pixels be stretched when outside of 0..1 range.
			// Repeat:	Tiles the texture, creating a repeating pattern
			// Clamp:	Clamps the texture to the last pixel at the edge
			// Mirror:	Tiles the texture, creating a repeating pattern by mirroring it at every integer boundary
			// MorrorOnce: Mirrors the texture once, then clamps to edge pixels.
			// https://docs.unity3d.com/ScriptReference/Texture-wrapMode.html
			// Using wrapMode sets the same wrapping mode on all axes. Different per-axis wrap modes can be set using wrapModeU, wrapModeV, wrapModeW. Querying the value
			// returns the U axis wrap mode(same as warpModeU getter)

			// Corresponds :	v. 	相一致; 符合; 类似于; 相当于; 通信
			// wrap:			v. 	包，裹(礼物等); 用…包裹(或包扎、覆盖等); 用…缠绕(或围紧)
			// typical			adj. 	典型的; 有代表性的; 一贯的; 平常的; 不出所料; 特有的
			// tile:			v. 	铺瓦; 铺地砖; 贴瓷砖; 平铺显示，并列显示，瓦片式显示(视窗)		个人感觉就是平铺了吧
			// whereas:			conj. 	(用以比较或对比两个事实) 然而，但是，尽管; (用于正式文件中句子的开头) 鉴于
			// Clamp:			n. 	夹具; 夹子; 夹钳; 车轮夹锁(用于锁住违章停放的车辆);
			// stretch			v. 	拉长; 拽宽; 撑大; 抻松; 有弹性(或弹力); 拉紧; 拉直; 绷紧
			// pattern			n. 	模式; 方式; 范例; 典范; 榜样; 样板; 图案; 花样; 式样
			// integer			n. 	整数
			// boundary			n. 	边界; 界限; 分界线; 使球越过边界线的击球(得加分);
			// axis				n.轴
			tex0.wrapMode = TextureWrapMode.Clamp;
			tex0.filterMode = FilterMode.Point;
			tex0.anisoLevel = 0;

			var tex1 = bakedData.AnimationTextures.Animation1 = new Texture2D(numberOfKeyFrames, numberOfBones, TextureFormat.RGBAFloat, false);
			tex1.wrapMode = TextureWrapMode.Clamp;
			tex1.filterMode = FilterMode.Point;
			tex1.anisoLevel = 0;

			var tex2 = bakedData.AnimationTextures.Animation2 = new Texture2D(numberOfKeyFrames, numberOfBones, TextureFormat.RGBAFloat, false);
			tex2.wrapMode = TextureWrapMode.Clamp;
			tex2.filterMode = FilterMode.Point;
			tex2.anisoLevel = 0;

			Color[] texture0Color = new Color[tex0.width * tex0.height];
			Color[] texture1Color = new Color[tex0.width * tex0.height];
			Color[] texture2Color = new Color[tex0.width * tex0.height];

			int runningTotalNumberOfKeyframes = 0;
			for (int i = 0; i < sampledBoneMatrices.Count; i++)
			{
				for (int boneIndex = 0; boneIndex < sampledBoneMatrices[i].GetLength(1); boneIndex++)
				{
					for (int keyframeIndex = 0; keyframeIndex < sampledBoneMatrices[i].GetLength(0); keyframeIndex++)
					{
						int index = Get1DCoord(runningTotalNumberOfKeyframes + keyframeIndex, boneIndex, tex0.width);

						// 将sampledBoneMatrices的数据全部存入到材质颜色中
						texture0Color[index] = sampledBoneMatrices[i][keyframeIndex, boneIndex].GetRow(0);
						texture1Color[index] = sampledBoneMatrices[i][keyframeIndex, boneIndex].GetRow(1);
						texture2Color[index] = sampledBoneMatrices[i][keyframeIndex, boneIndex].GetRow(2);
					}
				}

				AnimationClipData clipData = new AnimationClipData
				{
					Clip = animationClips[i],
					// 生成AnimationClipData需要的开始结束位置
					PixelStart = runningTotalNumberOfKeyframes + 1,
					PixelEnd = runningTotalNumberOfKeyframes + sampledBoneMatrices[i].GetLength(0) - 1
				};

				if (clipData.Clip.wrapMode == WrapMode.Default) clipData.PixelEnd -= 1;

				bakedData.Animations.Add(clipData);

				runningTotalNumberOfKeyframes += sampledBoneMatrices[i].GetLength(0);
			}

			tex0.SetPixels(texture0Color);
			tex1.SetPixels(texture1Color);
			tex2.SetPixels(texture2Color);

			runningTotalNumberOfKeyframes = 0;
			for (int i = 0; i < sampledBoneMatrices.Count; i++)
			{
				for (int boneIndex = 0; boneIndex < sampledBoneMatrices[i].GetLength(1); boneIndex++)
				{
					for (int keyframeIndex = 0; keyframeIndex < sampledBoneMatrices[i].GetLength(0); keyframeIndex++)
					{
						//int d1_index = Get1DCoord(runningTotalNumberOfKeyframes + keyframeIndex, boneIndex, bakedData.Texture0.width);

						Color pixel0 = tex0.GetPixel(runningTotalNumberOfKeyframes + keyframeIndex, boneIndex);
						Color pixel1 = tex1.GetPixel(runningTotalNumberOfKeyframes + keyframeIndex, boneIndex);
						Color pixel2 = tex2.GetPixel(runningTotalNumberOfKeyframes + keyframeIndex, boneIndex);

						if ((Vector4)pixel0 != sampledBoneMatrices[i][keyframeIndex, boneIndex].GetRow(0))
						{
							Debug.LogError("Error at (" + (runningTotalNumberOfKeyframes + keyframeIndex) + ", " + boneIndex + ") expected " + Format(sampledBoneMatrices[i][keyframeIndex, boneIndex].GetRow(0)) + " but got " + Format(pixel0));
						}
						if ((Vector4)pixel1 != sampledBoneMatrices[i][keyframeIndex, boneIndex].GetRow(1))
						{
							Debug.LogError("Error at (" + (runningTotalNumberOfKeyframes + keyframeIndex) + ", " + boneIndex + ") expected " + Format(sampledBoneMatrices[i][keyframeIndex, boneIndex].GetRow(1)) + " but got " + Format(pixel1));
						}
						if ((Vector4)pixel2 != sampledBoneMatrices[i][keyframeIndex, boneIndex].GetRow(2))
						{
							Debug.LogError("Error at (" + (runningTotalNumberOfKeyframes + keyframeIndex) + ", " + boneIndex + ") expected " +   Format(sampledBoneMatrices[i][keyframeIndex, boneIndex].GetRow(2)) + " but got " + Format(pixel2));
						}
					}
				}
				runningTotalNumberOfKeyframes += sampledBoneMatrices[i].GetLength(0);
			}

			tex0.Apply(false, true);
			tex1.Apply(false, true);
			tex2.Apply(false, true);
			
			// 创建Dictionary字段
			bakedData.AnimationsDictionary = new Dictionary<string, AnimationClipData>();
			foreach (var clipData in bakedData.Animations)
			{
				bakedData.AnimationsDictionary[clipData.Clip.name] = clipData;
			}
			
			GameObject.DestroyImmediate(instance);

			return bakedData;
		}

		public static string Format(Vector4 v)
		{
			return "(" + v.x + ", " + v.y + ", " + v.z + ", " + v.w + ")";
		}

		public static string Format(Color v)
		{
			return "(" + v.r + ", " + v.g + ", " + v.b + ", " + v.a + ")";
		}

		/// <summary>
		/// 从已有的SkinnedMeshRenderer和一个Mesh创建一个新的Mesh。
		/// </summary>
		/// <param name="originalRenderer"></param>
		/// <param name="mesh">如果为空，则生成新的newMesh是原来Renderer的sharedMesh的复制</param>
		/// <returns></returns>
		private static Mesh CreateMesh(SkinnedMeshRenderer originalRenderer, Mesh mesh = null)
		{
			// Mesh:
			// A class that allows creating or modifying meshes from script.
			// Meshes contain vertices and multiple triangle arrays.(后面一个例子，到时候可以去看看)
			// The triangle arrays are simply indices into the vertex arrays; three indices for each triangle.
			// For every vertex there can be a normal, two texture coordinates, color and tangent. There are optional though and can be removed at will.
			// All vertex information is stored in separate arrays of the same size, sof if your mesh has 10 vertices, you world also have 10-size arrays for normals and other attributes.
			// TODO 粘一个官方文档地址
			Mesh newMesh = new Mesh();
			Mesh originalMesh = mesh == null ? originalRenderer.sharedMesh : mesh;

			originalMesh.CopyMeshData(newMesh);

			Vector3[] vertices = originalMesh.vertices;
			Vector2[] boneIds = new Vector2[originalMesh.vertexCount];			// 骨骼id，骨骼数量=顶点数量？ 猜测是boneweight里面的index？
			Vector2[] boneInfluences = new Vector2[originalMesh.vertexCount];   // influence 影响				猜测是boneWeight里的weight？

			// 旧-》新的定向
			int[] boneRemapping = null;

			// 如果mesh非空，找到Mesh在sharedMesh中对应的bindPoses，把boneIndex0和bineIndex1映射到给定的Mesh上
			if (mesh != null)
			{
				// bindposes :
				// The bind poses. The bind pose at each index refers to the bone with the same index.
				// The bind pose is the inverse of the transformation matrix of the bone, when the bone is in the bind pose.
				// https://docs.unity3d.com/ScriptReference/Mesh-bindposes.html
				var originalBindPoseMatrices = originalRenderer.sharedMesh.bindposes;
				var newBindPoseMatrices = mesh.bindposes;
				
				if (newBindPoseMatrices.Length != originalBindPoseMatrices.Length)
				{
					//Debug.LogError(mesh.name + " - Invalid bind poses, got " + newBindPoseMatrices.Length + ", but expected "
					//				+ originalBindPoseMatrices.Length);
				}
				else
				{
					boneRemapping = new int[originalBindPoseMatrices.Length];
					for (int i = 0; i < boneRemapping.Length; i++)
					{
						// newBindPoseMatrices是LodData里面的mesh
						// 找到originalBindPoseMatrices里面等于newBindPoseMatrices[i]的index，存进boneRemapping[i]
						// originalBindPoseMatrices[boneRemapping[i]] = newBindPoseMatrices[i]
						boneRemapping[i] = Array.FindIndex(originalBindPoseMatrices, x => x == newBindPoseMatrices[i]);
					}
				}
			}


			// Mesh.boneWeights: each vertex can be affected by up to 4 different bones. The bone weights should be in descending order(most significant first) and add up  to 1.
			// BoneWeight: 
			// Skinning bine weights of a vertex in the mesh.
			// Each vertex is skinned with up to fout bones. All weights should sum up to one. Weights and bone indices should be defined in the order of decreasing weight. 
			// if a vertex is affected by less than four bones, the remaining weights should be zeroes.
			// file:///C:/Program%20Files/Unity/Hub/Editor/2019.2.2f1/Editor/Data/Documentation/en/ScriptReference/Mesh-boneWeights.html
			var boneWeights = originalMesh.boneWeights;
			var bones = originalRenderer.bones; // bones 见bindposes
			for (int i = 0; i < originalMesh.vertexCount; i++)
			{
				// QUESTION:只受两根骨骼影响吗?
				int boneIndex0 = boneWeights[i].boneIndex0;
				int boneIndex1 = boneWeights[i].boneIndex1;

				// 如果boneRemapping为空，表示mesh为空，则不需要进行映射
				if (boneRemapping != null)
				{
					// originalBindPoseMatrices[boneRemapping[i]] = newBindPoseMatrices[i]
					boneIndex0 = boneRemapping[boneIndex0];
					boneIndex1 = boneRemapping[boneIndex1];
				}

				#region 这里可能有一种说法，但是不懂，需要学习
				// 通过boneWights的boneIndex0和boneIndex1生成的boneInfluences,作为newMesh的UV2和UV3存储起来
				// QUESTION:+0.5是为了四舍五入吗？但是index为int，也没必要四舍五入
				// QUESTION:求index/length的比值有什么用
				// TODO 要看一下shader才知道了
				boneIds[i] = new Vector2((boneIndex0 + 0.5f) / bones.Length, (boneIndex1 + 0.5f) / bones.Length);

				float mostInfluentialBonesWeight = boneWeights[i].weight0 + boneWeights[i].weight1;

				// 同上疑问
				// 比较两个骨头的权重？
				boneInfluences[i] = new Vector2(boneWeights[i].weight0 / mostInfluentialBonesWeight, boneWeights[i].weight1 / mostInfluentialBonesWeight);
			}
			newMesh.vertices = vertices;
			newMesh.uv2 = boneIds;
			newMesh.uv3 = boneInfluences;
			#endregion
			return newMesh;
		}

		#region 没看懂
		/// <summary>
		/// 动画采样
		/// </summary>
		/// <param name="root">动画对象</param>
		/// <param name="clip">单个动画片段</param>
		/// <param name="renderer">SkinnedMeshRenderer</param>
		/// <param name="framerate">帧率</param>
		/// <returns>动画片段采样后生成的Matrices，一个行数为（帧率*动作时长）+3，列数为骨骼数量的4x4矩阵</returns>
		private static Matrix4x4[,] SampleAnimationClip(GameObject root, AnimationClip clip, SkinnedMeshRenderer renderer, float framerate)
		{
			var bindPoses = renderer.sharedMesh.bindposes;
			var bones = renderer.bones;
			// 创建一个长为（帧率*动作时长）+3，宽为骨骼数量的4x4矩阵
			// 个人猜测，用于记录每一帧的所有骨骼的位置(感觉没错)
			// [行，列]
			// QUESTION : +3 是为什么
			Matrix4x4[,] boneMatrices = new Matrix4x4[Mathf.CeilToInt(framerate * clip.length) + 3, bones.Length];

			// TODO 输出framerate， bones.Length
			//Debug.Log("clip.name:" + clip.name + "  clip.length:" + clip.length +"  boneMatrices.Length:" + boneMatrices.Length + "  boneMatrices.GetLength(0):" + boneMatrices.GetLength(0));
			// clip.name:Walk  clip.length:0.5333334  boneMatrices.Length:595  boneMatrices.GetLength(0):35

			// Array.GetLength(int dimension)获取一个 32 位整数，该整数表示 Array 的指定维中的元素数。
			// boneMatrices.GetLength(0) 获取的是Mathf.CeilToInt(framerate * clip.length) + 3
			// QUESTION : 为什么从1开始
			for (int i = 1; i < boneMatrices.GetLength(0) - 1; i++)
			{
				// 选取当前所在帧的clip数据作为一段时间的采样
				// QUESTION : 既然这里要-3， 为什么一开始要加三？
				float t = (float)(i - 1) / (boneMatrices.GetLength(0) - 3);

				// WrapMode :
				// Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.
				// The WrapMode that the animation system uses for a specific animation is determined this way:
				// You can set the WrapMode of an AnimationClip in the import settings of the clip. THis is the recommended way to control the WrapMode.
				// When an AnimationState is created, it inherits its WrapMode from the AnimationClip it is created from, but you can also change it from code.
				// If the WrapMode of an AnimationState is set to Default, the animation system will use the WrapMode from the Animtaion component.
				// Clamp		: ????? 没有
				// Once			: When thime reaches the end of the animation clip, the clip will automatically stop playing and time will be reset to beginning of the clip.
				// Loop			: When time reaches the end of the animation clip, time will continue at the begging.
				// PingPong		: When time reaches the end of the animation clip, time will ping pong back between beginning and end
				// Defalut		: Reads the default repeat mode set higher up
				// ClampForever	: Plays back the animation. When it reaches the end, it will keep playing the last fram and never stop playing.
				var oldWrapMode = clip.wrapMode;
				clip.wrapMode = WrapMode.Clamp;

				// Samples an animation at a given time for any animated properties.
				// It is recommended to use the Animation interface instead for performance reasons. This will sample animation at the given time. Any component properties that are
				// animated in the clip will be replaced with the sampled value. Most of the time you want to use Animation.Play instead. SampleAnimation is useful when you need to jump between frames in an unordered way 
				// or based on some special input.
				// https://docs.unity3d.com/ScriptReference/AnimationClip.SampleAnimation.html
				// unordered 无序的
				// QUESTION 为什么循环里面要从0-1之间取样
				clip.SampleAnimation(root, t * clip.length);
				clip.wrapMode = oldWrapMode;
				
				for (int j = 0; j < bones.Length; j++)
					// 从模型坐标转换成世界坐标
					boneMatrices[i, j] = bones[j].localToWorldMatrix * bindPoses[j];
			}

			for (int j = 0; j < bones.Length; j++)
			{
				// 感觉是骷髅死亡动作开始时状态不对的直接原因
				// QUESTION:但是为什么要这么乱赋值？
				boneMatrices[0, j] = boneMatrices[boneMatrices.GetLength(0) - 2, j];    // 把倒数第二帧赋值给第一帧
				boneMatrices[boneMatrices.GetLength(0) - 1, j] = boneMatrices[1, j];	// 把第二帧赋值给最后一帧
			}

			return boneMatrices;
		}
		#endregion

		#region Util methods

		public static void CopyMeshData(this Mesh originalMesh, Mesh newMesh)
		{
			var vertices = originalMesh.vertices;

			newMesh.vertices = vertices;
			newMesh.triangles = originalMesh.triangles;
			newMesh.normals = originalMesh.normals;
			newMesh.uv = originalMesh.uv;
			newMesh.tangents = originalMesh.tangents;
			newMesh.name = originalMesh.name;
		}

		private static int Get1DCoord(int x, int y, int width)
		{
			return y * width + x;
		}

		#endregion
}
}