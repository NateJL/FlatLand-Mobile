using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public GameManager manager;
    [Space(10)]
    [Header("NPC Data")]
    [ShowOnly]public int activeNpcs = 0;
    public int maxNpcCount = 50;
    public GameObject NpcPrefab;
    public List<GameObject> npcCollection;

    /// <summary>
    /// Function to be called by the GameManager to initialize the npc manager.
    /// </summary>
    public void Initialize()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        npcCollection = new List<GameObject>();
    }

    /// <summary>
    /// Function called to spawn Npcs, will spawn the amount given as parameter (staying below max).
    /// </summary>
    public void SpawnNpc(int numberToSpawn)
    {
        for(int i = 0; i < numberToSpawn; i++)
        {
            if (activeNpcs >= maxNpcCount)
                return;

            // TODO: get npc from pool
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
