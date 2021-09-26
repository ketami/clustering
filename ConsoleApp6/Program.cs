using System;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp6
{
    class Program
    {
        public const double q = 2;
        public const double StopQf = 10000;
        class Cluster
        {
            public string points;
            public double x;
            public double y;
            public double length;
        }
        static public int count = 0;
        static public double[] mins = new double[4];
        static public bool flag3 = false, flagexp = false, flag2 = false, flag4 = false,
            flag33 = false, flagexp3 = false, flag23 = false, flag43 = false;
        static void NewClusters(Cluster[] clusters, int len, double min, double[,] minDist)
        {
            for (int i = 0; i < len; i++)
            {

                for (int j = 0; j < len; j++)
                {
                    if (i < j && minDist[i, j] == min)
                    {
                        clusters[i].points = clusters[i].points + ", " + clusters[j].points;
                        clusters[i].x = (clusters[i].x) * clusters[i].length / (clusters[i].length + clusters[j].length) + (clusters[j].x) * clusters[j].length / (clusters[i].length + clusters[j].length);
                        clusters[i].y = (clusters[i].y) * clusters[i].length / (clusters[i].length + clusters[j].length) + (clusters[j].y) * clusters[j].length / (clusters[i].length + clusters[j].length);
                        clusters[i].length += clusters[j].length;
                        for (int z = j; z < (len - 1); z++)
                        {
                            clusters[z].points = clusters[z + 1].points;
                            clusters[z].x = clusters[z + 1].x;
                            clusters[z].y = clusters[z + 1].y;
                            clusters[z].length = clusters[z + 1].length;
                        }
                        len--;
                        MatDist(len, clusters);
                    }
                }
            }

        }

        static void Output(int len, Cluster[] clusters)
        {
            Console.WriteLine("Итерация " + count + " :");
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine(" Кластер " + (i + 1) + " = " + clusters[i].points);
            }

        }
        static void MatDist(int len, Cluster[] clusters)
        {
            if (flagexp == false | flag3 == false | flag4 == false |
            flag33 == false | flagexp3 == false | flag43 == false | flag2 == false | flag23 == false)
            {
                Output(len, clusters);
                double min = StopQf;
                double[,] MinDist = new double[len, len];
                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < len; j++)
                    {
                        if (i < j)
                        {
                            MinDist[i, j] = Math.Sqrt(Math.Pow((clusters[i].x - clusters[j].x), 2) + Math.Pow((clusters[i].y - clusters[j].y), 2));
                            if (MinDist[i, j] < min && MinDist[i, j] != 0)
                            {
                                min = MinDist[i, j];
                            }
                        }
                    }
                }
                Console.WriteLine(" Минимальное расстояние = " + min);
                if (min == StopQf)
                {
                    Console.WriteLine("Все точки объединились в один кластер. Нажмите любую клавишу для завершения программы.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                if (count < 4)
                {
                    mins[count] = min + count * q;
                }
                else
                {

                    Krit(mins[0], mins[1], mins[2], mins[3]);
                    mins[0] = mins[1];
                    mins[1] = mins[2];
                    mins[2] = mins[3];
                    mins[3] = min + count * q;

                }

                count++;
                NewClusters(clusters, len, min, MinDist);
            }
            else
            {
                Console.WriteLine("Выполнение программы завершено! Нажмите любую клавишу.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        static void Krit(double y0, double y1, double y2, double y3)
        {

            Krit33(y2 - y1, y3 - y1);
            Krit43(y2 - y1, y3 - y1);
            KritExp3(y2 - y1, y3 - y1);
            Krit23(y2 - y1, y3 - y1);

            Krit3(y1 - y0, y2 - y0, y3 - y0);
            KritExp(y1 - y0, y2 - y0, y3 - y0);
            Krit2(y1 - y0, y2 - y0, y3 - y0);
            Krit4(y1 - y0, y2 - y0, y3 - y0);
        }
        static void Krit33(double y1, double y2)
        {
            if (flag33 == false)
            {
                double s = 2 * y1 * y1 / 19 - 10 * y2 * y1 / 19 + 3 * y2 * y2 / 19;
                Console.Write("Кубический критерий по 3 точкам равен: " + s + "\n");
                if (s > 0)
                {
                    Console.WriteLine('\t' + "Кубический критерий по 3 точкам: кластеризация завершена.");
                    flag33 = true;
                }
            }
        }
        static void Krit43(double y1, double y2)
        {
            if (flag43 == false)
            {
                double s = 98 * y1 * y1 / 723 - (434 * y2 * y1) / 723 + 119 * y2 * y2 / 723;
                Console.Write("Биквадратный критерий по 3 точкам равен: " + s + "\n");
                if (s > 0)
                {
                    Console.WriteLine('\t' + "Биквадратный критерий по 3 точкам: кластеризация завершена.");
                    flag43 = true;
                }
            }
        }
        static void KritExp3(double y1, double y2)
        {
            if (flagexp3 == false)
            {
                double s = -0.622 * y1 * y1 + 0.334 * y1 * y2 - 0.045 * y2 * y2 + (y2 - 2 * y1) * (y2 - 2 * y1) / 6;
                Console.Write("Экспоненциальный критерий по 3 точкам равен: " + s + "\n");
                if (s > 0)
                {
                    Console.WriteLine('\t' + "Экспоненциальный критерий по 3 точкам: кластеризация завершена.");
                    flagexp3 = true;
                }
            }
        }
        static void Krit23(double y1, double y2)
        {
            if (flag23 == false)
            {
                double s = (2 * y1 * y1 - 14 * y2 * y1 + 5 * y2 * y2) / 39;
                Console.Write("Параболический критерий по 3 точкам равен: " + s + "\n");
                if (s > 0)
                {
                    Console.WriteLine('\t' + "Парабоолический критерий по 3 точкам: кластеризация завершена.");
                    flag23 = true;
                }
            }
        }
        static void Krit3(double y1, double y2, double y3)
        {
            if (flag3 == false)
            {
                double s = 81 * y1 * y1 / 940 + 63 * y1 * y2 / 470 - 147 * y3 * y1 / 470 - 9 * y2 * y2 / 188 + 45 * y3 * y3 / 188 - 177 * y2 * y3 / 470;
                Console.Write("Кубический критерий по 4 точкам равен: " + s + "\n");
                if (s > 0)
                {
                    Console.WriteLine('\t' + "Кубический критерий по 4 точкам: кластеризация завершена.");
                    flag3 = true;
                }
            }
        }
        static void KritExp(double y1, double y2, double y3)
        {
            if (flagexp == false)
            {
                double s = 0.001 * (66 * y1 * y1 + 2 * (59 * y2 - 100 * y3) * y1 - 49 * y2 * y2 + 227 * y3 * y3 - 346 * y2 * y3) - 0.059 * y1 * y3;
                Console.Write("Экспоненциальный критерий по 4 точкам равен: " + s + "\n");
                if (s > 0)
                {
                    Console.WriteLine('\t' + "Экспоненциальный критерий по 4 точкам: кластеризация завершена.");
                    flagexp = true;
                }
            }
        }
        static void Krit2(double y1, double y2, double y3)
        {
            if (flag2 == false)
            {
                double s = (19 * y1 * -11 * y2 * y2 + 41 * y3 * y3 + 12 * y1 * y2 - 64 * y1 * y3 - 46 * y2 * y3);
                Console.Write("Параболический критерий по 4 точкам равен: " + s + "\n");
                if (s > 0)
                {
                    Console.WriteLine('\t' + "Параболический критерий по 4 точкам: кластеризация завершена.");
                    flag2 = true;
                }
            }
        }
        static void Krit4(double y1, double y2, double y3)
        {
            if (flag4 == false)
            {
                double s = (1657 * y1 * y1 + 4206 * y1 * y2 - 6652 * y3 * y1 - 743 * y2 * y2 + 6023 * y3 * y3 - 11428 * y2 * y3) / 22085;
                Console.Write("Биквадратный критерий по 4 точкам равен: " + s + "\n");
                if (s > 0)
                {
                    Console.WriteLine('\t' + "Биквадратный критерий по 4 точкам: кластеризация завершена.");
                    flag4 = true;
                }
            }
        }
        static void Main(string[] args)
        {
            const int len = 33;
            Cluster[] clusters = new Cluster[len];
            double[] X = new double[len] { 0, 2, 3, 1, 3, 3, 1, 12, 13, 11, 13, 14, 11, 12, 13, 12, 13, 14, 12, 13, 14, 24, 22, 21, 23, 24, 22, 23, 24, 21, 2, 24, 10 };
            double[] Y = new double[len] { 0, 4, 3, 2, 0, 1, 1, 18, 17, 15, 14, 16, 16, 15, 18, 5, 2, 4, 3, 1, 2, 19, 22, 24, 21, 20, 39, 38, 39, 37, 26, 6, 36 };
            for (int i = 0; i < len; i++)
            {
                clusters[i] = new Cluster();
                clusters[i].points = Convert.ToString(i + 1);
                clusters[i].x = X[i];
                clusters[i].y = Y[i];
                clusters[i].length = 1;
            }
            MatDist(len, clusters);

        }
    }
}


