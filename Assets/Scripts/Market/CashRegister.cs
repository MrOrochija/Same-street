using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CashRegister : SoundsModule
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
        if (Keyboard.current == null && cooldown) return;

        if (playerMovement != null && !playerMovement.hasBox && isPlayerInside && isOtherInside && !isInteracting)
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

        DialogueData targetDialogue = GetDialogueForCurrentDayModule.GetDialogue(playerInfo, nPCMovement.gameObject);

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

    private void OnDialogueEnd()
    {
        if (dialogueModule != null)
        {
            dialogueModule.OnDialogueFinished -= OnDialogueEnd;
        }

        PlaySound(sounds[0]);

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