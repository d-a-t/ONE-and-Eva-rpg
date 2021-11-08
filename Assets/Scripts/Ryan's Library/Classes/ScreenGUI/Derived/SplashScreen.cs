using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {
	public Image Publisher;
	public Image Studios;

	public float EaseInTime = .5F;
	public float StayTime = 1F;
	public float EaseOutTime = .5F;

	public int NextSceneIndex = 1;

	private Maid Maid = new Maid();

	public void Awake() {
		Publisher.color = new Color(Publisher.color.r, Publisher.color.g, Publisher.color.b, 0);
		Studios.color = new Color(Publisher.color.r, Publisher.color.g, Publisher.color.b, 0);
	}

	public void Start() {
		float counter = 0;
		Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, .5F, delegate {
			Runservice.RunFor(Global.RunservicePriority.Heartbeat.Physics, EaseInTime, (float dt) => {
				counter += dt;
				Publisher.color = new Color(Publisher.color.r, Publisher.color.g, Publisher.color.b, counter / EaseInTime);
				return true;
			});
		});

		Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, .5F + EaseInTime + StayTime, delegate {
			counter = 0;
			Runservice.RunFor(Global.RunservicePriority.Heartbeat.Physics, EaseOutTime, (float dt) => {
				counter += dt;
				Publisher.color = new Color(Publisher.color.r, Publisher.color.g, Publisher.color.b, 1 - (counter * Mathf.Pow(EaseOutTime, -1)));
				return true;
			});

			Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, EaseOutTime, delegate {
				SoundController.PlayClipAtPoint("Startup", Camera.main.transform.position);
				Runservice.RunFor(Global.RunservicePriority.Heartbeat.Physics, EaseInTime, (float dt) => {
					counter += dt;
					Studios.color = new Color(Studios.color.r, Studios.color.g, Studios.color.b, counter / EaseInTime);
					return true;
				});

				Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, EaseInTime + StayTime, delegate {
					counter = 0;
					Runservice.RunFor(Global.RunservicePriority.Heartbeat.Physics, EaseOutTime, (float dt) => {
						counter += dt;
						Studios.color = new Color(Studios.color.r, Studios.color.g, Studios.color.b, 1 - (counter * Mathf.Pow(EaseOutTime, -1)));
						return true;
					});
					Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, EaseOutTime, delegate {
						SceneManager.LoadScene(NextSceneIndex);
					});
				});
			});
		});
	}
}
