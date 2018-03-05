using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
	List<GameObject> courses = new List<GameObject>();

	public Material groundMaterial, wallMaterial;
	public Course course;
	public GameObject courseObj;

	private int straightSegmentChance = 65;
	private float straightWidthMin = 1.5f, straightWidthMax = 5.0f;
	private float straightLengthMin = 1.5f, straightLengthMax = 6.0f;

	private int obstacleChance = 75;
	private float obstacleSizeMin = 0.1f, obstacleSizeMax = 1.0f;

	private int randomPercent() {
		return Random.Range(0, 100);
	}
	private int reverseConnection(int connection) {
		if (connection == 0) return 2;
		else if (connection == 1) return 3;
		else if (connection == 2) return 0;
		else if (connection == 3) return 1;
		else return -1;
	}

	void OnGUI() {
		if (GUILayout.Button("Create 100 Courses")) {
			foreach (GameObject o in courses) Destroy(o);
			courses.Clear();

			List<Course> courseData = new List<Course>();

			for (int i = 0; i < 100; i++) {
				Course c = GenerateCourse(Random.Range(3, 5), true);
				courseData.Add(c);
			}

			courseData.Sort((x, y) => x.GetScore().CompareTo(y.GetScore()));

			for (int i = 0; i < 100; i++) {
				GameObject go = courseData[i].Generate(groundMaterial, wallMaterial, "Course-" + (i + 1));
				go.GetComponent<Transform>().position = new Vector3(i * 25.0f, 0.0f, 0.0f);
				courses.Add(go);

				CourseInfo ci = go.AddComponent(typeof(CourseInfo)) as CourseInfo;
				ci.Init(courseData[i].GetScore(), courseData[i].GetPar());
			}
		}
	}

	Course GenerateCourse(int targetCount, bool addObstacles) {
		Course course = new Course();
		int segmentCounter = 0;

		while (segmentCounter < targetCount) {
			if (course.pieces.Count == 1) {
				course.AddSegment(0, 0, Random.Range(straightWidthMin, straightWidthMax), Random.Range(straightLengthMin, straightLengthMax));
				course.pieces[0].MarkConnection(2, -2);
				segmentCounter++;
			} else {
				int segment = -1, connection = -1;
				int addChance = 100 * ((course.pieces.Count - 1) / 4);

				foreach (Segment s in course.pieces) {
					if (s.GetSegmentType() != SegmentType.T) {
						for (int i = 3; i >= 0; i--) {
							if (s.GetConnection(i) == -1 && randomPercent() <= addChance) {
								s.MarkConnection(i, -2);

								segment = course.pieces.IndexOf(s);
								connection = i;
								break;
							}
						}
					}
				}

				if (segment == -1 || connection == -1) continue;

				bool isTopBottom = (connection == 0 || connection == 2);
				float width = Random.Range(straightWidthMin, isTopBottom ? straightWidthMax : course.pieces[segment].width);
				float length = Random.Range(straightLengthMin, isTopBottom ? course.pieces[segment].length : straightLengthMax);

				course.AddSegment(reverseConnection(connection), segment, width, length);
				segmentCounter++;

				if (addObstacles) {
					if (randomPercent() < obstacleChance) {
						course.AddObstacle(course.pieces.Count - 1, Random.Range(obstacleSizeMin, obstacleSizeMax), Random.Range(obstacleSizeMin, obstacleSizeMax));
					}
				}
			}
		}

		course.pieces[course.pieces.Count - 1].SetSegmentType(SegmentType.Green);
		course.CalculateScore();

		return course;
	}
}
