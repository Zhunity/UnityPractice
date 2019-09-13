using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// this example creates a quad mesh from scratch, creates bones
/// and assigns them, and animates the bones motion to make the
/// quad animate based on a simple animation curve.
/// Mesh.bindposes 和 Mesh.boneWeights 用的同一个例子的代码
/// 
/// 整理现有情报：
/// Mesh.boneWeights ：
/// The size of the array is either vertexCount or zero. 
/// each vertex can be affected by up to 4 different bones. 
/// The bone weights should be in descending order(most significant first) and add up  to 1.
/// 长度和顶点数一致。 每个顶点最多可以被4条不同骨骼影响，影响的权重以降序排序，所有权重加起来等于1
/// 
/// Mesh.bindposes ： 
/// The bind poses. The bind pose at each index refers to the bone with the same index.
/// The bind pose is the inverse of the transformation matrix of the bone, when the bone is in the bind pose.
/// 还不是很懂，绑定的姿势（？）。各个序号的绑定姿势涉及到相同编号的骨骼（？）
/// 绑定姿势跟在在绑定姿势的骨骼的矩阵信息相反（？）
/// bindPoses数量和骨骼数量一致吗？
/// --------------------------------------------------------------------------------------------------------------------------------
/// 疑问点：
/// 1、骨骼是怎么定义的，定义transform就可以了吗？
/// 按照视频内容，关节和骨骼都是同一个东西。
/// 
/// 2、boneIndex怎么和真正的骨骼映射在一起？
/// 应该是mesh.bindposes数组（或者_renderer.bones数组）的顺序
/// 
/// 3、bonePoses是什么？
/// https://gameinstitute.qq.com/course/detail/10116
/// 绑定姿势：骨骼的初始姿势，也被称为T-POSE
/// 
/// 4、bonePoses和BoneWeight的关系是什么？
/// 目测通过顶点通过boneweight的index，读取到bonePoses的骨骼，然后通过权重，设置每个顶点的位置
/// </summary>
public class BindPoseExample : MonoBehaviour
{
	private SkinnedMeshRenderer _renderer;
	private Animation _animaton;

	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uv;
	public Material material;
	public BoneWeight[] weights;

	[Serializable]
	public struct Bone
	{
		public string name;
		public Vector3 localPosition;
	}

	public Bone[] BoneList;

	private void Start()
	{
		InitComponent();
		InitRenderer();
		PlayAnimation();
		//_Start();
	}

	private void InitComponent()
	{
		_renderer = gameObject.AddComponent<SkinnedMeshRenderer>();
		_animaton = gameObject.AddComponent<Animation>();
	}

	private void InitRenderer()
	{
		// Assign bones and bind poses
		_renderer.sharedMesh = GetMesh();
		_renderer.material = material;
		_renderer.bones = GetBones();
	}

	private Mesh GetMesh()
	{
		// Build basic mesh
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.RecalculateNormals();
		// assign bone weights to mesh
		mesh.boneWeights = weights;
		mesh.bindposes = GetBindPos();
		return mesh;
	}

	/// <summary>
	/// Create Bone Transforms and Bind poses
	/// One bone at the bottom and one at the top
	/// </summary>
	/// <returns></returns>
	private Matrix4x4[] GetBindPos()
	{
		Transform[] bones = GetBones();
		int length = bones.Length;
		Matrix4x4[] bindPoses = new Matrix4x4[length];
		// The bind pose is bone's inverse transformation matrix
		// In this case the matrix we also make this matrix relative to the root
		// So that we can move the root game object around freely
		for(int i = 0; i < length; i ++)
		{
			bindPoses[i] = bones[i].worldToLocalMatrix * transform.localToWorldMatrix;
		}
		return bindPoses;
	}

	private Transform[] bones;
	private Transform[] GetBones()
	{
		if(bones == null)
		{
			int length = BoneList.Length;
			bones = new Transform[length];
			for (int i = 0; i < length; i ++ )
			{
				bones[i] = NewBone(BoneList[i].name, BoneList[i].localPosition);
			}
		}
		return bones;
	}

