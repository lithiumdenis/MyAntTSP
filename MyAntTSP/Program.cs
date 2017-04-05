using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAntTSP
{
    class Program
    {
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
            var N = 10;
            //Инициализация матрицы расстояний
            double[,] D = new double[N, N]; ;


            int A = 0, B = 0;












        }
    }
}
