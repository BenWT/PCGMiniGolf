using System;

// TODO Points will need to be triangulated eventually
// This will convert the list of vertices into correct triangles that should be displayable
// https://www.geometrictools.com/Documentation/TriangulationByEarClipping.pdf

public class Course<T> {
    public T[] points { get; private set; }
    public float fitness { get; private set; }

    private Random random;
    private Func<T> getRandomPoint;
    private Func<float, int> fitnessFunction;

    public Course(int size, Random random, Func<T> getRandomPoint, Func<float, int> fitnessFunction, bool shouldInit = true) {
        points = new T[size];
        this.random = random;
        this.getRandomPoint = getRandomPoint;
        this.fitnessFunction = fitnessFunction;

        if (shouldInit) {
            for (int i = 0; i < points.Length; i++) {
                points[i] = getRandomPoint();
            }
        }
    }

    public float CalculateFitness(int index) {
        fitness = fitnessFunction(index);
        return fitness;
    }

    public Course<T> Crossover(Course<T> otherParent) {
        Course<T> child = new Course<T>(points.Length, random, getRandomPoint, fitnessFunction, false);

        for (int i = 0; i < points.Length; i++) {
            child.points[i] = random.NextDouble() < 0.5 ? points[i] : otherParent.points[i];
        }

        return child;
    }

    public void Mutate(float mutationRate) {
        for (int i = 0; i < points.Length; i++) {
            if (random.NextDouble() < mutationRate) {
                points[i] = getRandomPoint();
            }
        }
    }
}
