using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class TriggerMarket : MonoBehaviour
{
    public bool toStore;
    public Image fadeImage;
    public GameObject exitPos;
    public GameObject door;
    public GameObject NPC;
    public GameObject usedNPC;
    public Light2D mainLight;
    public Light2D playerLight;
    public GameObject yaniNeko;
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerInfo playerInfo;
    
    private bool isPlayerInside = false;
    private bool isInteracting = false;

    void Start()
    {
        if (door != null) anim = door.GetComponent<Animator>();
    }

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
        if (playerInfo != null)
        {
            if (toStore && playerInfo.GetCanSleep()) return;

            if (!playerInfo.GetInStore() && isPlayerInside && !isInteracting && Keyboard.current != null)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    StartCoroutine(InteractionRoutine());
                }
            }
        }
    }

    private IEnumerator InteractionRoutine()
    {
        if (anim != null) anim.SetBool("isOpen", true);
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

        if (anim != null) anim.SetBool("isOpen", false);

        yield return StartCoroutine(FadeModule.FadeRoutine(fadeImage, 0f));

        if (!toStore)
        {
            playerInfo.SetInStore(false);
            playerInfo.SetCanSleep(true);

            if (usedNPC != null && NPC != null)
            {
                for (int i = usedNPC.transform.childCount - 1; i >= 0; i--)
                {
                    usedNPC.transform.GetChild(i).SetParent(NPC.transform, false);
                }
            }
            
            int j = playerInfo.GetDays();
            if (j == 0)
            {
                yaniNeko.SetActive(false);
                LightModule.ChangeLight(this, LightTrigger.LightingMode.SetDark, mainLight, playerLight);
            } 
            else if (j == 1)
            {
                yaniNeko.SetActive(false);
                LightModule.ChangeLight(this, LightTrigger.LightingMode.SetDark, mainLight, playerLight);
            }
        } 
        else 
        {
            playerInfo.SetInStore(true);
        }

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