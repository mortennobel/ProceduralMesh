using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor (typeof (Sierpinsky))] 
public class SierpinskyEditor : Editor {
	[MenuItem ("GameObject/Create Other/Sierpinsky")]
	static void Create(){
		GameObject gameObject = new GameObject("Sierpinsky");
		Sierpinsky s = gameObject.AddComponent<Sierpinsky>();
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		meshFilter.mesh = new Mesh();
		s.Rebuild();
	}
	
	public override void OnInspectorGUI ()
	{
		Sierpinsky obj;

		obj = target as Sierpinsky;

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
	
