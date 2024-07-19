using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlaneGenerator : MonoBehaviour
{

    //references
    Mesh myMesh;
    MeshFilter meshFilter;


    //plane settings
    [SerializeField] Vector3 planeSize = new Vector3(1, 1, 1);
    [SerializeField] int planeResolution = 1;

    //mesh values 
    List<Vector3> vertices;
    List<int> triangles;

    void Awake()
    {
        myMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = myMesh;
    }
    
    void Update()
    {
        //min avoids errors max keeps performance sane
        planeResolution = Mathf.Clamp(planeResolution, 1, 250);

        GeneratePlane(planeSize, planeResolution);
        AssignMesh();
    }

    void GeneratePlane(Vector2 size, int resolution)
    {
        //generate vertices
        vertices = new List<Vector3>();
        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;
        for (int y = 0; y < resolution + 1; y++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                vertices.Add(new Vector3(x * xPerStep, 0, y * yPerStep));

            }
        }

        //generate triangles
        triangles = new List<int>();
        for (int row = 0; row < resolution; row++)
        {
            for (int column = 0; column < resolution; column++)
            {
                int i = (row * resolution) + row + column;
                
                //first triangle
                triangles.Add(i);
                triangles.Add(i + (resolution) + 1);
                triangles.Add(i + (resolution) + 2);

                //second triangle
                triangles.Add(i);
                triangles.Add(i + resolution + 2);
                triangles.Add(i + 1);
            }
        }
    }

    void AssignMesh()
    {
        myMesh.Clear();
        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangles.ToArray();

        myMesh.RecalculateNormals();
    }
}
