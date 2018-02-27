using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CourseInfo))]
public class CourseInfoBuilder : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		CourseInfo script = (CourseInfo)target;

		if (GUILayout.Button("Save")) {
			PrefabUtility.CreatePrefab("Assets/Courses/" + script.gameObject.name + ".prefab", script.gameObject);

			AssetDatabase.CreateFolder("Assets/Courses", script.gameObject.name);

			foreach (Transform t in script.gameObject.GetComponent<Transform>()) {
				Mesh m = t.GetComponent<MeshFilter>().mesh;
				// Debug.Log("Assets/Courses/" + gameObject.name + "/" + t.gameObject.name);
				AssetDatabase.CreateAsset(m, "Assets/Courses/" + script.gameObject.name + "/" + t.gameObject.name + ".asset");
			}



			// script.SaveCourse();
		}
	}
}
