using System;
using System.Collections.Generic;
using System.Text;

namespace MyAntTSP
{
    class Program
    {
        /// <summary>
        /// Вероятность перехода из пункта в пункт
        /// </summary>
        /// <param name="way">Рассматриваемый путь из i в j</param>
        /// <param name="alpha">Параметр alpha</param>
        /// <param name="beta">Параметр beta</param>
        /// <param name="wayd">Набор всех путей из всех вершин</param>
        /// <returns></returns>
        public static double probability(WayData way, double alpha, double beta, List<WayData> wayd, List<int> taboo)
        {
            double sumDop = 0;
            bool flag;
            foreach (var item in wayd)
            {
                flag = true;
                //Если начальный путь совпадает с начальным путем для item
                if (item.i == way.i)
                {
                    //Если вершина находится в списке вершин, которые уже были рассмотрены ранее, то flag == false
                    foreach (var elem in taboo)
                    {
                        if (elem == item.j)
                            flag = false;
                    }

                    //Если в списке запрещенных вершин не примелькалось, то используем
                    if(flag == true)
                        sumDop += (Math.Pow((1.0 / item.length), beta) * Math.Pow(item.tau, alpha));
                }
            }
            return (Math.Pow((1.0 / way.length), beta) * Math.Pow(way.tau, alpha)) / sumDop;;
        }

        /// <summary>
        /// Получение пути, по которому будет из данной точки далее двигаться муравей 
        /// </summary>
        /// <param name="startPoint">Начальная вершина</param>
        /// <param name="alpha">Параметр alpha</param>
        /// <param name="beta">Параметр beta</param>
        /// <param name="wayd">Набор всех путей из всех вершин</param>
        /// <returns></returns>
        public static WayData getNextWay(int startPoint, double alpha, double beta, List<WayData> wayd, List<int> taboo, Random rnd)
        {
            //Вероятности переходов во все возможные вершины
            List<double> probs = new List<double>();
            //Сохранение вершин для соотнесения с вероятностями
            List<WayData> ways = new List<WayData>();

            bool flag;
            //Получаем список вероятностей вершин, в которые возможен переход
            foreach (var item in wayd)
            {
                flag = true;
                //Если начальный пункт совпадает с начальным пунктом для пути item
                if (item.i == startPoint)
                {
                    //Если вершина находится в списке вершин, которые уже были рассмотрены ранее, то flag == false
                    foreach (var elem in taboo)
                    {
                        if (elem == item.j)
                            flag = false;
                    }

                    //Если в списке запрещенных вершин не примелькалось, то используем
                    if (flag == true)
                    {
                        probs.Add(probability(item, alpha, beta, wayd, taboo));
                        ways.Add(item);
                    }
                }
            }

            //Запуск "рулетки"
            double rouletteVal = rnd.NextDouble(); //0.6 для теста
            //Console.WriteLine(rouletteVal);

            //Идентификатор промежутка, в который попадет значение рулетки
            int curr = 0;
            //Нахождение интересующего интервала. Если он найден, выходим из цикла
            for (double i = 0; ; i+=probs[curr], curr++)
            {
                if (i < rouletteVal && rouletteVal < (i + probs[curr]))
                    break;
            }

            //В точку, с которой сейчас будем уходить, больше ходить нельзя
            taboo.Add(startPoint);

            //Возвращаем путь, по которому предлагается двигаться далее
            return ways[curr];
        }

        /// <summary>
        /// Учесть то, что длины, к примеру, 4-2 равны 2-4. Для этого делаем дублирование
        /// </summary>
        /// <param name="wayd">Набор всех путей из всех вершин</param>
        public static void considerPermutations(List<WayData> wayd)
        {
            //Промежуточное сохранение
            List<WayData> wayp = new List<WayData>();
            foreach (var item in wayd)
            {
                wayp.Add(new WayData(item.j, item.i, item.length, item.tau));
            }

            foreach (var item in wayp)
            {
                wayd.Add(item);
            }
        }

        /// <summary>
        /// Визуализация пройденного муравьём пути в терминале
        /// </summary>
        /// <param name="allWay">Упорядоченные промежуточные пункты пути</param>
        /// <param name="wayLength">Длина пути</param>
        /// <param name="ant">Номер муравья</param>
        public static void printAllWay(List<double> allWay, double wayLength, int ant)
        {
            Console.Write("Путь " + (ant + 1) + ": ");
            for (int i = 0; i < allWay.Count - 1; i++)
            {
                Console.Write(allWay[i] + " -> ");
            }
            Console.Write(allWay[allWay.Count - 1] + ". Длина пути: " + wayLength + "\n");
        }

