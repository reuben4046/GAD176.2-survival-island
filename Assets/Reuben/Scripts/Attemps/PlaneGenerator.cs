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
    [SerializeField] int planeResolution = 100;

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

    float yPerStep;
    [SerializeField] float amplitude = 1f;
    [SerializeField] float fallOffAmmount = 1f;

    [SerializeField] float sineWaveStrength = 2f;
    void GeneratePlane(Vector3 size, int resolution)
    {
        //generate vertices
        vertices = new List<Vector3>();
        float xPerStep = size.x / resolution;
        float zPerStep = size.z / resolution;
        for (int z = 0; z < resolution + 1; z++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                float noise1 = Mathf.PerlinNoise(x * .1f, z * .1f);
                float noise2 = Mathf.PerlinNoise(x * .01f, z * .01f);
                float noise3 = Mathf.PerlinNoise(x * .001f, z * .001f);
                yPerStep = (noise1 + noise2 - noise3) * amplitude;
                vertices.Add(new Vector3(x * xPerStep, yPerStep, z * zPerStep));

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
