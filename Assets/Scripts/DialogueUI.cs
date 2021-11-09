using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
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
    [SerializeField] private DialogueObject ending;
    [SerializeField] private DialogueObject good_ending;
    [SerializeField] private DialogueObject death;
    public Background ending_I;
    public Background good_ending_I;
    public Background death_I;
    private bool startEnding = false;
    public GameObject textBox;

    private ResponseHandler responseHandler;
    private TypeWriterEffect typeWriterEffect;

    private void Start()
    {
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        CloseDialogueBox();

        ShowBackground(beats[_currentBeatIndex].background);
        ShowCharacter(beats[_currentBeatIndex].character);
        ShowDialogue(beats[_currentBeatIndex].testDialogue);
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true);
        StartCoroutine(routine: StepThroughDialogue(dialogueObject));
    }

    void ShowCharacter(DialogueCharacter character)
    {
        if (character == null)
        {
            portrait.enabled = false;
            return;
        }

        portrait.enabled = true;
        portrait.sprite = character.portrait;
    }

    void ShowBackground(Background bck)
    {
        if (bck == null)
        {
            background.enabled = false;
            return;
        }

        background.enabled = true;
        background.sprite = bck.bckground;
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typeWriterEffect.Run(dialogue, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            if (!startEnding)
                CloseDialogueBox();
            else
                SpecialClose();

            if (_currentBeatIndex < beats.Length - 1)
            {
                _currentBeatIndex++;
                NextSentence(_currentBeatIndex);
            }
            else if (!startEnding)
            {
                startEnding = true;
                // play the ending...
                dialogueBox.SetActive(true);
                portrait.enabled = false;

                int num = fuzzyLogic.GetComponent<FuzzyLogic>().EvaluateFuzzy();
                switch (num)
                {
                    case -1:
                        ShowBackground(death_I);
                        StartCoroutine(routine: StepThroughDialogue(death));
                        break;
                    case 0:
                        ShowBackground(ending_I);
                        StartCoroutine(routine: StepThroughDialogue(ending));
                        break;
                    case 1:
                        ShowBackground(good_ending_I);
                        StartCoroutine(routine: StepThroughDialogue(good_ending));
                        break;
                }
            }
        }
    }

        private void NextSentence(int index)
    {
        ShowBackground(beats[_currentBeatIndex].background);
        ShowCharacter(beats[_currentBeatIndex].character);
        ShowDialogue(beats[_currentBeatIndex].testDialogue);
    }

    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
    private void SpecialClose()
    {
        textBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
