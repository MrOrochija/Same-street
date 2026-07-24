using UnityEngine;

public class DeleteNPC : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            Debug.Log(1);
            Destroy(other.gameObject);
        }
    }
}
