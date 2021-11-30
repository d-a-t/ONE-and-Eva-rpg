using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class KeybindManager : MonoBehaviour
{
    private static KeybindManager instance;

    private GameObject[] keybindButtons;

    public static KeybindManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }

            return instance;
        }
    }

    public Dictionary<string, KeyCode> Keybinds { get; private set; }

    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    private string bindName;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    // use for initialization
    private void Start()
    {
        // menu
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");

        Keybinds = new Dictionary<string, KeyCode>();

        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("UP", PlayerPrefs.GetString("UP", "W"));
        BindKey("LEFT", PlayerPrefs.GetString("LEFT", "A"));
        BindKey("DOWN", PlayerPrefs.GetString("DOWN", "S"));
        BindKey("RIGHT", PlayerPrefs.GetString("RIGHT", "D"));

        BindKey("ATTACK", PlayerPrefs.GetString("ATTACK", "Space"));
        BindKey("SHOOT", PlayerPrefs.GetString("DOWN", "Mouse0"));

        BindKey("RUN", PlayerPrefs.GetString("RUN", "LeftShift"));
    }

    public void BindKey(string key, string keyBind) {
		BindKey(key, InputController.GetKeyCode(keyBind));
	}

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = Keybinds;
		Debug.Log(keyBind);
		if (key.Contains("ACT"))
        {
            currentDictionary = ActionBinds;
        }
        if (!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key, keyBind);
            MyInstance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            currentDictionary[myKey] = KeyCode.None;
            MyInstance.UpdateKeyText(key, KeyCode.None);
        }

        currentDictionary[key] = keyBind;
        MyInstance.UpdateKeyText(key, keyBind);
		PlayerPrefs.SetString(key, keyBind.ToString());
		bindName = string.Empty;
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();

        // should update player movement here too?
    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
