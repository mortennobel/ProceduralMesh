using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class Sierpinsky : MonoBehaviour {
	
	// these are only used during contruction of the mesh
	private List<Vector3> vertices = new List<Vector3>();
	private List<int> indices = new List<int>();
	
	public enum FractalType{
		Sierpinsky_Triangle,
		Sierpinsky_Pyramid,
		Sierpinsky_Carpet
	};
	
	public int depth = 3;
	public FractalType type = FractalType.Sierpinsky_Triangle;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Rebuild(){
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter==null){
			Debug.LogError("MeshFilter not found!");
			return;
		}
		
		
		switch (type){
		case FractalType.Sierpinsky_Triangle:
		{
			Vector3 p1 = new Vector3(0,0,0);
			Vector3 p2 = new Vector3(1,0,0);
			Vector3 p3 = new Vector3(0.5f,Mathf.Sqrt(0.75f),0);
			generateSierpinskyTriangle(p1, p2, p3, depth);
		}
			break;
		case FractalType.Sierpinsky_Pyramid:
		{
			Vector3 p1 = new Vector3(0,0,0);
			Vector3 p2 = new Vector3(1,0,0);
			Vector3 p3 = new Vector3(0.5f,0,Mathf.Sqrt(0.75f));
			Vector3 p4 = new Vector3(0.5f,Mathf.Sqrt(0.75f),Mathf.Sqrt(0.75f)/3);
			
			generateSierpinskyPyramid(p1, p2, p3, p4, depth);
		}	
			break;
		case FractalType.Sierpinsky_Carpet:
		{
			Vector3 p1 = new Vector3(0,0,0);
			Vector3 p2 = new Vector3(1,0,0);
			Vector3 p3 = new Vector3(1,1,0);
			Vector3 p4 = new Vector3(0,1,0);
			generateSierpinskyCarpet(p1, p2, p3,p4, depth);
		}
			break;
		
		default:			
			Debug.LogError(type+" is not supported");
			break;
		}
		Debug.Log("Vertices: "+vertices.Count+" Indices "+indices.Count);
		if (vertices.Count>65535){
			vertices.Clear();
			indices.Clear();
			Debug.LogError("Cannot create fractal: Too many vertices for a single (unoptimized) mesh");
			return;
		}
		Mesh mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.Clear();
		Vector3 []verticesV = new Vector3[vertices.Count];
		int[] indicesV = new int[indices.Count];
		
		for (int i=0;i<vertices.Count;i++){
			verticesV[i] = vertices[i];
		}
		for (int i=0;i<indices.Count;i++){
			indicesV[i] = indices[i];
		}
		mesh.vertices = verticesV;
		mesh.triangles = indicesV;
			
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
		
		// clean up
		vertices.Clear();
		indices.Clear();
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
	
	private Vector3 splitEdge(Vector3 p1, Vector3 p2){
		return p1+((p2-p1)*0.5f);
	}
	
	private Vector3 splitEdgeAndAdd(Vector3 p1, Vector3 p2, float fraction){
		Vector3 res = p1+((p2-p1)*fraction);
		vertices.Add(res);
		return res;
	}
	
	private void generateSierpinskyPyramid(Vector3 p1,Vector3 p2,Vector3 p3, Vector3 p4,int depth){
		if (depth == 0){
			addTriangle(p3, p2, p1);
			addTriangle(p1, p2, p4);
			addTriangle(p2, p3, p4);
			addTriangle(p1, p4, p3);
			return;
		}
		depth --;
		
		Vector3 p1p2 = splitEdge(p1,p2);
		Vector3 p1p3 = splitEdge(p1,p3);
		Vector3 p1p4 = splitEdge(p1,p4);
		Vector3 p2p3 = splitEdge(p2,p3);
		Vector3 p2p4 = splitEdge(p2,p4);
		Vector3 p3p4 = splitEdge(p3,p4);
		
		generateSierpinskyPyramid(p1,p1p2,p1p3,p1p4,depth);
		generateSierpinskyPyramid(p1p2,p2,p2p3,p2p4,depth);
		generateSierpinskyPyramid(p1p3,p2p3,p3,p3p4,depth);
		generateSierpinskyPyramid(p1p4,p2p4,p3p4,p4,depth);
		
	}
	
	private void generateSierpinskyTriangle(Vector3 p1,Vector3 p2,Vector3 p3, int depth){
		if (depth == 0){
			addTriangle(p1, p2, p3);
			return;
		}
		depth--;
		
		Vector3 p1p2 = splitEdge(p1,p2);
		Vector3 p1p3 = splitEdge(p1,p3);
		Vector3 p2p3 = splitEdge(p2,p3);
		
		generateSierpinskyTriangle(p1, p1p2, p1p3,depth); 
		generateSierpinskyTriangle(p1p2, p2, p2p3,depth); 
		generateSierpinskyTriangle(p1p3, p2p3, p3,depth); 
	}
	
	private void generateSierpinskyCarpet(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int depth){
		vertices.Add(p1);
		vertices.Add(p2);
		vertices.Add(p3);
		vertices.Add(p4);
		generateSierpinskyCarpet(p1, p2, p3, p4, 0, 1, 2, 3, depth);
	}
	
	private void generateSierpinskyCarpet(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int i1, int i2, int i3, int i4, int depth){
		if (depth == 0){
			indices.Add(i1);
			indices.Add(i2);
			indices.Add(i3);
			
			indices.Add(i1);
			indices.Add(i3);
			indices.Add(i4);
			return;
		}
		depth--;
		
		int indexStart = vertices.Count;
		Vector3 p5 = splitEdgeAndAdd(p1,p2, 1/3f);
		Vector3 p6 = splitEdgeAndAdd(p1,p2, 2/3f);
		Vector3 p7 = splitEdgeAndAdd(p2,p3, 1/3f);
		Vector3 p8 = splitEdgeAndAdd(p2,p3, 2/3f);
		Vector3 p9 = splitEdgeAndAdd(p3,p4, 1/3f);
		Vector3 p10 = splitEdgeAndAdd(p3,p4, 2/3f);
		Vector3 p11 = splitEdgeAndAdd(p4,p1, 1/3f);
		Vector3 p12 = splitEdgeAndAdd(p4,p1, 2/3f);
		Vector3 p13 = splitEdgeAndAdd(p12,p7, 1/3f);
		Vector3 p14 = splitEdgeAndAdd(p12,p7, 2/3f);
		Vector3 p15 = splitEdgeAndAdd(p8,p11, 1/3f);
		Vector3 p16 = splitEdgeAndAdd(p8,p11, 2/3f);
		int i5 = indexStart;
		int i6 = indexStart+1;
		int i7 = indexStart+2;
		int i8 = indexStart+3;
		int i9 = indexStart+4;
		int i10 = indexStart+5;
		int i11 = indexStart+6;
		int i12 = indexStart+7;
		int i13 = indexStart+8;
		int i14 = indexStart+9;
		int i15 = indexStart+10;
		int i16 = indexStart+11;
		
		
		generateSierpinskyCarpet(p1,p5,p13,p12,i1,i5,i13,i12,depth);
		generateSierpinskyCarpet(p5,p6,p14,p13,i5,i6,i14,i13,depth);
		generateSierpinskyCarpet(p6,p2,p7,p14,i6,i2,i7,i14,depth);
		generateSierpinskyCarpet(p14,p7,p8,p15,i14,i7,i8,i15,depth);
		generateSierpinskyCarpet(p15,p8,p3,p9,i15,i8,i3,i9,depth);
		generateSierpinskyCarpet(p16,p15,p9,p10,i16,i15,i9,i10,depth);
		generateSierpinskyCarpet(p11,p16,p10,p4,i11,i16,i10,i4,depth);
		generateSierpinskyCarpet(p12,p13,p16,p11,i12,i13,i16,i11,depth);
	}
}
