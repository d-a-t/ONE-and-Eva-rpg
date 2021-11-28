using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Response {
	[SerializeField] public string responseText;
	[SerializeField] private DialogueObject[] dialogueObject;
	[SerializeField] public int pointAmount;
	[SerializeField] public string loadScene;

	public string ResponseText => responseText;

	public DialogueObject[] DialogueObject => dialogueObject;
}
