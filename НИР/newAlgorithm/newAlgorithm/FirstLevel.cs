using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace newAlgorithm
{
    class FirstLevel
    {
        private List<int> I;                // Вектор интерпритируемых типов данных
        private List<int> Ii;               // Вектор интерпритируемых типов данных на текущем шагу алгоритма
        private List<int> mi;               // Вектор количества партий данных для каждого типа данных
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


        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="count_type">количество типов рассматриваемых данных</param>
        /// <param name="count_claims">количество требований всех типов данных</param>
        public FirstLevel(int count_type, List<int> count_claims)
        {
            countType = count_type;
            countClaims = count_claims;
            mi = new List<int>(countType);
            I = new List<int>(countType);
            Ii = new List<int>(countType);
        }


        /// <summary>
        /// Функция копирования значений между матрицами, предотвращающая копирование указателей
        /// </summary>
        /// <param name="inMatrix">Входная матрица</param>
        /// <returns>Выходная матрица</returns>
        private List<List<int>> CopyMatrix(List<List<int>> inMatrix)
        {
            List<List<int>> ret = new List<List<int>>();
            for (int i = 0; i < inMatrix.Count; i++)
            {
                ret.Add(CopyVector(inMatrix[i]));
            }
            return ret;
        }


        /// <summary>
        /// Функция копирования значений между векторами, предотвращающая копирование указателей
        /// </summary>
        /// <param name="inMatrix">Входной вектор</param>
        /// <returns>Выходной вектор</returns>
        private List<int> CopyVector(List<int> inMatrix)
        {
            List<int> ret = new List<int>();
            for (int i = 0; i < inMatrix.Count; i++)
            {
                ret.Add(inMatrix[i]);
            }
            return ret;
        }


        /// <summary>
        /// Алгоритм формирования начальных решений по составам партий всех типов
        /// </summary>
        public void GenerateStartSolution()
        {
            int claim = 2;
            A = new List<List<int>>();
            for (int i = 0; i < countType; i++)
            {
                I.Add(1);
                Ii.Add(1);
                mi.Add(claim);
                A.Add(new List<int>());
                A[i].Add(countClaims[i] - claim);
                A[i].Add(claim);
            }
            for (int i = 0; i < countType; i++)
            {
                if (A[i][0] < 2 || A[i][0] < A[i][1])
                {
                    A[i].Clear();
                    A[i].Add(countClaims[i]);
                    mi[i] = 1;
                    I[i] = 0;
                    Ii[i] = 0;
                }
            }
            G = 0;    
        }


        /// <summary>
        /// Функция вычисления f1 критерия
        /// </summary>
        /// <param name="inMatrix">Матрица А на текущем шаге</param>
        /// <returns>Значение критериия</returns>
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


        /// <summary>
        /// Функция проверки наличия оставшихся в рассмотрении типов
        /// </summary>
        /// <param name="type">список всех рассматриваемых типов</param>
        /// <returns>наличие еще рассматриваемых типов</returns>
        private bool CheckType(List<int> type)
        {
            int count = 0;
            for (int j = 0; j < countType; j++)
            {
                if (type[j] != 0)
                    count++;
            }
            if (count == 0)
                return false;
            return true;
        }


        /// <summary>
        /// Построчное формирование матрицы промежуточного решени
        /// </summary>
        /// <param name="type">тип рассматриваемого решения</param>
        /// <param name="ind2">индекс подставляемого решения</param>
        /// <returns>матрица А с подставленным новым решением в соответствующий тип</returns>
        private List<List<int>> SetTempAFromA2(int type, int ind2)
        {
            List<List<int>> result = CopyMatrix(Ai);
            result[type] = CopyVector(A2[type][ind2]);
            return result;
        }


        /// <summary>
        /// Формирование матрицы для передачи её в модуль расписания
        /// </summary>
        /// <param name="m">входная матрица А</param>
        /// <returns>сформированная матрица для уровня расписания</returns>
        private List<List<int>> GenerateR(List<List<int>> m)
        {
            List<List<int>> result = new List<List<int>>();
            int summ = 0;
            for (int i = 0; i < m.Count; i++)
            {
                summ += m[i].Count;
            }
            for (int i = 0; i < countType; i++)
            {
                result.Add(new List<int>());
                for (int j = 0; j < summ; j++)
                {
                    result[i].Add(0);
                }
            }
            int ind = 0;
            for (int i = 0; i < m.Count; i++)
            {
                for (int j = 0; j < m[i].Count; j++)
                {
                    result[i][ind] = m[i][j];
                    ind++;
                }
            }
            return result;
        }


        /// <summary>
        /// Функция получения неповторяющихся решений в матрице А2 на шаге 9
        /// </summary>
        /// <param name="inMatrix">входная матрица сформированных решений</param>
        /// <returns>Новые решения без повторений</returns>
        public List<List<int>> SortedMatrix(List<List<int>> inMatrix)
        {
            List<List<int>> temp = CopyMatrix(inMatrix);
            //Удаление повторяющихся строк
            int countLoops = 0;
            while (true)
            {
                for (int i = 1; i < temp.Count; i++)
                {
                    int lastIndexForDelete = temp.FindLastIndex(delegate(List<int> inList)
                    {
                        int count_find = 0;
                        if (inList.Count != temp[i].Count)
                        {
                            return false;
                        }
                        for (int k = 0; k < inList.Count; k++)
                        {
                            if (inList[k] == temp[i][k])
                            {
                                count_find++;
                            }
                        }
                        return count_find == inList.Count ? true : false;
                    });
                    if (lastIndexForDelete != i)
                    {
                        temp.RemoveAt(lastIndexForDelete);
                        inMatrix.RemoveAt(lastIndexForDelete);
                    }
                }
                countLoops++;
                if (countLoops > 100)
                    break;
            }
            return inMatrix;
        }

        
        /// <summary>
        /// Удаление повторений новых решений совпадающих с A1
        /// </summary>
        /// <param name="inMatrix">матрица новых решений</param>
        /// <param name="type">рассматриваемый тип</param>
        /// <returns>Полученные новые решения</returns>
        private List<List<int>> CheckMatrix(List<List<int>> inMatrix, int type)
        {

            foreach (List<int> row2 in A1[type])
            {
                foreach (List<int> rowMatrix in inMatrix.ToList())
                {
                    if (rowMatrix.Zip(row2, (a, b) => new { a, b }).All(pair => pair.a == pair.b))
                    {
                        inMatrix.Remove(rowMatrix);
                    }
                }
            }
            return inMatrix;
        }


        /// <summary>
        /// Формирование новых решений по составим партий текущего типа данных
        /// </summary>
        /// <param name="type">рассматриваемый тип</param>
        /// <returns>новые решения для этого типа</returns>
        private List<List<int>> NewData(int type)
        {
            List<List<int>> result = new List<List<int>>();
            foreach(List<int> row in A1[type])
            {
                for (int j = 1; j < row.Count; j++)
                {
                    result.Add(CopyVector(row));
                    if (row[0] > row[j] + 1)
                    {
                        result[result.Count - 1][0]--;
                        result[result.Count - 1][j]++;
                    }
                }
                if (result[result.Count - 1][0] == row[0])
                {
                    int summ = row[0];
                    result[result.Count - 1].Add(2);
                    for (int j = 1; j < row.Count; j++)
                    {
                        summ += row[j];
                        result[result.Count - 1][j] = 2;
                    }
                    result[result.Count - 1][0] = summ - 2 * (result[result.Count - 1].Count - 1);
                }
            }
            int count = 0;
            while (true)
            {
                for (int i = 1; i < result.Count; i++)
                {
                    for (int j = 1; j < result[i].Count; j++)
                    {
                        if (result[i][j] > result[i][j - 1])
                        {
                            result.Remove(result[i]);
                            break;
                        }
                    }
                }
                count++;
                if (count > 3)
                {
                    break;
                }
            }
            
            result = SortedMatrix(result);
            result = CheckMatrix(result,type);
            return result;
        }


        /// <summary>
        /// Формирование новых решений по составим партий текущего типа данных
        /// </summary>
        /// <param name="m">матрица А для печати</param>
        /// <returns>строка с составами партий по типам</returns>
        private string PrintA(List<List<int>> m)
        {
            string result = "";
            for (int i = 0; i < m.Count; i++)
            {
                for (int j = 0; j < m[i].Count - 1; j++)
                {
                    result += m[i][j] + ", ";
                }
                result += m[i][m[i].Count - 1] + "; ";
            }
            return result;
        }


        /// <summary>
        /// Проверка на достижение максимально возможного решения по составам типов
        /// </summary>
        /// <param name="inMatrix">Матрица текущих составов</param>
        private void CheckSolution(List<List<int>> inMatrix)
        {
            for (int i = 0; i < inMatrix.Count; i++)
            {
                int elem = inMatrix[i][0];
                if (elem == 2)
                {
                    int count = 1;
                    for (int j = 1; j < inMatrix[i].Count; j++)
                    {
                        if (inMatrix[i][j] == elem)
                        {
                            count++;
                        }
                    }
                    if (count == inMatrix[i].Count)
                    {
                        I[i] = 0;
                    }
                }
            }
        }


        /// <summary>
        /// Алгоритм формирования решения по составам паритй всех типов данных
        /// </summary>
        public void GenetateSolutionForAllTypes()
        {
            using (var file = new StreamWriter("output.txt"))
            {
                GenerateStartSolution();
                k = 0;
                var R = GenerateR(A);
                var shedule = new Shedule(R);
                R = shedule.ConstructShedule();
                // получаем решение от расписания
                // получаем критерий этого решения
                f1 = shedule.GetTime();
                MessageBox.Show(PrintA(A) + " Время обработки " + f1);
                f1Buf = f1;
                //Добавить вычисление значения критерия
                List<List<int>> MaxA = CopyMatrix(A);
                bool typeSolutionFlag = false;
                while (CheckType(I))
                {
                    // Копируем I в Ii
                    for (int i = 0; i < countType; i++)
                    {
                        Ii[i] = I[i];
                    }

                    // Буферезируем текущее решение для построение нового на его основе
                    Ai = CopyMatrix(A);
                    if (!typeSolutionFlag)
                    {
                        A1 = new List<List<List<int>>>();
                        for (int i = 0; i < countType; i++)
                        {
                            A1.Add(new List<List<int>>());
                            A1[i].Add(new List<int>());
                            A1[i][0] = CopyVector(A[i]);
                        }
                    }

                    List<List<int>> tempA = CopyMatrix(Ai);
                    List<List<int>> Abuf = CopyMatrix(Ai);
                    f1Buf = f1;

                    // Для каждого типа и каждого решения в типе строим новое решение и проверяем его на критерий
                    A2 = new List<List<List<int>>>();
                    string s;
                    for (int i = 0; i < countType; i++)
                    {
                        if (I[i] != 0)
                        {
                            A2.Add(new List<List<int>>());
                            A2[i] = NewData(i);
                            for (int j = 0; j < A2[i].Count; j++)
                            {
                                tempA = SetTempAFromA2(i, j);
                                CheckSolution(tempA);
                                R = GenerateR(tempA);
                                shedule = new Shedule(R);
                                R = shedule.ConstructShedule();
                                // получаем решение от расписания
                                // получаем критерий этого решения
                                int fBuf = shedule.GetTime();
                                s = PrintA(tempA);
                                file.WriteLine(s + " " + fBuf);
                                if (fBuf <= f1Buf)
                                {
                                    Abuf = CopyMatrix(tempA);
                                    typeSolutionFlag = true;
                                    f1Buf = fBuf;
                                }
                            }
                        }

                    }
                    if (!typeSolutionFlag)
                    {
                        for (int i = 0; i < countType - 1; i++)
                        {
                            if (I[i] != 0)
                            {
                                for (int j = i + 1; j < countType; j++)
                                {
                                    if (I[j] != 0)
                                    {
                                        A2[i] = NewData(i);
                                        A2[j] = NewData(j);
                                        for (int ii = 0; ii < A2[i].Count; ii++)
                                        {
                                            for (int jj = 0; jj < A2[j].Count; jj++)
                                            {
                                                {
                                                    tempA = SetTempAFromA2(i, ii);
                                                    tempA[j] = SetTempAFromA2(j, jj)[j];
                                                    CheckSolution(tempA);
                                                    R = GenerateR(tempA);
                                                    shedule = new Shedule(R);
                                                    R = shedule.ConstructShedule();
                                                    // получаем решение от расписания
                                                    // получаем критерий этого решения
                                                    int fBuf = shedule.GetTime();
                                                    s = PrintA(tempA);
                                                    file.WriteLine(s + " " + fBuf);
                                                    if (fBuf <= f1Buf)
                                                    {
                                                        Abuf = CopyMatrix(tempA);
                                                        typeSolutionFlag = true;
                                                        f1Buf = fBuf;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!typeSolutionFlag)
                    {
                        A1 = A2;
                    }
                    if (typeSolutionFlag)
                    {
                        A = CopyMatrix(Abuf);
                        f1 = f1Buf;
                        typeSolutionFlag = false;
                    }
                }
                file.Close();
                MessageBox.Show("Решения найдены");
            }
        }        
    }
}
