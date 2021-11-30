using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    public DialogueCharacter character;
    public Background background;
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;

    public string[] Dialogue => dialogue;   // getter

    public bool HasResponses => Responses != null && Responses.Length > 0;

    public Response[] Responses => responses;
}
