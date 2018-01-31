using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Course {
    public List<Segment> pieces = new List<Segment>();

	public Course() {
        Segment s = new Segment(SegmentType.T);
        s.width = 2.0f;
        s.length = 3.0f;
        pieces.Add(s);
	}

    public void AddSegment(int side, int index, float width, float length) {
        Segment s = new Segment(SegmentType.Square, width, length);
        s.MarkConnection(side, index);
        pieces.Add(s);
    }

	public GameObject Generate(Material mat) {
        List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();

        for (int i = 0; i < pieces.Count; i++) {
            Segment s = pieces[i];

            if (s.GetNumberOfConnections() != 0 && i != 0) {
                if (s.GetSegmentType() == SegmentType.Square) {
                    if (s.GetConnection(0) > -1) { // bottom connection
                        int connection = s.GetConnection(0);
                        s.center = pieces[connection].center + new Vector3((pieces[connection].width / 2) + (s.width / 2), 0.0f, 0.0f);
                    } else if (s.GetConnection(1) > -1) { // left connection
                        int connection = s.GetConnection(1);
                        s.center = pieces[connection].center - new Vector3(0.0f, 0.0f, (pieces[connection].length / 2) + (s.length / 2));
                    } else if (s.GetConnection(2) > -1) { // top connection
                        int connection = s.GetConnection(2);
                        s.center = pieces[connection].center - new Vector3((pieces[connection].width / 2) + (s.width / 2), 0.0f, 0.0f);
                    } else if (s.GetConnection(3) > -1) { // right connection
                        int connection = s.GetConnection(3);
                        s.center = pieces[connection].center + new Vector3(0.0f, 0.0f, (pieces[connection].length / 2) + (s.length / 2));
                    }

                    s.startVert = vertices.Count;
                    s.startTri = triangles.Count;

                    vertices.Add(s.center + new Vector3(-s.width/2, 0.0f, -s.length/2)); // Bottom Left
                    vertices.Add(s.center + new Vector3(s.width/2, 0.0f, -s.length/2)); // Top Left
                    vertices.Add(s.center + new Vector3(s.width/2, 0.0f, s.length/2)); // Top Right
                    vertices.Add(s.center + new Vector3(-s.width/2, 0.0f, s.length/2)); // Bottom Right

                    triangles.Add(s.startVert + 1);
                    triangles.Add(s.startVert);
                    triangles.Add(s.startVert + 3);

                    triangles.Add(s.startVert + 3);
                    triangles.Add(s.startVert + 2);
                    triangles.Add(s.startVert + 1);

                    s.vertCount = vertices.Count - s.startVert;
                    s.triCount = triangles.Count - s.startTri;
                } else if (s.GetSegmentType() == SegmentType.Curve) {

                }
            } else {
                s.center = Vector3.zero;
                s.startVert = vertices.Count;
                s.startTri = triangles.Count;

                vertices.Add(s.center + new Vector3(-s.width/2, 0.0f, -s.length/2)); // Bottom Left
                vertices.Add(s.center + new Vector3(s.width/2, 0.0f, -s.length/2)); // Top Left
                vertices.Add(s.center + new Vector3(s.width/2, 0.0f, s.length/2)); // Top Right
                vertices.Add(s.center + new Vector3(-s.width/2, 0.0f, s.length/2)); // Bottom Right

                triangles.Add(s.startVert + 1);
                triangles.Add(s.startVert);
                triangles.Add(s.startVert + 3);

                triangles.Add(s.startVert + 3);
                triangles.Add(s.startVert + 2);
                triangles.Add(s.startVert + 1);

                s.vertCount = vertices.Count - s.startVert;
                s.triCount = triangles.Count - s.startTri;
            }
        }

        for (int i = 0; i < pieces.Count; i++) {
            Segment s = pieces[i];
            int startCount = vertices.Count;

            int connectionBottom = s.GetConnection(0);
            int connectionLeft = s.GetConnection(1);
            int connectionTop = s.GetConnection(2);
            int connectionRight = s.GetConnection(3);

            vertices.Add(s.center + new Vector3(-s.width/2, 0.0f, -s.length/2)); // Bottom Left
            vertices.Add(s.center + new Vector3(s.width/2, 0.0f, -s.length/2)); // Top Left
            vertices.Add(s.center + new Vector3(s.width/2, 0.0f, s.length/2)); // Top Right
            vertices.Add(s.center + new Vector3(-s.width/2, 0.0f, s.length/2)); // Bottom Right

            vertices.Add(s.center + new Vector3(-s.width/2, 1.0f, -s.length/2)); // Bottom Left
            vertices.Add(s.center + new Vector3(s.width/2, 1.0f, -s.length/2)); // Top Left
            vertices.Add(s.center + new Vector3(s.width/2, 1.0f, s.length/2)); // Top Right
            vertices.Add(s.center + new Vector3(-s.width/2, 1.0f, s.length/2)); // Bottom Right

            if (connectionBottom >= 0) {
                Segment n = pieces[connectionBottom];

                vertices.Add(n.center + new Vector3(n.width/2, 0.0f, -n.length/2)); // Top Left
                vertices.Add(n.center + new Vector3(n.width/2, 0.0f, n.length/2)); // Top Right

                vertices.Add(n.center + new Vector3(n.width/2, 1.0f, -n.length/2)); // Top Left
                vertices.Add(n.center + new Vector3(n.width/2, 1.0f, n.length/2)); // Top Right

                triangles.Add(startCount + 8);
                triangles.Add(startCount);
                triangles.Add(startCount + 10);

                triangles.Add(startCount + 4);
                triangles.Add(startCount + 10);
                triangles.Add(startCount);

                triangles.Add(startCount + 9);
                triangles.Add(startCount + 7);
                triangles.Add(startCount + 3);

                triangles.Add(startCount + 9);
                triangles.Add(startCount + 11);
                triangles.Add(startCount + 7);
            } else if (connectionLeft >= 0) {
                Segment n = pieces[connectionLeft];

                vertices.Add(n.center + new Vector3(-n.width/2, 0.0f, -n.length/2)); // Bottom Left
                vertices.Add(n.center + new Vector3(n.width/2, 0.0f, -n.length/2)); // Top Left

                vertices.Add(n.center + new Vector3(-n.width/2, 1.0f, -n.length/2)); // Bottom Left
                vertices.Add(n.center + new Vector3(n.width/2, 1.0f, -n.length/2)); // Top Left

                triangles.Add(startCount + 8);
                triangles.Add(startCount + 3);  // Y
                triangles.Add(startCount + 10);

                triangles.Add(startCount + 7); // X
                triangles.Add(startCount + 10);
                triangles.Add(startCount + 3); // Y

                triangles.Add(startCount + 9);
                triangles.Add(startCount + 6); // Z
                triangles.Add(startCount + 2); // W

                triangles.Add(startCount + 9);
                triangles.Add(startCount + 11);
                triangles.Add(startCount + 6); // Z
            } else if (connectionTop >= 0) {
                Segment n = pieces[connectionTop];

                vertices.Add(n.center + new Vector3(-n.width/2, 0.0f, -n.length/2)); // Bottom Left
                vertices.Add(n.center + new Vector3(-n.width/2, 0.0f, n.length/2)); // Bottom Right

                vertices.Add(n.center + new Vector3(-n.width/2, 1.0f, -n.length/2)); // Bottom Left
                vertices.Add(n.center + new Vector3(-n.width/2, 1.0f, n.length/2)); // Bottom Right

                triangles.Add(startCount + 1);  // Y
                triangles.Add(startCount + 8);
                triangles.Add(startCount + 10);

                triangles.Add(startCount + 10);
                triangles.Add(startCount + 5); // X
                triangles.Add(startCount + 1); // Y

                triangles.Add(startCount + 6); // Z
                triangles.Add(startCount + 9);
                triangles.Add(startCount + 2); // W

                triangles.Add(startCount + 11);
                triangles.Add(startCount + 9);
                triangles.Add(startCount + 6); // Z
            } else if (connectionRight >= 0) {
                Segment n = pieces[connectionRight];

                vertices.Add(n.center + new Vector3(n.width/2, 0.0f, n.length/2)); // Top Right
                vertices.Add(n.center + new Vector3(-n.width/2, 0.0f, n.length/2)); // Bottom Right

                vertices.Add(n.center + new Vector3(n.width/2, 1.0f, n.length/2)); // Top Right
                vertices.Add(n.center + new Vector3(-n.width/2, 1.0f, n.length/2)); // Bottom Right

                triangles.Add(startCount + 8);
                triangles.Add(startCount + 1);  // Y
                triangles.Add(startCount + 10);

                triangles.Add(startCount + 5); // X
                triangles.Add(startCount + 10);
                triangles.Add(startCount + 1); // Y

                triangles.Add(startCount + 9);
                triangles.Add(startCount + 4); // Z
                triangles.Add(startCount); // W

                triangles.Add(startCount + 9);
                triangles.Add(startCount + 11);
                triangles.Add(startCount + 4); // Z
            }

            if (connectionBottom == -1) { // bottom connection
                triangles.Add(startCount + 3);
                triangles.Add(startCount);
                triangles.Add(startCount + 4);

                triangles.Add(startCount + 4);
                triangles.Add(startCount + 7);
                triangles.Add(startCount + 3);
            }
            if (s.GetConnection(1) == -1) { // left connection
                triangles.Add(startCount + 2);
                triangles.Add(startCount + 3);
                triangles.Add(startCount + 7);

                triangles.Add(startCount + 7);
                triangles.Add(startCount + 6);
                triangles.Add(startCount + 2);
            }
            if (s.GetConnection(2) == -1) { // top connection
                triangles.Add(startCount + 1);
                triangles.Add(startCount + 2);
                triangles.Add(startCount + 6);

                triangles.Add(startCount + 6);
                triangles.Add(startCount + 5);
                triangles.Add(startCount + 1);
            }
            if (s.GetConnection(3) == -1) { // right connection
                triangles.Add(startCount);
                triangles.Add(startCount + 1);
                triangles.Add(startCount + 5);

                triangles.Add(startCount + 5);
                triangles.Add(startCount + 4);
                triangles.Add(startCount);
            }
        }

		GameObject newObj = new GameObject("SquareTest");

		Mesh m = new Mesh();

		newObj.AddComponent<MeshRenderer>();
		newObj.AddComponent<MeshFilter>().mesh = m;

		m.vertices = vertices.ToArray();
		m.triangles = triangles.ToArray();
		m.RecalculateNormals();

        newObj.GetComponent<MeshRenderer>().material = mat;

        return newObj;
	}
}
