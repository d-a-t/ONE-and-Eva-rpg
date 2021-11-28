using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DialogueOptionScene : MonoBehaviour {
	public int Test;
	public static void LoadTakeOutRobotScene() {
		SceneManager.LoadScene("BossBattle");
	}

}
