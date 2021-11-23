using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// A custom class used to handle characters. Can be used for player characters or NPCs.
/// </summary>
/// <remarks>
/// <para>
/// This class can be easily instantiated and destroyed, using the Maid class. Note that this 
/// </para>
/// </remarks>
public class Character : Part, IControllable {
	public Animator Animator;

	[Header("Health")]
	public Variable<float> Health = new Variable<float>(100);
	public float MaxHealth;
	public Slider Healthbar;

	protected Maid ControlMaid = new Maid();
	public virtual void BindPlayerControls() {
	
	}

	public virtual void UnBindControls() {
		ControlMaid.DoCleaning();
	}

	public void TakeDamage(float val) {
		if (val > 0) {
			Health.Value -= val;
		} else {
			Debug.LogWarning("Cannot take negative damage.");
		}
	}

	public override void Awake() {
		base.Awake();
		MaxHealth = Health.Value;

		if (Healthbar) {
			Healthbar.maxValue = Health.Value;
			Healthbar.value = Health.Value;
		}
	}

	public override void Start() {
		if (Healthbar) {
			Maid.GiveTask(
				Health.Connect(
					(float val) => {
						Healthbar.value = val;
						return true;
					}
				)
			);
		}
	}
}