using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;
    private TypeWriterEffect typeWriterEffect;

    private DialogueUI dialogueUI;
    public FuzzyLogic fuzzyLogic;
    private int beatIndex = 0;

    private List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start()
    {
		dialogueUI = GetComponent<DialogueUI>();
        typeWriterEffect = GetComponent<TypeWriterEffect>();
    }

    public void ShowResponses(Response[] responses, int BI)
    {
        beatIndex = BI;
        float responseBoxHeight = 0;

        if (beatIndex != 0 && beatIndex == PlayerPrefs.GetInt("beatIndex", 0) && PlayerPrefs.GetInt("BeatLevel", 0) == 1) {
			ClearThroughResponse(DialogueSave.Singleton.PickedThis);
            PlayerPrefs.SetInt("beatIndex", 0);
			PlayerPrefs.SetInt("BeatLevel", 0);
			return;
		}

		int index = 0;
		foreach (Response response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.responseText;
            responseButton.GetComponent<Button>().onClick.AddListener(call: () => OnPickedResponse(response));
            responseButton.GetComponent<Button>().onClick.AddListener(call: () => {
				DialogueSave.Singleton.PickedThis = response;
			});

            tempResponseButtons.Add(responseButton);

			responseBoxHeight += responseButtonTemplate.sizeDelta.y;
			index += 1;
		}

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, y: responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response)
    {
        responseBox.gameObject.SetActive(false);

		foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();

        //Loading scene
        if (response?.loadScene?.Length > 0) {
            PlayerPrefs.SetInt("beatIndex", beatIndex);
            SceneManager.LoadScene(response.loadScene);
		}

        // add the points to the list for fuzzy logic
        fuzzyLogic.AddToList(response.pointAmount);
       
        StartCoroutine(routine: ResponseStepThrough(response.DialogueObject));
    }

    public void ClearThroughResponse(Response response) {
        responseBox.gameObject.SetActive(false);

		foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();

        // add the points to the list for fuzzy logic
        fuzzyLogic.AddToList(response.pointAmount);
		StartCoroutine(routine: ResponseStepThrough(response.DialogueObject));
    }

    private IEnumerator ResponseStepThrough(DialogueObject[] dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Length; i++)
        {
            dialogueUI.ShowCharacter(dialogueObject[i].character);
            for (int k = 0; k < dialogueObject[i].Dialogue.Length; k++)
            {
                string dialogue = dialogueObject[i].Dialogue[k];
                yield return typeWriterEffect.Run(dialogue, textLabel);

                if (k == dialogueObject[i].Dialogue.Length - 1 && dialogueObject[i].HasResponses) break;

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
        }

        // continue next beat in arrays
        beatIndex++;
        dialogueUI.NextSentence(beatIndex);
    }
}
