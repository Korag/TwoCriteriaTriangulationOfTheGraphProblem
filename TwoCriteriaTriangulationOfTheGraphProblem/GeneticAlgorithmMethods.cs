using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;

namespace TwoCriteriaTriangulationOfTheGraphProblem
{
    public class GeneticAlgorithmMethods
    {
        
        double BestFitnessScore= double.PositiveInfinity;
        double PopulationAverage=0;
        TRandom MutationProc = new TRandom();
        TRandom CrossoverProc = new TRandom();
        public void GeneticAlgorithm(Parameters parameters)//double[][] WeightMatrix, double[][] GraphMatrix, int Iterations)
        {
           
            int vertexAmount = parameters.NumberOfVertices;//GraphMatrix.GetLength(0);
            
            double[] BestGroups = new double[parameters.Popsize];
            double[][] Population = new double[vertexAmount][];
            double[] PopulationFitness = new double[vertexAmount];
            
            for (int i = 0; i < vertexAmount; i++)
            {
                Population[i] = new double[parameters.Popsize];
            }
            var InitialGroup = CreateInitialGroup(vertexAmount);
            BestFitnessScore=CalculateFitness(parameters.weightsMatrix, parameters.incidenceMatrix, InitialGroup);
            var InitialPopulation=CreateInitialPopulation(vertexAmount, parameters.Popsize,InitialGroup);

            parameters.Population = InitialPopulation;
            for (int i = 0; i < parameters.IterationsLimit; i++)
            {
                
                CompetetiveSelection(parameters);
                var test = parameters.Population;
                CreateNewPopulation(parameters);
            }


            
        }


        public void CreateNewPopulation(Parameters parameters)
        {
            int Winners = parameters.Popsize / 2;
            for (int k = Winners; k < parameters.Popsize; k++)
            {
                var NewGroup = CreateGroup(parameters.NumberOfVertices);
                for (int j = 0; j < NewGroup.Length; j++)
                {
                    parameters.Population[j][k] = NewGroup[j];
                }
            }
            for (int i = 0; i < parameters.Popsize; i++)
            {
                double CheckIfMutationHappen = MutationProc.Next(0, 100);
                double CheckIfCrossOverHappen = CrossoverProc.Next(0, 100);
                if (CheckIfMutationHappen <= parameters.MutationProbabilityValue)
                {
                    Mutation(parameters.Population[i]);
                }
                if (CheckIfCrossOverHappen <= parameters.CrossoverProbabilityValue)
                {
                    Crossover(parameters.Population[i]);
                }
            }
        }
        public void CompetetiveSelection(Parameters parameters)
        {
            //Population[10][5000]
            //GroupX[10]
            int NewGroupIndex = 0;
            double[] Group1 = new double[parameters.NumberOfVertices];
            double[] Group2 = new double[parameters.NumberOfVertices];
            for (int i = 0; i < parameters.Popsize;i+=2)
            {
                for (int k = 0; k < parameters.Popsize; k++)
                {
                    for (int w = 0; w < parameters.NumberOfVertices; w++)
                    {
                        Group1[w] = parameters.Population[w][i];
                        Group2[w] = parameters.Population[w][i + 1];
                    }
                    double FitnenssFirstGroup = CalculateFitness(parameters.weightsMatrix, parameters.incidenceMatrix, Group1);
                    double FitnessSecondGroup = CalculateFitness(parameters.weightsMatrix, parameters.incidenceMatrix, Group2);
                    
                    if (FitnenssFirstGroup > FitnessSecondGroup)
                    {
                        for (int p = 0; p < parameters.NumberOfVertices; p++)
                        {
                            parameters.Population[p][NewGroupIndex] = Group1[p];
                        }
                    }
                    else
                    {
                        for (int p = 0; p < parameters.NumberOfVertices; p++)
                        {
                            parameters.Population[p][NewGroupIndex] = Group2[p];
                        }
                    }
                    var test = parameters.Population;
                    
                }
                NewGroupIndex++;
            }
        }



