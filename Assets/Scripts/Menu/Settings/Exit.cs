using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public Canvas settingCanvas;

    void Start()
    {
        Button btn = GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(ButtonClicked);
        }
    }

    void ButtonClicked()
    {
        settingCanvas.gameObject.SetActive(false);
    }
}
