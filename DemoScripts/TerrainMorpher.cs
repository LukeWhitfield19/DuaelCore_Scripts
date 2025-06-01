using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TerrainMorpher : MonoBehaviour
{
    public Terrain terrain;
    private TerrainData terrainData;
    public int height =256;
    public int width =256;
    
    public float depth = 0;

    private float xOffset;
    private float yOffset;

    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private float strength = 100;

    private float[,] heights;

    public GameObject model;
    public GameObject leftArm;
    public GameObject rightLeg;
    public Vector3 lastTransXPo = Vector3.zero;
    public Vector3 newTransXpo = Vector3.zero;

    public Vector3 legPo = Vector3.zero;
    public Vector3 newLegPo = Vector3.zero;
    public Vector3 armPo = Vector3.zero;
    public Vector3 newArmPo = Vector3.zero;

    void Awake()
    {
      terrainData = terrain.terrainData; 
      heights = terrainData.GetHeights(0,0, terrainData.heightmapResolution, terrainData.heightmapResolution);
      lastTransXPo = model.transform.position;
      legPo = rightLeg.transform.position;
      armPo = leftArm.transform.position;
    }

    void Start()
    {
        /*This is here to generate moving terrain*/
        terrainData = generateTerrain();
        xOffset = UnityEngine.Random.Range(0f, 9999f);
        yOffset = UnityEngine.Random.Range(0f, 9999f);  
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Return))
       {
            /*These are here if generating terrain on click*/
            //xOffset = UnityEngine.Random.Range(0f, 9999f);
            //yOffset = UnityEngine.Random.Range(0f, 9999f);
            terrainData = generateTerrain();
            xOffset += Time.deltaTime * speed;
            //yOffset += Time.deltaTime * speed; //this is needed for the moving terrain
       }       
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlattenAll();
        }
 
        heights = terrainData.GetHeights(0,0, terrainData.heightmapResolution, terrainData.heightmapResolution);
    }

    TerrainData generateTerrain()
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3 (width, depth, height);
        terrainData.SetHeights(0,0, MorphTerrainRandom());
        return terrainData;
    }

    private float[,] MorphTerrainRandom()
    {
                
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCo = (float)x / height * depth + xOffset;
                float yCo = (float)y / width * depth + yOffset;
                heights[x,y] = Mathf.PerlinNoise(xCo,yCo);
            }
        }

        return heights;
    }
    
    public void moveTerrainWithModel()
    {
        newTransXpo = model.transform.position;
        newArmPo = leftArm.transform.position;
        newLegPo = rightLeg.transform.position;

        Vector3 tempBody = newTransXpo - lastTransXPo;
        Vector3 tempArm = newArmPo - armPo;
        Vector3 tempLeg = newLegPo - legPo;

        xOffset += Time.deltaTime  * tempBody.x  * 1;
        yOffset -= Time.deltaTime * tempBody.z * 1;
        depth += Time.deltaTime * tempArm.x * UnityEngine.Random.Range(4,5);
        depth = Mathf.Clamp(depth, 0, 20);
        terrainData = generateTerrain();

        xOffset += Time.deltaTime  * tempArm.x  * 1;
        yOffset += Time.deltaTime * tempArm.z * 1;
        terrainData = generateTerrain();

        xOffset += Time.deltaTime  * tempLeg.x  * 1;
        yOffset -= Time.deltaTime * tempLeg.z * 1;
        depth -= Time.deltaTime * tempLeg.z * UnityEngine.Random.Range(4,5);
        depth = Mathf.Clamp(depth, 0, 20);
        terrainData = generateTerrain();
        
    }

    private void FlattenAll()
    {
        int heightmapResolution = terrainData.heightmapResolution;
        for (int z = 0; z < heightmapResolution; z++)
        {
            for (int x = 0; x < heightmapResolution; x++)
            {
                heights[x,z] = 0.0f;
            }
        }

        terrainData.SetHeights(0,0, heights);
    }

    public void deformInFront(Vector3 point)
    {
        heights = terrainData.GetHeights(0,0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        int pointX = (int)(point.x / terrainData.size.x * width);
        int pointZ = (int)(point.z / terrainData.size.z * height);
        float[,] newHeights = new float[1,1];
        float y = heights[pointX, pointZ];
        y += strength; //* Time.deltaTime;
        if (y > terrainData.size.y)
        {
            y = terrainData.size.y;
        }
        newHeights[0,0] = y;
        heights[pointX, pointZ] = y;
        terrainData.SetHeights(pointX, pointZ, newHeights);

    }

    /*private void MorphTerrainUniform()
    {
        int heightmapResolution = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0,0, heightmapResolution, heightmapResolution);

        for (int z = 0; z < heightmapResolution; z++)
        {
            for (int x = 0; x < heightmapResolution; x++)
            {
                float cos = Mathf.Cos(x);
                float sin = -Mathf.Sin(z);
                heights[x,z] = (cos - sin) / 250;
            }
        }

        terrainData.SetHeights(0,0, heights);
    }*/
}