        public double GetGroupFitnessValue(double[][] WeightMatrix, double[][] GraphMatrix, List<double> Group)
        {
            double FitnessValue = 0;
            for (int i = 0; i < GraphMatrix.GetLength(0); i++)
            {
                if (Group.Contains(i)) 
                { 
                for (int j = 0; j < GraphMatrix.GetLength(0); j++)
                    {
                        if (Group.Contains(j))
                        {
                            FitnessValue += GraphMatrix[i][j] * WeightMatrix[i][j];
                        }
                    }
                }
            }
            return FitnessValue;
        }

        public double CalculateFitness(double[][] WeightMatrix, double[][] GraphMatrix, double[] Group) //XX
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

            double Group1FitnessValue = GetGroupFitnessValue(WeightMatrix, GraphMatrix, Group1);
            double Group2FitnessValue = GetGroupFitnessValue(WeightMatrix, GraphMatrix, Group2);
            double Group3FitnessValue = GetGroupFitnessValue(WeightMatrix, GraphMatrix, Group3);

            double CurrentGroupsFitnessScore = Group1FitnessValue + Group2FitnessValue + Group3FitnessValue;
            if(PopulationAverage==0)
            {
                PopulationAverage = CurrentGroupsFitnessScore;
            }
            
            return CurrentGroupsFitnessScore;
        }
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
        public double[][] CreateInitialPopulation(int VertexAmount, int GenerationSize, double[] InitialGroup)
        {
            TRandom rnd = new TRandom();
            double[][] NewPopulation = new double[VertexAmount][];

            for (int i = 0; i < VertexAmount; i++)
            {
                NewPopulation[i] = new double[GenerationSize];
            }
            for (int i = 0; i < InitialGroup.Length; i++)
            {
                NewPopulation[i][0] = InitialGroup[i];
            }

            for (int k = 1; k < GenerationSize; k++)
            {
                var NewGroup = CreateGroup(VertexAmount);
                for (int j = 0; j < NewGroup.Length; j++)
                {
                    NewPopulation[j][k] = NewGroup[j];
                }
            }

            return NewPopulation;
        }

