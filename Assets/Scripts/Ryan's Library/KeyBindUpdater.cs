using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Controls {
	UP, DOWN, LEFT, RIGHT, ATTACK, SHOOT, RUN
}

public class KeyBindUpdater : MonoBehaviour {
	public Text UP;
	public Text DOWN;
	public Text LEFT;
	public Text RIGHT;
	public Text ATTACK;
	public Text SHOOT;
	public Text RUN;

	public void UpdateKeybind(string control) {
		UpdateKeybind((Controls)System.Enum.Parse(typeof(Controls), control));
	}

	public void UpdateKeybind(Controls control) {
		InputController.LastKey.Connect((KeyCode val) => {
			switch (control) {
				case Controls.UP: {
						UP.text = val.ToString();
						PlayerPrefs.SetString("UP", val.ToString());
						break;
					}
				case Controls.DOWN: {
						DOWN.text = val.ToString();
						PlayerPrefs.SetString("DOWN", val.ToString());
						break;
					}
				case Controls.LEFT: {
						LEFT.text = val.ToString();
						PlayerPrefs.SetString("LEFT", val.ToString());
						break;
					}
				case Controls.RIGHT: {
						RIGHT.text = val.ToString();
						PlayerPrefs.SetString("RIGHT", val.ToString());
						break;
					}
				case Controls.ATTACK: {
						ATTACK.text = val.ToString();
						PlayerPrefs.SetString("ATTACK", val.ToString());
						break;
					}
				case Controls.SHOOT: {
						SHOOT.text = val.ToString();
						PlayerPrefs.SetString("SHOOT", val.ToString());
						break;
					}
				case Controls.RUN: {
						RUN.text = val.ToString();
						PlayerPrefs.SetString("RUN", val.ToString());
						break;
					}
			}
			return false;
		});
	}

	// Start is called before the first frame update
	public void Start() {
		if (!PlayerPrefs.HasKey("UP")) {
			PlayerPrefs.SetString("UP", "W");
		}
		if (!PlayerPrefs.HasKey("DOWN")) {
			PlayerPrefs.SetString("DOWN", "S");
		}
		if (!PlayerPrefs.HasKey("LEFT")) {
			PlayerPrefs.SetString("LEFT", "A");
		}
		if (!PlayerPrefs.HasKey("RIGHT")) {
			PlayerPrefs.SetString("RIGHT", "D");
		}
		if (!PlayerPrefs.HasKey("SHOOT")) {
			PlayerPrefs.SetString("SHOOT", "Mouse0");
		}
		if (!PlayerPrefs.HasKey("ATTACK")) {
			PlayerPrefs.SetString("ATTACK", "Space");
		}
		if (!PlayerPrefs.HasKey("RUN")) {
			PlayerPrefs.SetString("RUN", "LeftShift");
		}

		UP.text = PlayerPrefs.GetString("UP", "W");
		LEFT.text = PlayerPrefs.GetString("LEFT", "A");
		DOWN.text = PlayerPrefs.GetString("DOWN", "S");
		RIGHT.text = PlayerPrefs.GetString("RIGHT", "D");

		ATTACK.text = PlayerPrefs.GetString("ATTACK", "Space");
		SHOOT.text = PlayerPrefs.GetString("SHOOT", "Mouse0");
		RUN.text = PlayerPrefs.GetString("RUN", "LeftShift");
	}


}
