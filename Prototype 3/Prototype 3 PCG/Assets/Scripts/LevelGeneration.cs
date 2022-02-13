/// Wirttern By Pengxi Pix Wang
/// 
/// Referenced From https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
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
                TileData tileData = tile.GetComponent<TileGeneration>().GenerateTile(56, 56);
                levelData.AddTileData(tileData, zTileIndex, xTileIndex);
            }
        }

        treeGeneration.GenerateTrees(this.mapDepthInTiles * tileDepthInVertices, this.mapWidthInTiles * tileWidthInVertices, distanceBetweenVertices, levelData);
    }
}
