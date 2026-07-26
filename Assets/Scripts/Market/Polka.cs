using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Polka : MonoBehaviour
{
    public Sprite[] fruits;
    
    public SpriteRenderer posSpriteRenderer;
    public SpriteRenderer pos2SpriteRenderer;
    public SpriteRenderer pos3SpriteRenderer;

    private GameObject polka;
    private PlayerMovement playerMovement;
    private bool isPlayerInside = false;
    private NPCMovement nPCMovement;
    private bool isNPCInside = false;

    private Coroutine npcRoutine;

    void Start()
    {
        polka = gameObject.transform.parent.gameObject;
        
        if (posSpriteRenderer == null && polka.transform.Find("pos") != null)
            posSpriteRenderer = polka.transform.Find("pos").GetComponent<SpriteRenderer>();

        if (pos2SpriteRenderer == null && polka.transform.Find("pos2") != null)
            pos2SpriteRenderer = polka.transform.Find("pos2").GetComponent<SpriteRenderer>();

        if (pos3SpriteRenderer == null && polka.transform.Find("pos3") != null)
            pos3SpriteRenderer = polka.transform.Find("pos3").GetComponent<SpriteRenderer>();
            
        ChangeRandomSprites();
    }

    void Update()
    {
        if (Keyboard.current != null && isPlayerInside && (playerMovement == null || playerMovement.hasBox))
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                ChangeRandomSprites();
            }
        }
    }

    private void ChangeRandomSprites()
    {
        if (fruits == null || fruits.Length == 0) return;

        if (posSpriteRenderer != null)
        {
            int randomIndex = Random.Range(0, fruits.Length);
            posSpriteRenderer.sprite = fruits[randomIndex];
        }

        if (pos2SpriteRenderer != null)
        {
            int randomIndex = Random.Range(0, fruits.Length);
            pos2SpriteRenderer.sprite = fruits[randomIndex];
        }

        if (pos3SpriteRenderer != null)
        {
            int randomIndex = Random.Range(0, fruits.Length);
            pos3SpriteRenderer.sprite = fruits[randomIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerMovement = other.GetComponent<PlayerMovement>();
        }
        else if (other.CompareTag("NPC"))
        {
            isNPCInside = true;
            nPCMovement = other.GetComponent<NPCMovement>();
            
            if (npcRoutine != null) StopCoroutine(npcRoutine);
            npcRoutine = StartCoroutine(NPCLogicRoutine());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
        else if (other.CompareTag("NPC"))
        {
            isNPCInside = false;

            if (npcRoutine != null)
            {
                StopCoroutine(npcRoutine);
                npcRoutine = null;
            }
        }
    }

    private IEnumerator NPCLogicRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (isNPCInside)
        {
            while (isNPCInside && !HasAnyFruits())
            {
                yield return null;
            }

            if (isNPCInside && HasAnyFruits())
            {
                ClearAllFruits();
                nPCMovement.SetIsPaused(false);
            }

            yield return null;
        }
    }

    private bool HasAnyFruits()
    {
        bool pos1Has = posSpriteRenderer != null && posSpriteRenderer.sprite != null;
        bool pos2Has = pos2SpriteRenderer != null && pos2SpriteRenderer.sprite != null;
        bool pos3Has = pos3SpriteRenderer != null && pos3SpriteRenderer.sprite != null;

        return pos1Has || pos2Has || pos3Has;
    }

    private void ClearAllFruits()
    {
        if (posSpriteRenderer != null) posSpriteRenderer.sprite = null;
        if (pos2SpriteRenderer != null) pos2SpriteRenderer.sprite = null;
        if (pos3SpriteRenderer != null) pos3SpriteRenderer.sprite = null;
    }
}