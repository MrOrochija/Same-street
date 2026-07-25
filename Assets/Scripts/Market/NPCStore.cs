using UnityEngine;
using System.Collections.Generic;

public class NPCStore : MonoBehaviour
{
    [HideInInspector] public int i = 0;

    [System.Serializable]
    public class NumberData
    {
        public int number;
        public bool wait;
        public bool isPaused;
    }

    [System.Serializable]
    public class NumberGroup
    {
        public List<NumberData> numberList;
    }

    public List<NumberGroup> allowedGroups;

    private GameObject NPCS;
    private GameObject usedNPC;
    private GameObject spawn;

    public PlayerInfo playerInfo;

    public float spawnDelay = 3f;
    private float spawnTimer = 0f;
    private bool isWaitingToSpawn = false;

    private GameObject currentSpawnedNPC;
    private GameObject currentTemplateNPC;

    void Start()
    {
        Transform spawnTransform = transform.Find("Spawn");
        Transform usedNPCTransform = transform.Find("Used");
        Transform NPCSTransform = transform.Find("NPC");

        if (spawnTransform != null) spawn = spawnTransform.gameObject;
        if (usedNPCTransform != null) usedNPC = usedNPCTransform.gameObject;
        if (NPCSTransform != null) NPCS = NPCSTransform.gameObject;
    }

    void Update()
    {
        if (playerInfo != null && playerInfo.GetInStore())
        {
            if (currentSpawnedNPC == null)
            {
                if (currentTemplateNPC != null)
                {
                    currentTemplateNPC.transform.SetParent(usedNPC.transform);
                    currentTemplateNPC = null;
                }

                if (!isWaitingToSpawn)
                {
                    isWaitingToSpawn = true;
                    spawnTimer = spawnDelay;
                }
                else
                {
                    spawnTimer -= Time.deltaTime;

                    if (spawnTimer <= 0)
                    {
                        SpawnRandomNPC();
                        isWaitingToSpawn = false;
                    }
                }
            }
        }
    }

    public void SpawnRandomNPC()
    {
        if (spawn == null || NPCS == null || usedNPC == null) return;

        int availableNPCs = NPCS.transform.childCount;

        if (availableNPCs == 0)
        {
            playerInfo.SetInStore(false);
            return;
        }

        int randomIndex = Random.Range(0, availableNPCs);
        
        currentTemplateNPC = NPCS.transform.GetChild(randomIndex).gameObject;

        currentSpawnedNPC = Instantiate(
            currentTemplateNPC, 
            spawn.transform.position, 
            spawn.transform.rotation, 
            transform
        );

        currentSpawnedNPC.SetActive(true);

        NPCMovement movement = currentSpawnedNPC.GetComponent<NPCMovement>();
        if (movement != null)
        {
            movement.Init(allowedGroups, transform);
        }
    }
}