using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CashRegister : MonoBehaviour
{
    [SerializeField] private DialogueModule dialogueModule;

    private PlayerMovement playerMovement;
    private PlayerInfo playerInfo;

    private bool isPlayerInside = false;
    [HideInInspector] public bool isOtherInside = false;
    [HideInInspector] public NPCMovement nPCMovement;
    private bool isInteracting = false;
    private bool cooldown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            playerInfo = other.GetComponent<PlayerInfo>();
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerMovement = null;
            playerInfo = null;
        }
    }

    private void Update()
    {
        if (!cooldown && isPlayerInside && isOtherInside && !isInteracting && Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                InteractionRoutine();
            }
        }
    }

    private void InteractionRoutine()
    {
        isInteracting = true;

        if (playerMovement != null)
        {
            playerMovement.currentState = PlayerState.Frozen;
        }

        DialogueData targetDialogue = GetDialogueForCurrentDay();

        if (dialogueModule != null && targetDialogue != null)
        {
            dialogueModule.OnDialogueFinished += OnDialogueEnd;
            dialogueModule.StartDialogue(targetDialogue);
        }
        else
        {
            StartCoroutine(ResetInteraction());
        }
    }

    private DialogueData GetDialogueForCurrentDay()
    {
        if (playerInfo == null || nPCMovement == null) return null;

        NPCdialogues npcDialogues = nPCMovement.GetComponent<NPCdialogues>();
        if (npcDialogues == null || npcDialogues.dialogues == null) return null;

        int dayIndex = playerInfo.days;

        if (dayIndex >= 0 && dayIndex < npcDialogues.dialogues.Length)
        {
            return npcDialogues.dialogues[dayIndex].dialogue;
        }

        return null;
    }

    private void OnDialogueEnd()
    {
        if (dialogueModule != null)
        {
            dialogueModule.OnDialogueFinished -= OnDialogueEnd;
        }

        StartCoroutine(ResetInteraction());
    }

    private IEnumerator ResetInteraction()
    {
        cooldown = true;
        isInteracting = false;

        if (playerMovement != null)
        {
            playerMovement.currentState = PlayerState.Free;
        }

        if (nPCMovement != null)
        {
            nPCMovement.ResumeMovement();
        }

        yield return new WaitForSeconds(5f);

        cooldown = false;
    }
}