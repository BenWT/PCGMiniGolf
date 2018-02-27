using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment {
	SegmentType type;
	public int[] connections = { -1, -1, -1, -1 }; // Bottom, Left, Top, Right
	public float width, length;
	public Vector3 center = Vector3.zero;

	public int startVert, vertCount;
	public int startTri, triCount;

	public List<Obstacle> obstacles = new List<Obstacle>();

	public Segment(SegmentType type) {
		this.type = type;
		this.width = 1.0f;
		this.length = 1.0f;
	}
	public Segment(SegmentType type, float width, float length) {
		this.type = type;
		this.width = width;
		this.length = length;
	}

    public void Generate() {
        if (type == SegmentType.T) {
            Debug.Log("Generating T");
        } else if (type == SegmentType.Square) {
            Debug.Log("Generating Square");
        } else if (type == SegmentType.Green) {
            Debug.Log("Generating Green");
        }
    }

	public SegmentType GetSegmentType() {
		return type;
	}
	public void SetSegmentType(SegmentType type) {
		this.type = type;
	}
	public bool GetConnectionPair() {
		if (GetConnection(0) != -1 && GetConnection(2) != -1) return true;
		else if (GetConnection(1) != -1 && GetConnection(3) != -1) return true;
		else return false;
	}
	public int GetNumberOfConnections() {
		int count = 0;
		for (int i = 0; i < 4; i++) {
			if (GetConnection(i) != -1) count++;
		}
		return count;
	}
	public int GetLowestConnection() {
		int lowestConnection = 15000;

		for (int i = 0; i < 4; i++) {
			int c = GetConnection(i);
			if (c > -1 && c < lowestConnection) lowestConnection = c;
		}

		if (lowestConnection == 15000) lowestConnection = -1;
		return lowestConnection;
	}
	public int GetConnection(int connection) {
		return connections[connection];
	}
	public void MarkConnection(int side, int index) {
		connections[side] = index;
	}
}

public enum SegmentType {
	T,
	Square,
	Green
}

public class Obstacle {
	public Vector3 center;
	public float angle, x, y;
}
