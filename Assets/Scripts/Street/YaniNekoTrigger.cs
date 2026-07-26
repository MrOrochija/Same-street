using UnityEngine;

public class YaniNekoTrigger : MonoBehaviour
{
    public GameObject pos;
    public GameObject yaniNeko;
    public DialogueModule dialogueModule;
    public DialogueData dialogueData;
    private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();

            playerMovement.currentState = PlayerState.Frozen;

            if (pos != null && yaniNeko != null)
            {
                yaniNeko.transform.parent.gameObject.SetActive(true);
                yaniNeko.transform.position = pos.transform.position;
            }

            if (dialogueModule != null && dialogueData != null)
            {
                dialogueModule.OnDialogueFinished += OnDialogueEnd;
                dialogueModule.StartDialogue(dialogueData);
            }
        }
    }

    private void OnDialogueEnd()
    {
        yaniNeko.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);

        playerMovement.currentState = PlayerState.Free;
    }
}
