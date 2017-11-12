using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour {
	[HideInInspector]
	public CourseController course;
	public Rigidbody ball;
	public Transform holeObject;
	public int par;
	public Vector3 cameraPos;

	// [HideInInspector]
	public bool isActive = false;
	[HideInInspector]
	public int score;

	bool mouseDown = false, canShoot = true;
	Vector2 startPos;

	public void Start() {
		course = transform.parent.GetComponent<CourseController>();
		score = 0;
	}

	public void Update() {
		if (isActive && course.gamestate.state != Gamestate.Score) {
			if (Input.GetMouseButtonDown(0) && canShoot) {
				mouseDown = true;
				startPos = Input.mousePosition;
			} else if (Input.GetMouseButtonUp(0) && mouseDown) {
				mouseDown = false;
				Vector2 deltaM = (Vector2)Input.mousePosition - startPos;

				DoShoot(deltaM.normalized, deltaM.magnitude);
			}
		}

		// TODO: check for in hole
		// TODO: toggle canshoot if the ball is moving
	}

	public void InHole() {
		ball.position = new Vector3(holeObject.position.x, ball.position.y, holeObject.position.z);
		ball.velocity = Vector3.zero;
		ball.angularVelocity = Vector3.zero;

		course.FinishHole();
	}

	public void DoShoot(Vector2 direction, float force) {
		score++;
		ball.AddForce(new Vector3(-direction.x, 0, -direction.y) * force);
	}
}
