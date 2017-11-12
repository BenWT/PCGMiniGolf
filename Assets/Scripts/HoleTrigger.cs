using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrigger : MonoBehaviour {
	public HoleController hole;

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Ball") {
			hole.InHole();
		}
	}
}
