using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Course {
    public List<Segment> pieces = new List<Segment>();

    int par;
    float score;
    List<int> path;

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

    public void AddObstacle(int piece, float x, float y) {
        Obstacle o = new Obstacle();

        float w = (pieces[piece].width * 3) / (2 * 4);
        float l = (pieces[piece].length * 3) / (2 * 4);

        o.center = new Vector3(Random.Range(-w, w), 0.0f, Random.Range(-l, l));
        o.angle = Random.Range(0.0f, 90.0f);
        o.x = x;
        o.y = y;

        pieces[piece].obstacles.Add(o);
    }

	public GameObject Generate(Material mat, GameObject holePrefab) {
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

                    triangles.Add(s.startVert + 1); triangles.Add(s.startVert); triangles.Add(s.startVert + 3);
                    triangles.Add(s.startVert + 3); triangles.Add(s.startVert + 2); triangles.Add(s.startVert + 1);

                    s.vertCount = vertices.Count - s.startVert;
                    s.triCount = triangles.Count - s.startTri;
                }
            } else {
                s.center = Vector3.zero;
                s.startVert = vertices.Count;
                s.startTri = triangles.Count;

                vertices.Add(s.center + new Vector3(-s.width/2, 0.0f, -s.length/2)); // Bottom Left
                vertices.Add(s.center + new Vector3(s.width/2, 0.0f, -s.length/2)); // Top Left
                vertices.Add(s.center + new Vector3(s.width/2, 0.0f, s.length/2)); // Top Right
                vertices.Add(s.center + new Vector3(-s.width/2, 0.0f, s.length/2)); // Bottom Right

                triangles.Add(s.startVert + 1); triangles.Add(s.startVert); triangles.Add(s.startVert + 3);
                triangles.Add(s.startVert + 3); triangles.Add(s.startVert + 2); triangles.Add(s.startVert + 1);

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

                triangles.Add(startCount + 8); triangles.Add(startCount); triangles.Add(startCount + 10);
                triangles.Add(startCount + 4); triangles.Add(startCount + 10); triangles.Add(startCount);
                triangles.Add(startCount + 9); triangles.Add(startCount + 7); triangles.Add(startCount + 3);
                triangles.Add(startCount + 9); triangles.Add(startCount + 11); triangles.Add(startCount + 7);
            } else if (connectionLeft >= 0) {
                Segment n = pieces[connectionLeft];

                vertices.Add(n.center + new Vector3(-n.width/2, 0.0f, -n.length/2)); // Bottom Left
                vertices.Add(n.center + new Vector3(n.width/2, 0.0f, -n.length/2)); // Top Left
                vertices.Add(n.center + new Vector3(-n.width/2, 1.0f, -n.length/2)); // Bottom Left
                vertices.Add(n.center + new Vector3(n.width/2, 1.0f, -n.length/2)); // Top Left

                triangles.Add(startCount + 8); triangles.Add(startCount + 3); triangles.Add(startCount + 10);
                triangles.Add(startCount + 7); triangles.Add(startCount + 10); triangles.Add(startCount + 3);
                triangles.Add(startCount + 9); triangles.Add(startCount + 6); triangles.Add(startCount + 2);
                triangles.Add(startCount + 9); triangles.Add(startCount + 11); triangles.Add(startCount + 6);
            } else if (connectionTop >= 0) {
                Segment n = pieces[connectionTop];

                vertices.Add(n.center + new Vector3(-n.width/2, 0.0f, -n.length/2)); // Bottom Left
                vertices.Add(n.center + new Vector3(-n.width/2, 0.0f, n.length/2)); // Bottom Right
                vertices.Add(n.center + new Vector3(-n.width/2, 1.0f, -n.length/2)); // Bottom Left
                vertices.Add(n.center + new Vector3(-n.width/2, 1.0f, n.length/2)); // Bottom Right

                triangles.Add(startCount + 1); triangles.Add(startCount + 8); triangles.Add(startCount + 10);
                triangles.Add(startCount + 10); triangles.Add(startCount + 5); triangles.Add(startCount + 1);
                triangles.Add(startCount + 6); triangles.Add(startCount + 9); triangles.Add(startCount + 2);
                triangles.Add(startCount + 11); triangles.Add(startCount + 9); triangles.Add(startCount + 6);
            } else if (connectionRight >= 0) {
                Segment n = pieces[connectionRight];

                vertices.Add(n.center + new Vector3(n.width/2, 0.0f, n.length/2)); // Top Right
                vertices.Add(n.center + new Vector3(-n.width/2, 0.0f, n.length/2)); // Bottom Right
                vertices.Add(n.center + new Vector3(n.width/2, 1.0f, n.length/2)); // Top Right
                vertices.Add(n.center + new Vector3(-n.width/2, 1.0f, n.length/2)); // Bottom Right

                triangles.Add(startCount + 8); triangles.Add(startCount + 1);  triangles.Add(startCount + 10);
                triangles.Add(startCount + 5); triangles.Add(startCount + 10); triangles.Add(startCount + 1);
                triangles.Add(startCount + 9); triangles.Add(startCount + 4); triangles.Add(startCount);
                triangles.Add(startCount + 9); triangles.Add(startCount + 11); triangles.Add(startCount + 4);
            }

            if (connectionBottom == -1) { // bottom connection
                triangles.Add(startCount + 3); triangles.Add(startCount); triangles.Add(startCount + 4);
                triangles.Add(startCount + 4); triangles.Add(startCount + 7); triangles.Add(startCount + 3);
            }
            if (s.GetConnection(1) == -1) { // left connection
                triangles.Add(startCount + 2); triangles.Add(startCount + 3); triangles.Add(startCount + 7);
                triangles.Add(startCount + 7); triangles.Add(startCount + 6); triangles.Add(startCount + 2);
            }
            if (s.GetConnection(2) == -1) { // top connection
                triangles.Add(startCount + 1); triangles.Add(startCount + 2); triangles.Add(startCount + 6);
                triangles.Add(startCount + 6); triangles.Add(startCount + 5); triangles.Add(startCount + 1);
            }
            if (s.GetConnection(3) == -1) { // right connection
                triangles.Add(startCount); triangles.Add(startCount + 1); triangles.Add(startCount + 5);
                triangles.Add(startCount + 5); triangles.Add(startCount + 4); triangles.Add(startCount);
            }
        }

        for (int i = 0; i < pieces.Count; i++) {
            Segment s = pieces[i];
            int startCount;

            foreach (Obstacle o in s.obstacles) {
                // vertices.Add(s.center + o.center + new Vector3(-0.5f, 0.75f, -0.5f)); // Bottom Left
                // vertices.Add(s.center + o.center + new Vector3(0.5f, 0.75f, -0.5f)); // Top Left
                // vertices.Add(s.center + o.center + new Vector3(0.5f, 0.75f, 0.5f)); // Top Right
                // vertices.Add(s.center + o.center + new Vector3(-0.5f, 0.75f, 0.5f)); // Bottom Right
                // vertices.Add(s.center + o.center + new Vector3(-0.5f, 0.0f, -0.5f)); // Bottom Left
                // vertices.Add(s.center + o.center + new Vector3(0.5f, 0.0f, -0.5f)); // Top Left
                // vertices.Add(s.center + o.center + new Vector3(0.5f, 0.0f, 0.5f)); // Top Right
                // vertices.Add(s.center + o.center + new Vector3(-0.5f, 0.0f, 0.5f)); // Bottom Right

                // RotateAround(s.center + o.center + new Vector3(o.x/2, 0.75f, -o.y/2), s.center + o.center, 35);

                float a = Random.Range(0.0f, 90.0f);

                startCount = vertices.Count;
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.5f, -o.y/2), s.center + o.center, a)); // Top Left
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.5f, -o.y/2), s.center + o.center, a)); // Bottom Left
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.5f, o.y/2), s.center + o.center, a)); // Top Right
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.5f, o.y/2), s.center + o.center, a)); // Bottom Right
                triangles.Add(startCount); triangles.Add(startCount + 1); triangles.Add(startCount + 2);
                triangles.Add(startCount + 3); triangles.Add(startCount + 2); triangles.Add(startCount + 1);

                startCount = vertices.Count;
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.5f, -o.y/2), s.center + o.center, a)); // Bottom Left
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.5f, -o.y/2), s.center + o.center, a)); // Top Left
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.0f, -o.y/2), s.center + o.center, a)); // Bottom Left
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.0f, -o.y/2), s.center + o.center, a)); // Top Left
                triangles.Add(startCount); triangles.Add(startCount + 1); triangles.Add(startCount + 2);
                triangles.Add(startCount + 3); triangles.Add(startCount + 2); triangles.Add(startCount + 1);

                startCount = vertices.Count;
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.5f, -o.y/2), s.center + o.center, a)); // Top Left
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.5f, o.y/2), s.center + o.center, a)); // Top Right
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.0f, -o.y/2), s.center + o.center, a)); // Top Left
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.0f, o.y/2), s.center + o.center, a)); // Top Right
                triangles.Add(startCount); triangles.Add(startCount + 1); triangles.Add(startCount + 2);
                triangles.Add(startCount + 3); triangles.Add(startCount + 2); triangles.Add(startCount + 1);

                startCount = vertices.Count;
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.5f, o.y/2), s.center + o.center, a)); // Top Right
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.5f, o.y/2), s.center + o.center, a)); // Bottom Right
                vertices.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.0f, o.y/2), s.center + o.center, a)); // Top Right
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.0f, o.y/2), s.center + o.center, a)); // Bottom Right
                triangles.Add(startCount); triangles.Add(startCount + 1); triangles.Add(startCount + 2);
                triangles.Add(startCount + 3); triangles.Add(startCount + 2); triangles.Add(startCount + 1);

                startCount = vertices.Count;
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.5f, o.y/2), s.center + o.center, a)); // Bottom Right
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.5f, -o.y/2), s.center + o.center, a)); // Bottom Left
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.0f, o.y/2), s.center + o.center, a)); // Bottom Right
                vertices.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.0f, -o.y/2), s.center + o.center, a)); // Bottom Left
                triangles.Add(startCount); triangles.Add(startCount + 1); triangles.Add(startCount + 2);
                triangles.Add(startCount + 3); triangles.Add(startCount + 2); triangles.Add(startCount + 1);
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

        float w = (pieces[pieces.Count - 1].width * 3) / (2 * 4);
        float l = (pieces[pieces.Count - 1].length * 3) / (2 * 4);
        Vector3 p  = pieces[pieces.Count - 1].center + new Vector3(Random.Range(-w, w), 0.0f, Random.Range(-l, l));

        GameObject hole = GameObject.Instantiate(holePrefab, p, Quaternion.identity) as GameObject;
        hole.GetComponent<Transform>().parent = newObj.GetComponent<Transform>();

        CalculateScore();

        return newObj;
	}

    public float GetScore() {
        return score;
    }
    void CalculateScore() {
        int target = pieces.Count - 1;

        path = RecursivePath(target, new List<int>());
        path.Reverse();

        float distance = 0;

        for (int i = 0; i < path.Count - 1; i++) {
            distance += (pieces[path[i + 1]].center - pieces[path[i]].center).magnitude;
        }

        // TODO add to Score
        // Area of each square should affect, as well as distance
        // Take into account if there are straight lines vs corners. Do this using the connection pair function

        // TODO cleanup all code
        // TODO add code comments
        // TODO maybe do something about the obstacle placement, possibly factor in size of square when sizing the obstacle

        score = distance;

        CalculatePar();
    }
    List<int> RecursivePath(int index, List<int> path) {
        path.Add(index);
        if (path.Contains(0)) return path;
        if (pieces[index].GetLowestConnection() >= 0) return RecursivePath(pieces[index].GetLowestConnection(), path);
        return path;
    }

    public int GetPar() {
        return par;
    }
    void CalculatePar() {
        int numberOfObstacles = 0;
        foreach (int i in path) {
            if (pieces[i].obstacles.Count > 0) numberOfObstacles++;
        }

        par = Mathf.Max(path.Count + numberOfObstacles - 2, 2);
    }

    Vector3 RotateAround(Vector3 point, Vector3 pivot, float angle) {
        Vector3 direction = point - pivot;
        direction = Quaternion.Euler(new Vector3(0, angle, 0)) * direction;
        point = direction + pivot;
        return point;
    }


    // TODO remove for production
    void PrintIntArray(List<int> toPrint) {
        string val = "Path: ";

        foreach (int i in toPrint) {
            val += i.ToString() + " ";
        }

        Debug.Log(val);
    }
}
