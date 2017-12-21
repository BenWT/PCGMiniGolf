using System;
using System.Collections.Generic;

public class GeneticAlgorithm<T> {
    public List<Course<T>> population { get; private set; }
    public int generation { get; set; }
    public float bestFitness { get; private set; }
    public T[] bestPoints { get; private set; }

    public float mutationRate;
    private Random random;
    private float fitnessSum;

    public GeneticAlgorithm(int populationSize, int courseSize, Random random, Func<T> getRandomPoint, Func<float, int> fitnessFunction, float mutationRate = 0.01f) {
        generation = 1;
        this.mutationRate = mutationRate;
        population = new List<Course<T>>();
        this.random = random;

        for (int i = 0; i < populationSize; i++) {
            population.Add(new Course<T>(courseSize, random, getRandomPoint, fitnessFunction, true));
        }
    }

    public void NewGeneration() {
        if (population.Count <= 0) return;

        CalculateFitness();

        List<Course<T>> newPopulation = new List<Course<T>>();

        for (int i = 0; i < population.Count; i++) {
            Course<T> parent1 = ChooseParent(), parent2 = ChooseParent();

            Course<T> child = parent1.Crossover(parent2);
            child.Mutate(mutationRate);
            newPopulation.Add(child);
        }

        population = newPopulation;
        generation++;
    }

    public void CalculateFitness() {
        fitnessSum = 0;
        Course<T> best = population[0];

        for (int i = 0; i < population.Count; i++) {
            fitnessSum += population[i].CalculateFitness(i);

            if (population[i].fitness > best.fitness) best = population[i];
        }

        bestFitness = best.fitness;
        best.points.CopyTo(bestPoints, 0);
    }

    private Course<T> ChooseParent() {
        double randomNumber = random.NextDouble() * fitnessSum;

        for (int i = 0; i < population.Count; i++) {
            if (randomNumber < population[i].fitness) return population[i];

            randomNumber -= population[i].fitness;
        }

        return null;
    }
}
