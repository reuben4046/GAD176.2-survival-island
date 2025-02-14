using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRP : MonoBehaviour
{
    public float waterLevel = 0.4f;
    public float scale = 0.1f;
    public int size = 100;
    
    
    Cell[,] grid;

    void Start()
    {
        float[,] noiseMap = new float[size, size];
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 100000f);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[y, x] = noiseValue;
            }
        }

        float[,] fallOffMap = new float[size, size];
        for(int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                fallOffMap[y, x] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }    

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                Cell cell = new Cell();
                float noiseValue = noiseMap[x, y];
                noiseValue -= fallOffMap[x, y];
                cell.isWater = noiseValue < waterLevel;
                grid[x, y] = cell;
            }
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;
        for (int y = 0; y < size;y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.green;
                }
                Vector3 pos = new Vector3(x, 0, y);
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }

}