        public double[] CreateGroup(int GroupSize)
        {
            TRandom rnd = new TRandom();
            double[] Group = new double[GroupSize];
            for (int i = 0; i < GroupSize; i++)
            {
                Group[i] = 0;
            }
            int checkVertexAmount = GroupSize % 3;
            int normalNumber = (GroupSize - checkVertexAmount) / 3;
            if (checkVertexAmount == 0)
            {
                for (int i = 1; i < 3; i++)
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
            if (checkVertexAmount == 2)
            {
                int OneBonus = rnd.Next(1, 3);
                int TwoBonus = rnd.Next(1, 3);
                while (TwoBonus == OneBonus)
                {
                    TwoBonus = rnd.Next(1, 3);
                }
                for (int i = 1; i < 3; i++)
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
    }
}











//for (int i = 0; i < GenerationSize; i++)
//{
//    int index=rnd.Next(0, VertexAmount);
//    var value = NewPopulation[k][i];
//    NewPopulation[k][i] = NewPopulation[k][index];
//    NewPopulation[k][index] = value;
//}




//Są tutaj wszystkie funkcje do algorytmu genetycznego, ale trzeba każdą po kolei przerobić
#region Algorytm genetyczny wyznaczający ParetoFront, ale dla innego problemu

//private void CheckDomain(ref List<double[]> PointsOutsideTheDomain, double[][] ListOfPoints)
//{
//    PointsOutsideTheDomain = new List<double[]>();
//    for (int i = 0; i < ListOfPoints.Length; i++)
//    {
//        if (ListOfPoints[i][0] < F1LeftConstraint || ListOfPoints[i][0] > F1RightConstraint)
//        {
//            PointsOutsideTheDomain.Add(ListOfPoints[i]);
//        }
//        //if (ListOfPoints[i][0] > F1RightConstraint)
//        //{
//        //    PointsOutsideTheDomain.Add(ListOfPoints[i]);
//        //}
//        if (ListOfPoints[i][1] < F2LeftConstraint || ListOfPoints[i][1] > F2RightConstraint)
//        {
//            PointsOutsideTheDomain.Add(ListOfPoints[i]);
//        }
//        //if (ListOfPoints[i][1] > F2RightConstraint)
//        //{
//        //    PointsOutsideTheDomain.Add(ListOfPoints[i]);
//        //}
//    }
//}

//private double[] FindMinAndMax(double[][] ArrayWithValues)
//{
//    double[] tempArray = new double[2];
//    double min = ArrayWithValues[0][0];
//    double max = ArrayWithValues[0][0];
//    for (int i = 0; i < ArrayWithValues.Length; i++)
//    {
//        if (min > ArrayWithValues[i][0])
//        {
//            min = ArrayWithValues[i][0];
//        }
//        if (max < ArrayWithValues[i][0])
//        {
//            max = ArrayWithValues[i][0];
//        }
//    }
//    tempArray[0] = min;
//    tempArray[1] = max;
//    return tempArray;
//}

//private void CheckParetoDomain(ref List<double[]> PointsOutsideTheDomain, double[][] ListOfPoints)
//{
//    List<double> yy = new List<double>();
//    PointsOutsideTheDomain = new List<double[]>();
//    double blabla = F1RightConstraint - F1LeftConstraint;
//    double blabla2 = blabla / ListOfPoints.Length;
//    for (double i = (double)F1LeftConstraint, j = (double)F2LeftConstraint; i <= F1RightConstraint; i += blabla2, j += blabla2)
//    {

//        yy.Add(((1 + j) / i));
//    }

//    double y = 0;
//    for (int i = 0; i < ListOfPoints.Length; i++)
//    {
//        y = (1 + ListOfPoints[i][1]) / ListOfPoints[i][0];
//        //if (ListOfPoints[i][0] < F1LeftConstraint)
//        //{
//        //    PointsOutsideTheDomain.Add(ListOfPoints[i]);
//        //}
//        //if (ListOfPoints[i][0] > F1RightConstraint)
//        //{
//        //    PointsOutsideTheDomain.Add(ListOfPoints[i]);
//        //}
//        if (ListOfPoints[i][1] < yy[i])
//        {
//            PointsOutsideTheDomain.Add(ListOfPoints[i]);
//        }
//        if (ListOfPoints[i][1] > (yy[i] + (F2RightConstraint / F1LeftConstraint)))
//        {
//            PointsOutsideTheDomain.Add(ListOfPoints[i]);
//        }
//        //if (ListOfPoints[i][1] > F2RightConstraint)
//        //{
//        //    PointsOutsideTheDomain.Add(ListOfPoints[i]);
//        //}
//    }
//}

//private double Function2Value(double[] Chromosome)
//{
//    double FunctionValue = 0;
//    FunctionValue = ((1 + Chromosome[1]) / Chromosome[0]);
//    if (FunctionValue > 80)
//    {
//        FunctionValue = 80;
//    }
//    if (FunctionValue < -30)
//    {
//        FunctionValue = -30;
//    }
//    return FunctionValue;
//}



//private void Function2ValueCountForAllPopulation(double[][] Population, ref double[][] PopulationFunctionValue)
//{
//    for (int i = 0; i < Population.Length; i++)
//    {
//        PopulationFunctionValue[i][0] = Population[i][0];
//        PopulationFunctionValue[i][1] = Function2Value(Population[i]);
//    }
//}

//private void SearchForMinValue(double[][] PopulationAfterCrossing, ref double MinF1, ref double MinF2)
//{
//    MinF1 = PopulationAfterCrossing[0][0];
//    MinF2 = PopulationAfterCrossing[0][1];

//    for (int i = 0; i < PopulationAfterCrossing.Length; i++)
//    {
//        if (PopulationAfterCrossing[i][0] < MinF1)
//        {
//            MinF1 = PopulationAfterCrossing[i][0];
//        }
//        if (PopulationAfterCrossing[i][1] < MinF2)
//        {
//            MinF2 = PopulationAfterCrossing[i][1];
//        }
//    }
//}

//private void FillRandomValues(ref double[][] Population)
//{
//    for (int i = 0; i < Population.Length; i++)
//    {
//        if (refill)
//        {
//            for (int j = Pop_Size / 4; j < Pop_Size; j++)
//            {
//                Population[j] = new double[2];

//                Population[j][0] = trandom.NextDouble(F1LeftConstraint, F1RightConstraint);
//                Population[j][1] = trandom.NextDouble(F2LeftConstraint, F2RightConstraint);
//            }
//            break;
//        }
//        Population[i][0] = trandom.NextDouble(F1LeftConstraint, F1RightConstraint);
//        Population[i][1] = trandom.NextDouble(F2LeftConstraint, F2RightConstraint);

//    }
//}

//private void InitializePopulation(ref double[][] Population, int popsize)
//{
//    Population = new double[popsize][];
//    for (int i = 0; i < Population.Length; i++)
//    {
//        Population[i] = new double[2];
//    }
//}

//private void Selection(double[][] Population, ref double[][] PopulationAfterSelection)
//{
//    HashSet<int> numbers1 = new HashSet<int>();
//    HashSet<int> numbers2 = new HashSet<int>();

//    if (refill == true)
//    {
//        for (int j = Pop_Size / 4; j < Pop_Size; j++)
//        {
//            PopulationFunctionValue[j] = new double[2];

//            PopulationFunctionValue[j][0] = Population[j][0];
//            PopulationFunctionValue[j][1] = Function2Value(Population[j]);
//        }
//    }
//    else
//    {
//        Function2ValueCountForAllPopulation(Population, ref PopulationFunctionValue);
//    }


//    for (int i = 0; i < Pop_Size / 4; i++)
//    {

//        int trandom1 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));
//        int trandom2 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));

//        while (!numbers1.Contains(trandom1))
//        {
//            trandom1 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));
//            numbers1.Add(trandom1);
//        }

//        while (!numbers2.Contains(trandom2))
//        {
//            trandom2 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));
//            numbers2.Add(trandom2);
//        }

//        if (PopulationFunctionValue[trandom1][0] < PopulationFunctionValue[trandom2][0])
//        {
//            PopulationAfterSelection[i][0] = Population[trandom1][0];
//            PopulationAfterSelection[i][1] = Population[trandom1][1];
//        }
//        else
//        {
//            PopulationAfterSelection[i][0] = Population[trandom2][0];
//            PopulationAfterSelection[i][1] = Population[trandom2][1];
//        }

//    }
//    for (int i = Pop_Size / 4; i < Pop_Size / 2; i++)
//    {
//        int trandom1 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));
//        int trandom2 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));

//        while (!numbers1.Contains(trandom1))
//        {
//            trandom1 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));
//            numbers1.Add(trandom1);
//        }


//        while (!numbers2.Contains(trandom2))
//        {
//            trandom2 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));
//            numbers2.Add(trandom2);
//        }


//        if (PopulationFunctionValue[trandom1][1] < PopulationFunctionValue[trandom2][1])
//        {
//            PopulationAfterSelection[i][0] = Population[trandom1][0];
//            PopulationAfterSelection[i][1] = Population[trandom1][1];
//        }
//        else
//        {
//            PopulationAfterSelection[i][0] = Population[trandom2][0];
//            PopulationAfterSelection[i][1] = Population[trandom2][1];
//        }
//    }
//}

//private void Mutation(double[][] tempTab, ref double[][] resultTable)
//{
//    double Angle = 0;
//    double Radius = 0;
//    for (int i = 0; i < resultTable.Length; i++)
//    {
//        //jesli wylosowana liczba jest mniejsza niz prawdopodobienstwo mutacji to mutujemy(losujemy liczbe z 
//        //przedziału (0,1) wiec dla prawdopodobienstwa mutacji = 1 mutacja występuje za każdym razem)
//        if (trandom.NextDouble(0, 1) < _parameters.PlausOfMutation)
//        {
//            Angle = trandom.NextDouble(0, 2 * Math.PI);
//            Radius = trandom.NextDouble(0, 1);
//            resultTable[i][0] = Math.Cos(Angle) * Radius + tempTab[i][0];
//            resultTable[i][1] = Math.Sin(Angle) * Radius + tempTab[i][1];

//            //if (CheckPointsDomain(resultTable[i][0], resultTable[i][1]))
//            //{
//            //    resultTable[i][1] += 20;
//            //}



//            //sprawdzenie czy wychodzi z dziedziny jak tak to albo losujemy od nowa r i kąt albo dzielimy je przez 2 albo funkcja kary
//            //while (CheckPointsDomain(tempTab[i][0], tempTab[i][1]))
//            //{
//            //    //powtórne losowanie
//            //    //Angle = trandom.NextDouble(0, 2 * Math.PI);
//            //    //Radius = trandom.NextDouble(0, 1);

//            //    //dzielenie przez dwa
//            //    Angle /= 2;
//            //    Radius /= 2;
//            //    tempTab[i][0] = Math.Cos(Angle) * Radius + tempTab[i][0];
//            //    tempTab[i][1] = Math.Sin(Angle) * Radius + tempTab[i][1];
//            //
//        }
//        else
//        {
//            resultTable[i][0] = tempTab[i][0];
//            resultTable[i][1] = tempTab[i][1];
//        }
//    }
//}

//public void Crossing(double[][] Population, ref double[][] PopulationAfterCrossing)
//{
//    HashSet<int> numbers1 = new HashSet<int>();
//    HashSet<int> numbers2 = new HashSet<int>();


//    for (int i = 0; i < Pop_Size / 4; i++)
//    {
//        if (trandom.NextDouble(0, 1) < _parameters.PlausOfCrossing)
//        {
//            int trandom1 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));
//            int trandom2 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));

//            while (!numbers1.Contains(trandom1))
//            {
//                trandom1 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));
//                numbers1.Add(trandom1);
//            }

//            while (!numbers2.Contains(trandom2))
//            {
//                trandom2 = (int)(trandom.NextUInt(0, (uint)Pop_Size / 2));
//                numbers2.Add(trandom2);
//            }


//            // tabela pomocnicza jednowymiarowa z wyliczonymi współrzędnymi nowego osobnika
//            double[] tab = CreatePointUsingLines(Population[trandom1][0], Population[trandom1][1], Population[trandom2][0], Population[trandom2][1], 0, 0.0001, 0.1, 0.9);

//            PopulationAfterCrossing[i][0] = tab[0];
//            PopulationAfterCrossing[i][1] = tab[1];

//            PopulationFunctionValueAfterCrossing[i][0] = PopulationAfterCrossing[i][0];
//            PopulationFunctionValueAfterCrossing[i][1] = Function2Value(PopulationAfterCrossing[i]);

//            if (CheckPointsDomain(PopulationAfterCrossing[i][0], PopulationAfterCrossing[i][1]))
//            {
//                if (PopulationFunctionValueAfterCrossing[i][0] < 0)
//                {
//                    PopulationAfterCrossing[i][0] = -PopulationAfterCrossing[i][0];
//                }

//                if (PopulationFunctionValueAfterCrossing[i][1] < 0)
//                {
//                    PopulationAfterCrossing[i][1] = -PopulationAfterCrossing[i][1];
//                }

//                if (PopulationAfterCrossing[i][0] > 20)
//                {
//                    PopulationAfterCrossing[i][0] = PopulationAfterCrossing[i][0] / 2;
//                }

//                if (PopulationAfterCrossing[i][1] > 20)
//                {
//                    PopulationAfterCrossing[i][1] = PopulationAfterCrossing[i][1] / 2;
//                }


//                PopulationFunctionValueAfterCrossing[i][0] = PopulationAfterCrossing[i][0];
//                PopulationFunctionValueAfterCrossing[i][1] = Function2Value(PopulationAfterCrossing[i]);

//                PopulationFunctionValueAfterCrossing[i][0] += 100;
//                PopulationFunctionValueAfterCrossing[i][1] += 100;
//            }
//        }
//        else
//        {
//            PopulationAfterCrossing[i][0] = Population[i][0];
//            PopulationAfterCrossing[i][1] = Population[i][1];

//            PopulationFunctionValueAfterCrossing[i][0] = PopulationAfterCrossing[i][0];
//            PopulationFunctionValueAfterCrossing[i][1] = Function2Value(PopulationAfterCrossing[i]);
//        }
//    }
//}


////Funkcja do tworzenia dziecka z 2 osobników: xa - pierwsza współrzędna z pierwszego osobnika,xb - druga współrzędna z pierwszego osobnika,ya - pierwsza współrzędna z drugiego osobnika,yb - druga współrzędna z drugiego osobnika
////BeginOdlegloscOdProstej i EndOdlegloscOdProstej - ustalanie przedziału w jakim ma być losowana odległość prostej równoległej od prostej przechodzącej przez dwa punkty,BeginWartoscX i EndWartoscX - musi być jakiś przedział dla wylosowania x-owej punktu na równoległej 
//double[] CreatePointUsingLines(double xa, double xb, double ya, double yb, double BeginOdlegloscOdProstej, double EndOdlegloscOdProstej, double BeginWartoscX, double EndWartoscX)
//{
//    double[] tab = new double[2];
//    tab[0] = trandom.NextDouble(BeginWartoscX, EndWartoscX);
//    double a = trandom.NextDouble(BeginOdlegloscOdProstej, EndOdlegloscOdProstej);

//    // Piękna funkcja na prostą przechodzącą przez dwa punkty, tab[0] to losowy x dla tworzonego punktu, tab[1] to y nowego punktu liczony na podstawie wzoru
//    //tab[1] = (((ya - yb) / (xa - xb)) * tab[0]) + ((ya - ((ya - yb) / (ya - xb))) * xa);


//    tab[1] = ((yb - ya) * (tab[0] - xa) + ya * xb - ya * xb) / (xb - xa);
//    return tab;
//}

#endregion

//Poniżej znajduje się funkcja sterująca algorytmem genetycznym wraz z wyznaczaniem ParetoFrontu
#region BackgroundWorker dla algorytmu genetycznego

//private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) { if (!worker.IsBusy) Start.IsEnabled = true; }


//private void worker_DoWork(object sender, DoWorkEventArgs e)
//{
//    for (int i = 0; i < Pop_Size; i++)
//    {
//        Thread.Sleep((int)(Sleeper * 100));
//        _parameters.ListOfPoints.Add(new ObservablePoint(trandom.NextDouble(F1LeftConstraint, F1RightConstraint),
//                                                        trandom.NextDouble(F2LeftConstraint, F2RightConstraint)
//                                                        ));
//        DomainChart.EditBSeriesCollection(_parameters.ListOfPoints);
//    }



//    while (_parameters.IterationNumber < _parameters.IterationLimit && !_Stop)
//    {
//        // inicjalizacja tablic
//        InitializePopulation(ref PopulationAfterSelection, Pop_Size / 2);
//        InitializePopulation(ref PopulationAfterMutation, Pop_Size / 2);
//        InitializePopulation(ref PopulationAfterCrossing, Pop_Size / 4);

//        InitializePopulation(ref PopulationFunctionValue, Pop_Size);
//        InitializePopulation(ref PopulationFunctionValueAfterSelection, Pop_Size / 2);
//        InitializePopulation(ref PopulationFunctionValueAfterCrossing, Pop_Size / 4);

//        // czas na obserwacje popsize'a i wykresu wartosci funkcji

//        // wszystkie operacje wykonujemy na tablicy znajdujacej sie w Parameters   public double[][][] Population; // [2][popsize][popsize]

//        FillRandomValues(ref Population);

//        // operacja selekcji

//        // 2 warianty i tutaj trzeba zrobic ze na zasadzie losowej jest wybierana metoda selekcji

//        // selekcja turniejowa --> losujemy 2 punkty z populacji i wygrywa lepszy (o mniejszej wartosci), 
//        // dzielimy popsize na 2 rowne zbiory i jeden zbior jest porownywany wzgledem f1 a drugi wzgledem f2



//        Selection(Population, ref PopulationAfterSelection);

//        // parametr, aby w nastepnej iteracji uzupelnic brakujace osobniki w populacji
//        refill = true;


//        Function2ValueCountForAllPopulation(PopulationAfterSelection, ref PopulationFunctionValueAfterSelection);



//        // selekcja ruletkowa --> obliczamy fitness, jaki to jest procent z calosci dla danego osobnika, obliczamy dystrybuante,
//        // generujemy liczby losowe i szeregujemy okreslajac ktore elementy maja przetrwac

//        // mozna tutaj juz wrzucic te osobniki na wykres dziedziny



//        _parameters.RewriteThePoints(PopulationAfterSelection);

//        DomainChart.EditASeriesCollection(_parameters.ListOfPoints);

//        CheckDomain(ref PopulationOutsideTheDomain, PopulationAfterSelection);
//        _parameters.RewriteThePoints(PopulationOutsideTheDomain);
//        DomainChart.SetPointsOutsideTheDomain(_parameters.ListOfPoints);


//        _parameters.RewriteThePoints(PopulationFunctionValueAfterSelection);
//        ParetoChart.EditSeriesCollection(_parameters.ListOfPoints);

//        ParetoChart.MakeParetoFunctions(FindMinAndMax(PopulationFunctionValueAfterSelection));

//        CheckParetoDomain(ref PopulationOutsideTheDomain, PopulationFunctionValueAfterSelection);
//        _parameters.RewriteThePoints(PopulationOutsideTheDomain);

//        ParetoChart.SetPointsOutsideTheDomain(_parameters.ListOfPoints);



//        MainChart.EditSeriesCollection(PopulationFunctionValueAfterSelection, _parameters.IterationNumber);

//        // operacja
//        Mutation(PopulationAfterSelection, ref PopulationAfterMutation);


//        // losujemy ktore punkty zostana poddane mutacji (sprawdzamy wszystkie pod wzgledem prawdopodobienstwa) (prawdopodobienstwo mutacji dla kazdego osobnika)
//        // jezeli wylosowano osobnika to losujemy kat oraz dlugosc wektora
//        // sprawdzamy czy zmutowany osobnik znajduje sie w dziedzinie
//        // jezeli nie wykorzystujemy funkcje kary aby zwiekszyc wartosc osobnika

//        // mozna tutaj juz wrzucic te osobniki na wykres dziedziny


//        // operacja krzyzowania
//        // to jest chyba najtrudniejsza operacja, duzo pierdolenia z przeksztalceniami
//        // ogolnie to staramy sie tak skrzyzowac aby np calkowicie odbic jeden punkt 
//        // do dyskusji jak to robimy

//        Crossing(PopulationAfterMutation, ref PopulationAfterCrossing);

//        // mozna tutaj juz wrzucic te osobniki na wykres dziedziny


//        // obliczenie minimum
//        SearchForMinValue(PopulationFunctionValueAfterSelection, ref MinF1, ref MinF2);

//        _parameters.Minimum = $"{{{Math.Round(MinF1, 2)};{Math.Round(MinF2, 2)}}}";

//        // przepisywanie tablicy PopulationAfterCrossing do Population

//        Array.Clear(Population, 0, Population.Length);
//        Array.Copy(PopulationAfterCrossing, Population, PopulationAfterCrossing.Length);

//        // przepisywanie tablicy PopulationFunctionValueAfterCrossing do PopulationFunctionValue
//        Array.Clear(PopulationFunctionValue, 0, PopulationFunctionValue.Length);
//        Array.Copy(PopulationFunctionValueAfterCrossing, PopulationFunctionValue, PopulationFunctionValueAfterCrossing.Length);

//        // czyszczenie tablic
//        Array.Clear(PopulationAfterSelection, 0, PopulationAfterSelection.Length);
//        Array.Clear(PopulationAfterMutation, 0, PopulationAfterMutation.Length);
//        Array.Clear(PopulationAfterCrossing, 0, PopulationAfterCrossing.Length);

//        Array.Clear(PopulationFunctionValueAfterSelection, 0, PopulationFunctionValueAfterSelection.Length);
//        Array.Clear(PopulationFunctionValueAfterCrossing, 0, PopulationFunctionValueAfterCrossing.Length);

//        // finalne utworzenie wykresu dziedziny oraz pareto frontu a takze wykresu wartosci poszczegolnych funkcji

//        // jezeli przez 5 iteracji nie ma poprawy minimum zatrzymujemy algorytm

//        // musimy obliczyc jeszcze to spierdolone odchylenie
//        // suma po wszystkich punktach w populacji (od i do licznosci pareto frontu) |f(f1) - f2|

//        _parameters.IterationNumber++;
//        e.Cancel = true;
//        Thread.Sleep(4000);
//    }

#endregion
