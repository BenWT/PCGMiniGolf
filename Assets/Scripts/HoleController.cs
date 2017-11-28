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

	[HideInInspector]
	public bool isInPreview = true;
	[HideInInspector]
	public bool isActive = false;
	[HideInInspector]
	public int score;

	float ballY = 180.0f, ballPower = 50.0f, maxPower = 250.0f;
	bool canShoot = true;
	Vector2 startPos;

	float x = 0.0f, y = 80.0f;

	public void Start() {
		course = transform.parent.GetComponent<CourseController>();
		score = 0;
	}

	public void Update() {
		if (isActive) {
			if (isInPreview) {
				// allow rotate here
				x += Input.GetAxis("Mouse X") * 5f;
            	y -= Input.GetAxis("Mouse Y") * 5f;
				y = ClampAngle(y, -20, 80);

				Camera.main.GetComponent<Transform>().position = Quaternion.Euler(y,x,0) * new Vector3(0,0,-10) + transform.position;
				Camera.main.GetComponent<Transform>().LookAt(transform.position);

				if (Input.GetMouseButtonDown(0)) {
					isInPreview = false;

					// TODO: transition camera to ball
					Camera.main.GetComponent<Transform>().position = ball.transform.position + new Vector3(-3 * Mathf.Sin(ballY * Mathf.Deg2Rad), 2, -3 * Mathf.Cos(ballY * Mathf.Deg2Rad));
					Camera.main.GetComponent<Transform>().eulerAngles = new Vector3(30, ballY, 0);
				}
			} else {
				if (Input.GetKey(KeyCode.LeftArrow)) ballY -= 55 * Time.deltaTime;
				if (Input.GetKey(KeyCode.RightArrow)) ballY += 55 * Time.deltaTime;

				if (Input.GetKey(KeyCode.UpArrow)) ballPower += 75 * Time.deltaTime;
				if (Input.GetKey(KeyCode.DownArrow)) ballPower -= 75 * Time.deltaTime;

				ballPower = Mathf.Clamp(ballPower, 0.0f, maxPower);

				if (Input.GetKeyDown(KeyCode.Space)) {
					DoShoot(ball.transform.position - Camera.main.GetComponent<Transform>().position, ballPower);
				}

				Camera.main.GetComponent<Transform>().position = ball.transform.position + new Vector3(-3 * Mathf.Sin(ballY * Mathf.Deg2Rad), 2, -3 * Mathf.Cos(ballY * Mathf.Deg2Rad));
				Camera.main.GetComponent<Transform>().LookAt(ball.transform.position);
			}
		}

		// TODO: toggle canshoot if the ball is moving
	}

	public void InHole() {
		ball.position = new Vector3(holeObject.position.x, ball.position.y, holeObject.position.z);
		ball.velocity = Vector3.zero;
		ball.angularVelocity = Vector3.zero;

		course.FinishHole();
	}

	public void DoShoot(Vector3 direction, float force) {
		score++;
		ball.AddForce(new Vector3(direction.x, 0, direction.z) * force);
	}

	void OnGUI() {
		if (isActive && !isInPreview) GUILayout.Label(ballPower.ToString());
	}

	float ClampAngle(float angle, float min, float max) {
		if (angle < -360.0f) angle += 360.0f;
		if (angle > 360.0f) angle -= 360.0f;
		return Mathf.Clamp(angle, min, max);
	}
}
