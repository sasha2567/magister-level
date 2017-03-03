using System.Collections.Generic;
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
        public FirstLevel(int countType, List<int> countClaims, bool stat)
        {
            _countType = countType;
            _countClaims = countClaims;
            _staticSolution = stat;
            _i = new List<int>(_countType);
        }


        /// <summary>
        /// Функция копирования значений между матрицами, предотвращающая копирование указателей
        /// </summary>
        /// <param name="inMatrix">Входная матрица</param>
        /// <returns>Выходная матрица</returns>
        private List<List<int>> CopyMatrix(IEnumerable<List<int>> inMatrix)
        {
            return inMatrix.Select(CopyVector).ToList();
        }


        /// <summary>
        /// Функция копирования значений между векторами, предотвращающая копирование указателей
        /// </summary>
        /// <param name="inMatrix">Входной вектор</param>
        /// <returns>Выходной вектор</returns>
        private static List<int> CopyVector(List<int> inMatrix)
        {
            return inMatrix.ToList();
        }


        /// <summary>
        /// Алгоритм формирования начальных решений по составам партий всех типов
        /// </summary>
        public void GenerateStartSolution()
        {
            const int claim = 2;
            _a = new List<List<int>>();
            for (var i = 0; i < _countType; i++)
            {
                _i.Add(1);
                _a.Add(new List<int>());
                _a[i].Add(_countClaims[i] - claim);
                _a[i].Add(claim);
            }
            for (var i = 0; i < _countType; i++)
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
            return inMatrix.SelectMany(t => t).Sum();
        }


        /// <summary>
        /// Функция проверки наличия оставшихся в рассмотрении типов
        /// </summary>
        /// <param name="type">список всех рассматриваемых типов</param>
        /// <returns>наличие еще рассматриваемых типов</returns>
        private bool CheckType(IReadOnlyList<int> type)
        {
            var count = 0;
            for (var j = 0; j < _countType; j++)
            {
                if (type[j] > 0)
                    count++;
            }
            return count != 0;
        }


        /// <summary>
        /// Построчное формирование матрицы промежуточного решени
        /// </summary>
        /// <param name="type">тип рассматриваемого решения</param>
        /// <param name="ind2">индекс подставляемого решения</param>
        /// <returns>матрица А с подставленным новым решением в соответствующий тип</returns>
        private List<List<int>> SetTempAFromA2(int type, int ind2)
        {
            var result = CopyMatrix(_a);
            result[type] = CopyVector(_a2[type][ind2]);
            return result;
        }


        /// <summary>
        /// Формирование матрицы для передачи её в модуль расписания
        /// </summary>
        /// <param name="m">входная матрица А</param>
        /// <returns>сформированная матрица для уровня расписания</returns>
        private List<List<int>> GenerateR(IReadOnlyList<List<int>> m)
        {
            var result = new List<List<int>>();
            var summ = m.Sum(t => t.Count);
            for (var i = 0; i < _countType; i++)
            {
                result.Add(new List<int>());
                for (var j = 0; j < summ; j++)
                {
                    result[i].Add(0);
                }
            }
            var ind = 0;
            for (var i = 0; i < m.Count; i++)
            {
                for (var j = 0; j < m[i].Count; j++)
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
            var temp = CopyMatrix(inMatrix);
            //Удаление повторяющихся строк
            var countLoops = 0;
            while (true)
            {
                for (var i = 1; i < temp.Count; i++)
                {
                    var lastIndexForDelete = temp.FindLastIndex(delegate(List<int> inList)
                    {
                        if (inList.Count != temp[i].Count)
                        {
                            return false;
                        }
                        var countFind = inList.Where((t, k) => t == temp[i][k]).Count();
                        return countFind == inList.Count ? true : false;
                    });
                    if (lastIndexForDelete == i) continue;
                    temp.RemoveAt(lastIndexForDelete);
                    inMatrix.RemoveAt(lastIndexForDelete);
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
            var result = new List<List<int>>();
            foreach(var row in _a1[type])
            {
                for (var j = 1; j < row.Count; j++)
                {
                    result.Add(CopyVector(row));
                    if (row[0] <= row[j] + 1) continue;
                    result[result.Count - 1][0]--;
                    result[result.Count - 1][j]++;
                }
                if (result[result.Count - 1][0] != row[0]) continue;
                {
                    var summ = row[0];
                    result[result.Count - 1].Add(2);
                    for (var j = 1; j < row.Count; j++)
                    {
                        summ += row[j];
                        result[result.Count - 1][j] = 2;
                    }
                    result[result.Count - 1][0] = summ - 2 * (result[result.Count - 1].Count - 1);
                }
            }
            var count = 0;
            while (true)
            {
                for (var i = 1; i < result.Count; i++)
                {
                    for (var j = 1; j < result[i].Count; j++)
                    {
                        if (result[i][j] <= result[i][j - 1]) continue;
                        result.Remove(result[i]);
                        break;
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
        private static string PrintA(IEnumerable<List<int>> m)
        {
            var result = "";
            foreach (var t in m)
            {
                for (var j = 0; j < t.Count - 1; j++)
                {
                    result += t[j] + ", ";
                }
                result += t[t.Count - 1] + "; ";
            }
            return result;
        }


        /// <summary>
        /// Проверка на достижение максимально возможного решения по составам типов
        /// </summary>
        /// <param name="inMatrix">Матрица текущих составов</param>
        private void CheckSolution(IReadOnlyList<List<int>> inMatrix)
        {
            for (var i = 0; i < inMatrix.Count; i++)
            {
                var elem = inMatrix[i][0];
                if (elem != 2) continue;
                var count = 1;
                for (var j = 1; j < inMatrix[i].Count; j++)
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
                            if (_i[i] <= 0) continue;
                            _a2[i] = NewData(i);
                            for (var j = 0; j < _a2[i].Count; j++)
                            {
                                tempA = SetTempAFromA2(i, j);
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
                        if (!typeSolutionFlag)
                        {
                            file.WriteLine("комбинации типов");
                            for (var i = 0; i < _countType - 1; i++)
                            {
                                if (_i[i] <= 0) continue;
                                for (var j = i + 1; j < _countType; j++)
                                {
                                    if (_i[j] <= 0) continue;
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
        private IEnumerable<List<int>> GenerateMatrix()
        {
            var ret = new List<List<int>>();
            var n = new List<int>();
            var summ = 1;
            for (var i = 0; i < _countType; i++)
            {
                n.Add(0);
                if (_i[i] > 0)
                {
                    n[i] = _a2[i].Count;
                    summ *= _a2[i].Count;
                }
                else
                {
                    n[i] = 1;
                }
            }
            for (var i = 0; i < summ; i++)
            {
                ret.Add(new List<int>());
                for (var j = 0; j < _countType; j++)
                {
                    ret[i].Add(0);
                }
            }
            //заполнение первого столбца
            for (int i = 0, l = 0; i < summ; i += summ / n[0], l++)
            {
                for (var j = 0; j < summ / n[0]; j++)
                {
                    ret[i + j][0] = l;
                }
                
            }
            //заполнение средних столбцов
            for (var i = 1; i < _countType - 1; i++)
            {
                //получение интервала повторений значений
                var index = 1;
                for (var j = i; j < _countType; j++)
                {
                    index *= n[j];
                }
                
                
                for (var j = 0; j < summ; j += index)
                {
                    //получение интервала повторения одного значения
                    var index2 = 1;
                    for (var k = i + 1; k < _countType; k++)
                    {
                        index2 *= n[k];
                    }


                    for (int h = 0, l = 0; h < index; h += index2, l++)
                    {
                        for (var k = 0; k < index2; k++)
                        {
                            ret[j + h + k][i] = l;
                        }
                    }
                }
            }
            //заполнение последнего столбца
            for (var i = 0; i < summ; i += n[_countType - 1])
            {
                for (var j = 0; j < n[_countType - 1]; j++)
                {
                    ret[i + j][_countType - 1] = j;
                }

            }
            return ret;
        }


        /// <summary>
        /// Алгоритм формирования решения по составам паритй всех типов данных
        /// </summary>
        public void GenetateSolutionForAllTypesSecondAlgorithm()
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
                            if (_i[i] <= 0) continue;
                            _a2[i] = NewData(i);
                            for (var j = 0; j < _a2[i].Count; j++)
                            {
                                tempA = SetTempAFromA2(i, j);
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
                        if (!typeSolutionFlag)
                        {
                            file.WriteLine("комбинации типов");
                            var matrix = GenerateMatrix();
                            foreach (var row in matrix)
                            {
                                for (var j = 0; j < _countType; j++)
                                {
                                    if (_i[j] > 0)
                                    {
                                        tempA[j] = CopyVector(SetTempAFromA2(j, row[j])[j]);
                                    }
                                }
                            }
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
