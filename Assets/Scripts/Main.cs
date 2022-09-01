using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Main : MonoBehaviour {
    [SerializeField] private GameObject test;
    [SerializeField] private GameObject verticesPrefab;
    [SerializeField] private GameObject visiblePrefab;
    [SerializeField] private Transform visible, all;
    private PointsInViewport _pointsInViewport;
    private List< Vector3> _allVertices = new(), _visibleVertices = new ();
    private Camera _сamera;

    private void Awake(){
        _сamera = Camera.main;
        _pointsInViewport = new PointsInViewport(_сamera);
    }

    void Update(){


        /*var pointPosition = new Vector3(0, 0, 0);

        Debug.Log("Before:");
        Debug.Log(pointPosition);

         pointPosition = Camera.main.transform.InverseTransformPoint(pointPosition);


        Debug.Log("After:");
        Debug.Log(pointPosition);*/

        if (Input.GetMouseButtonDown(0)){
            var allChildrenMeshFilters = GetAllMeshFilters(test);


            foreach (var meshFilter in allChildrenMeshFilters)
                _allVertices.AddRange(meshFilter.GetHashVerticesWorld());

            _visibleVertices = _pointsInViewport.Get(_allVertices.ToArray(), _сamera).ToList();
            InstantiateVertex(verticesPrefab, all, _allVertices.ToArray());
            Debug.Log("All Vertices " + _allVertices.Count);
        }

        if (Input.GetMouseButtonDown(1)){
            InstantiateVertex(visiblePrefab, visible, _visibleVertices.ToArray());
            Debug.Log("Visible Vertices " + _visibleVertices.Count);
        }

    }

    private MeshFilter[] GetAllMeshFilters(GameObject go){
        return go.GetComponentsInChildren<MeshFilter>();
    }
    void InstantiateVertex(GameObject prefab, Transform parent, Vector3[] positions){
        foreach (var vertex in positions){
            Instantiate(prefab, vertex, Quaternion.identity, parent);
        }
    }
}
