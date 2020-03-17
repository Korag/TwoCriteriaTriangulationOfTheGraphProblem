using euler_graph_generator.GraphElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace euler_graph_generator.AdditionalMethods
{
    public static class FileSaver
    {
        public static string[] tempData;
        public static bool firstTime;

        public static void SaveToFile(Graph graph, double probability, double[][] incidenceMatrix, bool isConsistent, string isEuler, string message, bool deleteFile, List<int> eulerPath)
        {
            string filePath = "output.txt";

            if (deleteFile)
            {
                File.WriteAllText(filePath, "");
            }
            else
            {
                File.AppendAllText(filePath, "");
            }


            if (firstTime == false)
            {
                for (int i = 0; i < tempData.Length; i++)
                {
                    File.AppendAllText(filePath, tempData[i] + "\r\n");
                }
                if (message == "przed naprawą")
                {
                    return;
                }
            }


            string CurrentDate = "Date: " + DateTime.Now + "\r\n";
            string School = "Akademia Techniczno-Humanistyczna w Bielsku-Białej - 50 lat tradycji \r\n";
            string WorkingGroup = "Łukasz Czepielik, Kamil Haręża, Konrad Korzonkiewicz, Bartosz Wróbel \r\n\r\n";

            File.AppendAllText(filePath, CurrentDate);
            if (new FileInfo(filePath).Length < 30)
            {
                File.AppendAllText(filePath, School);
                File.AppendAllText(filePath, WorkingGroup);
            }



            string IsConsistent = isConsistent ? "TAK" : "NIE";

            File.AppendAllText(filePath, "Stan grafu: " + message + "\r\n");
            File.AppendAllText(filePath, "Ilość wierzchołków w grafie: " + graph.Vertices.Count() + "\r\n");
            File.AppendAllText(filePath, "Ilość krawędzi w grafie: " + graph.Edges.Count() + "\r\n");
            File.AppendAllText(filePath, "Prawdopodobieństwo utworzenia krawędzi: " + probability + "\r\n");
            File.AppendAllText(filePath, "Graf Spójny: " + IsConsistent + "\r\n");
            File.AppendAllText(filePath, "Graf Eulerowski: " + isEuler + "\r\n\r\n");




            File.AppendAllText(filePath, "Macierz incydencji " + message + ": \r\n\r\n");

            string Matrix = "";
            if (incidenceMatrix != null)
            {
                for (int i = 0; i < incidenceMatrix.Length; i++)
                {
                    for (int j = 0; j <= incidenceMatrix.Length; j++)
                    {
                        Matrix += incidenceMatrix[i][j] + " ";
                    }
                    Matrix += "\r\n";
                }
            }


            File.AppendAllText(filePath, Matrix);
            File.AppendAllText(filePath, "\r\n\r\n");
            string eulerPathString = "";
            File.AppendAllText(filePath, "Ścieżka/cykl eulera " + message + ": \r\n\r\n");
            if (eulerPath.Count >= 1)
            {
                for (int i = 0; i < eulerPath.Count; i++)
                {
                    if (i != eulerPath.Count - 1)
                    {
                        eulerPathString += eulerPath[i] + " => ";
                    }
                    else
                    {
                        eulerPathString += eulerPath[i];
                    }


                }

                File.AppendAllText(filePath, eulerPathString);
                File.AppendAllText(filePath, "\r\n\r\n");
            }
            else
            {
                File.AppendAllText(filePath, "Graf nie posiada ścieżki/cyklu eulera");
                File.AppendAllText(filePath, "\r\n\r\n");
            }

            if (firstTime == true)
            {
                tempData = File.ReadAllLines(filePath);
                File.WriteAllText(filePath, "");
            }

        }
    }
}
