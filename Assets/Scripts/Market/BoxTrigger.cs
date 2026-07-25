using UnityEngine;
using UnityEngine.InputSystem;

public class BoxTrigger : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private bool isPlayerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerMovement = other.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerMovement = null;
        }
    }

    private void Update()
    {   
        if (isPlayerInside && Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                playerMovement.hasBox = !playerMovement.hasBox;
                Debug.Log(playerMovement.hasBox);
            }
        }
    }
}
