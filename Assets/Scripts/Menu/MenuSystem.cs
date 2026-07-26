using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MenuSystem : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMovement;
    private Canvas guideCanvas;
    private Canvas menuCanvas;
    public TMP_Text text;
    private Canvas backgroundCanvas;

    private bool playPressed = false;
    private bool isOpen = false;

    void Start()
    {
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null) playerMovement.currentState = PlayerState.Frozen;
        }

        Transform guideCanvasTransform = transform.Find("Guide");
        Transform menuCanvasTransform = transform.Find("Menu");
        Transform settingCanvasTransform = transform.Find("Settings");
        Transform backgroundCanvasTransform = transform.Find("Background");

        if (guideCanvasTransform != null) guideCanvas = guideCanvasTransform.GetComponent<Canvas>();
        if (backgroundCanvasTransform != null) backgroundCanvas = backgroundCanvasTransform.GetComponent<Canvas>();

        if (menuCanvasTransform != null)
        {
            menuCanvas = menuCanvasTransform.GetComponent<Canvas>();

            Button[] buttons = menuCanvasTransform.GetComponentsInChildren<Button>(true);
            foreach (Button btn in buttons)
            {
                if (btn.name == "Play")
                {
                    btn.onClick.AddListener(OnPlayButtonClicked);
                }
                else if (btn.name == "Quit")
                {
                    btn.onClick.AddListener(OnQuitButtonClicked);
                }
            }
        }

        SetMenuVisible(true);
    }

    void Update()
    {
        if (Keyboard.current != null && playPressed)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                SetMenuVisible(!isOpen);
            }
            
        }
    }

    public void OnPlayButtonClicked()
    {
        playPressed = true;
        text.text = "Resume";
        SetMenuVisible(false);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void SetMenuVisible(bool value)
    {
        isOpen = value;
        if (menuCanvas != null) menuCanvas.gameObject.SetActive(value);
        if (backgroundCanvas != null) backgroundCanvas.gameObject.SetActive(value);

        if (playerMovement != null)
        {
            if (!value) playerMovement.currentState = PlayerState.Free;
            else playerMovement.currentState = PlayerState.Frozen;
        }
    }
}