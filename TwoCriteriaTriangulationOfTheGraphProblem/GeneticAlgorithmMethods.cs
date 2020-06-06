using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Troschuetz.Random;
using TwoCriteriaTriangulationOfTheGraphProblem.GraphMethods;

namespace TwoCriteriaTriangulationOfTheGraphProblem
{
    public class GeneticAlgorithmMethods
    {
        double BestFitnessScore = double.PositiveInfinity;
        double PopulationAverage = 0;
        double[][] PopulationToSave;
        TRandom MutationProc = new TRandom();
        TRandom CrossoverProc = new TRandom();
        Parameters parameters;

        public void GeneticAlgorithm(Parameters parameters)
        {
            //creating Matrixes for population and arrays for fitnesses
            this.parameters = parameters;
            int vertexAmount = parameters.NumberOfVertices;
            double[] BestGroups = new double[parameters.Popsize];
            double[][] Population = new double[vertexAmount][];
            double[] PopulationFitness = new double[parameters.Popsize];
            double[] FitnessGroup1 = new double[parameters.Popsize];
            double[] FitnessGroup2 = new double[parameters.Popsize];
            double[] FitnessGroup3 = new double[parameters.Popsize];
            PopulationToSave = new double[vertexAmount][];

            parameters.FitnessArray = PopulationFitness;
            parameters.FitnessGroup1 = FitnessGroup1;
            parameters.FitnessGroup2 = FitnessGroup2;
            parameters.FitnessGroup3 = FitnessGroup3;

            //matrix for filesave
            parameters.MatrixToSave = new List<double[][]>();
            parameters.FitnessesToSave = new List<double[]>();

            for (int i = 0; i < vertexAmount; i++)
            {
                Population[i] = new double[parameters.Popsize];
                PopulationToSave[i] = new double[parameters.Popsize];
            }

            //creating first population, calculating first fitness
            var InitialGroup = CreateInitialGroup(vertexAmount);

            BestFitnessScore = CalculateFitness(InitialGroup, parameters)[0];
            var InitialPopulation = CreateInitialPopulation(parameters.Popsize, parameters);

            parameters.Population = InitialPopulation;
        }

        //calling Selection, fill the population, updating saved file
        public void OneMoreTime()
        {
            CompetetiveSelection(parameters);
            CreateNewPopulation(parameters);

            PopulationToSave = GetMatrix(parameters.Population, PopulationToSave);

            parameters.MatrixToSave.Add(PopulationToSave);
            parameters.FitnessesToSave.Add((double[])parameters.FitnessArray.Clone());
        }

        //cloning matrix
        public double[][] GetMatrix(double[][] MatrixToClone, double[][] MatrixToReturn)
        {
            MatrixToReturn = (double[][])MatrixToClone.Clone();

            for (int j = 0; j < MatrixToClone.GetLength(0); j++)
            {
                MatrixToReturn[j] = (double[])MatrixToClone[j].Clone();
            }

            return MatrixToReturn;
        }

        //creating new population (half of last population won, so function is creating new units for the rest of population)
        public void CreateNewPopulation(Parameters parameters)
        {
            //getting winners amount
            int Winners = parameters.Popsize / 2;
            double[] CurrentGroup = new double[parameters.NumberOfVertices];

            //creating new groups
            for (int k = Winners; k < parameters.Popsize; k++)
            {
                var NewGroup = CreateGroup(parameters.NumberOfVertices);
                for (int j = 0; j < NewGroup.Length; j++)
                {
                    parameters.Population[j][k] = NewGroup[j];
                }
            }

            //checking if mutation or crossover occur
            for (int i = Winners; i < parameters.Popsize; i++)
            {
                double CheckIfMutationHappen = MutationProc.Next(0, 100);
                double CheckIfCrossOverHappen = CrossoverProc.Next(0, 100);

                for (int k = 0; k < parameters.NumberOfVertices; k++)
                {
                    CurrentGroup[k] = parameters.Population[k][i];
                }

                if (CheckIfMutationHappen <= parameters.MutationProbabilityValue)
                {
                    //mutation
                    Mutation(CurrentGroup);
                }
                if (CheckIfCrossOverHappen <= parameters.CrossoverProbabilityValue)
                {
                    //crossover
                    Crossover(CurrentGroup);
                }
            }
        }

