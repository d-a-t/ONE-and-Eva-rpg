using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneOnDeath : MonoBehaviour
{
	private Character parent;
	public string SceneLoadOnDeath;
	void Start()
    {
		parent = gameObject.GetComponent<Character>();
		parent.Health.Connect((float val) => {
			if (val <= 0) {
				if (GameObject.FindObjectsOfType<NPCAgent>().Length == 1) {
					SceneManager.LoadScene(SceneLoadOnDeath);
					PlayerPrefs.SetInt("BeatLevel", 1);

					return false;
				}
			}
			return true;
		});
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
