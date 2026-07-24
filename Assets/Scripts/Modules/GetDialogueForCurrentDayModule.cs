using UnityEngine;

public class GetDialogueForCurrentDayModule : MonoBehaviour
{
    public static DialogueData GetDialogue(PlayerInfo playerInfo, GameObject NPC)
    {
        if (playerInfo == null || NPC == null) return null;

        NPCdialogues npcDialogues = NPC.GetComponent<NPCdialogues>();
        if (npcDialogues == null || npcDialogues.dialogues == null) return null;

        int dayIndex = playerInfo.GetDays();

        if (dayIndex >= 0 && dayIndex < npcDialogues.dialogues.Length)
        {
            return npcDialogues.dialogues[dayIndex].dialogue;
        }

        return null;
    }
}