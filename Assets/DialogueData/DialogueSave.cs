using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DialogueSave : MonoBehaviour {
	public Response PickedThis;

	public static DialogueSave Singleton;

	public void Start() {
		if (Singleton) {
			GameObject.Destroy(this);
			return;
		} else {
			Singleton = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}
}
