using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour {
	public Text Header;
	public Text Body;

	[SerializeField] private CanvasGroup CanvasGroup;

	public void Hide() {
		CanvasGroup.alpha = 0;
	}

	public void Show() {
		CanvasGroup.alpha = 1;
	}
}
