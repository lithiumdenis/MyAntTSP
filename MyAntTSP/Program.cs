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
        private int m;
        private double q;
        private double rho;
        private int a;
        private int b;
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
        public int M
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
        public int A
        {
            get { return a; }
        }

        /// <summary>
        /// Конечная вершина
        /// </summary>
        public int B
        {
            get { return b; }
        }

        /// <summary>
        /// Количество вершин
        /// </summary>
        public int N
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

    struct WayType
    {
        public int itabu;
        public int length;
        public int[] tabu;
    }

    class Program : Data
    {

        public static double probability(int to, WayType ant, double[,] pheromone, double[,] distance, Data data)
        {
            // если вершина уже посещена, возвращаем 0
            for (int i = 0; i < ant.itabu; ++i)
                if (to == ant.tabu[i])
                    return 0;

            double sum = 0;
            int from = ant.tabu[ant.itabu - 1];
            // считаем сумму в знаминателе
            for (int j = 0; j < data.N; ++j)
            {
                int flag = 1;
                // проверяем, посещал ли муравей j вершину
                for (int i = 0; i < ant.itabu; ++i)
                    if (j == ant.tabu[i])
                        flag = 0;
                // если нет, тогда прибавляем к общей сумме
                if (flag != 0)
                    sum += Math.Pow(pheromone[from, j], data.Alpha) * Math.Pow(distance[from, j], data.Beta);
            }

            // возвращаем значение вероятности
            return Math.Pow(pheromone[from, to], data.Alpha) * Math.Pow(distance[from, to], data.Beta) / sum;
        }



        public static WayType AntColonyOptimization(Data data)
        {
            // инициализация данных о лучшем маршруте
            WayType way;
            way.itabu = 0;
            way.length = -1;
            way.tabu = new int[data.N];

            // инициализация данных о расстоянии и количестве феромона
            double[,] distance = new double[data.N, data.N];
            double[,] pheromone = new double[data.N, data.N];

            for (int i = 0; i < data.N; ++i)
            {
                for (int j = 0; j < data.N; ++j)
                {
                    pheromone[i, j] = 1.0 / data.N;
                    if (i != j)
                        distance[i, j] = 1.0 / data.D[i, j];
                }
            }

            // инициализация муравьев
            WayType[] ants = new WayType[data.M];
            for (int k = 0; k < data.M; ++k)
            {
                ants[k].itabu = 0;
                ants[k].length = 0;
                ants[k].tabu = new int[data.N];
                ants[k].tabu[ants[k].itabu++] = data.A;
            }

            // основной цикл
            for (int t = 0; t < data.Tmax; ++t)
            {
                // цикл по муравьям
                for (int k = 0; k < data.M; ++k)
                {
                    // поиск маршрута для k-го муравья
                    do
                    {
                        int j_max = -1;
                        double p_max = 0.0;
                        for (int j = 0; j < data.N; ++j)
                        {
                            // Проверка вероятности перехода в вершину j
                            if (ants[k].tabu[ants[k].itabu - 1] != j)
                            {
                                double p = probability(j, ants[k], pheromone, distance, data);
                                if (p != 0 && p >= p_max)
                                {
                                    p_max = p;
                                    j_max = j;
                                }
                            }
                        }
                        ants[k].length += Convert.ToInt32(data.D[ants[k].tabu[ants[k].itabu - 1], j_max]);
                        ants[k].tabu[ants[k].itabu++] = j_max;
                    } while (ants[k].tabu[ants[k].itabu - 1] != data.B);
                    // оставляем феромон на пути муравья
                    for (int i = 0; i < ants[k].itabu - 1; ++i)
                    {
                        int from = ants[k].tabu[i % ants[k].itabu];
                        int to = ants[k].tabu[(i + 1) % ants[k].itabu];
                        pheromone[from, to] += data.Q / ants[k].length;
                        pheromone[to, from] = pheromone[from, to];
                    }
                    // проверка на лучшее решение
                    if (ants[k].length < way.length || way.length < 0)
                    {
                        way.itabu = ants[k].itabu;
                        way.length = ants[k].length;
                        for (int i = 0; i < way.itabu; ++i) way.tabu[i] = ants[k].tabu[i];
                    }
                    // обновление муравьев
                    ants[k].itabu = 1;
                    ants[k].length = 0;
                }
                // цикл по ребрам
                for (int i = 0; i < data.N; ++i)
                    for (int j = 0; j < data.N; ++j)
                        // обновление феромона для ребра (i, j)
                        if (i != j)
                            pheromone[i, j] *= (1 - data.Rho);
            }
            // возвращаем кратчайший маршрут
            return way;
        }


        static void Main(string[] args)
        {

            Data info = initDefaults();

            printMatrix(info.D);

            WayType way = AntColonyOptimization(info);


            Console.Write("Длина пути: ");
            Console.WriteLine(way.length);

            Console.Write("Путь: ");
            Console.Write(++way.tabu[0]);

            for (int i = 1; i < way.itabu; ++i)
                Console.Write(" -> " + ++way.tabu[i]);
            Console.WriteLine();
        }
    }
}
