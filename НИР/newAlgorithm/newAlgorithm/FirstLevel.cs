using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newAlgorithm
{
    class FirstLevel
    {
        public static bool flag;
        private List<int> I;//Вектор интерпритируемых типов данных
        private List<int> Ii;//Вектор интерпритируемых типов данных на текущем шагу алгоритма
        private List<int> mi;//Вектор количества партий данных для каждого типа данных
        private List<int> countBatches;
        private List<int> np1i;//задание для каждого i-го типа данных количества решений по составам партий данных i-ых типов, сформированных на текущей ите-рации алгоритма
        private List<int> np2i;//задание для каждого i-го типа данных количества решений по составам партий данных i-ых типов, сформированных на текущей ите-рации алгоритма
        private List<List<int>> A1i;//Буферизированная матрица составов партий требований на k+1 шаге 
        private List<List<List<int>>> A1;//Матрица составов партий требований на k+1 шаге 
        private List<List<List<int>>> A2;//Матрица составов партий требований фиксированного типа
        private List<List<int>> A;//Матрица составов партий требований на k шаге
        private List<List<int>> ABuf;//Буферизированная матрица составов партий требований
        private int countType;//количество типов
        private List<int> countClaims;//Начальное количество требований для каждого типа данных
        private BatchTypeClaims test;
        private int i;//идентификатор текущего изменяемого типа
        private int G;
        private int q2;
        private int k;
        private List<int> fi;//Критерии решений всех типов данных
        private int f1;//Критерий текущего решения для всех типов
        private int f1Buf;//Критерий текущего решения для всех типов
        private bool solutionFlag;

        /* 
         * Конструктор с параметрами
         * 
         * count_type - количество типов рассматриваемых данных
         * count_claims - количество требований всех типов данных
         * 
         */
        public FirstLevel(int count_type, List<int> count_claims)
        {
            this.countType = count_type;
            this.countClaims = count_claims;
            this.mi = new List<int>(this.countType);
            this.np1i = new List<int>(this.countType);
            this.np2i = new List<int>(this.countType);
            this.I = new List<int>(this.countType);
            this.Ii = new List<int>(this.countType);
        }

        /*
         * Функция копирования значений между матрицами, предотвращающая копирование указателей
         * 
         */
        private List<List<int>> CopyMatrix(List<List<int>> inMatrix)
        {
            List<List<int>> ret = new List<List<int>>();
            for (int i = 0; i < inMatrix.Count; i++)
            {
                ret.Add(new List<int>());
                for (int j = 0; j < inMatrix[i].Count; j++)
                {
                    ret[i].Add(inMatrix[i][j]);
                }
            }
            return ret;
        }

        /*
         * Алгоритм формирования начальных решений по составам партий всех типов
         *  
         */ 
        public void GenerateStartSolution()
        {
            int claim = 2;
            this.A = new List<List<int>>();
            for (int i = 0; i < this.countType; i++)
            {
                this.I.Add(1);
                this.Ii.Add(1);
                this.mi.Add(claim);
                this.np1i.Add(1);
                this.np2i.Add(1);
                this.A.Add(new List<int>());
                this.A[i].Add(0);
                this.A[i].Add(this.countClaims[i - 1] - claim);
                this.A[i].Add(claim);
            }
            for (int i =0; i < this.countType; i++)
            {
                if (this.A[i][1] < 2 || this.A[i][1] < this.A[i][2])
                {
                    this.A[i].Clear();
                    this.A[i].Add(0);
                    this.A[i].Add(this.countClaims[i - 1]);
                    this.mi[i - 1] = 1;
                    this.np1i[i - 1] = 0;
                    this.I[i - 1] = 0;
                    this.Ii[i - 1] = 0;
                }
            }
            this.G = 0;    
        }

        /*
         * Функция вычисления f1 критерия
         * 
         */
        public int GetCriterion(List<List<int>> inMatrix)
        {
            int criterion = 0;
            for (int i = 1; i < inMatrix.Count; i++)
            {
                for (int j = 1; j < inMatrix[i].Count; j++)
                {
                    criterion += inMatrix[i][j];
                }
            }
            return criterion;
        }

        /*
         * Функция проверок для реализации алгоритма 
         * 
         * flag == 1 - проверка на количество требований в соответствии с теоремой 1
         * flag == 2 - проверка на количество требований в первой партии в соответствии с теоремой 1
         * 
         */
        public bool CheckingMatrix(int flag, int type)
        {
            switch (flag)
            {
                case 1:
                    try
                    {
                        for (int i = 2; i < this.A2[type][1].Count; i++)
                            if (this.A2[type][1][1] < this.A2[type][1][i])
                            {
                                return false;
                            }
                    }
                    catch
                    {
                        return false;
                    }
                    break;
                case 2:
                    try
                    {
                        if (this.A2[type][1][1] < 2)
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        /*
         * Функция проверки наличия оставшихся в рассмотрении типов
         * 
         */ 
        private bool CheckType(List<int> type)
        {
            int count = 0;
            for (int j = 0; j < this.countType; j++)
            {
                if (type[j] != 0)
                    count++;
            }
            if (count == 0)
                return true;
            return false;
        }

        public void Perestanovka()
        {
            countBatches = new List<int>();
            for (int ii = 0; ii < countType; ii++)
            {
                countBatches.Add(0);
            }
            for (int i = 0; i < countType - 1; i++)
            {
                for (int j = i + 1; j < countType; j++)
                {
                    for (countBatches[i] = 1; countBatches[i] < A2[i].Count; countBatches[i]++)
                    {
                        for (countBatches[j] = 1; countBatches[j] < A2[j].Count; countBatches[j]++)
                        {
                            MessageBox.Show("Solution " + (i + 1) + " type and " + (j + 1) + " type");
                            MessageBox.Show("Составы партий " + (i + 1) + " типа : " + A2[i][countBatches[i]][0] + "; " + A2[i][countBatches[i]][1] + "Составы партий " + (j + 1) + " типа : " + A2[j][countBatches[j]][0] + "; " + A2[j][countBatches[j]][1]);
                        }
                    }
                }
            }
        }

        /*
         * Алгоритм формирования решения по составам паритй всех типов данных
         * 
         */ 
        public void GenerateSolution()
        {
            this.GenerateStartSolution();
            this.k = 0;
            this.f1 = 0;
            this.f1Buf = this.f1;
            int currentChangeType = 0;
            //Добавить вычисление значения критерия
            List<List<int>> MaxA = this.CopyMatrix(this.A);
            int maxF1 = this.f1;
            string s = "";
            if (flag == true)
            {
                while (!this.CheckType(this.I))
                {
                    this.solutionFlag = false;
                    //1 - Копируем I в Ii
                    for (int j = 0; j < this.countType; j++)
                    {
                        this.Ii[j] = this.I[j];
                    }
                    this.ABuf = this.CopyMatrix(this.A);
                    //this.f1 = this.GetCriterion(this.ABuf);
                    //Для каждого рассматриваемого типа
                    for (this.i = 0; this.i < this.Ii.Count; this.i++)
                    {
                        if (this.Ii[this.i] != 0)
                        {
                            if (this.np1i[this.i] > 0)
                            {
                                this.A1[this.i] = this.CopyMatrix(this.ABuf);
                                //MessageBox.Show("Решение по составу партий данных " + (this.i + 1) + " типа на " + (this.k + 1) + " шаге алгоритма");
                                //Получение состава партий фиксированного типа
                                List<List<int>> toBatchAlgoritm = new List<List<int>>();
                                toBatchAlgoritm.Add(new List<int>());
                                toBatchAlgoritm = this.A1[this.i];
                                test = new BatchTypeClaims(this.f1, this.i + 1, this.countClaims[this.i], toBatchAlgoritm, this.A1[this.i]);
                                test.GenerateSolution();

                                //test.PrintMatrix(2);
                                //test.PrintMatrix(3);

                                List<List<int>> tempA2 = test.ReturnMatrix(3);
                                if (tempA2.Count < 2)
                                {
                                    this.mi[this.i]++;
                                    this.A2[this.i] = new List<List<int>>();
                                    this.A2[this.i].Add(new List<int>());
                                    this.A2[this.i][0].Add(0);
                                    int sum = 0;
                                    for (int j = 1; j < this.mi[this.i]; j++)
                                    {
                                        this.A2[this.i][0].Add(2);
                                        sum += 2;
                                    }
                                    this.A2[this.i][0][0] = this.countClaims[this.i] - sum;
                                    if (!this.CheckingMatrix(1, this.i) && !this.CheckingMatrix(2, this.i))
                                    {
                                        this.I[this.i] = 0;
                                        this.mi[this.i] = 0;
                                        continue;
                                    }
                                }
                                else
                                {
                                    //this.A2 = this.CopyMatrix(tempA2);
                                    this.A2[this.i] = new List<List<int>>();
                                    for (int ii = 1; ii < tempA2.Count; ii++)
                                    {
                                        //this.A2.Add(new List<int>());
                                        this.A2[this.i].Insert(ii, tempA2[ii]);
                                    }
                                }

                                //Буферизируем текущее решение для сравнения
                                this.A1i = this.CopyMatrix(this.A1[this.i]);

                                for (this.q2 = 1; this.q2 < this.A2[this.i].Count; this.q2++)
                                {
                                    this.A1i[this.i + 1] = this.A2[this.i][this.q2];
                                    int f1g = 0;
                                    //secondLevel = new SecondLevel();
                                    //List<List<int>> tempA = CopyMatrix(this.A1i);
                                    //secondLevel.GenerateSolution(tempA);
                                    //List<List<int>> tempMatrixA = secondLevel.ReturnAMatrix();
                                    //f1g = this.GetCriterion(tempMatrixA);
                                    Random rand = new Random();
                                    int ret = rand.Next(5, 15);
                                    //if(ret < 10)
                                    if (f1g >= this.f1Buf)
                                    //if (f1g - this.f1Buf < 0 || f1g == 0)
                                    {
                                        this.f1Buf = f1g;
                                        this.solutionFlag = true;
                                        this.ABuf = this.CopyMatrix(this.A1i);
                                    }
                                }
                            }
                        }
                    }
                    this.k++;
                    if (this.solutionFlag)
                    {
                        if (this.f1Buf > this.f1)
                        {
                            MaxA = this.CopyMatrix(ABuf);
                            maxF1 = this.f1Buf;
                        }
                        this.f1 = this.f1Buf;
                        this.A = this.CopyMatrix(ABuf);
                        s = "Решение на текущем шаге\n";
                        foreach (List<int> row in this.A)
                        {
                            foreach (int colum in row)
                            {
                                if (colum >= 2)
                                    s += colum + ", ";
                            }
                            s += "\n";
                        }
                        MessageBox.Show(s);
                    }
                    else
                    {
                        this.Perestanovka();
                    }
                }
            }
            MessageBox.Show("На текущем " + this.k + " шаге получено решение");
            s = "Максимальное решение\n";
            foreach (List<int> row in MaxA)
            {
                foreach (int colum in row)
                {
                    if(colum >= 2)
                        s += colum + ", ";
                }
                s += "\n";
            }
            MessageBox.Show(s);
            MessageBox.Show("Количество обработанных требований " + maxF1);
        }
    }
}
