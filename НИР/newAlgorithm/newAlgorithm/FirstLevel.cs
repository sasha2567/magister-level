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
        private List<int> I;                // Вектор интерпритируемых типов данных
        private List<int> Ii;               // Вектор интерпритируемых типов данных на текущем шагу алгоритма
        private List<int> mi;               // Вектор количества партий данных для каждого типа данных
        private List<int> countBatches;     // Количество требований каждого типа
        private List<List<int>> Ai;         // Буферизированная матрица составов партий требований на k+1 шаге 
        private List<List<List<int>>> A1;   // Матрица составов партий требований на k+1 шаге 
        private List<List<List<int>>> A2;   // Матрица составов партий требований фиксированного типа
        private List<List<int>> A;          // Матрица составов партий требований на k шаге
        private int countType;              // Количество типов
        private List<int> countClaims;      // Начальное количество требований для каждого типа данных
        private int i;                      // идентификатор текущего изменяемого типа
        private int k;                      // Шаг алгоритма
        private List<int> fi;               // Критерии решений всех типов данных
        private int f1;                     // Критерий текущего решения для всех типов
        private int f1Buf;                  // Критерий текущего решения для всех типов
        private int L;                      // Количество сегментов конвейера
        private int G;                      // Текущее значение критерия


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
         * Функция копирования значений между векторами, предотвращающая копирование указателей
         * 
         */
        private List<int> CopyVector(List<int> inMatrix)
        {
            List<int> ret = new List<int>();
            for (int i = 0; i < inMatrix.Count; i++)
            {
                ret.Add(inMatrix[i]);
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
                this.A.Add(new List<int>());
                this.A[i].Add(this.countClaims[i] - claim);
                this.A[i].Add(claim);
            }
            for (int i = 0; i < this.countType; i++)
            {
                if (this.A[i][0] < 2 || this.A[i][0] < this.A[i][1])
                {
                    this.A[i].Clear();
                    this.A[i].Add(this.countClaims[i]);
                    this.mi[i] = 1;
                    this.I[i] = 0;
                    this.Ii[i] = 0;
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
            for (int i = 0; i < inMatrix.Count; i++)
            {
                for (int j = 0; j < inMatrix[i].Count; j++)
                {
                    criterion += inMatrix[i][j];
                }
            }
            return criterion;
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
                return false;
            return true;
        }


        /*
         * Построчное формирование матрицы промежуточного решения
         * 
         */
        private List<List<int>> SetTempAFromA2(int type, int ind2)
        {
            List<List<int>> result = this.CopyMatrix(this.Ai);
            result[type] = this.CopyVector(this.A2[type][ind2]);
            return result;
        }


        /*
         * Формирование матрицы для передачи её в модуль расписания
         * 
         */
        private List<List<int>> RenerateR(List<List<int>> m)
        {
            List<List<int>> result = new List<List<int>>();
            return result;
        }


        /*
         * Формирование новых решений по составим партий текущего типа данных
         * 
         */
        private List<List<int>> NewData(int type)
        {
            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < this.A1[type].Count; i++)
            {
                result.Add(this.CopyVector(this.A1[type][i]));
                for (int j = 1; j < result[i].Count; j++)
                {
                    if (result[i][0] > result[i][j] + 1)
                    {
                        result[i][0]--;
                        result[i][j]++;
                    }
                }
            }
            return result;
        }        


        /*
         * Алгоритм формирования решения по составам паритй всех типов данных
         * 
         */
        public void GenetateSolutionForAllTypes()
        {
            this.GenerateStartSolution();
            this.k = 0;
            List<List<int>> R = this.RenerateR(this.A);
            Shedule shedule = new Shedule(R, this.L);
            //R = shedule.ConstructShedule();
            // получаем решение от расписания
            // получаем критерий этого решения
            this.f1 = this.GetCriterion(this.A);
            this.f1Buf = this.f1;
            //Добавить вычисление значения критерия
            List<List<int>> MaxA = this.CopyMatrix(this.A);
            int maxF1 = this.f1;
            while (this.CheckType(this.I))
            {
                // Копируем I в Ii
                for (int i = 0; i < this.countType; i++)
                {
                    this.Ii[i] = this.I[i];
                }
                // Буферезируем текущее решение для построение нового на его основе
                this.Ai = this.CopyMatrix(this.A);
                this.A1 = new List<List<List<int>>>();
                for (int i = 0; i < this.countType; i++)
                {   
                    this.A1.Add(new List<List<int>>());
                    this.A1[i].Add(new List<int>());
                    this.A1[i][0] = this.CopyVector(this.A[i]);
                }
                bool typeSolutionFlag = false;
                List<List<int>> tempA = this.CopyMatrix(this.Ai);
                // Для каждого типа и каждого решения в типе строим новое решение и проверяем его на критерий
                this.A2 = new List<List<List<int>>>();
                for (int i = 0; i < this.countType; i++)
                {
                    this.A2.Add(new List<List<int>>());
                    this.A2[i] = this.NewData(i);
                    for (int j = 0; j < this.A2[i].Count; j++)
                    {
                        tempA = this.SetTempAFromA2(i, j);
                        R = this.RenerateR(tempA);
                        shedule = new Shedule(R, this.L);
                        //R = shedule.ConstructShedule();
                        // получаем решение от расписания
                        // получаем критерий этого решения
                        int fBuf = 0;
                        if (fBuf > this.f1Buf)
                        {
                            this.Ai = this.SetTempAFromA2(i, j);
                            typeSolutionFlag = true;
                        }
                    }
                }
                if (!typeSolutionFlag)
                {
                    //for (int i = 0; i < this.countType; i++)
                    //{
                    //    this.A2[i] = this.CopyMatrix(this.A1[i]);
                    //}
                    for (int i = 0; i < this.countType - 1; i++)
                    {
                        for (int j = i + 1; j < this.countType; j++)
                        {
                            this.A2[i] = this.NewData(i);
                            this.A2[j] = this.NewData(j);
                            for (int ii = 0; ii < this.A2[i].Count; ii++)
                            {
                                for (int jj = 0; jj < this.A2[j].Count; jj++)
                                {
                                    {
                                        tempA = this.SetTempAFromA2(i, ii);
                                        tempA[j] = this.SetTempAFromA2(j, jj)[j];
                                        R = this.RenerateR(tempA);
                                        shedule = new Shedule(R, this.L);
                                        //R = shedule.ConstructShedule();
                                        // получаем решение от расписания
                                        // получаем критерий этого решения
                                        int fBuf = 0;
                                        if (fBuf > this.f1Buf)
                                        {
                                            this.Ai = this.SetTempAFromA2(ii, jj);
                                            typeSolutionFlag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }        
    }
}
