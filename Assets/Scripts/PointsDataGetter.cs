using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointsDataGetter : MonoBehaviour {
    [SerializeField] private GameObject test;
    [SerializeField] private GameObject verticesPrefab;
    [SerializeField] private GameObject visiblePrefab;
    [SerializeField] private Transform visible, all;
    private PointsInViewport _pointsInViewport;
    private readonly List< Vector3> _allVertices = new();
    private List< Vector3> _visibleVertices = new ();
    private MeshFilter[] _allChildrenMeshFilters;
    private Camera _сamera;
    private DataFileIO m_DataFileFileIO;

    private void Awake(){
        _сamera = Camera.main;
        _pointsInViewport = new PointsInViewport(_сamera);
        m_DataFileFileIO = new DataFileIO();
        _allChildrenMeshFilters = GetAllMeshFilters(test);
    }

    void Update(){
        if (Input.GetMouseButtonDown(0)){
            _allVertices.Clear();
            foreach (var meshFilter in _allChildrenMeshFilters)
                _allVertices.AddRange(meshFilter.GetHashVerticesWorldPosition());

            InstantiateVertex(verticesPrefab, all, _allVertices.ToArray());
            Debug.Log("All Vertices " + _allVertices.Count);
        }

        if (Input.GetMouseButtonDown(1)){
            _visibleVertices.Clear();
            _visibleVertices = _pointsInViewport.Get(_allVertices.ToArray(), _сamera).ToList();
            InstantiateVertex(visiblePrefab, visible, _visibleVertices.ToArray());
            Debug.Log("Visible Vertices " + _visibleVertices.Count);
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            var data = new InstantiateData();
            GetData(data);
            m_DataFileFileIO.Save(data);
        }
    }

    private void GetData(InstantiateData data){
        data.cloudOfPoint = GetCloudOfPoints();
        data.instantiatePoint = WordToCameraOriginArPoint(test.transform.position);

    }

    private Vector4 WordToCameraOriginArPoint(Vector3 worldPoint){
        Vector4 arPoint = _сamera.transform.InverseTransformPoint(worldPoint);
        var distance = Vector3.Distance(Vector3.zero, arPoint);
        arPoint.w = distance;
        
        return arPoint;
    }

    private Vector4[] GetCloudOfPoints(){
        var verticesInCameraOrigin = new List<Vector4>();
        foreach (var vertex in _visibleVertices)
            verticesInCameraOrigin.Add(WordToCameraOriginArPoint(vertex));
        
        return verticesInCameraOrigin.ToArray();
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
