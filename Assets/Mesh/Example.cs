using UnityEngine;

public class Example : MonoBehaviour
{
	Vector3[] newVertices;
	Vector2[] newUV;
	int[] newTriangles;

	void Start()
	{
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = newVertices;
		mesh.uv = newUV;
		mesh.triangles = newTriangles;
	}
}