using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lamp : MonoBehaviour
{
    public Sprite lampOff;
    public Sprite lampOn;

    private SpriteRenderer spriteRenderer;
    private Light2D lampLight;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Transform squareTransform = transform.Find("Square");
        if (squareTransform != null)
        {
            Transform lightTransform = squareTransform.Find("Light");
            if (lightTransform != null) 
            {
                lampLight = lightTransform.GetComponent<Light2D>();
            }
        }
    }

    public void Activate()
    {
        if (spriteRenderer != null && lampOn != null) 
            spriteRenderer.sprite = lampOn;

        if (lampLight != null) 
            lampLight.intensity = 1f;
    }

    public void Deactivate()
    {
        if (spriteRenderer != null && lampOff != null) 
            spriteRenderer.sprite = lampOff;

        if (lampLight != null) 
            lampLight.intensity = 0f;
    }
}