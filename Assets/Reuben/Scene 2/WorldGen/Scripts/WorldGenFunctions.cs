
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace EasyWorldGen2D
{
    public class WorldGenFunctions
    {

        public static void SetUp(Transform objectTransform)
        {
            Tilemap tilemap = new Tilemap();
            tilemap = objectTransform.GetComponent<Tilemap>();

            if (objectTransform.GetComponentInChildren<Tilemap>() == null)
            {
               CreateNewGrid(tilemap, objectTransform);
            }


        }

        public static void CreateNewGrid(Tilemap objTilemap, Transform objTransform)
        {

            var grid = new GameObject("Grid").AddComponent<Grid>();
            objTilemap = new GameObject("Tilemap").AddComponent<Tilemap>();
            objTilemap.gameObject.AddComponent<TilemapRenderer>();

            grid.cellLayout = GridLayout.CellLayout.Rectangle;
            objTilemap.GetComponent<TilemapRenderer>().sortOrder = TilemapRenderer.SortOrder.BottomLeft;
            objTilemap.transform.SetParent(grid.transform);

            grid.transform.SetParent(objTransform);
        }



        public static void Generate(Tiles tilesToUse, TileAndGenerationSettings settings, Tilemap tilemap, int noiseType, int seed, float scaleFactor, int noiseModifier, bool isIsland)
        {
            Coord[,] coord = new Coord[settings.width, settings.height];
            //Debug.Log("Generating");

            for (int x = 0; x < coord.GetLength(0); x++)
            {
                for (int y = 0; y < coord.GetLength(1); y++)
                {
                    coord[x, y] = new Coord();
                    Coord newCoordinate = coord[x, y];

                    float noiseValue_1 = new float();
                    float noiseValue_2 = new float();

                    if(noiseType == 0) //Simplex
                    {
                        noiseValue_1 = noise.snoise(new float2((x / scaleFactor) + (seed * noiseModifier), (y / scaleFactor) + (seed * noiseModifier)));
                        noiseValue_2 = noise.snoise(new float2((x / scaleFactor) + (seed * 2 * noiseModifier), (y / scaleFactor) + (seed * 2 * noiseModifier)));
                    }
                    else if(noiseType == 1) //Perlin
                    {
                        noiseValue_1 = noise.cnoise(new float2((x / scaleFactor) + (seed * noiseModifier), (y / scaleFactor) + (seed * noiseModifier)));
                        noiseValue_2 = noise.cnoise(new float2((x / scaleFactor) + (seed * 2 * noiseModifier), (y / scaleFactor) + (seed * 2 * noiseModifier)));
                    }
                    else if(noiseType == 2) //Both
                    {
                        noiseValue_1 = noise.cnoise(new float2((x / scaleFactor) + (seed * noiseModifier), (y / scaleFactor) + (seed * noiseModifier)));
                        noiseValue_2 = noise.snoise(new float2((x / scaleFactor) + (seed * noiseModifier), (y / scaleFactor) + (seed * noiseModifier)));
                    }


                    //average both
                    float noiseValue = (noiseValue_1 + noiseValue_2) / 2f;

                    Vector3Int tileLocation = new Vector3Int(x, y);

                    if (isIsland)
                    {
                        noiseValue = MakeIsland(settings, noiseValue, new Vector2(x, y));
                    }


                    SetTiles(newCoordinate, tilesToUse, settings, tilemap, tileLocation, noiseValue, desiredYValue: noiseValue);
                }
            }

        }

        private static void SetTiles(Coord coordinate, Tiles m_tiles, TileAndGenerationSettings settings, Tilemap objectTilemap, Vector3Int tileLocation, float noiseValue, float desiredYValue)
        {
            coordinate.isHighestValue = noiseValue < settings.highestValue_size && noiseValue > settings.secondHighestValue_size;
            coordinate.isSecondHighestValue = noiseValue < settings.secondHighestValue_size && noiseValue > settings.secondLowestValue_size;
            coordinate.isSecondLowestValue = noiseValue < settings.secondLowestValue_size && noiseValue > settings.lowestValue_size;
            coordinate.isLowestValue = noiseValue < settings.lowestValue_size;

            Vector3 vertexPosition = new Vector3((tileLocation.x * settings.width), (tileLocation.y * settings.height), desiredYValue);


            if (coordinate.isHighestValue)
            {
                //Debug.Log(m_tiles.highestValue);
                objectTilemap.SetTile(tileLocation, m_tiles.highestValue);

            }
            else if (coordinate.isSecondHighestValue)
            {
                //Debug.Log(m_tiles.secondHighestValue);
                objectTilemap.SetTile(tileLocation, m_tiles.secondHighestValue);

            }
            else if (coordinate.isSecondLowestValue)
            {
                //Debug.Log(m_tiles.secondLowestValue);
                objectTilemap.SetTile(tileLocation, m_tiles.secondLowestValue);

            }
            else if (coordinate.isLowestValue)
            {
                //Debug.Log(m_tiles.lowestValue);
                objectTilemap.SetTile(tileLocation, m_tiles.lowestValue);

            }

        }

        private static float MakeIsland(TileAndGenerationSettings settings, float noiseValue, Vector2 tilePosition)
        {

            //Find Center
            float center_width = settings.width / 2;
            float center_height = settings.height / 2;

            Vector2 center = new Vector2(center_width, center_height);
            float distance = Vector2.Distance(tilePosition, center);
            float avgSize_withdamping = (settings.width + settings.height) / (float)settings.islandFalloutSize;

            if (distance > 0)
            {
                noiseValue = ((noiseValue * avgSize_withdamping) - distance) / avgSize_withdamping;
            }

            return noiseValue;
        }

        public static int GenerateSeed(TileAndGenerationSettings settings)
        {
            int seed;

            if (settings.isUsingRandomSeed)
            {
                seed = UnityEngine.Random.Range(1000, 9999);
                settings.seed = seed;
            }
            else
            {
                seed = settings.seed;
            }

            return seed;
        }

        public static void Clear(Transform objTransform)
        {
            try
            {
                //Debug.Log("Clearing");
                objTransform.GetComponentInChildren<Grid>().GetComponentInChildren<Tilemap>();
            }
            catch
            {
                //Debug.Log("Could't clear");
            }
            return;
        }

    }
    public class Coord
    {

        public bool isHighestValue;
        public bool isSecondHighestValue;
        public bool isSecondLowestValue;
        public bool isLowestValue;
    }

    public class Tiles
    {
        public RuleTile highestValue;
        public RuleTile secondHighestValue;
        public RuleTile secondLowestValue;
        public RuleTile lowestValue;
    }

    public class TileAndGenerationSettings
    {
        public int width;
        public int height;
        public float islandFalloutSize;

        public bool isUsingRandomSeed;
        public int seed;

        public float highestValue_size;
        public float lowestValue_size;
        public float secondHighestValue_size;
        public float secondLowestValue_size;
    }
}

