using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomMeshLightmap))]
public class CustomMeshLightmapEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		CustomMeshLightmap script = (CustomMeshLightmap)target;

		if (GUILayout.Button("Add Lightmaps")) {
			if (!AssetDatabase.IsValidFolder("Assets/Courses/LightmapFriendly/" + script.transform.parent.name)) {
				AssetDatabase.CreateFolder("Assets/Courses/LightmapFriendly", script.transform.parent.name);
			}

			Mesh m = script.GetComponent<MeshFilter>().mesh;

			Mesh mesh = new Mesh();
			mesh.vertices = m.vertices;
			mesh.uv = m.uv;
			mesh.normals = m.normals;
			mesh.tangents = m.tangents;
			mesh.colors = m.colors;
			mesh.triangles = m.triangles;
			Unwrapping.GeneratePerTriangleUV(mesh);
			Unwrapping.GenerateSecondaryUVSet(mesh);

			AssetDatabase.CreateAsset(mesh, "Assets/Courses/LightmapFriendly/" + script.transform.parent.name + "/" + script.gameObject.name + ".asset");
		}
	}
}
