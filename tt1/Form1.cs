using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection;

namespace tt1
{
    public partial class Form1 : Form
    {
        // массив входных обучающих векторов
       public Vector[] X = {
    new Vector(0, 0),
    new Vector(0, 1),
    new Vector(1, 0),
    new Vector(1, 1)
};

        // массив выходных обучающих векторов
        public Vector[] Y = {
    new Vector(0.0), // 0 ^ 0 = 0
    new Vector(1.0), // 0 ^ 1 = 1
    new Vector(1.0), // 1 ^ 0 = 1
    new Vector(0.0) // 1 ^ 1 = 0
};
  


        Network network = new Network(new int[] { 2000, 30,  1 }); // создаём сеть с двумя входами, тремя нейронами в скрытом слое и одним выходом
        private static bool run() 
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(0);

            MethodBase currentMethodName = sf.GetMethod();
            string asdd = currentMethodName.DeclaringType.FullName;
            string currentM = new StackTrace().GetFrame(0).GetMethod().DeclaringType.FullName + "." + new StackTrace().GetFrame(0).GetMethod().Name;
            return false;
        }

        public Form1()
        {
            InitializeComponent();

            bool asd = run();

            //dbcore dbcore = new dbcore();
            string str= @"<response><lst name=\""responseHeader\"">< int name = \""status\"" > 0 </ int > < int name = \""QTime\"" > 3 </ int >  < lst name = \""params\"" >   < str name = \""q\"" >    ((task_name: кенекнн OR task_id:кенекнн) OR global_all:кенекнн) AND(entity_type: task) AND right:e_22017 AND task_state: 0     </ str >     < str name = \""version\"" > 2.2 </ str >      < str name = \""start\"" > 0 </ str >       < str name = \""rows\"" > 120 </ str >        < str name = \""indent\"" > on </ str >         < str name = \""fl\"" > task_id </ str >          </ lst >          </ lst >          < result name = \""response\"" numFound = \""0\"" start = \""0\"" />               </ response > ";
            str = str.Replace(" ", "");
            char[] charArray = str.ToCharArray();
            foreach (char c in str) 
            {
                if (Convert.ToString(c, 2).PadLeft(8, '0').Length > 8)
                {

                }
                Console.WriteLine(Convert.ToString(c, 2).PadLeft(8, '0'));
            }


            network.Train(X, Y, 0.5, 0.01, 100000); // запускаем обучение сети 

            for (int i = 0; i < 4; i++)
            {
                Vector output = network.Forward(X[i]);
                Console.WriteLine("X: {0} {1}, Y: {2}, output: {3}", X[i][0], X[i][1], Y[i][0], output[0]);
            }
            
        }

      


        


        public class Vector
        {
            public double[] v; // значения вектора
            public int n; // длина вектора

            // конструктор из длины
            public Vector(int n)
            {
                this.n = n; // копируем длину
                v = new double[n]; // создаём массив
            }

            // создание вектора из вещественных значений
            public Vector(params double[] values)
            {
                n = values.Length;
                v = new double[n];

                for (int i = 0; i < n; i++)
                    v[i] = values[i];
            }

            // обращение по индексу
            public double this[int i]
            {
                get { return v[i]; } // получение значение
                set { v[i] = value; } // изменение значения
            }
        }
        public class Matrix
        {
            double[][] v; // значения матрицы
            public int n, m; // количество строк и столбцов

            // создание матрицы заданного размера и заполнение случайными числами из интервала (-0.5, 0.5)
            public Matrix(int n, int m, Random random)
            {
                this.n = n;
                this.m = m;

                v = new double[n][];

                for (int i = 0; i < n; i++)
                {
                    v[i] = new double[m];

                    for (int j = 0; j < m; j++)
                        v[i][j] = random.NextDouble() - 0.5; // заполняем случайными числами
                }
            }

            // обращение по индексу
            public double this[int i, int j]
            {
                get { return v[i][j]; } // получение значения
                set { v[i][j] = value; } // изменение значения
            }
        }

        public class Network
        {
            // обновление весовых коэффициентов, alpha - скорость обучения
            void UpdateWeights(double alpha)
            {
                for (int k = 0; k < layersN; k++)
                {
                    for (int i = 0; i < weights[k].n; i++)
                    {
                        for (int j = 0; j < weights[k].m; j++)
                        {
                            weights[k][i, j] -= alpha * deltas[k][i] * L[k].x[j];
                        }
                    }
                }
            }

