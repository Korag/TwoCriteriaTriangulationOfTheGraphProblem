using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TwoCriteriaTriangulationOfTheGraphProblem
{
    public class FileSaver
    {

        public void SaveToFileAsync(Parameters parameters)
        {

            string filePath = "C:\\Users\\Dominik\\Desktop\\lol\\File.txt";
            if (File.Exists("C:\\Users\\Dominik\\Desktop\\lol\\File.txt"))
            {
                File.WriteAllText(filePath, " ");
            }


            string CurrentDate = "Date: " + DateTime.Now + "\r\n";
            string School = "University of Bielsko-Biala \r\n";
            string WorkingGroup = "Łukasz Czepielik, Dominik Pezda, Konrad Boroń \r\n\r\n";
            StreamWriter txt = new StreamWriter("C:\\Users\\Dominik\\Desktop\\lol\\File.txt"); //sciezka do poprawy!!!!!!
            txt.Write(CurrentDate);
            txt.Write(School);
            txt.Write(WorkingGroup);

            txt.WriteLine();
            txt.WriteLine("Number of vertices in the graph: " + parameters.NumberOfVertices);
            txt.WriteLine("Number of iterations: " + parameters.IterationsLimit);

            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine("Incidence Matrix: ");
            SaveIncidenceMatrixToFile(parameters.incidenceMatrix, txt);
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine("Weights Matrix: ");
            SaveWeightMatrixToFile(parameters.weightsMatrix, txt);
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine("Vertex belonging to groups for each iteration:");
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine("Start Population");
            SaveMatrixToFile(parameters.Population, txt, parameters.Popsize);
            txt.WriteLine("Generation Fitness");
            SaveFitness(parameters.FitnessArray, txt);
            for (int i = 0; i < parameters.MatrixToSave.Count; i++)
            {

                txt.WriteLine("Iteration " + (i + 1));
                var MatrixFromGeneticAlg = parameters.MatrixToSave[i];
                SaveMatrixToFile(MatrixFromGeneticAlg, txt, parameters.Popsize);
                txt.WriteLine("Generation Fitness:");
                var FitnessFromGeneticAlg = parameters.FitnessesToSave[i];
                SaveFitness(FitnessFromGeneticAlg, txt);
                txt.WriteLine();
                txt.WriteLine("Best Triangulation Groups");
                GetBestUnit(MatrixFromGeneticAlg, FitnessFromGeneticAlg, txt);
            }




            txt.Close();

        }

 


        public void GetBestUnit(double[][] GenAlgMatrix, double[] Fitness, StreamWriter txt)
        {
            List<List<int>> Groups = new List<List<int>>();
            List<int> Group1 = new List<int>();
            List<int> Group2 = new List<int>();
            List<int> Group3 = new List<int>();
            double min = double.PositiveInfinity;
            int GroupID = 0;
            for (int i = 0; i < Fitness.Length; i++)
            {
                if (Fitness[i] < min)
                {
                    min = Fitness[i];
                    GroupID = i;
                }
            }

            for (int i = 0; i < GenAlgMatrix.GetLength(0); i++)
            {
                if (GenAlgMatrix[i][GroupID] == 1)
                {
                    Group1.Add(i);
                }
                if (GenAlgMatrix[i][GroupID] == 2)
                {
                    Group2.Add(i);
                }
                if (GenAlgMatrix[i][GroupID] == 3)
                {
                    Group3.Add(i);
                }
            }
            Groups.Add(Group1);
            Groups.Add(Group2);
            Groups.Add(Group3);

            for (int i = 0; i < Groups.Count; i++)
            {
                txt.Write("Group " + i+ " Vertices: ");
                var CurGroup = Groups[i];
                for (int j = 0; j < Groups[i].Count; j++)
                {

                    txt.Write(CurGroup[j] + " ");
                }
                txt.WriteLine();
            }

            txt.WriteLine("Fitness of best unit: " + min);
            txt.WriteLine();
            txt.WriteLine();


        }
        public void SaveWeightMatrixToFile(double[][] Matrix, StreamWriter txt)
        {


            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                txt.Write("Vertex \t " + i + ":   ");
                for (int j = 0; j < Matrix.GetLength(0); j++)
                {


                    txt.Write(Matrix[i][j].ToString("00.00") + " ");
                }
            txt.WriteLine();
        }
        }
        public void SaveIncidenceMatrixToFile(double[][] Matrix, StreamWriter txt)
        {


            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                txt.Write("Vertex \t " + i + ":   ");
                for (int j = 0; j < Matrix.GetLength(0); j++)
                {


                    txt.Write(Matrix[i][j]+ " ");
                }
                txt.WriteLine();
            }
        }


        public void SaveMatrixToFile(double[][] Matrix,StreamWriter txt,int Population)
        {
          
            
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                txt.Write("Vertex \t " + i+":   ");
                for (int j = 0; j < Population ; j++)
                {
                    
                    
                    txt.Write(Matrix[i][j]+ " ");
                }
                txt.WriteLine();
            }
        }
        public void SaveMatrixWithDigits(double[][] Matrix, StreamWriter txt)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                txt.Write("Vertex \t " + i + ":   ");
                for (int j = 0; j < Matrix.GetLength(0); j++)
                {
     
                    txt.Write(Matrix[i][j].ToString("00.00") + " ");
                }
                txt.WriteLine();
            }
        }
        public void SaveFitness(double[] Array, StreamWriter txt)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                txt.Write(Array[i] + " ");
            }
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine();
        }
    }
}
