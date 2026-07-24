using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TriggerMarket : MonoBehaviour
{
    public Image fadeImage;
    public GameObject exitPos;
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

            if (!isInteracting)
            {
                playerMovement = null;
            }
        }
    }

    private void Update()
    {
        if (!playerInfo.inStore && isPlayerInside && !isInteracting && Keyboard.current != null)
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

        PlayerMovement localPlayer = playerMovement;

        if (localPlayer != null)
        {
            localPlayer.currentState = PlayerState.Frozen;
        }

        yield return StartCoroutine(FadeModule.FadeRoutine(fadeImage, 1f));

        if (localPlayer != null && exitPos != null)
        {
            localPlayer.gameObject.transform.position = exitPos.transform.position;
        }

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeModule.FadeRoutine(fadeImage, 0f));

        if (localPlayer != null)
        {
            localPlayer.currentState = PlayerState.Free;
        }

        isInteracting = false;
        
        if (!isPlayerInside)
        {
            playerMovement = null;
        }
    }
}