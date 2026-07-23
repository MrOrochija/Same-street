using System;
using UnityEngine;

[Serializable]
public class Dialogues
{
    public DialogueData dialogue; 
}

public class NPCdialogues : MonoBehaviour
{
    public Dialogues[] dialogues;
}