using System;
using System.Collections.Generic;
using System.Text;

namespace MyAntTSP
{
    class Data
    {
        private double alpha;
        private double beta;
        private double tmax;
        private double m;
        private double q;
        private double rho;
        private double a;
        private double b;
        private int n;
        private double[,] d;

        /// <summary>
        /// Вес фермента
        /// </summary>
        public double Alpha
        {
            get { return alpha; }
        }

        /// <summary>
        /// Коэффициент эвристики
        /// </summary>
        public double Beta
        {
            get { return beta; }
        }

        /// <summary>
        /// Время жизни колонии
        /// </summary>
        public double Tmax
        {
            get { return tmax; }
        }

        /// <summary>
        /// Количество муравьев в колонии
        /// </summary>
        public double M
        {
            get { return m; }
        }

        /// <summary>
        /// Количество феромона
        /// </summary>
        public double Q
        {
            get { return q; }
        }

        /// <summary>
        /// Коэффициент испарения феромона
        /// </summary>
        public double Rho
        {
            get { return rho; }
        }

        /// <summary>
        /// Начальная вершина
        /// </summary>
        public double A
        {
            get { return a; }
        }

        /// <summary>
        /// Конечная вершина
        /// </summary>
        public double B
        {
            get { return b; }
        }

        /// <summary>
        /// Количество вершин
        /// </summary>
        public double N
        {
            get { return n; }
        }

        /// <summary>
        /// Матрица расстояний
        /// </summary>
        public double [,] D
        {
            get { return d; }
        }

        /// <summary>
        /// Инициализация данных тестовым набором параметров
        /// </summary>
        /// <returns></returns>
        public static Data initDefaults()
        {
            int N = 5;

            return new Data
            {
                alpha = 1,
                beta = 3,
                tmax = 100,
                m = 20,
                q = 100,
                rho = 0.5,
                n = N,
                a = 1,
                b = 5,
                d = initOfDMatrix(N)
            };
        }

        /// <summary>
        /// Инициализация данных с учётом изменения параметров пользователем
        /// </summary>
        /// <returns></returns>
        public static Data initByUser()
        {
            Console.Write("Введите количество вершин: ");
            int _N = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите начальную вершину из промежутка [0;" + (_N - 1) + "]: ");
            int _A = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите конечную вершину из промежутка [0;" + (_N - 1) + "]: ");
            int _B = Convert.ToInt32(Console.ReadLine());

            return new Data
            {
                alpha = 1,
                beta = 3,
                tmax = 100,
                m = 20,
                q = 100,
                rho = 0.5,
                n = _N,
                a = _A,
                b = _B,
                d = initOfDMatrix(10)
            };
        }

        /// <summary>
        /// Инициализация матрицы расстояний
        /// </summary>
        /// <param name="N">Число вершин</param>
        /// <returns></returns>
        protected static double[,] initOfDMatrix(int N)
        {
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
            return D;
        }

        /// <summary>
        /// Печать квадратной матрицы в консоль
        /// </summary>
        /// <param name="D">Квадратная матрица</param>
        protected static void printMatrix(double[,] D)
        {
            for (int i = 0; i < D.GetLength(0); i++)
            {
                for (int j = 0; j < D.GetLength(1); j++)
                {
                    Console.Write("(" + i + "," + j + "): " + D[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    class Program : Data
    {
        static void Main(string[] args)
        {

            Data info = initDefaults();

            printMatrix(info.D);














        }
    }
}
