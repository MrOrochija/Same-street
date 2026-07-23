using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    public DialogueData dialogue;
    public DialogueModule dialogueModule;
    
    private bool isPlayerInside = false;
    private bool isInteracting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInside = false;
    }

    private void Update()
    {
        if (isPlayerInside && !isInteracting && Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                dialogueModule.StartDialogue(dialogue);
            }
        }
    }
}