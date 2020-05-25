using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine("Number of vertices in the graph: " + parameters.NumberOfVertices);
            txt.WriteLine("Number of iterations: " + parameters.IterationsLimit);

            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine("Incidence Matrix: ");
            SaveMatrixToFile(parameters.incidenceMatrix, txt);
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine("Weights Matrix: ");
            SaveMatrixWithDigits(parameters.weightsMatrix,txt);
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine("Iteracja 1");
            SaveMatrixToFile(parameters.Population,txt);
            SaveFitness(parameters.FitnessArray, txt);
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine();
            for (int i = 0; i < parameters.MatrixToSave.Count; i++)
            {
                
                txt.WriteLine("Iteracja "+parameters.IterationNumber);
                var MatrixFromGeneticAlg = parameters.MatrixToSave[i];
                SaveMatrixToFile(MatrixFromGeneticAlg, txt);
                txt.WriteLine("Fitness Pokolenia:");
                var FitnessFromGeneticAlg = parameters.FitnessesToSave[i];
                SaveFitness(FitnessFromGeneticAlg, txt);
                txt.WriteLine();
                txt.WriteLine();
                txt.WriteLine();
            }




            txt.Close();

        }

        public void SaveMatrixToFile(double[][] Matrix,StreamWriter txt)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(0); j++)
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
