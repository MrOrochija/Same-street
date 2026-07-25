using UnityEngine;

public class DeleteNPC : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            Destroy(other.gameObject);
        }
    }
}
