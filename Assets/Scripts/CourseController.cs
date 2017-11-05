using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseController : MonoBehaviour {
	public GamestateController gamestate;
	public List<HoleController> holes = new List<HoleController>();
	public int currentHole;

	public int courseScore;

	public void Begin(GamestateController gamestate) {
		this.gamestate = gamestate;
		currentHole = 0;
		courseScore = 0;
		PositionHole();
	}

	void PositionHole() {
		Camera.main.GetComponent<Transform>().position = holes[currentHole].cameraPos;
		holes[currentHole].isActive = true;
	}

	void FinishHole() {
		holes[currentHole].isActive = false;
		currentHole++;

		if (currentHole >= holes.Count) {
			// last hole
		} else {
			PositionHole();
		}
	}
}
