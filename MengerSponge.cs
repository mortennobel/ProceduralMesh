using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class MengerSponge : MonoBehaviour {
	
	public int depth = 3;
	// these are only used during contruction of the mesh
	private List<Vector3> vertices = new List<Vector3>();
	private List<int> indices = new List<int>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void addTriangle(Vector3 p1, Vector3 p2, Vector3 p3){
		vertices.Add(p1);
		vertices.Add(p2);
		vertices.Add(p3);
		indices.Add(indices.Count);
		indices.Add(indices.Count);
		indices.Add(indices.Count);
	}
	
	private void addQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4){
		// add first triangle
		int p1Index = vertices.Count;
		indices.Add(vertices.Count);
		vertices.Add(p1);
		
		indices.Add(vertices.Count);
		vertices.Add(p2);
		int p3Index = vertices.Count;
		indices.Add(vertices.Count);
		vertices.Add(p3);
		
		// Add second triangle
		indices.Add(p1Index); // reuse vertex from triangle above
		indices.Add(p3Index); // reuse vertex from triangle above
		indices.Add(vertices.Count);
		vertices.Add(p4);
	}
	
	public void Rebuild(){
		Vector3 p1 = new Vector3(0,0,0);
		Vector3 p2 = new Vector3(1,1,1);
		generateMengerSponge(p1, p2, depth);
			
		// todo - this implementation creates a lot of hidden faces that could easily be removed
		// todo another optimization is to share vertices betwen different quads.
		
	}
	
	private void addCube(Vector3 pStart, Vector3 pEnd){
		float length = pEnd.x-pStart.x;
		Vector3 p1 = pStart;
		Vector3 p2 = pStart+Vector3.right*length;
		Vector3 p3 = pStart+Vector3.forward*length+Vector3.right*length;
		Vector3 p4 = pStart+Vector3.forward*length;
		
		Vector3 p5 = pEnd-Vector3.forward*length-Vector3.right*length;
		Vector3 p6 = pEnd-Vector3.forward*length;
		Vector3 p7 = pEnd;
		Vector3 p8 = pEnd-Vector3.right*length;
		
		addQuad(p1,p4,p3,p2);
		addQuad(p7,p6,p2,p3);
		addQuad(p7,p3,p4,p8);
		addQuad(p1,p5,p8,p4);
		addQuad(p1,p2,p6,p5);
		addQuad(p7,p8,p5,p6);
		return;
	}
	
	private void generateMengerSponge(Vector3 pStart, Vector3 pEnd, int depth){
		if (depth == 0){
			addCube(pStart, pEnd);
			return;
		}
		depth --;
		float length = pEnd.x-pStart.x;
		
		Vector3 endOffset = Vector3.one*length/3.0f;
		
		for (int x=0;x<3;x++){
			for (int y=0;y<3;y++){
				for (int z=0;z<3;z++){
					if ((x==1&&y==1) || (x==1 && z==1) || (y==1 && z==1)){
						continue;
					}
					Vector3 newStart = pStart+Vector3.right*length*(x/3.0f)+Vector3.forward*length*(z/3.0f)+Vector3.up*length*(y/3.0f);
					generateMengerSponge(newStart, newStart+endOffset, depth);
				}
			}
		}
		
	}
}
