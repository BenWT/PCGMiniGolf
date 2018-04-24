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
        s.width = 1.0f;
        s.length = 1.5f;
        pieces.Add(s);
	}

    public void AddSegment(int side, int index, float width, float length) {
        Segment s = new Segment(SegmentType.Square, width, length);
        s.MarkConnection(side, index);

        // Calculate Center
        if (s.GetConnection(0) > -1) {
            s.center = pieces[s.GetConnection(0)].center + new Vector3((pieces[s.GetConnection(0)].width / 2) + (s.width / 2), 0.0f, 0.0f);
        } else if (s.GetConnection(1) > -1) {
            s.center = pieces[s.GetConnection(1)].center - new Vector3(0.0f, 0.0f, (pieces[s.GetConnection(1)].length / 2) + (s.length / 2));
        } else if (s.GetConnection(2) > -1) {
            s.center = pieces[s.GetConnection(2)].center - new Vector3((pieces[s.GetConnection(2)].width / 2) + (s.width / 2), 0.0f, 0.0f);
        } else if (s.GetConnection(3) > -1) {
            s.center = pieces[s.GetConnection(3)].center + new Vector3(0.0f, 0.0f, (pieces[s.GetConnection(3)].length / 2) + (s.length / 2));
        }

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

    public GameObject Generate(Material groundMat, Material wallMat, string name) {
        List<Vector3> groundV = new List<Vector3>();
		List<int> groundT = new List<int>();

        List<Vector3> wallV = new List<Vector3>();
		List<int> wallT = new List<int>();

        List<Vector3> obstacleV = new List<Vector3>();
		List<int> obstacleT = new List<int>();

        foreach (Segment s in pieces) {
            // Ground
            int groundStart = groundV.Count;

            groundV.Add(s.center + new Vector3(-s.width/2, 0.0f, -s.length/2));
            groundV.Add(s.center + new Vector3(s.width/2, 0.0f, -s.length/2));
            groundV.Add(s.center + new Vector3(s.width/2, 0.0f, s.length/2));
            groundV.Add(s.center + new Vector3(-s.width/2, 0.0f, s.length/2));

            groundT = AddTriangles(groundT, groundStart);

            // Partial Walls
            int wallStart = wallV.Count;

            if (s.GetConnection(0) > -1) {
                wallV.Add(pieces[s.GetConnection(0)].center + new Vector3(pieces[s.GetConnection(0)].width/2, 0.0f, -pieces[s.GetConnection(0)].length/2));
                wallV.Add(pieces[s.GetConnection(0)].center + new Vector3(pieces[s.GetConnection(0)].width/2, 0.5f, -pieces[s.GetConnection(0)].length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.5f, -s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.0f, -s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.0f, s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.5f, s.length/2));
                wallV.Add(pieces[s.GetConnection(0)].center + new Vector3(pieces[s.GetConnection(0)].width/2, 0.5f, pieces[s.GetConnection(0)].length/2));
                wallV.Add(pieces[s.GetConnection(0)].center + new Vector3(pieces[s.GetConnection(0)].width/2, 0.0f, pieces[s.GetConnection(0)].length/2));
            } else if (s.GetConnection(1) > -1) {
                wallV.Add(pieces[s.GetConnection(1)].center + new Vector3(-pieces[s.GetConnection(1)].width/2, 0.0f, -pieces[s.GetConnection(1)].length/2));
                wallV.Add(pieces[s.GetConnection(1)].center + new Vector3(-pieces[s.GetConnection(1)].width/2, 0.5f, -pieces[s.GetConnection(1)].length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.5f, s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.0f, s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.0f, s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.5f, s.length/2));
                wallV.Add(pieces[s.GetConnection(1)].center + new Vector3(pieces[s.GetConnection(1)].width/2, 0.5f, -pieces[s.GetConnection(1)].length/2));
                wallV.Add(pieces[s.GetConnection(1)].center + new Vector3(pieces[s.GetConnection(1)].width/2, 0.0f, -pieces[s.GetConnection(1)].length/2));
            } else if (s.GetConnection(2) > -1) {
                wallV.Add(pieces[s.GetConnection(2)].center + new Vector3(-pieces[s.GetConnection(2)].width/2, 0.0f, pieces[s.GetConnection(2)].length/2));
                wallV.Add(pieces[s.GetConnection(2)].center + new Vector3(-pieces[s.GetConnection(2)].width/2, 0.5f, pieces[s.GetConnection(2)].length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.5f, s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.0f, s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.0f, -s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.5f, -s.length/2));
                wallV.Add(pieces[s.GetConnection(2)].center + new Vector3(-pieces[s.GetConnection(2)].width/2, 0.5f, -pieces[s.GetConnection(2)].length/2));
                wallV.Add(pieces[s.GetConnection(2)].center + new Vector3(-pieces[s.GetConnection(2)].width/2, 0.0f, -pieces[s.GetConnection(2)].length/2));
            } else if (s.GetConnection(3) > -1) {
                wallV.Add(pieces[s.GetConnection(3)].center + new Vector3(pieces[s.GetConnection(3)].width/2, 0.0f, pieces[s.GetConnection(3)].length/2));
                wallV.Add(pieces[s.GetConnection(3)].center + new Vector3(pieces[s.GetConnection(3)].width/2, 0.5f, pieces[s.GetConnection(3)].length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.5f, -s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.0f, -s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.0f, -s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.5f, -s.length/2));
                wallV.Add(pieces[s.GetConnection(3)].center + new Vector3(-pieces[s.GetConnection(3)].width/2, 0.5f, pieces[s.GetConnection(3)].length/2));
                wallV.Add(pieces[s.GetConnection(3)].center + new Vector3(-pieces[s.GetConnection(3)].width/2, 0.0f, pieces[s.GetConnection(3)].length/2));
            }

            wallT = AddTriangles(wallT, wallStart);
            wallT = AddTriangles(wallT, wallStart + 4);

            // Regular Walls
            if (s.GetConnection(0) == -1) {
                wallStart = wallV.Count;

                wallV.Add(s.center + new Vector3(-s.width/2, 0.0f, s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.5f, s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.5f, -s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.0f, -s.length/2));

                wallT = AddTriangles(wallT, wallStart);
            }
            if (s.GetConnection(1) == -1) {
                wallStart = wallV.Count;

                wallV.Add(s.center + new Vector3(s.width/2, 0.0f, s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.5f, s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.5f, s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.0f, s.length/2));

                wallT = AddTriangles(wallT, wallStart);
            }
            if (s.GetConnection(2) == -1) {
                wallStart = wallV.Count;

                wallV.Add(s.center + new Vector3(s.width/2, 0.0f, -s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.5f, -s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.5f, s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.0f, s.length/2));

                wallT = AddTriangles(wallT, wallStart);
            }
            if (s.GetConnection(3) == -1) {
                wallStart = wallV.Count;

                wallV.Add(s.center + new Vector3(-s.width/2, 0.0f, -s.length/2));
                wallV.Add(s.center + new Vector3(-s.width/2, 0.5f, -s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.5f, -s.length/2));
                wallV.Add(s.center + new Vector3(s.width/2, 0.0f, -s.length/2));

                wallT = AddTriangles(wallT, wallStart);
            }

            // Obstacles
            int obstacleStart = obstacleV.Count;

            foreach (Obstacle o in s.obstacles) {
                float a = Random.Range(0.0f, 90.0f);

                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.35f, -o.y/2), s.center + o.center, a)); // Bottom Left
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.35f, -o.y/2), s.center + o.center, a)); // Top Left
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.35f, o.y/2), s.center + o.center, a)); // Top Right
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.35f, o.y/2), s.center + o.center, a)); // Bottom Right
                obstacleT = AddTriangles(obstacleT, obstacleStart);

                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.35f, -o.y/2), s.center + o.center, a)); // Top Left
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.35f, -o.y/2), s.center + o.center, a)); // Bottom Left
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.0f, -o.y/2), s.center + o.center, a)); // Bottom Left
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.0f, -o.y/2), s.center + o.center, a)); // Top Left
                obstacleT = AddTriangles(obstacleT, obstacleStart + 4);

                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.35f, o.y/2), s.center + o.center, a)); // Top Right
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.35f, -o.y/2), s.center + o.center, a)); // Top Left
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.0f, -o.y/2), s.center + o.center, a)); // Top Left
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.0f, o.y/2), s.center + o.center, a)); // Top Right
                obstacleT = AddTriangles(obstacleT, obstacleStart + 8);

                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.35f, o.y/2), s.center + o.center, a)); // Bottom Right
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.35f, o.y/2), s.center + o.center, a)); // Top Right
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(o.x/2, 0.0f, o.y/2), s.center + o.center, a)); // Top Right
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.0f, o.y/2), s.center + o.center, a)); // Bottom Right
                obstacleT = AddTriangles(obstacleT, obstacleStart + 12);

                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.35f, -o.y/2), s.center + o.center, a)); // Bottom Left
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.35f, o.y/2), s.center + o.center, a)); // Bottom Right
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.0f, o.y/2), s.center + o.center, a)); // Bottom Right
                obstacleV.Add(RotateAround(s.center + o.center + new Vector3(-o.x/2, 0.0f, -o.y/2), s.center + o.center, a)); // Bottom Left
                obstacleT = AddTriangles(obstacleT, obstacleStart + 16);
            }
        }

        GameObject courseGO = new GameObject(name);

        // Initialise Ground Mesh
        GameObject groundGO = new GameObject("Ground");
        Mesh groundM = CreateMesh(groundV, groundT, true);

        groundGO.AddComponent<MeshFilter>().mesh = groundM;
        groundGO.AddComponent<MeshRenderer>();
        groundGO.AddComponent<MeshCollider>();

        groundGO.GetComponent<MeshRenderer>().material = groundMat;
        groundGO.GetComponent<Transform>().parent = courseGO.GetComponent<Transform>();

        // Initialise Walls Mesh
        GameObject wallGO = new GameObject("Walls");
        Mesh wallM = CreateMesh(wallV, wallT, true);

        wallGO.AddComponent<MeshFilter>().mesh = wallM;
        wallGO.AddComponent<MeshRenderer>();
        wallGO.AddComponent<MeshCollider>();

        wallGO.GetComponent<MeshRenderer>().material = wallMat;
        wallGO.GetComponent<Transform>().parent = courseGO.GetComponent<Transform>();

        // Initialise Obstacles Mesh
        if (obstacleV.Count > 0) {
            GameObject obstacleGO = new GameObject("Obstacles");
            Mesh obstacleM = CreateMesh(obstacleV, obstacleT, false);

            obstacleGO.AddComponent<MeshFilter>().mesh = obstacleM;
            obstacleGO.AddComponent<MeshRenderer>();
            obstacleGO.AddComponent<MeshCollider>();

            obstacleGO.GetComponent<MeshRenderer>().material = wallMat;
            obstacleGO.GetComponent<Transform>().parent = courseGO.GetComponent<Transform>();
        }

        float w = (pieces[pieces.Count - 1].width * 3) / (2 * 4);
        float l = (pieces[pieces.Count - 1].length * 3) / (2 * 4);
        
        GameObject hole = new GameObject("Hole");
        hole.GetComponent<Transform>().position = pieces[pieces.Count - 1].center + new Vector3(Random.Range(-w, w), 0.0f, Random.Range(-l, l));
        hole.GetComponent<Transform>().parent = courseGO.GetComponent<Transform>();

        return courseGO;
    }
    List<int> AddTriangles(List<int> triangles, int startCount) {
        triangles.Add(startCount + 1);
        triangles.Add(startCount);
        triangles.Add(startCount + 3);

        triangles.Add(startCount + 3);
        triangles.Add(startCount + 2);
        triangles.Add(startCount + 1);

        return triangles;
    }
    Mesh CreateMesh(List<Vector3> v, List<int> t, bool doubleSided) {
        if (doubleSided) {
            v.AddRange(v);
            t.AddRange(t);
        }

        Mesh m = new Mesh();
        m.vertices = v.ToArray();
        m.triangles = t.ToArray();

        if (doubleSided) {
            int[] triangles = m.triangles;
            for (int i = (triangles.Length / 2); i < triangles.Length; i++) triangles[i] += v.Count / 2;
            for (int i = (triangles.Length / 2); i < triangles.Length; i+=3) {
                int temp = triangles[i + 0];
    			triangles[i + 0] = triangles[i + 2];
    			triangles[i + 2] = temp;
            }
            m.triangles = triangles;
        }

        m.RecalculateNormals();

        return m;
    }

    public float GetScore() {
        return score;
    }
    public void CalculateScore() {
        int target = pieces.Count - 1;

        path = RecursivePath(target, new List<int>());
        path.Reverse();

        float distance = 0;
        for (int i = 0; i < path.Count - 1; i++) distance += (pieces[path[i + 1]].center - pieces[path[i]].center).magnitude;

        int numberOfStraights = 0, numberOfCorners = 0;
        for (int i = 0; i < path.Count; i++) {
            if (pieces[path[i]].GetConnectionPair()) numberOfStraights++;
            else numberOfCorners++;
        }

        // Obstacle Count
        int numberOfObstacles = 0;
        foreach (int i in path) {
            if (pieces[i].obstacles.Count > 0) numberOfObstacles++;
        }

        score = (distance / path.Count - 1) * ((float)numberOfCorners / (float)path.Count) * numberOfObstacles;

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

        par = Mathf.Max(path.Count - 1, 2);
    }

    Vector3 RotateAround(Vector3 point, Vector3 pivot, float angle) {
        Vector3 direction = point - pivot;
        direction = Quaternion.Euler(new Vector3(0, angle, 0)) * direction;
        point = direction + pivot;
        return point;
    }
}
