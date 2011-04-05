using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (TetrahedronUV))] 
public class TetrahedronEditorUV : Editor {
	[MenuItem ("GameObject/Create Other/Tetrahedron UV")]
	static void Create(){
		GameObject gameObject = new GameObject("TetrahedronUV");
		TetrahedronUV s = gameObject.AddComponent<TetrahedronUV>();
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		meshFilter.mesh = new Mesh();
		s.Rebuild();
	}
	
	public override void OnInspectorGUI ()
	{
		TetrahedronUV obj;

		obj = target as TetrahedronUV;

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
