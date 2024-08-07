
using UnityEngine;
using UnityEngine.Tilemaps;

using EasyWorldGen2D;


namespace WorlGenSetUp
{
    [ExecuteAlways]

    public class WorldGen : MonoBehaviour
    {


        public int width = 100, height = 100;
        public float scaleFactor = 25;
        public int noiseModifier = 1;

        public enum NoiseType { Simplex, Perlin, Both }
        public NoiseType noiseType;

        public int _seed;
        public bool randomSeed;

        [SerializeField] RuleTile highestValue;
        [SerializeField] RuleTile secondHighestValue;
        [SerializeField] RuleTile secondLowestValue;
        [SerializeField] RuleTile lowestValue;



        public float highestValue_size = 1, secondHighestValue_size = 0.3f, secondLowestValue_size = 0.2f, lowestValue_size = -0.2f;

        public bool isIsland;
        public float islandFalloutSize;


        public void UseAllGenerateMethods()
        {
            WorldGenFunctions.SetUp(transform);
            WorldGenFunctions.Clear(transform);

            Tiles tilesToUse = Tiles_Settings();
            TileAndGenerationSettings settings = TileSettings_Settings();

            int seed = WorldGenFunctions.GenerateSeed(settings);
            _seed = seed;

            WorldGenFunctions.Generate(tilesToUse, settings, transform.GetComponentInChildren<Tilemap>(), (int)noiseType, seed, scaleFactor, noiseModifier, isIsland);
        }

        private Tiles Tiles_Settings()
        {
            Tiles tiles = new Tiles();
            tiles.lowestValue = lowestValue;
            tiles.highestValue = highestValue;
            tiles.secondHighestValue = secondHighestValue;
            tiles.secondLowestValue = secondLowestValue;

            return tiles;
        }

        private TileAndGenerationSettings TileSettings_Settings()
        {
            TileAndGenerationSettings tileSettings = new TileAndGenerationSettings();

            tileSettings.width = width;
            tileSettings.height = height;
            tileSettings.islandFalloutSize = islandFalloutSize;

            tileSettings.isUsingRandomSeed = randomSeed;
            tileSettings.seed = _seed;

            tileSettings.highestValue_size = highestValue_size;
            tileSettings.lowestValue_size = lowestValue_size;
            tileSettings.secondHighestValue_size = secondHighestValue_size;
            tileSettings.secondLowestValue_size = secondLowestValue_size;

            return tileSettings;
        }

    }
}




