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

	public CameraState state = CameraState.Preview;

	[HideInInspector]
	public bool isActive = false;
	[HideInInspector]
	public int score;

	float ballY = 90.0f, maxPower = 200.0f;
	public float ballPower = 50.0f;
	bool canShoot = true;
	Vector2 startPos;

	float x = 0.0f, y = 80.0f;

	public void Start() {
		course = transform.parent.GetComponent<CourseController>();
		score = 0;
	}

	public void Begin() {
		state = CameraState.Preview;
		ball.transform.localPosition = new Vector3(0, 0.12f, 0);
		score = 0;
		x = 0.0f;
		y = 80.0f;
		ballY = 90.0f;
		ballPower = 50.0f;
		maxPower = 200.0f;
		ball.gameObject.SetActive(true);
	}

	IEnumerator ExitPreview() {
		state = CameraState.Transition;
		float time = 0;

		Vector3 startPos = Camera.main.GetComponent<Transform>().position;
		Quaternion startRot = Camera.main.GetComponent<Transform>().rotation;

		Vector3 targetPos = ball.transform.position + new Vector3(-3 * Mathf.Sin(ballY * Mathf.Deg2Rad), 2, -3 * Mathf.Cos(ballY * Mathf.Deg2Rad));
		Quaternion targetRot = Quaternion.LookRotation(ball.transform.position - targetPos);

		while (time < 0.5f) {
			time += Time.deltaTime;

			Camera.main.GetComponent<Transform>().position = Vector3.Lerp(startPos, targetPos, time * 2);
			Camera.main.GetComponent<Transform>().rotation = Quaternion.Lerp(startRot, targetRot, time * 2);

			yield return null;
		}

		state = CameraState.Game;
	}

	public void Update() {
		if (isActive) {
			if (state == CameraState.Preview) {
				x += Input.GetAxis("Mouse X") * 5f;
				y -= Input.GetAxis("Mouse Y") * 5f;
				y = ClampAngle(y, 15, 80);

				Camera.main.GetComponent<Transform>().position = Quaternion.Euler(y,x,0) * new Vector3(0,0,-10) + (transform.position + cameraPos);
				Camera.main.GetComponent<Transform>().LookAt(transform.position + cameraPos);

				if (Input.GetMouseButtonDown(0)) {
					StartCoroutine(ExitPreview());
				}
			} else if (state == CameraState.Game) {
				if (Input.GetKey(KeyCode.LeftArrow)) ballY -= 55 * Time.deltaTime;
				if (Input.GetKey(KeyCode.RightArrow)) ballY += 55 * Time.deltaTime;

				if (Input.GetKey(KeyCode.UpArrow)) ballPower += 75 * Time.deltaTime;
				if (Input.GetKey(KeyCode.DownArrow)) ballPower -= 75 * Time.deltaTime;

				ballPower = Mathf.Clamp(ballPower, 0.0f, maxPower);

				Camera.main.GetComponent<Transform>().position = ball.transform.position + new Vector3(-3 * Mathf.Sin(ballY * Mathf.Deg2Rad), 2, -3 * Mathf.Cos(ballY * Mathf.Deg2Rad));
				Camera.main.GetComponent<Transform>().rotation = Quaternion.LookRotation(ball.transform.position - Camera.main.GetComponent<Transform>().position);

				if (Input.GetKeyDown(KeyCode.Space)) {
					if (ball.GetComponent<Rigidbody>().velocity.magnitude < 0.75f) {
						DoShoot(ball.transform.position - Camera.main.GetComponent<Transform>().position, ballPower);
					}
				}
			} else if (state == CameraState.Post) {
				if (Input.GetMouseButtonDown(0)) {
					course.FinishHole();
				}
			}
		}
	}

	public void InHole() {
		ball.position = new Vector3(holeObject.position.x, holeObject.position.y, holeObject.position.z);
		ball.velocity = Vector3.zero;
		ball.angularVelocity = Vector3.zero;
		ball.transform.localPosition = new Vector3(0, 0.12f, 0);
		ball.gameObject.SetActive(false);

		state = CameraState.Post;
	}

	public void DoShoot(Vector3 direction, float force) {
		score++;
		ball.AddForce(new Vector3(direction.x, 0, direction.z) * force);
	}

	float ClampAngle(float angle, float min, float max) {
		if (angle < -360.0f) angle += 360.0f;
		if (angle > 360.0f) angle -= 360.0f;
		return Mathf.Clamp(angle, min, max);
	}
}

public enum CameraState {
	Preview,
	Transition,
	Game,
	Post
}
