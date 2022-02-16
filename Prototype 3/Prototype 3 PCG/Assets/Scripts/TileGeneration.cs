/// Wirttern By Pengxi Pix Wang
/// 
/// Referenced From https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum VisualizationMode { Height, Heat}

public class TileData
{
    public float[,] heightMap;
    public float[,] heatMap;
    public TerrainType[,] heightTerrainTypes;
    public TerrainType[,] heatTerrainTypes;
    public Mesh mesh;
    public TileData(float[,] heightMap, float[,] heatMap,
      TerrainType[,] i_HeightTerrainTypes, TerrainType[,] i_HeatTerrainTypes,
      Mesh mesh)
    {
        this.heightMap = heightMap;
        this.heatMap = heatMap;
        this.heightTerrainTypes = i_HeightTerrainTypes;
        this.heatTerrainTypes = i_HeatTerrainTypes;
        this.mesh = mesh;
    }
}

public class TileGeneration : MonoBehaviour
{

    private TerrainType[] terrianTypes = new TerrainType[5];

    [SerializeField]
    private TerrainType[] heatTerrainTypes = new TerrainType[3];

    [SerializeField]
    private VisualizationMode visualizationMode;

    [SerializeField]
    NoiseMap noiseMap;

    [SerializeField]
    private MeshRenderer meshRender;

    [SerializeField]
    private MeshFilter meshFilter;

    [SerializeField]
    private MeshCollider meshCollider;

    [SerializeField]
    private float mapScale;

    [SerializeField]
    private float heightMultiplier;

    [SerializeField]
    private AnimationCurve heightCurve;

    [SerializeField]
    private AnimationCurve heatCurve;

    //[SerializeField]
    //private Wave[] waveSeeds;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void UpdateMeshVertices(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);
        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                float height = heightMap[zIndex, xIndex];
                Vector3 vertex = meshVertices[vertexIndex];
                meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);
                vertexIndex++;
            }
        }
        this.meshFilter.mesh.vertices = meshVertices;
        this.meshFilter.mesh.RecalculateBounds();
        this.meshFilter.mesh.RecalculateNormals();
        this.meshCollider.sharedMesh = this.meshFilter.mesh;
    }

    public TileData GenerateTile(float centerVertexZ, float maxDistanceZ, TerrainType[] i_terrainTypes, TerrainType[] i_temperatureTerrainTypes)
    {
        System.Array.Copy(i_terrainTypes, terrianTypes, i_terrainTypes.Length);
        System.Array.Copy(i_temperatureTerrainTypes, heatTerrainTypes, i_temperatureTerrainTypes.Length);

        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int tileWidth = tileDepth;

        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;

        print(LevelGeneration.waveSeeds[0].seed);
        float[,] heightMap = this.noiseMap.GenerateMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, LevelGeneration.waveSeeds);
        
        Vector3 tileDimensions = this.meshFilter.mesh.bounds.size;
        float distanceBetweenVertices = tileDimensions.z / (float)tileDepth;
        float vertexOffsetZ = this.gameObject.transform.position.z / distanceBetweenVertices;

        float[,] uniformHeatMap = this.noiseMap.GenerateUniformNoiseMap(tileDepth, tileWidth, centerVertexZ, maxDistanceZ, vertexOffsetZ);
        float[,] randomHeatMap = this.noiseMap.GenerateMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, LevelGeneration.waveSeeds);
        float[,] heatMap = new float[tileDepth, tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                heatMap[zIndex, xIndex] = uniformHeatMap[zIndex, xIndex] * randomHeatMap[zIndex, xIndex];
                heatMap[zIndex, xIndex] += this.heatCurve.Evaluate(heightMap[zIndex, xIndex]) * heightMap[zIndex, xIndex];
            }
        }



        TerrainType[,] chosenHeightTerrainTypes = new TerrainType[tileDepth, tileWidth];
        Texture2D heightTexture = BuildTexture(heightMap, this.terrianTypes, chosenHeightTerrainTypes);
        TerrainType[,] chosenHeatTerrainTypes = new TerrainType[tileDepth, tileWidth];
        Texture2D heatTexture = BuildTexture(heatMap, this.heatTerrainTypes, chosenHeatTerrainTypes);

        switch (this.visualizationMode)
        {
            case VisualizationMode.Height:
                this.meshRender.material.mainTexture = heightTexture;
                break;
            case VisualizationMode.Heat:
                this.meshRender.material.mainTexture = heatTexture;
                break;
        }
        UpdateMeshVertices(heightMap);

        TileData tileData = new TileData(heightMap, heatMap, chosenHeightTerrainTypes, chosenHeatTerrainTypes, this.meshFilter.sharedMesh);

        return tileData;
    }

    private Texture2D BuildTexture(float[,] heightMap, TerrainType[] i_terrainTypes, TerrainType[,] i_chosenTerrainTypes)
    {

        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];

                TerrainType terrainType = SelectTerrainType(height, i_terrainTypes);

                colorMap[colorIndex] = terrainType.color;

                i_chosenTerrainTypes[zIndex, xIndex] = terrainType;
            }
        }

        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();
        return tileTexture;
    }

    TerrainType SelectTerrainType(float height, TerrainType[] i_terrainTypes)
    {
        foreach (TerrainType terrainType in i_terrainTypes)
        {
            if (height < terrainType.threshold)
            {
                return terrainType;
            }
        }
        return i_terrainTypes[i_terrainTypes.Length - 1];

    }
}
