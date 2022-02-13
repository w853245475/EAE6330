/// Wirttern By Pengxi Pix Wang
/// 
/// Referenced From https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Using Flyweight Pattern To Record Terrain Type
/// </summary>
public class TerrainType
{
    public string name;
    public float threshold;
    public Color color;
    public int index;
    //public Texture2D terrainTexture;

    public TerrainType(string i_name, float i_threshold, Color i_color, int i_index)
    {
        this.name = i_name;
        this.threshold = i_threshold;
        this.color = i_color;
        this.index = i_index;
    }
}

public class LevelData
{
    private int tileDepthInVertices, tileWidthInVertices;
    public TileData[,] tilesData;
    public LevelData(int tileDepthInVertices, int tileWidthInVertices, int levelDepthInTiles, int levelWidthInTiles)
    {
        // build the tilesData matrix based on the level depth and width
        tilesData = new TileData[tileDepthInVertices * levelDepthInTiles, tileWidthInVertices * levelWidthInTiles];
        this.tileDepthInVertices = tileDepthInVertices;
        this.tileWidthInVertices = tileWidthInVertices;
    }
    public void AddTileData(TileData tileData, int tileZIndex, int tileXIndex)
    {
        // save the TileData in the corresponding coordinate
        tilesData[tileZIndex, tileXIndex] = tileData;
    }

    public TileCoordinate ConvertToTileCoordinate(int zIndex, int xIndex)
    {
        // the tile index is calculated by dividing the index by the number of tiles in that axis
        int tileZIndex = (int)Mathf.Floor((float)zIndex / (float)this.tileDepthInVertices);
        int tileXIndex = (int)Mathf.Floor((float)xIndex / (float)this.tileWidthInVertices);
        // the coordinate index is calculated by getting the remainder of the division above
        // we also need to translate the origin to the bottom left corner
        int coordinateZIndex = this.tileDepthInVertices - (zIndex % this.tileDepthInVertices) - 1;
        int coordinateXIndex = this.tileWidthInVertices - (xIndex % this.tileDepthInVertices) - 1;
        TileCoordinate tileCoordinate = new TileCoordinate(tileZIndex, tileXIndex, coordinateZIndex, coordinateXIndex);
        return tileCoordinate;
    }
}

public class TileCoordinate
{
    public int tileZIndex;
    public int tileXIndex;
    public int coordinateZIndex;
    public int coordinateXIndex;
    public TileCoordinate(int tileZIndex, int tileXIndex, int coordinateZIndex, int coordinateXIndex)
    {
        this.tileZIndex = tileZIndex;
        this.tileXIndex = tileXIndex;
        this.coordinateZIndex = coordinateZIndex;
        this.coordinateXIndex = coordinateXIndex;
    }
}

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    private TreeGeneration treeGeneration;

    [SerializeField]
    private int mapWidthInTiles, mapDepthInTiles;

    [SerializeField]
    private GameObject tilePrefab;

    private TerrainType[] terrainTypes = new TerrainType[5];

    private TerrainType[] temperatureTerrainTypes = new TerrainType[3];

    /// <summary>
    /// Private Terrain References
    /// </summary>
    private TerrainType water = new TerrainType("Water", 0.3f, new Color32(10, 209, 254, 99), 0);
    private TerrainType sand = new TerrainType("Sand", 0.4f, new Color32(255, 250, 147, 100), 1);
    private TerrainType grass = new TerrainType("Grass", 0.6f, new Color32(0, 196, 43, 77), 2);
    private TerrainType mountain = new TerrainType("Mountain", 0.7f, new Color32(143, 91, 1, 56), 3);
    private TerrainType snow = new TerrainType("Snow", 0.9f, new Color32(183, 255, 249, 100), 4);

    /// <summary>
    /// Temperature TerrainTypes
    /// </summary>
    private TerrainType hot = new TerrainType("Hot", 0.3f, new Color32(255, 97, 44, 100), 2);
    private TerrainType warm = new TerrainType("Warm", 0.5f, new Color32(255, 206, 104, 100), 1);
    private TerrainType cold = new TerrainType("Cold", 0.9f, new Color32(137, 255, 245, 100), 0);


    void Start()
    {
        terrainTypes[0] = water;
        terrainTypes[1] = sand;
        terrainTypes[2] = grass;
        terrainTypes[3] = mountain;
        terrainTypes[4] = snow;

        temperatureTerrainTypes[0] = hot;
        temperatureTerrainTypes[1] = warm;
        temperatureTerrainTypes[2] = cold;

        GenerateMap();
    }
    void GenerateMap()
    {

        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;

        // calculate the number of vertices of the tile in each axis using its mesh
        Vector3[] tileMeshVertices = tilePrefab.GetComponent<MeshFilter>().sharedMesh.vertices;
        int tileDepthInVertices = (int)Mathf.Sqrt(tileMeshVertices.Length);
        int tileWidthInVertices = tileDepthInVertices;

        float distanceBetweenVertices = (float)tileDepth / (float)tileDepthInVertices;

        // build an empty LevelData object, to be filled with the tiles to be generated
        LevelData levelData = new LevelData(tileDepthInVertices, tileWidthInVertices, this.mapDepthInTiles, this.mapWidthInTiles);


        for (int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++)
        {
            for (int zTileIndex = 0; zTileIndex < mapDepthInTiles; zTileIndex++)
            {

                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileWidth,
                  this.gameObject.transform.position.y,
                  this.gameObject.transform.position.z + zTileIndex * tileDepth);

           
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;


                // generate the Tile texture and save it in the levelData
                TileData tileData = tile.GetComponent<TileGeneration>().GenerateTile(56, 56, terrainTypes, temperatureTerrainTypes);
                levelData.AddTileData(tileData, zTileIndex, xTileIndex);
            }
        }

        treeGeneration.GenerateTrees(this.mapDepthInTiles * tileDepthInVertices, this.mapWidthInTiles * tileWidthInVertices, distanceBetweenVertices, levelData);
    }
}
