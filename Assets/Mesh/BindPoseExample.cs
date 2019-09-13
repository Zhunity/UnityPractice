using UnityEngine;
using System.Collections;

// this example creates a quad mesh from scratch, creates bones
// and assigns them, and animates the bones motion to make the
// quad animate based on a simple animation curve.
public class BindPoseExample : MonoBehaviour
{
	private SkinnedMeshRenderer _renderer;
	private Animation _animaton;

	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uv;
	public Material material;
	public BoneWeight[] weights;

	private void Start()
	{
		InitComponent();
		InitRenderer();
		//_Start();
	}

	private void InitComponent()
	{
		_renderer = gameObject.AddComponent<SkinnedMeshRenderer>();
		_animaton = gameObject.AddComponent<Animation>();
	}

	private void InitRenderer()
	{
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
		Matrix4x4[] bindPoses = new Matrix4x4[2];

		

		// The bind pose is bone's inverse transformation matrix
		// In this case the matrix we also make this matrix relative to the root
		// So that we can move the root game object around freely
		bindPoses[0] = bones[0].worldToLocalMatrix * transform.localToWorldMatrix;
		bindPoses[1] = bones[1].worldToLocalMatrix * transform.localToWorldMatrix;
		return bindPoses;
	}

	private Transform[] bones;
	private Transform[] GetBones()
	{
		if(bones == null)
		{
			bones = new Transform[2];
			bones[0] = NewBone("Low", Vector3.zero);
			bones[1] = NewBone("Upper", new Vector3(0, 5, 0));
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