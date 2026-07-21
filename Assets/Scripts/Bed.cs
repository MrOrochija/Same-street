using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Image fadeImage;
    private PlayerInfo playerInfo;
    private PlayerMovement playerMovement;
    
    private bool isPlayerInside = false;
    private bool isInteracting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInfo = other.GetComponent<PlayerInfo>();
            playerMovement = other.GetComponent<PlayerMovement>();
            
            if (playerInfo != null)
            {
                isPlayerInside = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerInfo = null;
            playerMovement = null;
        }
    }

    private void Update()
    {
        if (isPlayerInside && !isInteracting && Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame && playerInfo != null)
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

        yield return StartCoroutine(Fade.FadeRoutine(fadeImage, 1f));

        yield return new WaitForSeconds(1f);

        playerInfo.days++;
        Debug.Log($"день: {playerInfo.days}");

        yield return StartCoroutine(Fade.FadeRoutine(fadeImage, 0f));

        if (playerMovement != null && playerMovement.currentState != PlayerState.Combat)
        {
            playerMovement.currentState = PlayerState.Free;
        }

        isInteracting = false;
    }
}