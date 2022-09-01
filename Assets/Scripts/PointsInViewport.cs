using System.Collections.Generic;
using UnityEngine;

public class PointsInViewport {
    private readonly Camera _cam;

    public PointsInViewport(Camera cam){
        _cam = cam;
    }
    
    public Vector3[] Get(Vector3[] vertices, Camera cam){
        var cameraPosition = cam.transform.position;
        var visibleVertices = new List<Vector3>();

        foreach (var vertex in vertices){
            if(!VertexInViewportRect(vertex)) continue;
            
            var rayDirection = vertex - cameraPosition;
            var ray = new Ray(cameraPosition, rayDirection);

            if (!Physics.Raycast(ray, out var hitInfo)) continue;
            
            if (hitInfo.point == vertex)
                visibleVertices.Add(vertex);
        }

        return visibleVertices.ToArray();
    }

    private bool VertexInViewportRect(Vector3 point){
        var pointInViewport = _cam.WorldToViewportPoint(point);
        var inCamFrustum = pointInViewport.x is > 0 and < 1 && pointInViewport.y is > 0 and < 1; ;
        var inFrontOfCam = pointInViewport.z > 0;
    
        return inCamFrustum && inFrontOfCam;
    }
}