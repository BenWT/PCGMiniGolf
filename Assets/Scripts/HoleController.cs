using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour {
	[HideInInspector]
	public CourseController course;
	public Rigidbody ball;
	public int par;
	public Vector3 cameraPos;

	// [HideInInspector]
	public bool isActive = false;
	[HideInInspector]
	public int holeScore;

	bool mouseDown = false, canShoot = true;
	Vector2 startPos;

	public void Start() {
		course = transform.parent.GetComponent<CourseController>();
		holeScore = 0;
	}

	public void Update() {
		if (isActive) {
			if (Input.GetMouseButtonDown(0) && canShoot) {
				mouseDown = true;
				startPos = Input.mousePosition;
			} else if (Input.GetMouseButtonUp(0) && mouseDown) {
				mouseDown = false;
				Vector2 deltaM = (Vector2)Input.mousePosition - startPos;

				DoShoot(deltaM.normalized, deltaM.magnitude);
			}
		}
	}

	public void DoShoot(Vector2 direction, float force) {
		Debug.Log(direction.x + " " + direction.y + " " + force);

		// holeScore++;
		ball.AddForce(new Vector3(-direction.x, 0, -direction.y) * force);

		// TODO: check for in hole
	}
}
