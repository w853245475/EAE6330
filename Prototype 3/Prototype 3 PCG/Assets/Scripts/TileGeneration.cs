/// Wirttern By Pengxi Pix Wang
/// 
/// Referenced From https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TerrainType
{
    public string name;
    public float threshold;
    public Color color;
    public int index;
    public Texture terrainTexture;
}

[System.Serializable]
public class Biome
{
    public string name;
    public Color color;
}
[System.Serializable]
public class BiomeRow
{
    public Biome[] biomes;
}


enum VisualizationMode { Height, Heat, Biome }

public class TileData
{
    public float[,] heightMap;
    public float[,] heatMap;
    public TerrainType[,] chosenHeightTerrainTypes;
    public TerrainType[,] chosenHeatTerrainTypes;
    public Mesh mesh;
    public TileData(float[,] heightMap, float[,] heatMap,
      TerrainType[,] chosenHeightTerrainTypes, TerrainType[,] chosenHeatTerrainTypes,
      Mesh mesh)
    {
        this.heightMap = heightMap;
        this.heatMap = heatMap;
        this.chosenHeightTerrainTypes = chosenHeightTerrainTypes;
        this.chosenHeatTerrainTypes = chosenHeatTerrainTypes;
        this.mesh = mesh;
    }
}

public class TileGeneration : MonoBehaviour
{
    [SerializeField]
    private Biome[] biomes;

    [SerializeField]
    private TerrainType[] terrianTypes;

    [SerializeField]
    private TerrainType[] heatTerrainTypes;

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

    [SerializeField]
    private Color waterColor;

    [SerializeField]
    private Wave[] waveSeeds;


    // Start is called before the first frame update
    void Start()
    {
        GenerateTile(56,56);    
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
        // iterate through all the heightMap coordinates, updating the vertex index
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                float height = heightMap[zIndex, xIndex];
                Vector3 vertex = meshVertices[vertexIndex];
                // change the vertex Y coordinate, proportional to the height value
                meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);
                vertexIndex++;
            }
        }
        // update the vertices in the mesh and update its properties
        this.meshFilter.mesh.vertices = meshVertices;
        this.meshFilter.mesh.RecalculateBounds();
        this.meshFilter.mesh.RecalculateNormals();
        // update the mesh collider
        this.meshCollider.sharedMesh = this.meshFilter.mesh;
    }

    public TileData GenerateTile(float centerVertexZ, float maxDistanceZ)
    {
        // calculate tile depth and width based on the mesh vertices
        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int tileWidth = tileDepth;


        // calculate the offsets based on the tile position
        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;

        // generate a heightMap using Perlin Noise
        float[,] heightMap = this.noiseMap.GenerateMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, waveSeeds);
        
        // calculate vertex offset based on the Tile position and the distance between vertices
        Vector3 tileDimensions = this.meshFilter.mesh.bounds.size;
        float distanceBetweenVertices = tileDimensions.z / (float)tileDepth;
        float vertexOffsetZ = this.gameObject.transform.position.z / distanceBetweenVertices;

        // generate a heatMap using uniform noise
        float[,] uniformHeatMap = this.noiseMap.GenerateUniformNoiseMap(tileDepth, tileWidth, centerVertexZ, maxDistanceZ, vertexOffsetZ);
        // generate a heatMap using Perlin Noise
        float[,] randomHeatMap = this.noiseMap.GenerateMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, this.waveSeeds);
        float[,] heatMap = new float[tileDepth, tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                // mix both heat maps together by multiplying their values
                heatMap[zIndex, xIndex] = uniformHeatMap[zIndex, xIndex] * randomHeatMap[zIndex, xIndex];
                // makes higher regions colder, by adding the height value to the heat map
                heatMap[zIndex, xIndex] += this.heatCurve.Evaluate(heightMap[zIndex, xIndex]) * heightMap[zIndex, xIndex];
            }
        }


        // build a Texture2D from the height map
        TerrainType[,] chosenHeightTerrainTypes = new TerrainType[tileDepth, tileWidth];
        Texture2D heightTexture = BuildTexture(heightMap, this.terrianTypes, chosenHeightTerrainTypes);
        // build a Texture2D from the heat map
        TerrainType[,] chosenHeatTerrainTypes = new TerrainType[tileDepth, tileWidth];
        Texture2D heatTexture = BuildTexture(heatMap, this.heatTerrainTypes, chosenHeatTerrainTypes);

        switch (this.visualizationMode)
        {
            case VisualizationMode.Height:
                // assign material texture to be the heightTexture
                this.meshRender.material.mainTexture = heightTexture;
                break;
            case VisualizationMode.Heat:
                // assign material texture to be the heatTexture
                this.meshRender.material.mainTexture = heatTexture;
                break;
        }
        // update the tile mesh vertices according to the height map
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
                // transform the 2D map index is an Array index
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];
                // choose a terrain type according to the height value
                TerrainType terrainType = ChooseTerrainType(height, i_terrainTypes);
                // assign the color according to the terrain type
                colorMap[colorIndex] = terrainType.color;

                i_chosenTerrainTypes[zIndex, xIndex] = terrainType;
            }
        }
        // create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();
        return tileTexture;
    }

    TerrainType ChooseTerrainType(float height, TerrainType[] i_terrainTypes)
    {
        // for each terrain type, check if the height is lower than the one for the terrain type
        foreach (TerrainType terrainType in i_terrainTypes)
        {
            // return the first terrain type whose height is higher than the generated one
            if (height < terrainType.threshold)
            {
                return terrainType;
            }
        }
        return i_terrainTypes[i_terrainTypes.Length - 1];
    }

    private Texture2D BuildBiomeTexture(TerrainType[,] heightTerrainTypes, TerrainType[,] heatTerrainTypes, TerrainType[,] moistureTerrainTypes)
    {
        int tileDepth = heatTerrainTypes.GetLength(0);
        int tileWidth = heatTerrainTypes.GetLength(1);
        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                int colorIndex = zIndex * tileWidth + xIndex;
                TerrainType heightTerrainType = heightTerrainTypes[zIndex, xIndex];
                // check if the current coordinate is a water region
                if (heightTerrainType.name != "water")
                {
                    // if a coordinate is not water, its biome will be defined by the heat and moisture values
                    TerrainType heatTerrainType = heatTerrainTypes[zIndex, xIndex];
                    TerrainType moistureTerrainType = moistureTerrainTypes[zIndex, xIndex];
                    // terrain type index is used to access the biomes table
                    //Biome biome = this.biomes[moistureTerrainType.index].biomes[heatTerrainType.index];
                    Biome biome = this.biomes[heatTerrainType.index];
                    // assign the color according to the selected biome
                    colorMap[colorIndex] = biome.color;
                }
                else
                {
                    // water regions don't have biomes, they always have the same color
                    colorMap[colorIndex] = this.waterColor;
                }
            }
        }
        // create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.filterMode = FilterMode.Point;
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();
        return tileTexture;
    }
}
