using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int worldSize = 100;
    public int chunkSize = 16;
    public int dirtLayerHeight = 31;
    //eigentlich 5, keine artefakte deswegen
    public float surfaceValue = 0.2f;
    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public float seed;
    public Texture2D cavenoiseTexture;
    [Header("Tile Sprites")]
    // public Sprite stone;
    // public Sprite dirt;
    // public Sprite grass;
    // public Sprite ruins;
    public TIleAtlas TileAtlas;
    public float heightMultiplier = 25f;
    public int heightAddition = 25;
    public bool generateCaves = true;
    public List<Vector2> worldTiles = new List<Vector2>();
    public GameObject[] worldChunks;
    public int ruinsChance = 10;

    [Header("artefacts")]
    public float cointreasureRarity;
    public float cointreasureSize;
    public float goldtreasureRarity;
    public float goldtreasureSize;
    public Texture2D coinSpread;
    public Texture2D goldSpread;

    private void OnValidate()
    {
        if (cavenoiseTexture == null)
        {
            cavenoiseTexture = new Texture2D(worldSize, worldSize);
            coinSpread = new Texture2D(worldSize, worldSize);
            goldSpread = new Texture2D(worldSize, worldSize);
        }

        GenerateNoiseTexture(caveFreq, surfaceValue, cavenoiseTexture);
        GenerateNoiseTexture(cointreasureRarity, cointreasureSize, coinSpread);
        GenerateNoiseTexture(goldtreasureRarity, goldtreasureSize, goldSpread);

    }




    // Start is called before the first frame update
    private void Start()
    {
        seed = Random.Range(-1000, 1000);
        if (cavenoiseTexture == null)
        {
            cavenoiseTexture = new Texture2D(worldSize, worldSize);
            coinSpread = new Texture2D(worldSize, worldSize);
            goldSpread = new Texture2D(worldSize, worldSize);
        }
        GenerateNoiseTexture(caveFreq, surfaceValue, cavenoiseTexture);
        GenerateNoiseTexture(cointreasureRarity, cointreasureSize, coinSpread);
        GenerateNoiseTexture(goldtreasureRarity, goldtreasureSize, goldSpread);
       // GenerateNoiseTexture();
        CreateChunks();
        GenerateTerrain();
    }

    public void CreateChunks()
    {
        int numChunks = worldSize / chunkSize;
        worldChunks = new GameObject[numChunks];

        for (int i = 0; i < numChunks; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform;
            worldChunks[i] = newChunk;
        }
    }


    public void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
            Debug.Log(height);
            for (int y = 0; y < height; y++)
            {
                Sprite tileSprite;
                if (y < height - dirtLayerHeight)
                {
                    if (coinSpread.GetPixel(x, y).r > 0.5f)
                    {
                        tileSprite = TileAtlas.cointreasure.tileSprite;
                    }
                    else if (goldSpread.GetPixel(x, y).r > 0.5f)
                    {
                        tileSprite = TileAtlas.goldtreasure.tileSprite;
                    }
                    else
                    {
                        tileSprite = TileAtlas.redstone.tileSprite;
                    }
                }
                else if (y < height - 23)
                {
                    tileSprite = TileAtlas.redsand.tileSprite;
                }
                else if (y < height - 17)
                {
                    tileSprite = TileAtlas.brown.tileSprite;
                }
                else if (y < height - 15)
                {
                    tileSprite = TileAtlas.lava.tileSprite;
                }
                else if (y < height - 10)
                {
                    tileSprite = TileAtlas.graveldirt.tileSprite;
                }
                else if (y < height - 5)
                {
                    tileSprite = TileAtlas.dirt.tileSprite;
                }
                else if (y < height - 2)
                    {
                    tileSprite = TileAtlas.orange.tileSprite;
                }
                else
                {
                    tileSprite = TileAtlas.sand.tileSprite;
                }
            if (generateCaves == true) { 
                if (cavenoiseTexture.GetPixel(x, y).r > surfaceValue)
                {
                        placeTile(tileSprite, x, y);
                }
            }
            else
                {
                    placeTile(tileSprite, x, y);
                }
            if (y >= height -1)
                {
                    int t = Random.Range(0, ruinsChance);
                    if (t == 1)
                    {
                        if (worldTiles.Contains(new Vector2(x, y)))
                        {
                            GenerateTree(x, y + 1);
                        }
                    }
                }
            }
        }
    }

    public void GenerateNoiseTexture( float frequency, float limit, Texture2D noiseTexture)
    {
        //noiseTexture = new Texture2D(worldSize, worldSize);

        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++ )
            {
                float v = Mathf.PerlinNoise(( x + seed) * frequency, (y + seed) * frequency);
                if (v > limit)
                    noiseTexture.SetPixel(x, y, Color.white);
                else
                    noiseTexture.SetPixel(x, y, Color.black);
               // noiseTexture.SetPixel(x, y, new Color(v, v, v));
            }
        }

        noiseTexture.Apply();
    }
    // Update is called once per frame
   // void Update()
    //{
        
  //  }

    public void GenerateTree(int x, int y)
    {
        placeTile(TileAtlas.ruins.tileSprite, x, y);
        placeTile(TileAtlas.ruins.tileSprite, x, y + 1);
        placeTile(TileAtlas.ruins.tileSprite, x-1, y);
        placeTile(TileAtlas.ruins.tileSprite, x+1, y);
    }

    public void placeTile(Sprite tileSprite, int x, int y)
    {
        GameObject newTile = new GameObject(name = "Tile");
        float chunkCoord = (Mathf.Round(x / chunkSize) * chunkSize);
        chunkCoord /= chunkSize;
        newTile.transform.parent = worldChunks[(int)chunkCoord].transform;

        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        newTile.name = tileSprite.name;
        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

        worldTiles.Add(newTile.transform.position - Vector3.one * 0.5f);
    }

}
