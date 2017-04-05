using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAntTSP
{
    class Program
    {
        /// <summary>
        /// Печать квадратной матрицы в консоль
        /// </summary>
        /// <param name="D">Квадратная матрица</param>
        private static void printMatrix(double[,] D)
        {
            for (int i = 0; i < D.GetLength(0); i++)
            {
                for (int j = 0; j < D.GetLength(1); j++)
                {
                    Console.Write("(" + i + "," + j + "): " + D[i,j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            //var N_MIN = 3;      // минимальное количество вершин
            //var N_MAX = 30;     // максимальное количество вершин
            var ALPHA = 1;      // вес фермента
            var BETA = 3;       // коэффициент эвристики
            var T_MAX = 100;    // время жизни колонии
            var M = 20;         // количество муравьев в колонии
            var Q = 100;        // количество феромона
            var RHO = 0.5;      // коэффициент испарения феромона


            //Количество вершин
            var N = 5;
            //Инициализация матрицы расстояний
            double[,] D = new double[N, N];

            //Создание нового генератора ПСП
            Random rnd = new Random();

            //Заполнение матрицы расстояний случайными значениями
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    //Расстояние из i в i равно 0, как изначально и есть
                    if (i != j)
                    {
                        //Если симметричное расстояние еще не заполнено
                        //Расстояние из i в j эквивалентно расстоянию из j в i 
                        if (D[j, i] == 0)
                        {
                            //Присваиваем случайное целочисленное расстояние из промежутка (вершины не совпадают, так что не с 0)
                            D[i, j] = rnd.Next(1, 1000);

                        }
                        else
                        {
                            //Присваиваем симметричное значение
                            D[i, j] = D[j, i];
                        }
                    }
                    
                }
            }

            Console.WriteLine("Матрица расстояний:");
            printMatrix(D);
            

            int A = 0, B = 0;












        }
    }
}
