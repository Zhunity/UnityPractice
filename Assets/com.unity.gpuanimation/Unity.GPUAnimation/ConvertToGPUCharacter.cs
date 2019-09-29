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
		/// source-chamber distance(不像)
		/// chamber				n. 	会议厅; (议会的) 议院; (作特定用途的) 房间，室
		/// tightly				adv. 	紧紧地; 牢固地; 紧密地
		/// restriction			n. 	限制规定; 限制法规; 限制; 约束; 制约因素
		/// portal				n. 	壮观的大门; 豪华的入口; 门户网站; 入口站点
		/// Aliasing			n. 	别名使用；混淆现象
		/// via					prep. 	经由，经过(某一地方); 通过，凭借(某人、系统等
		/// sneak				v. 	偷偷地走; 溜; 偷偷地做; 偷带; 偷拿; 偷走(不重要的或小的东西)
		/// concept				n. 	概念; 观念
		/// optimizable			 可优化的
		/// hell				n. 	地狱; 苦难的经历; 悲惨的境况; (有人认为含冒犯意) 该死，见鬼
		/// kinda				用于笔语中，表示非正式会话中 kind of 的发音;
		/// lawless				adj. 	无法律的; 不遵守法律的; 目无法纪的; 不法的
		/// compatible			adj. 	可共用的; 兼容的; 可共存的; (因志趣等相投而) 关系好的，和睦相处的
		/// interact with		v. 	与…相互作用
		/// grabbed				v. 	抓住; 攫取; (试图) 抓住，夺得; 利用，抓住(机会)
		/// pierce				v. 	扎; 刺破; 穿透; 穿过; 透入; 冲破; 突破
		/// hassle				n. 	困难; 麻烦; 分歧; 争论; 烦恼
		/// streamline			v. 	使成流线型; 使(系统、机构等)效率更高; (尤指) 使增产节约
		/// drill down to		深入到 
		/// recursively			递归地; 递归; 递归的; 递归删除; 回归
		/// removal				n. 	移动; 调动; 去除; 除去; 消除; 清除; 免职; 解职
		/// maintaining			v. 	维持; 保持; 维修; 保养; 坚持(意见); 固执己见
		/// chronological		adj. 	按发生时间顺序排列的; 按时间计算的(年龄)(相对于身体、智力或情感等方面的发展而言); 
		/// adjacent			adj. 	与…毗连的; 邻近的
		/// galore				adj. 	大量; 很多
		/// dare to				v. 	胆敢
		/// segmenting			v. 	分割; 划分
		/// segmenting via		分段通过 
		/// extensively			adv. 	广大地；广泛地
		/// fancy				adj. 	异常复杂的; 太花哨的; 精致的; 有精美装饰的; 绚丽的; 花哨的; 昂贵的; 奢华的
		/// categorizing		v. 	将…分类; 把…加以归类
		/// intending			v.打算; 计划; 想要; 意指
		/// Quoted				v. 	引用; 引述; 举例说明; 开价; 出价; 报价
		/// prior				adj. 	先前的; 较早的; 在前的; 优先的; 占先的; 较重要的; 在前面的
		/// http://gametorrahod.com/designing-an-efficient-system-with-version-numbers/
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