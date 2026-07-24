using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private int days = 0;
    private bool canSleep = false;
    [HideInInspector] public bool inStore = false;

    public int GetDays()
    {
        return days;
    }

    public void AddDay()
    {
        days++;
    }

    public void SetCanSleep(bool value)
    {
        canSleep = value;
    }

    public bool GetCanSleep()
    {
        return canSleep;
    }

    public void SetInStore(bool value)
    {
        inStore = value;
    }

    public bool GetInStore()
    {
        return inStore;
    }
}