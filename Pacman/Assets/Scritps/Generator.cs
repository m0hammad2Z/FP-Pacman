using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject dotPrefab, powerPalletPrefab, speedBoostPrefab, throughWallsPrefab; // the dot prefab to spawn
    public float distanceBetweenDots = 1.0f; // the distance between each dot
    public float palletGenerateRate = 3;    // The rate of pallets to generate
    public float speedBoostDotstGenerateRate = 1.5f;    // The rate of speedBoostDots to generate
    public float throughWallsDotstGenerateRate = 1.5f;    // The rate of Through Walls Dots to generate
    public LayerMask wallLayer; // the layer for the walls
    public Transform surface;   // The surface where the dots will be generated

    public List<GameObject> gameObjects = new List<GameObject>();
    bool isGenerated;

    private void Update()
    {
        if ((PacmanManager.isDie || PacmanManager.isWin) && isGenerated)
        {
            if (gameObjects.Count > 0)
                foreach (GameObject go in gameObjects)
                {
                    Destroy(go);
                }
            gameObjects.Clear();
            isGenerated = false;
        }
        else if (!PacmanManager.isDie && gameObjects.Count <= 0 && !isGenerated)
        {
            Generate();
        }

        if (PacmanManager.isDie || PacmanManager.isWin)
        {
            isGenerated = false;
        }
        
        if(PacmanManager.score >= gameObjects.Count)
        {
            PacmanManager.isWin = true;
        }
    }

    void Generate()
    {
        isGenerated = true;


        // Get the mesh renderer component of the GameObject
        MeshRenderer renderer = surface.GetComponent<MeshRenderer>();

        // Calculate the bounds of the GameObject in world space
        Bounds bounds = renderer.bounds;

        // Get the minimum and maximum bounds of the GameObject in world space along the x-axis
        float minX = bounds.min.x;
        float maxX = bounds.max.x;

        // Get the minimum and maximum bounds of the GameObject in world space along the z-axis
        float minZ = bounds.min.z;
        float maxZ = bounds.max.z;


        minX = Mathf.Min(minX, maxX);    // Determine 
        minZ = Mathf.Min(minZ, maxZ);    // the min
        maxX = Mathf.Max(minX, maxX);    // and
        maxZ = Mathf.Max(minZ, maxZ);    // the max


        var rand = 0.0f;
        var xHeight = bounds.size.x / distanceBetweenDots;  // Calculate the height of x axis
        var zHeight = bounds.size.z / distanceBetweenDots;  // Calculate the height of z axis

        //Normalize the rate
        float normalizedPalletGenerateRate = palletGenerateRate * ((xHeight * zHeight) / 100);
        float normalizedspeedBoostGenerateRate = speedBoostDotstGenerateRate * ((xHeight * zHeight) / 100) + normalizedPalletGenerateRate;
        float normalizedthroughWallsGenerateRate = throughWallsDotstGenerateRate * ((xHeight * zHeight) / 100) + normalizedspeedBoostGenerateRate;

        print(normalizedPalletGenerateRate);
        print(normalizedspeedBoostGenerateRate);
        print(normalizedthroughWallsGenerateRate);

        for (float x = minX; x <= maxX; x += distanceBetweenDots)
        {
            for (float z = minZ; z <= maxZ; z += distanceBetweenDots)
            {
                // Guess a number between 1 and the size of the surface
                rand = Random.Range(1, xHeight * zHeight - 1);

                if (!Physics.CheckSphere(new Vector3(x, 0.5f, z), 0.5f, wallLayer))
                {
                    if (rand <= normalizedPalletGenerateRate)
                    {
                        GameObject o = Instantiate(powerPalletPrefab, new Vector3(x, 0.8f, z), Quaternion.identity);
                        o.transform.SetParent(transform);
                        gameObjects.Add(o);
                    }
                    else if (rand <= normalizedspeedBoostGenerateRate)
                    {
                        GameObject o = Instantiate(speedBoostPrefab, new Vector3(x, 0.8f, z), Quaternion.identity);
                        o.transform.SetParent(transform);
                        gameObjects.Add(o);
                    }else if(rand <= normalizedthroughWallsGenerateRate)
                    {
                        GameObject o = Instantiate(throughWallsPrefab, new Vector3(x, 0.8f, z), Quaternion.identity);
                        o.transform.SetParent(transform);
                        gameObjects.Add(o);
                    }
                    else
                    {
                        GameObject o = Instantiate(dotPrefab, new Vector3(x, 0.3f, z), Quaternion.identity);
                        o.transform.SetParent(transform);
                        gameObjects.Add(o);
                    }
                }
            }
        }
    }

}

