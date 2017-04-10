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
        /// <returns></returns>
        public static double probability(WayData way, double alpha, double beta, List<WayData> wayd)
        {
            double sumDop = 0;

            double a1 = 0;
            double a2 = 0;
            double a3 = 0;
            double a4 = 0;

            foreach (var item in wayd)
            {
                //Если вершина не использовалась и начальный путь совпадает с начальным путем для item
                if (item.isUsed == false && item.i == way.i)
                {
                    a3 = Math.Pow((1.0 / item.length), beta);
                    a4 = Math.Pow(item.tau, alpha);
                    a1 = (a3 * a4);
                    sumDop += a1;
                }
            }

            a2 = 100 * (Math.Pow((1.0 / way.length), beta) * Math.Pow(way.tau, alpha)) / sumDop;

            return a2;
        }


        static void Main(string[] args)
        {
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

            double alpha = 1;
            double beta = 1;

            double p1 = probability(wayd[0], alpha, beta, wayd);
            double p2 = probability(wayd[1], alpha, beta, wayd);
            double p3 = probability(wayd[2], alpha, beta, wayd);
            double p4 = probability(wayd[3], alpha, beta, wayd);

        }
    }

    public class WayData
    {
        public int i;               //Пункт i
        public int j;               //Пункт j
        public double length;       //Расстояние между пунктами i и j
        public double tau;          //Количество феромона на пути
        public bool isUsed;         //Был ли уже здесь этот муравей 
        public WayData(int I, int J, double Length, double Tau)
        {
            i = I;
            j = J;
            length = Length;
            tau = Tau;
            isUsed = false; //default 
        }
    }
}