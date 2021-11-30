using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// A class that attaches itself to a empty gameObject. Makes it easy to toggle if it should render.
/// </summary>
public class Part : MonoBehaviour, IDisposable {
	public Maid Maid = new Maid();

	[Header("Positional")]
	/// <summary>
	/// Defines the 2D model which contains the sprites. Useful for a collage of sprites acting as one.
	/// </summary>
	public Transform Model;
	public CFrame CFrame {
		get { return gameObject.transform.GetCFrame(); }
		set { gameObject.transform.UpdateFromCFrame(value); }
	}

	public Collider2D Collider;
	public Rigidbody2D Rigidbody;

	[Header("Rendering")]
	private bool _FlipX = false;
	private bool _FlipY = false;
	public bool FlipX {
		get { return _FlipX; }
		set {
			if (_FlipX != value) {
				_FlipX = value;
				FlipSpriteX(value);
			}
		}
	}
	

	public bool FlipY {
		get { return _FlipY; }
		set {
			if (_FlipX != value) {
				_FlipY = value;
				FlipSpriteY(value);
			}
		}
	}

	private void FlipSpriteX(bool val) {
		foreach (Part v in Children) {
			if (v) {
				v.FlipX = val;
			}
		}
		foreach (SpriteRenderer v in Renders) {
			if (v) {
				v.flipX = val;
			}
		}
	}

	private void FlipSpriteY(bool val) {
		foreach (Part v in Children) {
			if (v) {
				v.FlipY = val;
			}
		}
		foreach (SpriteRenderer v in Renders) {
			if (v) {
				v.flipY = val;
			}
		}
	}

	private List<SpriteRenderer> Renders = new List<SpriteRenderer>();
	private List<Collider2D> Colliders = new List<Collider2D>();
	private List<Part> Children = new List<Part>();


	/// <summary>
	/// Defines if the model should be rendered.
	/// </summary>
	public Variable<bool> Visible = new Variable<bool>(true);

	[Header("Physics")]
	public Variable<int> PhysicsLayer = new Variable<int>(8);

	public static Part Create(CFrame cframe) {
		GameObject temp = new GameObject();
		temp.transform.UpdateFromCFrame(cframe);
		temp.SetActive(false);

		Part self = temp.AddComponent<Part>();
		GameObject model = new GameObject();
		model.transform.parent = temp.transform;
		self.Model = model.transform;
		temp.SetActive(true);

		

		return self;
	}

	public virtual void Awake() {
		Maid.GiveTask(gameObject);
		PhysicsLayer.Value = gameObject.layer;

		if (Model == null) {
			Transform child = (new GameObject("Model")).transform;
			child.parent = this.gameObject.transform;
			Model = child;
			Model.localPosition = new Vector3();
		}

		foreach (Part v in Model.GetComponentsInChildren(typeof(Part))) {
			Children.Add(v);
		}

		Renders.Add(GetComponent<SpriteRenderer>());
		foreach (SpriteRenderer v in Model.GetComponentsInChildren(typeof(SpriteRenderer))) {
			if (Children.Count > 0) {
				foreach (Part b in Children) {
					if (!v.gameObject.transform.IsChildOf(b.gameObject.transform)) {
						Renders.Add(v);
					}
				}
			} else {
				Renders.Add(v);
			}
		}

		foreach (Collider2D v in Model.GetComponentsInChildren(typeof(Collider2D))) {
			if (Children.Count > 0) {
				foreach (Part b in Children) {
					if (!v.gameObject.transform.IsChildOf(b.gameObject.transform)) {
						Colliders.Add(v);
					}
				}
			} else {
				Colliders.Add(v);
			}
		}
	}

	// Start is called before the first frame update
	public virtual void Start() {
		//Changes if the part should be rendered depending on the Visible value.
		Listener<bool> visibleChange = Visible.Connect((bool val) => {
			foreach (Part v in Children) {
				if (v) {
					v.Visible.Value = val;
				}
			}
			foreach (SpriteRenderer v in Renders) {
				if (v) {
					v.enabled = val;
				}
			}
			return true;
		});
		visibleChange.Name = "visibleChange";
		Maid.GiveTask(visibleChange);
		Visible.Call();

		//Changes if the part should be rendered depending on the Visible value.
		Listener<int> PhysicsLayerChange = PhysicsLayer.Connect((int val) => {
			gameObject.layer = val;
			foreach (Part v in Children) {
				v.PhysicsLayer.Value = val;
			}
			foreach (Collider2D v in Colliders) {
				v.gameObject.layer = val;
			}
			return true;
		});
		PhysicsLayerChange.Name = "PhysicsLayerChange";
		Maid.GiveTask(PhysicsLayerChange);
		PhysicsLayer.Call();
	}

	public virtual void Dispose() {
		Maid.Dispose();
		if (this && this.gameObject) {
			GameObject.Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy() {
		Maid.Dispose();
	}
}
