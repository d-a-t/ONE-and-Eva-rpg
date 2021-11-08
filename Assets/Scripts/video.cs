using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class video : MonoBehaviour {

	private float counter = 0;
	void Update() {
		counter += Time.deltaTime;
		if (counter > 10) {
			SceneManager.LoadScene("TitleScreen");
		}
	}
}
