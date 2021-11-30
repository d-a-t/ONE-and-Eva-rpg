using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueCharacter.Asset", menuName = "Dialogue/Character")]
public class DialogueCharacter : ScriptableObject
{
    public Sprite portrait;
    //public Sprite nameTag;
    public Color32 color;
}
