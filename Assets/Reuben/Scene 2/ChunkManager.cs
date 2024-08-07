using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static ChunkManager instance;

    public Vector2 worldSize;
    public int resolution = 16;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateChunks();
    }

    void GenerateChunks()
    {
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int z = 0; z < worldSize.y; z++)
            {
                GameObject current = new GameObject("Terrain " + (x * y), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

            }
        }
    }
}

class TerrainGenerator
{
    public void Init ()
    {

    }

    public void Generate ()
    {

    }
}
