using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance;

    [HideInInspector] public int days = 0;
    [HideInInspector] public bool canSleep = false;
    [HideInInspector] public bool inStore = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}