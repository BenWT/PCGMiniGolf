using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseController : MonoBehaviour {
	public GamestateController gamestate;
	public List<HoleController> holes = new List<HoleController>();
	public List<int> randomHoleOrder = new List<int>();
	public int currentHole;
	public bool showScore = true;

	public void Begin(GamestateController gamestate, bool doRandom) {
		this.gamestate = gamestate;
		currentHole = 0;
		RandomiseHoleOrder(doRandom);

		foreach (HoleController h in holes) {
			h.score = 0;
		}

		StartCoroutine(PositionHole());
	}

	IEnumerator PositionHole() {
		float time = 0;

		Vector3 startPos = Camera.main.GetComponent<Transform>().position;
		Quaternion startRot = Camera.main.GetComponent<Transform>().rotation;

		Vector3 targetPos = Quaternion.Euler(80, 0, 0) * new Vector3(0, 0, -10) + (holes[randomHoleOrder[currentHole]].transform.position + holes[randomHoleOrder[currentHole]].cameraPos);
		Quaternion targetRot = Quaternion.Euler(80,0,0);

		while (time < 1.0f) {
			time += Time.deltaTime;

			Camera.main.GetComponent<Transform>().position = Vector3.Slerp(startPos, targetPos, time);
			Camera.main.GetComponent<Transform>().rotation = Quaternion.Slerp(startRot, targetRot, time);

			yield return null;
		}

		holes[randomHoleOrder[currentHole]].isActive = true;
		holes[randomHoleOrder[currentHole]].Begin();
	}

	public IEnumerator ReturnToMenu() {
		float time = 0;

		gamestate.ChangeState(Gamestate.Transition);

		Vector3 startPos = Camera.main.GetComponent<Transform>().position;
		Quaternion startRot = Camera.main.GetComponent<Transform>().rotation;

		Vector3 targetPos = new Vector3(35, 65, -20);
		Quaternion targetRot = Quaternion.Euler(90, 0, 0);

		while (time < 1.0f) {
			time += Time.deltaTime;

			Camera.main.GetComponent<Transform>().position = Vector3.Slerp(startPos, targetPos, time);
			Camera.main.GetComponent<Transform>().rotation = Quaternion.Slerp(startRot, targetRot, time);

			yield return null;
		}

		gamestate.ChangeState(Gamestate.Menu);
	}

	public void FinishHole() {
		holes[randomHoleOrder[currentHole]].isActive = false;
		currentHole = currentHole + 1;

		if (currentHole >= holes.Count) {
			if (showScore) {
				gamestate.ChangeState(Gamestate.GameScore);
			} else {
				StartCoroutine(ReturnToMenu());
			}
		} else {
			StartCoroutine(PositionHole());
		}
	}

	// Lottery Problem, mentioned by Oliver
	public void RandomiseHoleOrder(bool doRandom) {
		randomHoleOrder.Clear();
		for (int i = 0; i < holes.Count; i++) randomHoleOrder.Add(i);

		if (doRandom) {
			for (int i = 0; i < holes.Count; i++) {
				int n = Random.Range(0, randomHoleOrder.Count - i);

				int item = randomHoleOrder[n];
				randomHoleOrder.RemoveAt(n);
				randomHoleOrder.Add(item);
			}
		}
	}
}
