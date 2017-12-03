using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObject : MonoBehaviour {

	public ProceduralObjectType objectType;

	// Generic Properties
	float width, length, slopeAngle;
	bool isHole;

	// Specific Properties
	bool roundCorners; // Square Object
	float radius; // Circle Object
	float curveAngle; // Curve Object
	float parallelogramOffset; // Parallelogram Object
	float trapeziumOffset, trapeziumScale; // Trapezium Object

	// Generator Properties
	int circleSegments = 16;

	public void Start() {
		this.objectType = ProceduralObjectType.Circle;
		Generate();
	}

	// Constructors
	public void Initialise() {
		objectType = ProceduralObjectType.Square;
		width = Random.Range(1.0f, 10.0f);
		length = Random.Range(1.0f, 10.0f);
		slopeAngle = Random.Range(0.0f, 10.0f);
	}
	public void InitialiseSquare(float width, float length, float slopeAngle, bool isHole, bool roundCorners) {
		this.width = width;
		this.length = length;
		this.slopeAngle = slopeAngle;
		this.isHole = isHole;
		this.roundCorners = roundCorners;
	}
	public void InitialiseCircle(float slopeAngle, bool isHole, float radius) {
		this.slopeAngle = slopeAngle;
		this.isHole = isHole;
		this.radius = radius;
	}
	public void InitialiseCurve(float width, float length, float slopeAngle, bool isHole, float curveAngle) {
		this.width = width;
		this.length = length;
		this.slopeAngle = slopeAngle;
		this.isHole = isHole;
		this.curveAngle = curveAngle;
	}
	public void InitialiseParallelogram(float width, float length, float slopeAngle, bool isHole, float parallelogramOffset) {
		this.width = width;
		this.length = length;
		this.slopeAngle = slopeAngle;
		this.isHole = isHole;
		this.parallelogramOffset = parallelogramOffset;
	}
	public void InitialiseTrapezium(float width, float length, float slopeAngle, bool isHole, float trapeziumOffset, float trapeziumScale) {
		this.width = width;
		this.length = length;
		this.slopeAngle = slopeAngle;
		this.isHole = isHole;
		this.trapeziumOffset = trapeziumOffset;
		this.trapeziumScale = trapeziumScale;
	}

	// Generators
	public void CreateMesh(List<Vector3> vertices, List<int> triangles) {
		GameObject newObj = new GameObject("SquareTest");

		Mesh m = new Mesh();
		newObj.AddComponent<MeshRenderer>();
		newObj.AddComponent<MeshFilter>().mesh = m;

		m.vertices = vertices.ToArray();
		m.triangles = triangles.ToArray();
		m.RecalculateNormals();
	}
	public void Generate() {
		if (objectType == ProceduralObjectType.Square) GenerateSquare();
		else if (objectType == ProceduralObjectType.Circle) GenerateCircle();
		else if (objectType == ProceduralObjectType.Curve) GenerateCurve();
	}
	private void GenerateSquare() {
		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();

		vertices.Add(new Vector3(1.0f, 0.0f, -1.0f)); // Top Left
		vertices.Add(new Vector3(1.0f, 0.0f, 1.0f)); // Top Right
		vertices.Add(new Vector3(-1.0f, 0.0f, -1.0f)); // Bottom Left
		vertices.Add(new Vector3(-1.0f, 0.0f, 1.0f)); // Bottom Right

		triangles.Add(0);
		triangles.Add(2);
		triangles.Add(3);

		triangles.Add(3);
		triangles.Add(1);
		triangles.Add(0);

		CreateMesh(vertices, triangles);
	}
	private void GenerateCircle() {
		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();

		vertices.Add(new Vector3(0.0f, 0.0f, 0.0f)); // Centre

		for (int i = 0; i < circleSegments; i++) {
			float theta = i * (360.0f / circleSegments) * Mathf.Deg2Rad;
			vertices.Add(new Vector3(Mathf.Cos(theta), 0.0f, Mathf.Sin(theta)));
		}

		for (int i = 1; i < circleSegments + 1; i++) {
			if (i == 1) {
				triangles.Add(0);
				triangles.Add(i);
				triangles.Add(circleSegments);
			} else {
				triangles.Add(0);
				triangles.Add(i);
				triangles.Add(i - 1);
			}
		}

		CreateMesh(vertices, triangles);
	}
	private void GenerateCurve() {
		// TODO: Implement
	}
	private void GenerateParallelogram() {
		// TODO: Implement
	}
	private void GenerateTrapezium() {
		// TODO: Implement
	}
}

public enum ProceduralObjectType {
	Square,
	Circle,
	Curve,
	Parallelogram,
	Trapezium
}
