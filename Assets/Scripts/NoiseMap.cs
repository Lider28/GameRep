using UnityEngine;

public enum MapType
{
    Noise,
    Color
}

public enum SeedType
{
    Random,
    Preset
}
public class NoiseMap : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float scale;

    [SerializeField] int octaves;
    [SerializeField] float persistence;
    [SerializeField] float lacunarity;

    [SerializeField] int seed;
    [SerializeField] Vector2 offset;
    [SerializeField] private SeedType typ = SeedType.Preset;

    [SerializeField] MapType type = MapType.Noise;
    [SerializeField] private NoiseMapRenderer noiseMapRenderer;

    private void Start()
    {
        GenerateMap();
    }

    public void RebuildMap()
    {
        CleanMap();
        GenerateMap();
    }

    private void CleanMap()
    {
        foreach (GameObject spawnedTile in noiseMapRenderer.spawnedTiles)
        {
            if (spawnedTile != null)
            {
                Destroy(spawnedTile.gameObject);
            }
        }
        foreach (GameObject spawnedObject in noiseMapRenderer.spawnedObjectsList)
        {
            if (spawnedObject != null)
            {
                Destroy(spawnedObject.gameObject);
            }
        }
    }

    private void GenerateMap()
    {
        if (typ == SeedType.Random)
        {
            seed = Random.Range(1, 10000000);
        }
        float[] noiseMap = NoiseMapGenerator.GenerateNoiseMap(width, height, seed, scale, octaves, persistence, lacunarity, offset);

        NoiseMapRenderer mapRenderer = FindObjectOfType<NoiseMapRenderer>();
        mapRenderer.RenderMap(width, height, noiseMap, type);
    }
}
