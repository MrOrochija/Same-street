using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class YaniNeko : MonoBehaviour
{
    public Image fadeImage;
    private PlayerMovement playerMovement;
    
    private bool isPlayerInside = false;
    private bool isInteracting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            
            isPlayerInside = true;
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

        yield return StartCoroutine(FadeModule.FadeRoutine(fadeImage, 1f));

        

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeModule.FadeRoutine(fadeImage, 0f));

        if (playerMovement != null)
        {
            playerMovement.currentState = PlayerState.Free;
        }

        isInteracting = false;
    }
}