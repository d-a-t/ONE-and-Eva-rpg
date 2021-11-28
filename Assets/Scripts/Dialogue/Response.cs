using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Response
{
    [SerializeField] public string responseText;
    [SerializeField] private DialogueObject[] dialogueObject;
    [SerializeField] public int pointAmount;

    public string ResponseText => responseText;

    public DialogueObject[] DialogueObject => dialogueObject;
}
