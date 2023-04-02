using System;
using System.Collections.Generic;
using UnityEngine;


public enum TypeOfSpawn
{
    Tree,
    Mount,
    HightMount,
    Snow,
    None
}
public class NoiseMapRenderer  : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer spriteRen;

    [Serializable]
    public struct TerrainLevel
    {
        public string name;
        public float heightTile;
        public Color color;
        public GameObject prefab;
        public TypeOfSpawn spawn;
    }
    [SerializeField] public List<TerrainLevel> terrainLevel = new List<TerrainLevel>();

    [SerializeField]private GenerateObject spawnObj ;

    public List<GameObject> spawnedTiles;
    public List<GameObject> spawnedObjectsList;

    public void RenderMap(int width, int height, float[] noiseMap, MapType type)
    {
        if (type == MapType.Noise)
        {
            //ApplyColorMap(width, height, GenerateNoiseMap(noiseMap));
        }
        else if (type == MapType.Color)
        {
            ApplyColorMap(width, height, GenerateColorMap(noiseMap), GenerateNoiseMap(noiseMap));
            MakeMap(width, height, noiseMap);
        }
    }

    

    private void ApplyColorMap(int width, int height, Color[] colors, Color[] noise)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colors);
        texture.Apply();

        Texture2D text = new Texture2D(width, height);
        text.wrapMode = TextureWrapMode.Clamp;
        text.filterMode = FilterMode.Point;
        text.SetPixels(noise);
        text.Apply();

        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), 
            new Vector2(0.5f, 0.5f), 100.0f);
        spriteRen.sprite = Sprite.Create(text, new Rect(0.0f, 0.0f, text.width, text.height), 
            new Vector2(0.5f, 0.5f), 100.0f);
    }

    private Color[] GenerateNoiseMap(float[] noiseMap)
    {
        Color[] colorMap = new Color[noiseMap.Length];
        for (int i = 0; i < noiseMap.Length; i++)
        {
            colorMap[i] = Color.Lerp(Color.black, Color.white, noiseMap[i]);
        }
        return colorMap;
    }

    private Color[] GenerateColorMap(float[] noiseMap)
    {
        Color[] colorMap = new Color[noiseMap.Length];
        for (int i = 0; i < noiseMap.Length; i++)
        {
            colorMap[i] = terrainLevel[terrainLevel.Count-1].color;
            foreach (var level in terrainLevel)
            {
                if (noiseMap[i] < level.heightTile)
                {
                    colorMap[i] = new Color(level.color.r*255f, level.color.g*255f, level.color.b*255f, 1);
                    break;
                }
            }
        }

        return colorMap;
    }

    private void MakeMap(int width, int height, float[] noiseMap)
    {
        
        spawnedTiles = new List<GameObject>(width * height);
        spawnedObjectsList = new List<GameObject>(width * height);
        Color[][] colorMap = new Color[height][];
        for (int index = 0; index < height; index++)
        {
            colorMap[index] = new Color[width];
        }

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[z][x] = terrainLevel[terrainLevel.Count-1].color;
                foreach (var level in terrainLevel)
                {
                    if (noiseMap[x+z*width] < level.heightTile)
                    {
                        GameObject spawnTile = Instantiate(level.prefab, new Vector3(x - (width/2), 1, z - (width/2)),
                            Quaternion.identity);
                        spawnedTiles.Add(spawnTile);
                        if (level.spawn != TypeOfSpawn.None)
                        {
                            GameObject sp = spawnObj.ObjectSpawner(x-(width/2), z-(width/2), level.spawn);
                            spawnedObjectsList.Add(sp);
                        }
                        break;
                    }
                }
            }
        }
    }
}
