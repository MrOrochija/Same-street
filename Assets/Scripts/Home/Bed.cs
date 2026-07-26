using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Bed : MonoBehaviour
{
    public Image fadeImage;
    public Light2D mainLight;
    public Light2D playerLight;
    public GameObject yaniNeko;
    public LampModule lampModule;
    public DialogueModule dialogueModule;
    public DialogueData dialogueData;
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
        if (playerInfo != null && playerInfo.GetCanSleep() && isPlayerInside && !isInteracting && Keyboard.current != null)
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

        LightModule.ChangeLight(this, LightTrigger.LightingMode.SetSunny, mainLight, playerLight);
        lampModule.Deactivate();

        yield return new WaitForSeconds(1f);

        if (playerInfo.GetDays() == 5)
        {
            dialogueModule.OnDialogueFinished += OnDialogueEnd;
            dialogueModule.StartDialogue(dialogueData);
        }
        else
        {
            playerInfo.SetCanSleep(false);
            if (yaniNeko != null && playerInfo.GetDays() != 4) yaniNeko.SetActive(true);
            playerInfo.AddDay();
            Debug.Log($"день: {playerInfo.GetDays()}");

            yield return StartCoroutine(FadeModule.FadeRoutine(fadeImage, 0f));

            if (playerMovement != null)
            {
                playerMovement.currentState = PlayerState.Free;
            }

            isInteracting = false;
        }
    }

    private void OnDialogueEnd()
    {
        Application.Quit();
    }
}