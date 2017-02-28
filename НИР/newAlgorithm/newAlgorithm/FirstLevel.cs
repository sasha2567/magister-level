﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace newAlgorithm
{
    class FirstLevel
    {
        private readonly List<int> _i;                  // Вектор интерпритируемых типов данных
        private List<List<int>> _ai;                    // Буферизированная матрица составов партий требований на k+1 шаге 
        private List<List<List<int>>> _a1;              // Матрица составов партий требований на k+1 шаге 
        private List<List<List<int>>> _a2;              // Матрица составов партий требований фиксированного типа
        private List<List<int>> _a;                     // Матрица составов партий требований на k шаге
        private readonly int _countType;                // Количество типов
        private readonly List<int> _countClaims;        // Начальное количество требований для каждого типа данных
        private int _f1;                                // Критерий текущего решения для всех типов
        private int _f1Buf;                             // Критерий текущего решения для всех типов
        private readonly bool _staticSolution;          // Признак фиксированных партий


        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="countType">количество типов рассматриваемых данных</param>
        /// <param name="count_claims">количество требований всех типов данных</param>
        public FirstLevel(int countType, List<int> count_claims, bool stat)
        {
            _countType = countType;
            _countClaims = count_claims;
            _staticSolution = stat;
            _i = new List<int>(_countType);
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
            _a = new List<List<int>>();
            for (int i = 0; i < _countType; i++)
            {
                _i.Add(1);
                _a.Add(new List<int>());
                _a[i].Add(_countClaims[i] - claim);
                _a[i].Add(claim);
            }
            for (int i = 0; i < _countType; i++)
            {
                if (_a[i][0] < 2 || _a[i][0] < _a[i][1])
                {
                    _a[i].Clear();
                    _a[i].Add(_countClaims[i]);
                    _i[i] = 0;
                }
            }
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
            for (int j = 0; j < _countType; j++)
            {
                if (type[j] > 0)
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
            List<List<int>> result = CopyMatrix(_a);
            result[type] = CopyVector(_a2[type][ind2]);
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
            for (int i = 0; i < _countType; i++)
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
                        int countFind = 0;
                        if (inList.Count != temp[i].Count)
                        {
                            return false;
                        }
                        for (int k = 0; k < inList.Count; k++)
                        {
                            if (inList[k] == temp[i][k])
                            {
                                countFind++;
                            }
                        }
                        return countFind == inList.Count ? true : false;
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

            foreach (var row2 in _a1[type])
            {
                foreach (var rowMatrix in inMatrix.ToList())
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
            foreach(List<int> row in _a1[type])
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
                        _i[i] = 0;
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
                var r = GenerateR(_a);
                var shedule = new Shedule(r);
                shedule.ConstructShedule();
                _f1 = shedule.GetTime();
                //MessageBox.Show(PrintA(A) + " Время обработки " + f1);
                _f1Buf = _f1;
                file.WriteLine(PrintA(_a) + " " + _f1Buf + " Начальное решение");
                var maxA = CopyMatrix(_a);
                var typeSolutionFlag = true;
                if (!_staticSolution)
                {
                    while (CheckType(_i))
                    {
                        // Буферезируем текущее решение для построение нового на его основе
                        _ai = CopyMatrix(_a);
                        if (typeSolutionFlag)
                        {
                            _a1 = new List<List<List<int>>>();
                            for (var i = 0; i < _countType; i++)
                            {
                                _a1.Add(new List<List<int>>());
                                _a1[i].Add(new List<int>());
                                _a1[i][0] = CopyVector(_a[i]);
                            }
                            typeSolutionFlag = false;
                        }

                        var tempA = CopyMatrix(_ai);
                        var abuf = CopyMatrix(_ai);
                        _f1Buf = _f1;

                        // Для каждого типа и каждого решения в типе строим новое решение и проверяем его на критерий
                        _a2 = new List<List<List<int>>>();
                        string s;
                        file.WriteLine("окрестность 1 вида");
                        for (var i = 0; i < _countType; i++)
                        {
                            _a2.Add(new List<List<int>>());
                            if (_i[i] > 0)
                            {
                                _a2[i] = NewData(i);
                                for (var j = 0; j < _a2[i].Count; j++)
                                {
                                    tempA = SetTempAFromA2(i, j);
                                    r = GenerateR(tempA);
                                    shedule = new Shedule(r);
                                    shedule.ConstructShedule();
                                    int fBuf = shedule.GetTime();
                                    s = PrintA(tempA);
                                    file.Write(s + " " + fBuf);
                                    //MessageBox.Show(s + " Время обработки " + fBuf);                                    
                                    if (fBuf < _f1Buf)
                                    {
                                        abuf = CopyMatrix(tempA);
                                        typeSolutionFlag = true;
                                        _f1Buf = fBuf;
                                        file.Write(" +");
                                    }
                                    file.WriteLine();
                                }
                            }

                        }
                        if (!typeSolutionFlag)
                        {
                            file.WriteLine("комбинации типов");
                            for (var i = 0; i < _countType - 1; i++)
                            {
                                if (_i[i] > 0)
                                {
                                    for (var j = i + 1; j < _countType; j++)
                                    {
                                        if (_i[j] > 0)
                                        {
                                            _a2[i] = NewData(i);
                                            _a2[j] = NewData(j);
                                            for (var ii = 0; ii < _a2[i].Count; ii++)
                                            {
                                                for (var jj = 0; jj < _a2[j].Count; jj++)
                                                {
                                                    {
                                                        tempA = SetTempAFromA2(i, ii);
                                                        tempA[j] = CopyVector(SetTempAFromA2(j, jj)[j]);
                                                        r = GenerateR(tempA);
                                                        shedule = new Shedule(r);
                                                        shedule.ConstructShedule();
                                                        var fBuf = shedule.GetTime();
                                                        s = PrintA(tempA);
                                                        file.Write(s + " " + fBuf);
                                                        //MessageBox.Show(s + " Время обработки " + fBuf);
                                                        if (fBuf < _f1Buf)
                                                        {
                                                            abuf = CopyMatrix(tempA);
                                                            typeSolutionFlag = true;
                                                            _f1Buf = fBuf;
                                                            file.Write(" +");
                                                        }
                                                        file.WriteLine();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (typeSolutionFlag)
                        {
                            //MessageBox.Show("Лучшее решение "+PrintA(Abuf) + " Время обработки " + f1Buf);
                            _a = CopyMatrix(abuf);
                            _f1 = _f1Buf;
                        }
                        else
                        {
                            for (int i = 0; i < _countType; i++)
                            {
                                _a1[i] = CopyMatrix(_a2[i]);
                                if (!_a1[i].Any() || !_a1[i][0].Any())
                                {
                                    _i[i] = 0;
                                }
                            }
                        }
                    }
                }
                file.Close();
                MessageBox.Show("Решения найдены");
            }
        }


        /// <summary>
        /// Формирование матрици перебора для всех возможных решений из А2
        /// </summary>
        /// <returns>Матрица номеров решений из А2</returns>
        private List<List<int>> GenerateMatrix()
        {
            var ret = new List<List<int>>();
            var temp = new List<int>();
            var summ = 1;
            for (int i = 0; i < _countType; i++)
            {
                temp.Add(0);
                if (_i[i] > 0)
                {
                    temp[i] = _a2[i].Count;
                    summ *= _a2[i].Count;
                }
            }
            for (int i = 0; i < summ; i++)
            {
                ret.Add(new List<int>());
                for (int j = 0; j < _countType; j++)
                {
                    ret[i].Add(0);
                }
            }
            var count = 1;
            for (int k = _countType - 1; k >= 0; k--)
            {
                for (int j = 0; j < summ; j++)
                {
                    for (int i = 0; i < temp[k]; i++)
                    {
                        for (int l = 0; l < count; l++)
                        {
                               ret[j+l][k] = i;
                        }
                    }
                }
                count *= temp[k];
            }
            return ret;
        }


        /// <summary>
        /// Алгоритм формирования решения по составам паритй всех типов данных
        /// </summary>
        public void GenetateSolutionForAllTypesSecondAlgorithm()
        {
            GenerateStartSolution();
            _a2 = new List<List<List<int>>>();
            _a2.Add(new List<List<int>>());
            _a2.Add(new List<List<int>>());
            _a2.Add(new List<List<int>>());

            for (var i = 0; i < 3; i++)
            {
                _a2[i].Add(new List<int>());
                _a2[i].Add(new List<int>());
                _a2[i].Add(new List<int>());
            }
            var matrix = GenerateMatrix();
            var t = PrintA(matrix);
            var flagg = false;
            if (flagg)
            {
                using (var file = new StreamWriter("output.txt"))
                {
                    GenerateStartSolution();
                    var r = GenerateR(_a);
                    var shedule = new Shedule(r);
                    shedule.ConstructShedule();
                    _f1 = shedule.GetTime();
                    //MessageBox.Show(PrintA(A) + " Время обработки " + f1);
                    _f1Buf = _f1;
                    file.WriteLine(PrintA(_a) + " " + _f1Buf + " Начальное решение");
                    var maxA = CopyMatrix(_a);
                    var typeSolutionFlag = true;
                    if (!_staticSolution)
                    {
                        while (CheckType(_i))
                        {
                            // Буферезируем текущее решение для построение нового на его основе
                            _ai = CopyMatrix(_a);
                            if (typeSolutionFlag)
                            {
                                _a1 = new List<List<List<int>>>();
                                for (var i = 0; i < _countType; i++)
                                {
                                    _a1.Add(new List<List<int>>());
                                    _a1[i].Add(new List<int>());
                                    _a1[i][0] = CopyVector(_a[i]);
                                }
                                typeSolutionFlag = false;
                            }

                            var tempA = CopyMatrix(_ai);
                            var abuf = CopyMatrix(_ai);
                            _f1Buf = _f1;

                            // Для каждого типа и каждого решения в типе строим новое решение и проверяем его на критерий
                            _a2 = new List<List<List<int>>>();
                            string s;
                            file.WriteLine("окрестность 1 вида");
                            for (var i = 0; i < _countType; i++)
                            {
                                _a2.Add(new List<List<int>>());
                                if (_i[i] > 0)
                                {
                                    _a2[i] = NewData(i);
                                    for (var j = 0; j < _a2[i].Count; j++)
                                    {
                                        tempA = SetTempAFromA2(i, j);
                                        r = GenerateR(tempA);
                                        shedule = new Shedule(r);
                                        shedule.ConstructShedule();
                                        int fBuf = shedule.GetTime();
                                        s = PrintA(tempA);
                                        file.Write(s + " " + fBuf);
                                        //MessageBox.Show(s + " Время обработки " + fBuf);                                    
                                        if (fBuf < _f1Buf)
                                        {
                                            abuf = CopyMatrix(tempA);
                                            typeSolutionFlag = true;
                                            _f1Buf = fBuf;
                                            file.Write(" +");
                                        }
                                        file.WriteLine();
                                    }
                                }

                            }
                            if (!typeSolutionFlag)
                            {
                                file.WriteLine("комбинации типов");
                                //while (true)
                                //{
                                //    for (var i = 0; i < _countType; i++)
                                //    {
                                //        if (_i[i] > 0)
                                //        {
                                //            tempA[i] = CopyVector(SetTempAFromA2(i, indexes[i])[i]);
                                //        }
                                //    }
                                //    r = GenerateR(tempA);
                                //    shedule = new Shedule(r);
                                //    shedule.ConstructShedule();
                                //    var fBuf = shedule.GetTime();
                                //    s = PrintA(tempA);
                                //    file.Write(s + " " + fBuf);
                                //    //MessageBox.Show(s + " Время обработки " + fBuf);
                                //    if (fBuf < _f1Buf)
                                //    {
                                //        abuf = CopyMatrix(tempA);
                                //        typeSolutionFlag = true;
                                //        _f1Buf = fBuf;
                                //        file.Write(" +");
                                //    }
                                //    file.WriteLine();
                                //    indexes[indexChangeType]++;
                                //    if (indexes[indexChangeType] >= _a2[indexChangeType].Count)
                                //    {
                                //        indexes[indexChangeType] = 0;
                                //        indexChangeType--;
                                //    }
                                //    if (indexChangeType < 0)
                                //    {
                                //        break;
                                //    }
                                //}


                            }
                            if (typeSolutionFlag)
                            {
                                //MessageBox.Show("Лучшее решение "+PrintA(Abuf) + " Время обработки " + f1Buf);
                                _a = CopyMatrix(abuf);
                                _f1 = _f1Buf;
                            }
                            else
                            {
                                for (int i = 0; i < _countType; i++)
                                {
                                    _a1[i] = CopyMatrix(_a2[i]);
                                    if (!_a1[i].Any() || !_a1[i][0].Any())
                                    {
                                        _i[i] = 0;
                                    }
                                }
                            }
                        }
                    }
                    file.Close();
                    MessageBox.Show("Решения найдены");
                }
            }
        }
    }
}
