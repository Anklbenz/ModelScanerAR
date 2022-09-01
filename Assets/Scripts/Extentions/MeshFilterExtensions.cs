using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
public static class MeshFilterExtensions
{
    public static Vector3[] GetHashVertices(this MeshFilter meshFilter){
        var hashVertices = new HashSet<Vector3>();
        foreach (var vertex in meshFilter.sharedMesh.vertices)
            hashVertices.Add(vertex.Round(3));
        
        return hashVertices.ToArray();
    }
    
    public static Vector3[] GetHashVerticesWorld(this MeshFilter meshFilter){
        var verticesLocal = meshFilter.GetHashVertices();
        var worldPosVertices = new List<Vector3>();

        foreach (var vertex in verticesLocal){
            var vertexWorldPos =  meshFilter.transform.TransformPoint(vertex);
            worldPosVertices.Add(vertexWorldPos);
        }

        return worldPosVertices.ToArray();
    }
    
}