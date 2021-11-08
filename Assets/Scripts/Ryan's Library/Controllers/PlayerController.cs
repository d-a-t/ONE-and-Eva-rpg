using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// This takes a Character and binds controls onto them.
/// </summary>
public sealed class PlayerController : Singleton {
	public static PlayerController Singleton;
	public Variable<int> Score = new Variable<int>();
	public Character PlayerCharacter;

	public int TotalScore;

	public void Awake() {
		if (Singleton == null) {
			Singleton = this;
		}
	}

	public void Start() {
		if (PlayerCharacter) {
			PlayerCharacter.BindPlayerControls();

			PlayerCharacter.Health.Connect((float val) => {
				PlayerPrefs.SetFloat("Score", PlayerCharacter.Health.Value);

				if (val <= 0) {
					SceneManager.LoadScene("GameOver");
				}

				return (val > 0);
			});
		}
	}
}
