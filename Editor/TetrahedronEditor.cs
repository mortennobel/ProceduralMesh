using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (Tetrahedron))] 
public class TetrahedronEditor : Editor {
	[MenuItem ("GameObject/Create Other/Tetrahedron")]
	static void Create(){
		GameObject gameObject = new GameObject("Tetrahedron");
		Tetrahedron s = gameObject.AddComponent<Tetrahedron>();
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		meshFilter.mesh = new Mesh();
		s.Rebuild();
	}
	
	public override void OnInspectorGUI ()
	{
		Tetrahedron obj;

		obj = target as Tetrahedron;

		if (obj == null)
		{
			return;
		}
	
		base.DrawDefaultInspector();
		EditorGUILayout.BeginHorizontal ();
		
		// Rebuild mesh when user click the Rebuild button
		if (GUILayout.Button("Rebuild")){
			obj.Rebuild();
		}
		EditorGUILayout.EndHorizontal ();
	}
}
