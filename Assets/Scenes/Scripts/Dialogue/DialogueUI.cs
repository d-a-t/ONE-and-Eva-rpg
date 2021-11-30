using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour {
	[SerializeField] private TMP_Text textLabel;
	[SerializeField] private GameObject dialogueBox;

	// sprites
	public Image portrait;
	public Image background;

	public DialogueBeat[] beats;
	int _currentBeatIndex = 0;
	//------

	// for the endings of the game
	[Header("EndingSettings")]
	public GameObject fuzzyLogic;
	[SerializeField] private DialogueObject[] good_ending;
	[SerializeField] private DialogueObject[] bad_ending;
	[SerializeField] private DialogueObject transition;
	private bool startEnding = false;
	private int endingCount = 0;
	private bool isFinished = false;
	public GameObject textBox;

	private ResponseHandler responseHandler;
	private TypeWriterEffect typeWriterEffect;

	private void Start() {
		_currentBeatIndex = PlayerPrefs.GetInt("beatIndex", 0);

		typeWriterEffect = GetComponent<TypeWriterEffect>();
		responseHandler = GetComponent<ResponseHandler>();

		CloseDialogueBox();

		ShowBackground(beats[_currentBeatIndex].Dialogue.background);
		ShowCharacter(beats[_currentBeatIndex].Dialogue.character);
		ShowDialogue(beats[_currentBeatIndex].Dialogue);
	}

	public void ShowDialogue(DialogueObject dialogueObject) {
		dialogueBox.SetActive(true);
		StartCoroutine(routine: StepThroughDialogue(dialogueObject));
	}

	public void ShowCharacter(DialogueCharacter character) {
		if (character == null) {
			portrait.enabled = false;
			return;
		}

		portrait.enabled = true;
		portrait.sprite = character.portrait;
	}

	public void ShowBackground(Background bck) {
		if (bck == null) {
			background.enabled = false;
			return;
		}

		background.enabled = true;
		background.sprite = bck.bckground;
	}

	private IEnumerator StepThroughDialogue(DialogueObject dialogueObject) {
		for (int i = 0; i < dialogueObject.Dialogue.Length; i++) {
			string dialogue = dialogueObject.Dialogue[i];
			yield return typeWriterEffect.Run(dialogue, textLabel);

			if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

			yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
		}

		if (dialogueObject.HasResponses) {
			responseHandler.ShowResponses(dialogueObject.Responses, _currentBeatIndex);
			yield break;    // response handler will take it from here
		} else {
			if (!startEnding)
				CloseDialogueBox();

			if (_currentBeatIndex < beats.Length - 1) {
				_currentBeatIndex++;
				NextSentence(_currentBeatIndex);
			} else if (!startEnding && endingCount == 0) {
				startEnding = true;
				endingCount = 1;

				// play the ending...
				dialogueBox.SetActive(true);
				portrait.enabled = false;

				int num = fuzzyLogic.GetComponent<FuzzyLogic>().EvaluateFuzzy();
				switch (num) {
					case -1:
						StartCoroutine(routine: StepThroughEnd(bad_ending));
						break;
					case 1:
						StartCoroutine(routine: StepThroughEnd(good_ending));
						break;
				}
			}
		}
	}

	private IEnumerator StepThroughEnd(DialogueObject[] dialogueObject) {
		for (int i = 0; i < dialogueObject.Length; i++) {
			ShowCharacter(dialogueObject[i].character);
			for (int k = 0; k < dialogueObject[i].Dialogue.Length; k++) {
				string dialogue = dialogueObject[i].Dialogue[k];
				yield return typeWriterEffect.Run(dialogue, textLabel);

				if (k == dialogueObject[i].Dialogue.Length - 1 && dialogueObject[i].HasResponses) break;

				yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
			}
		}
		SpecialClose();
	}

	int count = 0;
	public void NextSentence(int index) {
		_currentBeatIndex = index;  // should correct when transferring from Responses...

		if (index >= beats.Length) {
			ShowDialogue(transition);
			return;
		}

		ShowBackground(beats[_currentBeatIndex].Dialogue.background);
		ShowCharacter(beats[_currentBeatIndex].Dialogue.character);
		ShowDialogue(beats[_currentBeatIndex].Dialogue);

		count++;
	}

	private void CloseDialogueBox() {
		dialogueBox.SetActive(false);
		textLabel.text = string.Empty;
	}
	private void SpecialClose() {
		textBox.SetActive(false);
		portrait.enabled = false;
		textLabel.text = string.Empty;
        
		if (startEnding) {
				int num = fuzzyLogic.GetComponent<FuzzyLogic>().EvaluateFuzzy();
				switch (num) {
					case -1:
						SceneManager.LoadScene("BossBattle");
						break;
					case 1:
                        PlayerPrefs.SetFloat("Score", PlayerPrefs.GetFloat("Score", 100) + 100);
						SceneManager.LoadScene("WinScreen");
						break;
				}
		}
	}
}
