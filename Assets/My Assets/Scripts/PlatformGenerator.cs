using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to handle the platform's generation.
/// Platforms are generated regularly along the Z-axis, but randomly along
/// the X-axis.
/// </summary>

public class PlatformGenerator : MonoBehaviour
{
    static private PlatformGenerator _inst;
    static public PlatformGenerator Inst
    {
        get { return _inst; }
    }

    /// <summary>
    /// Limit value for the generation of the X value -> [-spreadValue, spreadValue[
    /// </summary>
    [SerializeField] float spreadValue;

    /// <summary>
    /// An global object which will be the parent object for all the plaforms
    /// </summary>
    [SerializeField] GameObject spawnedPlatformGlobalObject;

    /// <summary>
    /// A list of all the references to the generated platforms. It will be used
    /// to modify or delete the plaforms.
    /// </summary>
    private List<GameObject> spawnedPlatformsList;


    int iteration = 1;

    // DO NOT TOUCH
    float splitDistance = 2.48f;
    float firstPlatformZ = 3.963429f;
    

    private void Awake()
    {
        _inst = this;
    }


    private void Start()
    {
        spawnedPlatformsList = new List<GameObject>();
        InitialSpawn();
    }


    /// <summary>
    /// We create 10 aligned platforms (X=0) at the beginning of the game, so the
    /// player can't instantly lose when he starts playing.
    /// </summary>
    public void InitialSpawn ()
    {
        for (int i = 0; i < 10; i++)
        {
            SpawnPlatform(spreadSpawn:false, forceSimplePlatform:true);
        }
    }


    /// <summary>
    /// Function handling the next platform generation.
    /// </summary>
    /// <param name="spreadSpawn">Should the generation use the random spread parameter?</param>
    /// <param name="forceSimplePlatform">Should the generated platforms be "simple" one?</param>
    public void SpawnPlatform (bool spreadSpawn = true, bool forceSimplePlatform = false)
    {
        // generating the X-position of the next platform.
        float x;
        if (spreadSpawn) x = Random.Range(-spreadValue, spreadValue);
        else x = 0;

        // random selection of the next platform based on a randomized number
        int randSelec = Random.Range(0, 100);

        GameObject selectedPlatform = LoadPrefabPlatform("Simple platform");

        /* Random selection of the generated platform:
         * 70% probability to generate a    simple platform,
         * 10%                              platform with a levitating prisma,
         * 10*                              spiked platform,
         * 10%                              platform with a grow bonus (grow all platforms)
         */

        if (!forceSimplePlatform)
        {
            if (randSelec > 70 && randSelec < 080) selectedPlatform = LoadPrefabPlatform("Prisma platform");
            if (randSelec > 80 && randSelec < 090) selectedPlatform = LoadPrefabPlatform("Growable platform");
            if (randSelec > 90 && randSelec < 100) selectedPlatform = LoadPrefabPlatform("Spike platform");
        }
        
        GameObject inst = Instantiate(
            selectedPlatform,
            new Vector3(x, 0, firstPlatformZ + iteration * splitDistance),
            Quaternion.identity);

        // We add the generated platform to the list 
        spawnedPlatformsList.Add(inst);
        iteration++;
        // ... and to the main parent object
        inst.transform.parent = spawnedPlatformGlobalObject.transform;
    }


    public void DestroyPlatform ()
    {
        Destroy(spawnedPlatformsList[0]);
        spawnedPlatformsList.RemoveAt(0);
    }


    private GameObject LoadPrefabPlatform (string prefabName)
    {
        return Resources.Load("Prefabs/" + prefabName) as GameObject;
    }


    public void GrowPlaforms (Vector3 growValues)
    {
        foreach (GameObject pf in spawnedPlatformsList)
        {
            pf.transform.localScale = new Vector3 (
                pf.transform.localScale.x + growValues.x,
                pf.transform.localScale.y + growValues.y,
                pf.transform.localScale.z + growValues.z);
        }
    }

    public int platformsAmount()
    {
        return spawnedPlatformsList.Count;
    }

}
