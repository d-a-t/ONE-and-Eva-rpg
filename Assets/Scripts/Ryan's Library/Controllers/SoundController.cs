using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[Serializable] public class AudioClipDict : SerializableDictionary<string, AudioClip> { }
[Serializable] public class AudioSourceDict : SerializableDictionary<string, AudioSource> { }

/// <summary>
/// Controls sound settings.
/// </summary>
public sealed class SoundController : Singleton {
	public static SoundController Singleton;

	public AudioMixer GlobalAudioMixer;

	public AudioSource BackgroundMusic;
	public AudioClipDict Sounds;
	public AudioSourceDict Musics;
	private Dictionary<string, AudioSource> soundsList = new Dictionary<string, AudioSource>();

	public Variable<float> MasterVolume = new Variable<float>(1);
	public Variable<float> MusicVolume = new Variable<float>(1);
	public Variable<float> EffectVolume = new Variable<float>(1);

	public static AudioSource PlayClipAtPoint(string clipName, Vector3 pos) {
		AudioSource temp = SoundController.Singleton.soundsList.ContainsKey(clipName) ? SoundController.Singleton.soundsList[clipName].PlayClipAtPoint(pos) : null;
		if (temp) {
			temp.volume = SoundController.Singleton.EffectVolume.Value;
			temp.outputAudioMixerGroup = SoundController.Singleton.GlobalAudioMixer.FindMatchingGroups("Effects")[0];
		}
		return temp;
	}

	public void Awake() {
		if (!Singleton) {
			Singleton = this;
		}
		// if audio settings haven't been changed
		if (!PlayerPrefs.HasKey("MasterVolume")) {
			// set audio to 100%
			PlayerPrefs.SetFloat("MasterVolume", 1);
		}
		if (!PlayerPrefs.HasKey("MusicVolume")) {
			// set audio to 100%
			PlayerPrefs.SetFloat("MusicVolume", 1);
		}
		if (!PlayerPrefs.HasKey("EffectVolume")) {
			// set audio to 100%
			PlayerPrefs.SetFloat("EffectVolume", 1);
		}

		foreach (KeyValuePair<string,AudioClip> val in Sounds) {
			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = val.Value;
			soundsList[val.Key] = source;
		}
	}

	public void Start() {
		Listener<float> masterVolumeUpdate = MasterVolume.Connect((float val) => {
			AudioListener.volume = val;
			PlayerPrefs.SetFloat("MasterVolume", val);
			return true;
		});
		masterVolumeUpdate.Name = "masterVolumeUpdate";
		Maid.GiveTask(masterVolumeUpdate);

		MasterVolume.Value = PlayerPrefs.GetFloat("MasterVolume");

		Listener<float> musicVolumeUpdate = MusicVolume.Connect((float val) => {
			if (BackgroundMusic) {
				BackgroundMusic.volume = val;
			}
			PlayerPrefs.SetFloat("MusicVolume", val);
			return true;
		});
		musicVolumeUpdate.Name = "musicVolumeUpdate";
		Maid.GiveTask(musicVolumeUpdate);
		MusicVolume.Call();

		Listener<float> effectVolumeUpdate = EffectVolume.Connect((float val) => {
			PlayerPrefs.SetFloat("EffectVolume", val);
			return true;
		});
		effectVolumeUpdate.Name = "effectVolumeUpdate";
		Maid.GiveTask(effectVolumeUpdate);

		EffectVolume.Value = PlayerPrefs.GetFloat("EffectVolume");
	}
}
