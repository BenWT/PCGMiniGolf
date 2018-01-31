using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

	public Material courseMaterial;
	public Course course;
	public GameObject courseObj;

	private int straightSegmentChance = 65;
	private float straightWidthMin = 2.0f, straightWidthMax = 8.0f;
	private float straightLengthMin = 2.0f, straightLengthMax = 10.0f;

	private float curveWidthMin = 2.0f, curveWidthMax = 8.0f;
	private float curveLengthMin = 2.0f, curveLengthMax = 10.0f;
	private float curveAngleMin = 10.0f, curveAngleMax = 70.0f;

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
		if (GUILayout.Button("Create Course")) {
			Destroy(courseObj);
			course = GenerateCourse(Random.Range(3, 12));
			courseObj = course.Generate(courseMaterial);
		}
	}

	Course GenerateCourse(int targetCount) {
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

				// TODO: Add higher chance to continue path, rather than "random"
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

				if (randomPercent() <= straightSegmentChance) {
					bool isTopBottom = (connection == 0 || connection == 2);
					float width = Random.Range(straightWidthMin, isTopBottom ? straightWidthMax : course.pieces[segment].width);
					float length = Random.Range(straightLengthMin, isTopBottom ? course.pieces[segment].length : straightLengthMax);

					course.AddSegment(reverseConnection(connection), segment, width, length);
				} else {
					bool isTopBottom = (connection == 0 || connection == 2);
					float width = Random.Range(straightWidthMin, isTopBottom ? straightWidthMax : course.pieces[segment].width);
					float length = Random.Range(straightLengthMin, isTopBottom ? course.pieces[segment].length : straightLengthMax);

					course.AddSegment(reverseConnection(connection), segment, width, length);
				}
				segmentCounter++;
			}
		}

		// TODO: Add Green

		return course;
	}
}
