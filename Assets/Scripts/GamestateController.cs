using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamestateController : MonoBehaviour {
	public Gamestate state = Gamestate.Menu;
	public CourseController mainCourse;
	public CourseController practiceCourse;

	public List<Text> scoreboardText = new List<Text>();

	public GameObject menuPanel;
	public GameObject scoreboardPanel;
	public Text ingameScore;
	public Button practiceButton;
	public Button studyButton;
	public Button returnButton;

	void Start() {
		practiceButton.onClick.AddListener(Event_LoadPractice);
		studyButton.onClick.AddListener(Event_LoadStudy);
		returnButton.onClick.AddListener(Event_ReturnToMenu);


				menuPanel.gameObject.SetActive((this.state == Gamestate.Menu));
				scoreboardPanel.gameObject.SetActive((this.state == Gamestate.GameScore));
	}

	public void ChangeState(Gamestate state) {
		this.state = state;

		menuPanel.gameObject.SetActive((this.state == Gamestate.Menu));
		scoreboardPanel.gameObject.SetActive((this.state == Gamestate.GameScore));

		if (this.state == Gamestate.Menu) {
			Camera.main.GetComponent<Transform>().position = new Vector3(35, 65, -20);
			Camera.main.GetComponent<Transform>().rotation = Quaternion.Euler(90, 0, 0);
		} else if (this.state == Gamestate.Practice) {
			practiceCourse.Begin(this, false);
		} else if (this.state == Gamestate.Game) {
			mainCourse.Begin(this, true);
		} else if (this.state == Gamestate.GameScore) {
			int total = 0;

			for (int i = 0; i < 12; i++) {
				scoreboardText[i].text = mainCourse.holes[mainCourse.randomHoleOrder[i]].score.ToString();
				total += mainCourse.holes[mainCourse.randomHoleOrder[i]].score;
			}

			scoreboardText[12].text = total.ToString();
		}
	}

	void Update() {

		if (this.state == Gamestate.Menu) {
			ingameScore.text = "";
		} else if (this.state == Gamestate.Practice) {
			ingameScore.text = "Hole " + (practiceCourse.currentHole + 1) +
				"\nPar: " + practiceCourse.holes[practiceCourse.currentHole].par +
				"\nScore: " + practiceCourse.holes[practiceCourse.currentHole].score +
				"\nPower: " + Mathf.Round(practiceCourse.holes[practiceCourse.currentHole].ballPower / 2.0f) + "%";
		} else if (this.state == Gamestate.Game) {
			ingameScore.text = "Hole " + (mainCourse.randomHoleOrder[mainCourse.currentHole] + 1) +
				"\nPar: " + mainCourse.holes[mainCourse.randomHoleOrder[mainCourse.currentHole]].par +
				"\nScore: " + mainCourse.holes[mainCourse.randomHoleOrder[mainCourse.currentHole]].score +
				"\nPower: " + Mathf.Round(mainCourse.holes[mainCourse.randomHoleOrder[mainCourse.currentHole]].ballPower / 2.0f) + "%";
		} else if (this.state == Gamestate.GameScore) {
			ingameScore.text = "";
		} else if (this.state == Gamestate.Transition) {
			ingameScore.text = "";
		}
	}

	void Event_LoadPractice() {
		ChangeState(Gamestate.Practice);
	}

	void Event_LoadStudy() {
		ChangeState(Gamestate.Game);
	}

	void Event_ReturnToMenu() {
		Debug.Log("fired event 1");
		testTestTest();
	}
	void testTestTest(){
		Debug.Log("fired event 2");
		mainCourse.StartCoroutine(mainCourse.ReturnToMenu());
	}
}

[System.Serializable]
public enum Gamestate {
	Menu,
	Practice,
	Game,
	GameScore,
	Transition
}
