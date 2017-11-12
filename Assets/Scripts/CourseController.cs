using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseController : MonoBehaviour {
	public GamestateController gamestate;
	public List<HoleController> holes = new List<HoleController>();
	public int currentHole;

	public void Begin(GamestateController gamestate) {
		this.gamestate = gamestate;
		currentHole = 0;
		PositionHole();
	}

	void PositionHole() {
		Camera.main.GetComponent<Transform>().position = holes[currentHole].transform.position + holes[currentHole].cameraPos;
		holes[currentHole].isActive = true;
	}

	public void FinishHole() {
		holes[currentHole].isActive = false;
		currentHole = currentHole + 1;

		Debug.Log("finish hole");

		if (currentHole >= holes.Count) {
			gamestate.state = Gamestate.Score;
		} else {
			PositionHole();
		}
	}

	void OnGUI() {
		if (gamestate.state == Gamestate.Score) {
			int scoreTotal = 0, parTotal = 0;

			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();

			GUILayout.Label("Hole", GUILayout.ExpandWidth(true));
			for (int i = 0; i < holes.Count; i++) {
				GUILayout.Label((i + 1).ToString(), GUILayout.ExpandWidth(true));
			}
			GUILayout.Space(50);
			GUILayout.Label("Total", GUILayout.ExpandWidth(true));

			GUILayout.EndVertical();
			GUILayout.BeginVertical();

			GUILayout.Label("Score", GUILayout.ExpandWidth(true));
			for (int i = 0; i < holes.Count; i++) {
				GUILayout.Label(holes[i].score.ToString(), GUILayout.ExpandWidth(true));
				scoreTotal+= holes[i].score;
			}
			GUILayout.Space(50);
			GUILayout.Label(scoreTotal.ToString(), GUILayout.ExpandWidth(true));

			GUILayout.EndVertical();
			GUILayout.BeginVertical();

			GUILayout.Label("Par", GUILayout.ExpandWidth(true));
			for (int i = 0; i < holes.Count; i++) {
				GUILayout.Label(holes[i].par.ToString(), GUILayout.ExpandWidth(true));
				parTotal+= holes[i].par;
			}
			GUILayout.Space(50);
			GUILayout.Label(parTotal.ToString(), GUILayout.ExpandWidth(true));

			GUILayout.EndVertical();
			GUILayout.EndHorizontal();

			if (GUILayout.Button("Menu")) {
				gamestate.ChangeState(Gamestate.Menu);
			}
		}
	}
}
