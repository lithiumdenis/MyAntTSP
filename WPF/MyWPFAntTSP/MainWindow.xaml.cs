using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace MyWPFAntTSP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var r = new Random();
            ValuesA = new ChartValues<ObservablePoint>();

            ValuesA.Add(new ObservablePoint(Convert.ToDouble(textBoxX1.Text), Convert.ToDouble(textBoxY1.Text)));
            ValuesA.Add(new ObservablePoint(Convert.ToDouble(textBoxX2.Text), Convert.ToDouble(textBoxY2.Text)));
            ValuesA.Add(new ObservablePoint(Convert.ToDouble(textBoxX3.Text), Convert.ToDouble(textBoxY3.Text)));
            ValuesA.Add(new ObservablePoint(Convert.ToDouble(textBoxX4.Text), Convert.ToDouble(textBoxY4.Text)));
            ValuesA.Add(new ObservablePoint(Convert.ToDouble(textBoxX5.Text), Convert.ToDouble(textBoxY5.Text)));
            ValuesA.Add(new ObservablePoint(Convert.ToDouble(textBoxX6.Text), Convert.ToDouble(textBoxY6.Text)));
            ValuesA.Add(new ObservablePoint(Convert.ToDouble(textBoxX7.Text), Convert.ToDouble(textBoxY7.Text)));

            DataContext = this;
        }

        public ChartValues<ObservablePoint> ValuesA { get; set; }

        private void RandomizeOnClick(object sender, RoutedEventArgs e)
        {
            ValuesA[0].X = Convert.ToDouble(textBoxX1.Text.Replace('.', ','));
            ValuesA[0].Y = Convert.ToDouble(textBoxY1.Text.Replace('.', ','));

            ValuesA[1].X = Convert.ToDouble(textBoxX2.Text.Replace('.', ','));
            ValuesA[1].Y = Convert.ToDouble(textBoxY2.Text.Replace('.', ','));

            ValuesA[2].X = Convert.ToDouble(textBoxX3.Text.Replace('.', ','));
            ValuesA[2].Y = Convert.ToDouble(textBoxY3.Text.Replace('.', ','));

            ValuesA[3].X = Convert.ToDouble(textBoxX4.Text.Replace('.', ','));
            ValuesA[3].Y = Convert.ToDouble(textBoxY4.Text.Replace('.', ','));

            ValuesA[4].X = Convert.ToDouble(textBoxX5.Text.Replace('.', ','));
            ValuesA[4].Y = Convert.ToDouble(textBoxY5.Text.Replace('.', ','));

            ValuesA[5].X = Convert.ToDouble(textBoxX6.Text.Replace('.', ','));
            ValuesA[5].Y = Convert.ToDouble(textBoxY6.Text.Replace('.', ','));

            ValuesA[6].X = Convert.ToDouble(textBoxX7.Text.Replace('.', ','));
            ValuesA[6].Y = Convert.ToDouble(textBoxY7.Text.Replace('.', ','));
        }

        /// <summary>
        /// Расстояние между двумя точками
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double dist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        /// <summary>
        /// Проверка на неравенство треугольника
        /// </summary>
        /// <param name="A">Длина стороны А</param>
        /// <param name="B">Длина стороны Б</param>
        /// <param name="C">Длина стороны С</param>
        /// <returns></returns>
        public static bool neravTreug(double A, double B, double C)
        {
            if (A > B + C || B > A + C || C > B + A)
                return false; //Не треугольник
            else
                return true; //Треугольник

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //Количество городов
            int countOfPoints = 7; //константа
            //Город, с которого надо начать путь
            int startPoint = 1;

            //Количество феромона
            double Q = 60;
            //Коэффициент испарения феромона
            double rho = 0.5;

            List<WayData> wayd = new List<WayData>();

            listBox.Items.Clear();

            //Создаем массив из 7 городов
            PointD[] town = new PointD[7];
            town[0] = new PointD();
            town[1] = new PointD();
            town[2] = new PointD();
            town[3] = new PointD();
            town[4] = new PointD();
            town[5] = new PointD();
            town[6] = new PointD();

            //Получаем список городов

            town[0].X = Convert.ToDouble(textBoxX1.Text.Replace('.', ','));
            town[0].Y = Convert.ToDouble(textBoxY1.Text.Replace('.', ','));

            town[1].X = Convert.ToDouble(textBoxX2.Text.Replace('.', ','));
            town[1].Y = Convert.ToDouble(textBoxY2.Text.Replace('.', ','));

            town[2].X = Convert.ToDouble(textBoxX3.Text.Replace('.', ','));
            town[2].Y = Convert.ToDouble(textBoxY3.Text.Replace('.', ','));

            town[3].X = Convert.ToDouble(textBoxX4.Text.Replace('.', ','));
            town[3].Y = Convert.ToDouble(textBoxY4.Text.Replace('.', ','));

            town[4].X = Convert.ToDouble(textBoxX5.Text.Replace('.', ','));
            town[4].Y = Convert.ToDouble(textBoxY5.Text.Replace('.', ','));

            town[5].X = Convert.ToDouble(textBoxX6.Text.Replace('.', ','));
            town[5].Y = Convert.ToDouble(textBoxY6.Text.Replace('.', ','));

            town[6].X = Convert.ToDouble(textBoxX7.Text.Replace('.', ','));
            town[6].Y = Convert.ToDouble(textBoxY7.Text.Replace('.', ','));

            //Получаем все дистанции между всеми разными городами (42)
            fillWayd(wayd, town);

            //Если неравенство треугольника везде выполнится, то так и останется
            bool neravDet = false;
            double dist1 = 0;
            double dist2 = 0;
            double dist3 = 0;


            //Проверка на неравенство треугольника
            //Для любых трёх разных городов
            for (int i = 0; i < town.Length; i++)
            {
                for (int j = 0; j < town.Length; j++)
                {
                    for (int k = 0; k < town.Length; k++)
                    {
                        //исключаем возможность одинаковых городов
                        if(i != j && j != k && i != k)
                        {
                            dist1 = dist(town[i].X, town[i].Y, town[j].X, town[j].Y); //3
                            dist2 = dist(town[i].X, town[i].Y, town[k].X, town[k].Y); //5
                            dist3 = dist(town[j].X, town[j].Y, town[k].X, town[k].Y); //10

                            if (neravTreug(dist1, dist2, dist3) == false)
                                neravDet = true;
                        }
                    }
                }
            }

            //Только если неравенство выполнилось, продолжаем работу
            if (neravDet == false)
            {

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
                    printAllWay(allWay, wayLength, ant, listBox);

                    //Работа с феромонами
                    //Положительный прирост феромона на пройденном пути
                    double deltaTau = Q / wayLength;

                    //Обновляем феромоны в зависимости от результатов
                    updatePheromone(wayd, allWay, deltaTau, rho);
                }
            }
            else
                MessageBox.Show("Неравенство треугольника не выполнено! Попробуйте ещё раз");

        }


        public static void fillWayd(List<WayData> wayd, PointD[] town)
        {
            for (int i = 0; i < town.Length; i++)
            {
                for (int j = 0; j < town.Length; j++)
                {
                    if (i != j)
                        wayd.Add(new WayData(i + 1, j + 1, dist(town[i].X, town[i].Y, town[j].X, town[j].Y), 1)); //+1 чтобы нумерация городов была не с 0
                }
            }
        }

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
                    if (flag == true)
                        sumDop += (Math.Pow((1.0 / item.length), beta) * Math.Pow(item.tau, alpha));
                }
            }
            return (Math.Pow((1.0 / way.length), beta) * Math.Pow(way.tau, alpha)) / sumDop; ;
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
            for (double i = 0; ; i += probs[curr], curr++)
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
        /// Визуализация пройденного муравьём пути в терминале
        /// </summary>
        /// <param name="allWay">Упорядоченные промежуточные пункты пути</param>
        /// <param name="wayLength">Длина пути</param>
        /// <param name="ant">Номер муравья</param>
        public static void printAllWay(List<double> allWay, double wayLength, int ant, ListBox listbox)
        {
            string str = "";

            str += "Путь " + (ant + 1) + ": ";
            //Console.Write("Путь " + (ant + 1) + ": ");
            for (int i = 0; i < allWay.Count - 1; i++)
            {
                str += allWay[i] + " -> ";
                //Console.Write(allWay[i] + " -> ");
            }
            str += allWay[allWay.Count - 1] + ". Длина пути: " + wayLength + "\n";
            listbox.Items.Add(str);
            //Console.Write(allWay[allWay.Count - 1] + ". Длина пути: " + wayLength + "\n");
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
                if (flag == false)
                {
                    //Муравей ничего не принес, только испарение
                    item.tau = (1 - rho) * item.tau;
                }
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

    public class PointD
    {
        public double X;
        public double Y;
    }
}