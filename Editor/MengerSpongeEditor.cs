using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (MengerSponge))] 
public class MengerSpongeEditor : Editor {
[MenuItem ("GameObject/Create Other/MengerSponge")]
	static void Create(){
		GameObject gameObject = new GameObject("MengerSponge");
		MengerSponge s = gameObject.AddComponent<MengerSponge>();
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		meshFilter.mesh = new Mesh();
		s.Rebuild();
	}
	
	public override void OnInspectorGUI ()
	{
		MengerSponge obj;

		obj = target as MengerSponge;

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
