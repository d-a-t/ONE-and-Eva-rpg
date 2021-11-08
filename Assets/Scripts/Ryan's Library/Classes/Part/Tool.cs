using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A class that defines an item that which the player can equip and use with unique functions, as compared to a regular item. Ex: Minecraft pickaxe vs apple.
/// </summary>
public class Tool : Part, IControllable {
	[Header("Character Info")]
	/// <summary>
	/// Defines who's controlling the tool.
	/// </summary>
	public Transform Controller;
	public Animator Animator;

	/// <summary>
	/// Defines if the tool is equipped.
	/// </summary>
	//public Variable<bool> Equipped = new Variable<bool>(false);

	/// <summary>
	/// Defines if the tool can be equipped.
	/// </summary>
	//public bool CanEquip = true;

	/// <summary>
	/// Defines if the tool can be unequipped.
	/// </summary>
	//public bool CanUnequip = true;

	/*
	public virtual bool Equip() {
		if (!CanEquip) {
			return false;
		}
		Controller.Value = Holder.Value;
		Equipped.Value = true;
		this.Visible.Value = true;

		BindControls();

		return true;
	}
	

	/// <summary>
	/// Unequips the tool and makes it invisible.
	/// </summary>
	public virtual bool Unequip() {
		if (!CanUnequip) {
			return false;
		}
		Controller.Value = null;
		Equipped.Value = false;
		this.Visible.Value = false;

		UnbindControls();

		return true;
	}
	*/

	public Maid ControlMaid = new Maid();

	public virtual void BindPlayerControls() { }

	public virtual void UnBindControls() { }

	public override void Awake() {
		base.Awake();
		//Controller.Val
	}
}
