using System.CodeDom.Compiler;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public int depth = 20;

    public int width = 256;
    public int height = 256;
    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    private float[,] fallOffMap;

    // void Start ()
    // {
    //     offsetX = Random.Range(0f, 9999f);
    //     offsetY = Random.Range(0f, 9999f);

    //     fallOffMap = FallOffMap();
    // }

    void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);


        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        //fallOffMap = FallOffMap();
        // offsetX += Time.deltaTime;
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight (x, y);
            }
        }
        return heights;
    }

    float CalculateHeight (int x, int y)
    {
        //float xCoord = (float)x / width * scale + offsetX;
        //float yCoord = (float)y / height * scale + offsetY;
        
        float xv = x / (float)width * 2 - 1;
        float yv = y / (float)height * 2 - 1;
        float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));


        float noiseValue = Mathf.PerlinNoise(xv , yv);
        fallOffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));

        noiseValue -= fallOffMap[x, y];

        //float noiseValue = Mathf.PerlinNoise(xCoord , yCoord);
        // noiseValue -= FallOffMap()[y, x];


        return noiseValue;
    }



    void CalculateFallOff()
    {
        float xv = x / (float)width * 2 - 1;
        float yv = y / (float)height * 2 - 1;
        float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
        fallOffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
    }

    // float[,] FallOffMap()
    // {
    //     float[,] fallOffMap = new float[width, height];
    //     for(int x = 0; x < width; x++)
    //     {
    //         for(int y = 0; y < height; y++)
    //         {
    //             float xv = x / (float)width * 2 - 1;
    //             float yv = y / (float)height * 2 - 1;
    //             float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
    //             fallOffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
    //         }
    //     }  
    //     return fallOffMap;
    // }
}
