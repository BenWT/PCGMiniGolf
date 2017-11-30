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
	public void InitialiseParallelogram(float width, float length, float slopeAngle, bool isHole, float curveAngle) {
		// TODO
	}
	public void InitialiseTrapezium(float width, float length, float slopeAngle, bool isHole, float curveAngle) {
		// TODO
	}

	// Generators
	public void Generate() {
		if (objectType == ProceduralObjectType.Square) GenerateSquare();
		else if (objectType == ProceduralObjectType.Circle) GenerateCircle();
		else if (objectType == ProceduralObjectType.Curve) GenerateCurve();
	}
	private void GenerateSquare() {
		// TODO: Implement
	}
	private void GenerateCircle() {
		// TODO: Implement
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
