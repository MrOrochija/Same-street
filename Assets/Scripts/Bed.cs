using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Bed : MonoBehaviour
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
        if (playerInfo != null && playerInfo.canSleep && isPlayerInside && !isInteracting && Keyboard.current != null)
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

        Debug.Log($"день: {playerInfo.days}");
        playerInfo.days++;

        yield return StartCoroutine(FadeModule.FadeRoutine(fadeImage, 0f));

        if (playerMovement != null)
        {
            playerMovement.currentState = PlayerState.Free;
        }

        isInteracting = false;
    }
}