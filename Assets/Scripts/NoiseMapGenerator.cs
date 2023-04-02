using UnityEngine;

public class NoiseMapGenerator : MonoBehaviour
{
    public static float[] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, 
        float persistence, float lacunarity, Vector2 offset)
    {
        
        float[] noiseMap = new float[width*height];
        
        System.Random rand = new System.Random(seed);
        
        Vector2[] octavesOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            
            float xOffset = rand.Next(-100000, 100000) + offset.x;
            float yOffset = rand.Next(-100000, 100000) + offset.y;
            octavesOffset[i] = new Vector2(xOffset / width, yOffset / height);
        }

        if (scale < 0)
        {
            scale = 0.0001f;
        }
        
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                float superpositionCompensation = 0;

                for (int i = 0; i < octaves; i++)
                {

                    float xResult = (x - halfWidth) / scale * frequency + octavesOffset[i].x * frequency;
                    float yResult = (y - halfHeight) / scale * frequency + octavesOffset[i].y * frequency;

                    float generatedValue = Mathf.PerlinNoise(xResult, yResult);

                    noiseHeight += generatedValue * amplitude;

                    noiseHeight -= superpositionCompensation;


                    amplitude *= persistence;
                    frequency *= lacunarity;
                    superpositionCompensation = amplitude / 2;
                }

                noiseMap[y * width + x] = Mathf.Clamp01(noiseHeight);
            }
        }

        return noiseMap;
    }
}