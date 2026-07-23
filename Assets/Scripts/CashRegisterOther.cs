using UnityEngine;

public class CashRegisterOther : MonoBehaviour
{
    public CashRegister cashRegister;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            cashRegister.isOtherInside = true;
            cashRegister.nPCMovement = other.GetComponent<NPCMovement>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            cashRegister.isOtherInside = true;
            cashRegister.nPCMovement = null;
        }
    }
}