        //competetive selection
        public void CompetetiveSelection(Parameters parameters)
        {
            //getting population units and their fitnesses
            int NewGroupIndex = 0;
            double[] Group1 = new double[parameters.NumberOfVertices];
            double[] Group2 = new double[parameters.NumberOfVertices];

            for (int i = 0; i < parameters.Popsize - 1; i += 2)
            {
                for (int w = 0; w < parameters.NumberOfVertices; w++)
                {
                    Group1[w] = parameters.Population[w][i];
                    Group2[w] = parameters.Population[w][i + 1];
                }

                double[] FitnenssFirstGroup = CalculateFitness(Group1, parameters);
                double[] FitnessSecondGroup = CalculateFitness(Group2, parameters);

                //checking which unit is better and choosing it
                if (FitnenssFirstGroup[0] < FitnessSecondGroup[0])
                {
                    parameters.FitnessArray[NewGroupIndex] = FitnenssFirstGroup[0];
                    parameters.FitnessGroup1[NewGroupIndex] = FitnenssFirstGroup[1];
                    parameters.FitnessGroup2[NewGroupIndex] = FitnenssFirstGroup[2];
                    parameters.FitnessGroup3[NewGroupIndex] = FitnenssFirstGroup[3];

                    for (int p = 0; p < parameters.NumberOfVertices; p++)
                    {
                        parameters.Population[p][NewGroupIndex] = Group1[p];
                    }
                }

                else
                {
                    parameters.FitnessArray[NewGroupIndex] = FitnessSecondGroup[0];
                    parameters.FitnessGroup1[NewGroupIndex] = FitnessSecondGroup[1];
                    parameters.FitnessGroup2[NewGroupIndex] = FitnessSecondGroup[2];
                    parameters.FitnessGroup3[NewGroupIndex] = FitnessSecondGroup[3];

                    for (int p = 0; p < parameters.NumberOfVertices; p++)
                    {
                        parameters.Population[p][NewGroupIndex] = Group2[p];
                    }
                }

                NewGroupIndex++;
            }
        }

        //calculate group fitness
        public double GetGroupFitnessValue(Parameters parameters, List<double> Group)
        {
            double FitnessValue = 0;

            for (int i = 0; i < parameters.incidenceMatrix.GetLength(0); i++)
            {
                if (Group.Contains(i))
                {
                    for (int j = 0; j < parameters.incidenceMatrix.GetLength(0); j++)
                    {
                        if (!Group.Contains(j))
                        {
                            FitnessValue += parameters.incidenceMatrix[i][j] * parameters.weightsMatrix[i][j];
                        }
                    }
                }
            }

            return FitnessValue;
        }

        //calculate fitness of one unit
        public double[] CalculateFitness(double[] Group, Parameters parameters) //XX
        {
            List<double> Group1 = new List<double>();
            List<double> Group2 = new List<double>();
            List<double> Group3 = new List<double>();

            for (int i = 0; i < Group.GetLength(0); i++)
            {
                if (Group[i] == 1)
                {
                    Group1.Add(i);
                }
                if (Group[i] == 2)
                {
                    Group2.Add(i);
                }
                if (Group[i] == 3)
                {
                    Group3.Add(i);
                }
            }

            double Group1FitnessValue = GetGroupFitnessValue(parameters, Group1);
            double Group2FitnessValue = GetGroupFitnessValue(parameters, Group2);
            double Group3FitnessValue = GetGroupFitnessValue(parameters, Group3);

            double CurrentGroupsFitnessScore = Group1FitnessValue + Group2FitnessValue + Group3FitnessValue;

            if (PopulationAverage == 0)
            {
                PopulationAverage = CurrentGroupsFitnessScore;
            }

            double[] Fitness = new double[4];
            Fitness[0] = CurrentGroupsFitnessScore;
            Fitness[1] = Group1FitnessValue;
            Fitness[2] = Group2FitnessValue;
            Fitness[3] = Group3FitnessValue;

            return Fitness;
        }

        //creating first unit
        public double[] CreateInitialGroup(int VertexAmount)
        {
            double[] InitialGroup = new double[VertexAmount];
            int ChromosomeGroup = 1;

            for (int i = 0; i < VertexAmount; i++)
            {
                InitialGroup[i] = (double)ChromosomeGroup;
                ChromosomeGroup++;

                if (ChromosomeGroup == 4)
                {
                    ChromosomeGroup = 1;
                }
            }

            return InitialGroup;
        }

        //creating first population
        public double[][] CreateInitialPopulation(int GenerationSize, Parameters parameters)
        {
            //getting random values to fill population matrix
            TRandom rnd = new TRandom();
            double[][] NewPopulation = new double[parameters.NumberOfVertices][];

            for (int i = 0; i < parameters.NumberOfVertices; i++)
            {
                NewPopulation[i] = new double[GenerationSize];
            }

            //creating units and calculating fitness of each group
            for (int k = 0; k < GenerationSize; k++)
            {
                var NewGroup = CreateGroup(parameters.NumberOfVertices);

                for (int j = 0; j < NewGroup.Length; j++)
                {
                    NewPopulation[j][k] = NewGroup[j];
                }

                var FitnessScore = CalculateFitness(NewGroup, parameters);
                parameters.FitnessArray[k] = FitnessScore[0];
                parameters.FitnessGroup1[k] = FitnessScore[1];
                parameters.FitnessGroup2[k] = FitnessScore[2];
                parameters.FitnessGroup3[k] = FitnessScore[3];

            }

            return NewPopulation;
        }

