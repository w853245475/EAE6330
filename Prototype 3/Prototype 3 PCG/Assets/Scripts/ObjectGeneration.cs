/// Wirttern By Pengxi Pix Wang
/// 
/// Referenced From https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ObjectGeneration : MonoBehaviour
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
    private GameObject[] boats;
    [SerializeField]
    private GameObject[] greenTrees;


    public void SpawnObjects(int levelDepth, int levelWidth, float distanceBetweenVertices, LevelData levelData)
    {
        float[,] treeMap = this.noiseMapGeneration.GenerateMap(levelDepth, levelWidth, levelScale, 0, 0, this.waves);
        float levelSizeX = levelWidth * distanceBetweenVertices;
        float levelSizeZ = levelDepth * distanceBetweenVertices;
        for (int zIndex = 0; zIndex < levelDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < levelWidth; xIndex++)
            {
                TileCoordinate tileCoordinate = levelData.ConvertToTileCoordinate(zIndex, xIndex);
                TileData tileData = levelData.tilesData[tileCoordinate.tileZIndex, tileCoordinate.tileXIndex];
                int tileWidth = tileData.heightMap.GetLength(1);
                Vector3[] meshVertices = tileData.mesh.vertices;
                int vertexIndex = tileCoordinate.coordinateZIndex * tileWidth + tileCoordinate.coordinateXIndex;

                TerrainType terrainType = tileData.heightTerrainTypes[tileCoordinate.coordinateZIndex, tileCoordinate.coordinateXIndex];

                float treeValue = treeMap[zIndex, xIndex];
                int terrainTypeIndex = terrainType.index;

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
                        if (neighborValue >= maxValue)
                        {
                            maxValue = neighborValue;
                        }
                    }
                }

                if (treeValue == maxValue)
                {
                    if (terrainType.name != "Water")
                    {
                    
                    
                        Vector3 treePosition = new Vector3(xIndex * distanceBetweenVertices - 4.6f, meshVertices[vertexIndex].y-0.2f, zIndex * distanceBetweenVertices - 4.15f);
                        Vector3 stonePosition = new Vector3(xIndex * distanceBetweenVertices - 4.6f, meshVertices[vertexIndex].y, zIndex * distanceBetweenVertices - 4.15f);

                        if (terrainType.name == "Snow")
                        {
                            Vector3 snowTreePosition = new Vector3(xIndex * distanceBetweenVertices - 4.6f, meshVertices[vertexIndex].y - 0.4f, zIndex * distanceBetweenVertices - 4.15f);

                            GameObject tree = Instantiate(this.snowTreePrefab, snowTreePosition, Quaternion.identity) as GameObject;
                            tree.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        }
                        else if(terrainType.name == "Sand")
                        {
                            int rand = Random.Range(0, 3);
                            Instantiate(this.stonePrefab[rand], stonePosition, Quaternion.identity).transform.localScale = new Vector3(1f, 1f, 1f);

                        }
                        else if(terrainType.name == "Grass")
                        {

                            int rand = Random.Range(0, 2);

                            if(rand == 1)
                                Instantiate(greenTrees[rand], treePosition, Quaternion.identity).transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                            else
                                Instantiate(greenTrees[rand], treePosition, Quaternion.identity).transform.localScale = new Vector3(0.05f, 0.1f, 0.05f);

                        }
                        else if(terrainType.name == "Mountain")
                        {
                        }
                        
                    }
                    else
                    {
                        Vector3 boatPosition = new Vector3(xIndex * distanceBetweenVertices - 4.6f, meshVertices[vertexIndex].y, zIndex * distanceBetweenVertices - 4.15f);

                        int rand = Random.Range(0, 5);
                        Instantiate(this.boats[rand], boatPosition, Quaternion.identity).transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            }
        }
    }
}
