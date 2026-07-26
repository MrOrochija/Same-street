using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private int days = 0;
    private bool canSleep = false;
    private bool inStore = false;
    private bool music = true;

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

    public void SetMusic(bool value)
    {
        music = value;
    }

    public bool GetMusic()
    {
        return music;
    }
}