        //creating group
        public double[] CreateGroup(int GroupSize)
        {
            //filling whole group with 0
            TRandom rnd = new TRandom();
            double[] Group = new double[GroupSize];

            for (int i = 0; i < GroupSize; i++)
            {
                Group[i] = 0;
            }

            //checking size of each group
            int checkVertexAmount = GroupSize % 3;
            int normalNumber = (GroupSize - checkVertexAmount) / 3;

            //creating group if vertex amount is divided by 3
            if (checkVertexAmount == 0)
            {
                for (int i = 1; i < 4; i++)
                {
                    for (int j = 0; j < normalNumber; j++)
                    {
                        int index = rnd.Next(0, GroupSize);

                        if (Group[index] == 0)
                        {
                            Group[index] = i;
                        }
                        else
                        {
                            j--;
                        }
                    }
                }
            }

            //creating group if exactly one of groups is bigger than others
            if (checkVertexAmount == 1)
            {
                int OneMore = rnd.Next(1, 3);

                for (int i = 1; i < 4; i++)
                {
                    for (int j = 0; j < normalNumber; j++)
                    {
                        int index = rnd.Next(0, GroupSize);
                        if (Group[index] == 0)
                        {
                            Group[index] = i;
                        }
                        else
                        {
                            j--;
                        }
                    }
                }

                for (int i = 0; i < GroupSize; i++)
                {
                    if (Group[i] == 0)
                    {
                        Group[i] = OneMore;
                    }
                }
            }

            //creating group if exactly one group size is less than others
            if (checkVertexAmount == 2)
            {
                int OneBonus = rnd.Next(1, 3);
                int TwoBonus = rnd.Next(1, 3);

                while (TwoBonus == OneBonus)
                {
                    TwoBonus = rnd.Next(1, 3);
                }

                for (int i = 1; i < 4; i++)
                {
                    for (int j = 0; j < normalNumber; j++)
                    {
                        int index = rnd.Next(0, GroupSize);

                        if (Group[index] == 0)
                        {
                            Group[index] = i;
                        }
                        else
                        {
                            j--;
                        }
                    }
                }

                for (int i = 0; i < GroupSize; i++)
                {
                    if (Group[i] == 0)
                    {
                        Group[i] = OneBonus;
                        break;
                    }
                }

                for (int i = 0; i < GroupSize; i++)
                {
                    if (Group[i] == 0)
                    {
                        Group[i] = TwoBonus;
                    }
                }

            }

            return Group;
        }

        //crossover function, swap random two elements
        public void Crossover(double[] Group)
        {
            TRandom rnd = new TRandom();
            int index = rnd.Next(0, Group.Length);
            int index2 = rnd.Next(0, Group.Length);

            while (index == index2)
            {
                index2 = rnd.Next(0, Group.Length);
            }

            var value = Group[index];
            Group[index] = Group[index2];
            Group[index2] = Group[index];
        }

        //mutation function, changing one value in unit, checking if group is correct
        public void Mutation(double[] Group)
        {
            TRandom rnd = new TRandom();
            int MutationNumber = rnd.Next(1, 3);
            bool works = false;

            while (works == false)
            {
                int MutationIndex = rnd.Next(0, Group.Length);
                double Prev = Group[MutationIndex];
                Group[MutationIndex] = MutationNumber;
                var check = CheckGroup(Group);

                if (check == false)
                {
                    Group[MutationIndex] = Prev;
                }
                else
                {
                    works = true;
                }
            }
        }

        //function to check if the size of each group in unit is correct
        public bool CheckGroup(double[] Group)
        {
            bool GroupIsValid = true;
            int CountOne = 0;
            int CountTwo = 0;
            int CountThree = 0;
            decimal MaxVertex = Math.Floor((decimal)Group.Length / 3);

            for (int i = 0; i < Group.Length; i++)
            {
                if (Group[i] == 1)
                {
                    CountOne++;
                }
                if (Group[i] == 2)
                {
                    CountTwo++;
                }
                if (Group[i] == 3)
                {
                    CountThree++;
                }
            }

            if (CountOne > MaxVertex + 1 || CountTwo > MaxVertex + 1 || CountThree > MaxVertex + 1)
            {
                GroupIsValid = false;
            }

            return GroupIsValid;
        }

        //its KB functions but imo it does nothing
        public double[] SumOfWeights()
        {
            //var result = CalculateFitness(CreateInitialGroup(parameters.NumberOfVertices), parameters);
            return parameters.FitnessArray;
        }

        //function to count edges
        public int[] EdgeCounts()
        {
            var population = parameters.Population;
            var result = new int[population[0].Length];

            for (int i = 0; i < population[0].Length; i++)
            {
                //var edgeCount = 0;
                var groupGraphs = GraphGenerationMethods.GenerateGraphFromCaran(population, i).
                    Where(x => x != null).
                    Where(x => x.VertexCount != 0);
                groupGraphs.ToList().ForEach(x => EdgeMethod.ConnectAllVertices(x));
                var edgeCount = groupGraphs.Select(x => x.EdgeCount).Sum();

                result[i] = edgeCount;
            }

            return result;
        }
    }
}