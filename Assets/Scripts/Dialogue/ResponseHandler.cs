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

        foreach (Response response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.responseText;
            responseButton.GetComponent<Button>().onClick.AddListener(call: () => OnPickedResponse(response));

            tempResponseButtons.Add(responseButton);

            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
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

        // add the points to the list for fuzzy logic
        fuzzyLogic.AddToList(response.pointAmount);

      //  PlayerPrefs.SetFloat("beatsIndex", )

        if (response.loadScene != null) {
			SceneManager.LoadScene(response.loadScene);
		}

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
