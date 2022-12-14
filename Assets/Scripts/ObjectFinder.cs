using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectFinder : MonoBehaviour {
    [SerializeField] private GameObject test;
    [SerializeField] private GameObject verticesPrefab;
    [SerializeField] private GameObject visiblePrefab;
    [SerializeField] private Transform visible, all;
    private PointsInViewport _pointsInViewport;
    private Vector4[] _points;
    private List<Ray> _rays = new();
    private Camera _camera;
    private DataFileIO _dataFileIO;
    private InstantiateData _instantiateData;

    private void Awake(){
        _camera = Camera.main;
        _pointsInViewport = new PointsInViewport(_camera);
        _dataFileIO = new DataFileIO();
 }

    void FixedUpdate(){
        if (Input.GetKeyDown(KeyCode.L)){
            _instantiateData = _dataFileIO.TryLoad();
            _points = _instantiateData.cloudOfPoint;
            Debug.Log($"Data loaded:  {_points.Length != 0}");
        }

        if (_points != null){
            //   var camLocalPoint = _camera.transform.InverseTransformPoint(_camera.transform.position); 
            foreach (var point in _points){
                var cameraTransform = _camera.transform;
                var wordPoint = cameraTransform.TransformPoint(point);
                var rayDirection = wordPoint - cameraTransform.position;
                var ray = new Ray(wordPoint, rayDirection);

                if (Mathf.PingPong(Time.time, 0.0001f) > 0)
                    Debug.DrawRay(_camera.transform.position, rayDirection, Color.green);

                if (Physics.Raycast(ray, out var raycastHit)){
                    if (Math.Abs(raycastHit.distance - point.w) < 0.1f)
                        Debug.DrawRay(_camera.transform.position, rayDirection*3, Color.red);
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.Space)){

        }
    }

    private void OnDrawGizmos(){
        if(_rays == null ) return;
        
    //    foreach (var ray in _rays){
      //  }
    }
}
