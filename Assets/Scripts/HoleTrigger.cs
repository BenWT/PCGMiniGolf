using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrigger : MonoBehaviour {
	public HoleController hole;

	void Update() {
		if (!hole) {
			hole = transform.parent.GetComponent<HoleController>();
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Ball") {
			hole.InHole();
		}
	}
}
