using UnityEngine;

public class InStoreTrigger : MonoBehaviour
{
    private PlayerInfo playerInfo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInfo = other.GetComponent<PlayerInfo>();
            playerInfo.inStore = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInfo.inStore = false;
            playerInfo = null;
        }
    }
}
