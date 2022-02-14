/// Wirttern By Pengxi Pix Wang
/// 
/// Referenced From https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TreeGeneration : MonoBehaviour
{
    [SerializeField]
    private NoiseMap noiseMapGeneration;
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private float levelScale;
    [SerializeField]
    private float neighborRadius;
    [SerializeField]
    private GameObject treePrefab;
    [SerializeField]
    private GameObject snowTreePrefab;
    [SerializeField]
    private GameObject[] junglesPrefab;
    [SerializeField]
    private GameObject[] stonePrefab;
    [SerializeField]
    private GameObject pineTree;

    public void SpawnObjects(int levelDepth, int levelWidth, float distanceBetweenVertices, LevelData levelData)
    {

        // generate a tree noise map using Perlin Noise
        float[,] treeMap = this.noiseMapGeneration.GenerateMap(levelDepth, levelWidth, levelScale, 0, 0, this.waves);
        float levelSizeX = levelWidth * distanceBetweenVertices;
        float levelSizeZ = levelDepth * distanceBetweenVertices;
        for (int zIndex = 0; zIndex < levelDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < levelWidth; xIndex++)
            {
                // convert from Level Coordinate System to Tile Coordinate System and retrieve the corresponding TileData
                TileCoordinate tileCoordinate = levelData.ConvertToTileCoordinate(zIndex, xIndex);
                TileData tileData = levelData.tilesData[tileCoordinate.tileZIndex, tileCoordinate.tileXIndex];
                int tileWidth = tileData.heightMap.GetLength(1);
                // calculate the mesh vertex index
                Vector3[] meshVertices = tileData.mesh.vertices;
                int vertexIndex = tileCoordinate.coordinateZIndex * tileWidth + tileCoordinate.coordinateXIndex;
                // get the terrain type of this coordinate
                TerrainType terrainType = tileData.heightTerrainTypes[tileCoordinate.coordinateZIndex, tileCoordinate.coordinateXIndex];
                // check if it is a water terrain. Trees cannot be placed over the water
                if (terrainType.name != "Water")
                {
                    float treeValue = treeMap[zIndex, xIndex];
                    int terrainTypeIndex = terrainType.index;
                    // compares the current tree noise value to the neighbor ones
                    int neighborZBegin = (int)Mathf.Max(0, zIndex - this.neighborRadius);
                    int neighborZEnd = (int)Mathf.Min(levelDepth - 1, zIndex + this.neighborRadius);
                    int neighborXBegin = (int)Mathf.Max(0, xIndex - this.neighborRadius);
                    int neighborXEnd = (int)Mathf.Min(levelWidth - 1, xIndex + this.neighborRadius);
                    float maxValue = 0f;
                    for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++)
                    {
                        for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++)
                        {
                            float neighborValue = treeMap[neighborZ, neighborX];
                            // saves the maximum tree noise value in the radius
                            if (neighborValue >= maxValue)
                            {
                                maxValue = neighborValue;
                            }
                        }
                    }
                    // if the current tree noise value is the maximum one, place a tree in this location
                    if (treeValue == maxValue)
                    {
                        if(terrainType.name == "Snow")
                        {
                            Vector3 treePosition = new Vector3(xIndex * distanceBetweenVertices - 4.1f, meshVertices[vertexIndex].y - 0.15f, zIndex * distanceBetweenVertices - 4.9f);
                            GameObject tree = Instantiate(this.snowTreePrefab, treePosition, Quaternion.identity) as GameObject;
                            tree.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        }
                        else if(terrainType.name == "Sand")
                        {

                        }
                        else if(terrainType.name == "Grass")
                        {
                            Vector3 treePosition = new Vector3(xIndex * distanceBetweenVertices - 4.6f, meshVertices[vertexIndex].y - 0.2f, zIndex * distanceBetweenVertices - 4.15f);

                            //TreeInstance1 tree = new TreeInstance1(treePosition, new Vector3(0.05f, 0.05f, 0.05f), pineTree);

                            Instantiate(this.pineTree, treePosition, Quaternion.identity).transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                            //GameObject tree = Instantiate(this.pineTree, treePosition, Quaternion.identity) as GameObject;
                            //tree.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);


                            for (int i = 0; i < (int)Random.Range(1, 3); i++)
                            {
                                Vector3 junglePosition = new Vector3(xIndex * distanceBetweenVertices - 4.1f + Random.Range(0.1f, 0.2f), meshVertices[vertexIndex].y - 0.1f, zIndex * distanceBetweenVertices - 4.9f + Random.Range(0.1f, 0.2f));
                                GameObject jungle = Instantiate(this.junglesPrefab[(int)Random.Range(0, junglesPrefab.Length - 1)], junglePosition, Quaternion.identity) as GameObject;
                                jungle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                            }

                        }
                        else if(terrainType.name == "Mountain")
                        {


                        }
                        
                    }
                }
            }
        }
    }
}