        /// <summary>
        /// Обновление феромона после выполнения прохода муравья
        /// </summary>
        /// <param name="wayd">Набор всех путей из всех вершин</param>
        /// <param name="allWay">Упорядоченные промежуточные пункты пути</param>
        /// <param name="deltaTau">Положительный прирост феромона на пройденном пути</param>
        /// <param name="rho">Коэффициент испарения феромона</param>
        public static void updatePheromone(List<WayData> wayd, List<double> allWay, double deltaTau, double rho)
        {
            bool flag;
            //Рассмотрим каждый путь в отдельности
            foreach (var item in wayd)
            {
                flag = false;
                //Посмотрим, не является ли он таким, по которому ходил муравей
                for (int i = 0; i < allWay.Count - 1; i++)
                {
                    //Если начало и конец пути равны текущему и следующему пункту пройденного пути (или является их перестановкой)
                    if ((item.i == allWay[i] && item.j == allWay[i + 1]) || (item.i == allWay[i + 1] && item.j == allWay[i]))
                    {
                        //Добавляем фермон, который принес муравей и делаем испарение
                        item.tau = (1 - rho) * item.tau + deltaTau;
                        //Отметим то, что данный путь уже учтён
                        flag = true;
                        break;
                    }
                }
                if(flag == false)
                {
                    //Муравей ничего не принес, только испарение
                    item.tau = (1 - rho) * item.tau;
                }
            }
        }

        static void Main(string[] args)
        {
            //Количество городов
            int countOfPoints = 5;
            //Город, с которого надо начать путь
            int startPoint = 1;

            //Количество феромона
            double Q = 60;
            //Коэффициент испарения феромона
            double rho = 0.5;

            List<WayData> wayd = new List<WayData>();
            wayd.Add(new WayData(1, 2, 38, 3));
            wayd.Add(new WayData(1, 3, 74, 2));
            wayd.Add(new WayData(1, 4, 59, 2));
            wayd.Add(new WayData(1, 5, 45, 2));
            wayd.Add(new WayData(2, 3, 46, 1));
            wayd.Add(new WayData(2, 4, 61, 1));
            wayd.Add(new WayData(2, 5, 72, 1));
            wayd.Add(new WayData(3, 4, 49, 2));
            wayd.Add(new WayData(3, 5, 85, 2));
            wayd.Add(new WayData(4, 5, 42, 1));

            //Сохраняем ещё и возможные перестановки городов местами
            considerPermutations(wayd);

            double alpha = 1;
            double beta = 1;

            //Количество муравьёв, которые пройдут путь
            int countOfAnt = 60;

            //Инициализация рандомайзера
            Random rnd = new Random();

            for (int ant = 0; ant < countOfAnt; ant++)
            {
                //Запрещённые пункты, где муравей уже был
                List<int> taboo = new List<int>();

                //Сохраняем все вершины пути по-порядку
                List<double> allWay = new List<double>();
                //Сохраняем длину данного пути
                double wayLength = 0;

                //Первая итерация, добавляем в путь первую вершину
                allWay.Add(startPoint);
                WayData nextWay = getNextWay(startPoint, alpha, beta, wayd, taboo, rnd);
                allWay.Add(nextWay.j);
                wayLength += nextWay.length;

                //Пока не закончатся города, в которые можно ходить
                while (taboo.Count != countOfPoints - 1)
                {
                    //Вызываем метод для последней вершины, куда перешли
                    nextWay = getNextWay(nextWay.j, alpha, beta, wayd, taboo, rnd);
                    allWay.Add(nextWay.j);
                    wayLength += nextWay.length;
                }

                //Вывод полученного для данного муравья пути
                printAllWay(allWay, wayLength, ant);

                //Работа с феромонами
                //Положительный прирост феромона на пройденном пути
                double deltaTau = Q / wayLength;

                //Обновляем феромоны в зависимости от результатов
                updatePheromone(wayd, allWay, deltaTau, rho);
            }
        }
    }

    public class WayData
    {
        public int i;               //Пункт i
        public int j;               //Пункт j
        public double length;       //Расстояние между пунктами i и j
        public double tau;          //Количество феромона на пути
        
        public WayData(int I, int J, double Length, double Tau)
        {
            i = I;
            j = J;
            length = Length;
            tau = Tau;
        }
    }
}