using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseInfo : MonoBehaviour {

	public float score;
	public int par;

	public void Init(float score, int par) {
		this.score = score;
		this.par = par;
	}

	public float GetScore() {
		return score;
	}
	public int GetPar() {
		return par;
	}
}
