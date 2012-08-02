using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

/**
 * Simple example of creating a procedural 6 sided cube
 */
[RequireComponent (typeof (MeshFilter))] 
[RequireComponent (typeof (MeshRenderer))]
public class SimpleProceduralCube : MonoBehaviour {
	void Start () {
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		Mesh mesh = new Mesh ();
		meshFilter.mesh = mesh;
		
		mesh.vertices = new Vector3[]{
			// face 1 (xy plane, z=0)
			new Vector3(0,0,0), 
			new Vector3(1,0,0), 
			new Vector3(1,1,0), 
			new Vector3(0,1,0), 
			// face 2 (zy plane, x=1)
			new Vector3(1,0,0), 
			new Vector3(1,0,1), 
			new Vector3(1,1,1), 
			new Vector3(1,1,0), 
			// face 3 (xy plane, z=1)
			new Vector3(1,0,1), 
			new Vector3(0,0,1), 
			new Vector3(0,1,1), 
			new Vector3(1,1,1), 
			// face 4 (zy plane, x=0)
			new Vector3(0,0,1), 
			new Vector3(0,0,0), 
			new Vector3(0,1,0), 
			new Vector3(0,1,1), 
			// face 5  (zx plane, y=1)
			new Vector3(0,1,0), 
			new Vector3(1,1,0), 
			new Vector3(1,1,1), 
			new Vector3(0,1,1), 
			// face 6 (zx plane, y=0)
			new Vector3(0,0,0), 
			new Vector3(0,0,1), 
			new Vector3(1,0,1), 
			new Vector3(1,0,0), 
		};
		
		int faces = 6; // here a face = 2 triangles
		
		List<int> triangles = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		
		for (int i = 0; i < faces; i++) {
			int triangleOffset = i*4;
			triangles.Add(0+triangleOffset);
			triangles.Add(2+triangleOffset);
			triangles.Add(1+triangleOffset);
			
			triangles.Add(0+triangleOffset);
			triangles.Add(3+triangleOffset);
			triangles.Add(2+triangleOffset);
			
			// same uvs for all faces
			uvs.Add(new Vector2(0,0));
			uvs.Add(new Vector2(1,0));
			uvs.Add(new Vector2(1,1));
			uvs.Add(new Vector2(0,1));
		}
		
		mesh.triangles = triangles.ToArray();

		mesh.uv = uvs.ToArray();

		renderer.material = new Material(Shader.Find("Diffuse"));
		
		mesh.RecalculateNormals(); 
		mesh.RecalculateBounds (); 
		mesh.Optimize();
	} 
}