using UnityEngine;
using UnityEngine.AI;

public class SoundTrigger : SoundsModule
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerInfo>().GetMusic())
            {
                if (HasSound(1) && other.GetComponent<PlayerInfo>().GetCanSleep())
                {
                    PlayLoopSound(sounds[1], 0.25f);
                    return;
                }
            
                PlayLoopSound(sounds[0], 0.25f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerInfo>().GetMusic()) StopSound();            
        }
    }

    private bool HasSound(int index)
    {
        return sounds != null && index >= 0 && index < sounds.Length && sounds[index] != null;
    }
}