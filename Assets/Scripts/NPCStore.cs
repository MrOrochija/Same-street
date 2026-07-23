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

    public GameObject[] NPC;
    private GameObject spawn;

    void Start()
    {
        Transform spawnTransform = transform.Find("Spawn");

        if (spawnTransform != null)
        {
            spawn = spawnTransform.gameObject;
            SpawnRandomNPC();
        }
    }

    public void SpawnRandomNPC()
    {
        if (spawn == null || NPC.Length == 0) return;

        int randomIndex = Random.Range(0, NPC.Length);

        GameObject randomNPC = Instantiate(
            NPC[randomIndex], 
            spawn.transform.position, 
            spawn.transform.rotation, 
            transform
        );

        randomNPC.SetActive(true);

        NPCMovement movement = randomNPC.GetComponent<NPCMovement>();
        if (movement != null)
        {
            movement.Init(allowedGroups, transform);
        }
    }
}