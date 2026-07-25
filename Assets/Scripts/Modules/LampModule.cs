using UnityEngine;

public class LampModule : MonoBehaviour
{
    private Transform lamps;

    void Start()
    {
        lamps = gameObject.transform;
    }

    public void Activate()
    {
        if (lamps == null) return;

        Lamp[] allLamps = lamps.GetComponentsInChildren<Lamp>();

        foreach (Lamp lamp in allLamps)
        {
            lamp.Activate();
        }
    }

    public void Deactivate()
    {
        if (lamps == null) return;

        Lamp[] allLamps = lamps.GetComponentsInChildren<Lamp>();

        foreach (Lamp lamp in allLamps)
        {
            lamp.Deactivate();
        }
    }
}