            public void Train(Vector[] X, Vector[] Y, double alpha, double eps, int epochs)
            {
                int epoch = 1; // номер эпохи

                double error; // ошибка эпохи

                do
                {
                    error = 0; // обнуляем ошибку

                    // проходимся по всем элементам обучающего множества
                    for (int i = 0; i < X.Length; i++)
                    {
                        Forward(X[i]); // прямое распространение сигнала
                        Backward(Y[i], ref error); // обратное распространение ошибки
                        UpdateWeights(alpha); // обновление весовых коэффициентов
                    }

                    Console.WriteLine("epoch: {0}, error: {1}", epoch, error); // выводим в консоль номер эпохи и величину ошибку

                    epoch++; // увеличиваем номер эпохи
                } while (epoch <= epochs && error > eps);
            }

            // обратное распространение
            public void Backward(Vector output, ref double error)
            {
                int last = layersN - 1;

                error = 0; // обнуляем ошибку

                for (int i = 0; i < output.n; i++)
                {
                    double e = L[last].z[i] - output[i]; // находим разность значений векторов

                    deltas[last][i] = e * L[last].df[i]; // запоминаем дельту
                    error += e * e / 2; // прибавляем к ошибке половину квадрата значения
                }

                // вычисляем каждую предудущю дельту на основе текущей с помощью умножения на транспонированную матрицу
                for (int k = last; k > 0; k--)
                {
                    for (int i = 0; i < weights[k].m; i++)
                    {
                        deltas[k - 1][i] = 0;

                        for (int j = 0; j < weights[k].n; j++)
                            deltas[k - 1][i] += weights[k][j, i] * deltas[k][j];

                        deltas[k - 1][i] *= L[k - 1].df[i]; // умножаем получаемое значение на производную предыдущего слоя
                    }
                }
            }
            struct LayerT
            {
                public Vector x; // вход слоя
                public Vector z; // активированный выход слоя
                public Vector df; // производная функции активации слоя
            }

            Matrix[] weights; // матрицы весов слоя
            LayerT[] L; // значения на каждом слое
            Vector[] deltas; // дельты ошибки на каждом слое

            int layersN; // число слоёв

            public Network(int[] sizes)
            {
                Random random = new Random(DateTime.Now.Millisecond); // создаём генератор случайных чисел

                layersN = sizes.Length - 1; // запоминаем число слоёв

                weights = new Matrix[layersN]; // создаём массив матриц весовых коэффициентов
                L = new LayerT[layersN]; // создаём массив значений на каждом слое
                deltas = new Vector[layersN]; // создаём массив для дельт

                for (int k = 1; k < sizes.Length; k++)
                {
                    weights[k - 1] = new Matrix(sizes[k], sizes[k - 1], random); // создаём матрицу весовых коэффициентов

                    L[k - 1].x = new Vector(sizes[k - 1]); // создаём вектор для входа слоя
                    L[k - 1].z = new Vector(sizes[k]); // создаём вектор для выхода слоя
                    L[k - 1].df = new Vector(sizes[k]); // создаём вектор для производной слоя

                    deltas[k - 1] = new Vector(sizes[k]); // создаём вектор для дельт
                }
            }

            // прямое распространение
            public Vector Forward(Vector input)
            {
                for (int k = 0; k < layersN; k++)
                {
                    if (k == 0)
                    {
                        for (int i = 0; i < input.n; i++)
                            L[k].x[i] = input[i];
                    }
                    else
                    {
                        for (int i = 0; i < L[k - 1].z.n; i++)
                            L[k].x[i] = L[k - 1].z[i];
                    }

                    for (int i = 0; i < weights[k].n; i++)
                    {
                        double y = 0;

                        for (int j = 0; j < weights[k].m; j++)
                            y += weights[k][i, j] * L[k].x[j];

                        // активация с помощью сигмоидальной функции
                        L[k].z[i] = 1 / (1 + Math.Exp(-y));
                        L[k].df[i] = L[k].z[i] * (1 - L[k].z[i]);

                        // активация с помощью гиперболического тангенса
                        //L[k].z[i] = Math.Tanh(y);
                        //L[k].df[i] = 1 - L[k].z[i] * L[k].z[i];

                        // активация с помощью ReLU
                        //L[k].z[i] = y > 0 ? y : 0;
                        //L[k].df[i] = y > 0 ? 1 : 0;
                    }
                }

                return L[layersN - 1].z; // возвращаем результат
            }
        }
    }
}
