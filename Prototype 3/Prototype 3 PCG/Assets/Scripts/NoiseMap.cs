/// Wirttern By Pengxi Pix Wang
/// 
/// Referenced From https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public float seed;
    public float frequency;
    public float amplitude;

    public Wave(float i_seed, float i_frequency, float i_ampli)
    {
        seed = i_seed;
        frequency = i_frequency;
        amplitude = i_ampli;
    }
}

public class NoiseMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float[,] GenerateMap(int i_width, int i_depth, float i_scale, float offsetX, float offsetZ, Wave[] waveSeed)
    {
        float[,] noiseMap = new float[i_depth, i_width];

        for(int z = 0; z < i_depth; z++)
        {
            for(int x = 0; x < i_width; x++)
            {
                float actualX = (x + offsetX) / i_scale;
                float actualZ = (z + offsetZ) / i_scale;

                float noise = 0f;
                float normalization = 0f;
                foreach (Wave wave in waveSeed)
                {
                    noise += wave.amplitude * Mathf.PerlinNoise(actualX * wave.frequency + wave.seed, actualZ * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }
                noise /= normalization;

                noiseMap[z, x] = noise;
            }
        }

        return noiseMap;
    }

    public float[,] GenerateUniformNoiseMap(int mapDepth, int mapWidth, float centerVertexZ, float maxDistanceZ, float offsetZ)
    {
        float[,] noiseMap = new float[mapDepth, mapWidth];
        for (int zIndex = 0; zIndex < mapDepth; zIndex++)
        {
            float sampleZ = zIndex + offsetZ;
            float noise = Mathf.Abs(sampleZ - centerVertexZ) / maxDistanceZ;
            for (int xIndex = 0; xIndex < mapWidth; xIndex++)
            {
                noiseMap[mapDepth - zIndex - 1, xIndex] = noise;
            }
        }
        return noiseMap;
    }
}
