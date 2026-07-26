using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

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
    public LampModule lampModule;
    public GameObject market;
    public GameObject house;
    private Sprite marketSprite;
    private Sprite houseSprite;
    public Sprite oldMarket;
    public Sprite oldHouse;
    public GameObject trigger;
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerInfo playerInfo;
    
    private bool isPlayerInside = false;
    private bool isInteracting = false;

    void Start()
    {
        if (door != null) anim = door.GetComponent<Animator>();

        if (market != null)
        {
            SpriteRenderer sr = market.GetComponent<SpriteRenderer>();
            if (sr != null) marketSprite = sr.sprite;
        }

        if (house != null)
        {
            SpriteRenderer sr = house.GetComponent<SpriteRenderer>();
            if (sr != null) houseSprite = sr.sprite;
        }
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
        if (playerInfo != null && playerMovement != null)
        {
            if (toStore && playerInfo.GetCanSleep() && playerMovement.hasBox) return;

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

        if (!toStore)
        {
            playerInfo.SetInStore(false);
            playerInfo.SetCanSleep(true);
            
            int j = playerInfo.GetDays();
            if (j == 0)
            {
                yaniNeko.SetActive(false);
                LightModule.ChangeLight(this, LightTrigger.LightingMode.SetDark, mainLight, playerLight);
                lampModule.Activate();
            } 
            else if (j == 1)
            {
                yaniNeko.SetActive(false);
                LightModule.ChangeLight(this, LightTrigger.LightingMode.SetDark, mainLight, playerLight);
                lampModule.Activate();
            }
            else if (j == 2)
            {
                yaniNeko.SetActive(false);
                LightModule.ChangeLight(this, LightTrigger.LightingMode.SetDark, mainLight, playerLight);
                lampModule.Activate();

                Transform mellTransform = usedNPC.transform.Find("Mell");

                if (mellTransform != null) Destroy(mellTransform.gameObject);
            }
            else if (j == 3)
            {
                yaniNeko.SetActive(false);
                LightModule.ChangeLight(this, LightTrigger.LightingMode.SetDark, mainLight, playerLight);
                lampModule.Activate();

                Transform yuiTransform = usedNPC.transform.Find("NPCgirl2");

                if (yuiTransform != null) Destroy(yuiTransform.gameObject);
            }
            else if (j == 4)
            {
                yaniNeko.SetActive(false);
                LightModule.ChangeLight(this, LightTrigger.LightingMode.SetDark, mainLight, playerLight);
                lampModule.Activate();
                
                Transform tomTransform = usedNPC.transform.Find("Tom");

                if (tomTransform != null) Destroy(tomTransform.gameObject);
            }
            else if (j == 5)
            {
                yaniNeko.SetActive(false);
                LightModule.ChangeLight(this, LightTrigger.LightingMode.SetDark, mainLight, playerLight);
                lampModule.Activate();

                if (market != null && oldMarket != null)
                {
                    Destroy(market.GetComponent<Animator>());
                    SpriteRenderer sr = market.GetComponent<SpriteRenderer>();
                    if (sr != null) sr.sprite = oldMarket;
                }
                if (house != null && oldHouse != null)
                {
                    SpriteRenderer sr = house.GetComponent<SpriteRenderer>();
                    if (sr != null) sr.sprite = oldHouse;
                }

                if (trigger != null) trigger.SetActive(true);
            }

            if (usedNPC != null && NPC != null)
            {
                for (int i = usedNPC.transform.childCount - 1; i >= 0; i--)
                {
                    usedNPC.transform.GetChild(i).SetParent(NPC.transform, false);
                }
            }
        } 
        else 
        {
            playerInfo.SetInStore(true);
        }

        if (localPlayer != null && exitPos != null)
        {
            localPlayer.gameObject.transform.position = exitPos.transform.position;
        }

        yield return new WaitForSeconds(1f);

        if (anim != null) anim.SetBool("isOpen", false);

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