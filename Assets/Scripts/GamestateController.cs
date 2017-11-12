using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamestateController : MonoBehaviour {
	public Gamestate state = Gamestate.Menu;
	public Transform courseObject;
	public GameObject[] coursePrefabs;

	public void ChangeState(Gamestate state) {
		this.state = state;

		if (this.state == Gamestate.Menu) {
			if (courseObject) Destroy(courseObject.gameObject);
			// TODO: reset camera
		} else if (this.state == Gamestate.Practice) {
			// TODO: load practice course
		} else if (this.state == Gamestate.Game1) {
			if (courseObject) Destroy(courseObject.gameObject);
			GameObject course = Instantiate(coursePrefabs[0], Vector3.zero, Quaternion.identity) as GameObject;
			courseObject = course.GetComponent<Transform>();

			course.GetComponent<CourseController>().Begin(this);
		} else if (this.state == Gamestate.Game2) {
			// TODO: Load procedural course
		}
	}

	void OnGUI() {
		if (this.state == Gamestate.Menu) {
			if (GUILayout.Button("Practice")) {
				ChangeState(Gamestate.Practice);
			}
			if (GUILayout.Button("Game 1")) {
				ChangeState(Gamestate.Game1);
			}
			if (GUILayout.Button("Game 2")) {
				ChangeState(Gamestate.Game2);
			}
		}
	}
}

[System.Serializable]
public enum Gamestate {
	Menu,
	Practice,
	Game1,
	Game2,
	Score
}
