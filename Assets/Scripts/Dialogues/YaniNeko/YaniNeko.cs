using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class YaniNeko : MonoBehaviour
{
    [SerializeField] private DialogueModule dialogueModule;
    private PlayerMovement playerMovement;
    private PlayerInfo playerInfo;
    
    private bool isPlayerInside = false;
    private bool isInteracting = false;

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
        if (isPlayerInside && !isInteracting && Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartCoroutine(InteractionRoutine());
            }
        }
    }

    private IEnumerator InteractionRoutine()
    {
        isInteracting = true;

        if (playerMovement != null)
        {
            playerMovement.currentState = PlayerState.Frozen;
        }

        DialogueData targetDialogue = GetDialogueForCurrentDayModule.GetDialogue(playerInfo, gameObject);

        if (dialogueModule != null && targetDialogue != null)
        {
            dialogueModule.OnDialogueFinished += OnDialogueEnd;
            dialogueModule.StartDialogue(targetDialogue);
        }

        yield break;
    }

    private void OnDialogueEnd()
    {
        if (dialogueModule != null)
        {
            dialogueModule.OnDialogueFinished -= OnDialogueEnd;
        }

        if (playerMovement != null)
        {
            playerMovement.currentState = PlayerState.Free;
        }

        isInteracting = false;
    }
}