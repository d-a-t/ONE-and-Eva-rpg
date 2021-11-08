using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : Part {
	private int ignoreLayer;
	private int layerHit;
	private Listener<float> scanUpdate;

	public bool Enabled = true;

	public float ScanLength = .1F;

	public Variable<Collider2D> Hit = new Variable<Collider2D>(null, true);

	private void OnTriggerStay2D(Collider2D objHit) {
		if (Enabled) {
			Hit.Value = objHit;
		}
	}
	private void OnTriggerEnter2D(Collider2D objHit) {
		if (Enabled) {
			Hit.Value = objHit;
		}
	}
	private void OnTriggerExit2D(Collider2D objHit) {
		if (Enabled) {
			if (objHit == Hit.Value) {
				Hit.Value = null;
			}
		}
	}

    public static Hitbox Create(Vector2 size, CFrame cframe, int layer) {
		Part temp = Part.Create(cframe);
		temp.gameObject.SetActive(false);
		temp.gameObject.layer = layer;
		temp.PhysicsLayer.Value = layer;

		Hitbox self = temp.gameObject.AddComponent<Hitbox>();

		BoxCollider2D collider = temp.gameObject.AddComponent<BoxCollider2D>();
		collider.size = size;
		collider.isTrigger = true;

		self.Collider = collider;

		temp.gameObject.SetActive(true);

		self.CFrame = cframe;

		return self;
    }
}