	private Transform NewBone(string name, Vector3 localPosition)
	{
		Transform bone = new GameObject(name).transform;
		bone.parent = transform;
		// Set the position relative to the parent
		bone.localRotation = Quaternion.identity;
		bone.localPosition = localPosition;
		return bone;
	}

	[Serializable]
	public struct KeyFrameSample
	{
		public Vector4[] keyFrame; // 0 : time	1 : value	2 : intTangent	3 : outTangent

	}

	private void PlayAnimation()
	{
		// Assign a simple waving animation to the bottom bone
		AnimationCurve curve = new AnimationCurve();
		curve.keys = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 5, 0, 0), new Keyframe(2, 0.0F, 0, 0) };

		// Create the clip with the curve
		AnimationClip clip = new AnimationClip();
		clip.SetCurve("Lower", typeof(Transform), "m_LocalPosition.z", curve);
		clip.legacy = true;

		// Add and play the clip
		clip.wrapMode = WrapMode.Loop;
		_animaton.AddClip(clip, "test");
		_animaton.Play("test");
	}

	void _Start()
	{
		var rend = gameObject.AddComponent<SkinnedMeshRenderer>();
		var anim = gameObject.AddComponent<Animation>();
		// Build basic mesh
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[] { new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(-1, 5, 0), new Vector3(1, 5, 0) };
		mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
		mesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
		mesh.RecalculateNormals();
		rend.material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Mesh/Mesh.mat");
		// assign bone weights to mesh
		BoneWeight[] weights = new BoneWeight[4];
		weights[0].boneIndex0 = 0;
		weights[0].weight0 = 1;
		weights[1].boneIndex0 = 0;
		weights[1].weight0 = 1;
		weights[2].boneIndex0 = 1;
		weights[2].weight0 = 1;
		weights[3].boneIndex0 = 1;
		weights[3].weight0 = 1;
		mesh.boneWeights = weights;

		//// Create Bone Transforms and Bind poses
		//// One bone at the bottom and one at the top

		Transform[] bones = new Transform[2];
		Matrix4x4[] bindPoses = new Matrix4x4[2];
		bones[0] = new GameObject("Lower").transform;
		bones[0].parent = transform;
		// Set the position relative to the parent
		bones[0].localRotation = Quaternion.identity;
		bones[0].localPosition = Vector3.zero;
		// The bind pose is bone's inverse transformation matrix
		// In this case the matrix we also make this matrix relative to the root
		// So that we can move the root game object around freely
		bindPoses[0] = bones[0].worldToLocalMatrix * transform.localToWorldMatrix;

		bones[1] = new GameObject("Upper").transform;
		bones[1].parent = transform;
		// Set the position relative to the parent
		bones[1].localRotation = Quaternion.identity;
		bones[1].localPosition = new Vector3(0, 15, 0);
		// The bind pose is bone's inverse transformation matrix
		// In this case the matrix we also make this matrix relative to the root
		// So that we can move the root game object around freely
		bindPoses[1] = bones[1].worldToLocalMatrix * transform.localToWorldMatrix;

		// bindPoses was created earlier and was updated with the required matrix.
		// The bindPoses array will now be assigned to the bindposes in the Mesh.
		mesh.bindposes = bindPoses;

		// Assign bones and bind poses
		rend.bones = bones;
		rend.sharedMesh = mesh;

		// Assign a simple waving animation to the bottom bone
		AnimationCurve curve = new AnimationCurve();
		curve.keys = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 3, 0, 0), new Keyframe(2, 0.0F, 0, 0) };

		// Create the clip with the curve
		AnimationClip clip = new AnimationClip();
		clip.SetCurve("Lower", typeof(Transform), "m_LocalPosition.z", curve);
		clip.legacy = true;

		// Add and play the clip
		clip.wrapMode = WrapMode.Loop;
		anim.AddClip(clip, "test");
		anim.Play("test");
	}
}