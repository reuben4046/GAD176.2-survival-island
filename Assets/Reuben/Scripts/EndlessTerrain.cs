using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{

    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public LODInfo[] detailLevels;
    public static float maxViewDistance;

    public Transform veiwer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;
    Vector2 veiwerPositionOld;
    static MapGenerator mapGenerator;
    int chunkSize;
    int chunksVisibleInViewDistance;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();

    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();

        maxViewDistance = detailLevels [detailLevels.Length - 1].visibleDistanceThreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);

        UpdateVisibleChunks();
    }

    void Update() {
        viewerPosition = new Vector2(veiwer.position.x, veiwer.position.z);

        if ((veiwerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
            veiwerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++) {
            terrainChunksVisibleLastUpdate[i].SetVisable(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++) {
            for (int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++) {
                Vector2 veiwedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (terrainChunkDictionary.ContainsKey(veiwedChunkCoord)) {
                    terrainChunkDictionary[veiwedChunkCoord].UpdateTerrainChunk();
                } else {
                    terrainChunkDictionary.Add(veiwedChunkCoord, new TerrainChunk(veiwedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
                }
            } 
        }
    }  

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;

        MapData mapData;
        bool mapDataReceived;
        int previousLODIndex = -1;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material) {
            this.detailLevels = detailLevels;

            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;

            meshObject.transform.position = positionV3;
            meshObject.transform.parent = parent;
            SetVisable(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++) {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
            }

            mapGenerator.RequestMapData(position, OnMapDataReceived);
        }

        void OnMapDataReceived(MapData mapData) {
            this.mapData = mapData;
            mapDataReceived = true;

            Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            UpdateTerrainChunk();
        }

        public void UpdateTerrainChunk() {
            if (mapDataReceived) {
                float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance (viewerPosition));
                bool visable = viewerDistanceFromNearestEdge <= maxViewDistance;

                if (visable) {
                    int lodIndex = 0;

                    for (int i = 0; i < detailLevels.Length - 1; i++) {
                        if (viewerDistanceFromNearestEdge > detailLevels[i].visibleDistanceThreshold) {
                            lodIndex = i + 1;
                        } else {
                            break;
                        }
                    }

                    if (lodIndex != previousLODIndex) {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if (lodMesh.hasMesh) {
                            previousLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                        } else if (!lodMesh.hasRequestedMesh) {
                            lodMesh.RequestMesh(mapData);
                        }
                    }

                    terrainChunksVisibleLastUpdate.Add(this);
                }

                SetVisable(visable);
            }
        }

        public void SetVisable(bool visable) {
            meshObject.SetActive(visable);
        }

        public bool IsVisable() {
            return meshObject.activeSelf;
        }
    }

    class LODMesh{
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallBack;

        public LODMesh(int lod, System.Action updateCallBack) {
            this.lod = lod;
            this.updateCallBack = updateCallBack;
        }

        void OnMeshDataReceived(MeshData meshData) {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            updateCallBack();
        }

        public void RequestMesh(MapData mapData) {
                hasRequestedMesh = true;
                mapGenerator.RequestMeshData(mapData, lod, OnMeshDataReceived);
        }
    }

    [System.Serializable]
    public struct LODInfo {
        public int lod;
        public float visibleDistanceThreshold;
    }

} 

