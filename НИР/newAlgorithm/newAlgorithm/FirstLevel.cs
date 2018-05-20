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
        private List<List<int>> _abuf;                  // Буферизированная матрица составов партий требований на k+1 шаге
        private List<List<List<int>>> _a1;              // Матрица составов партий требований на k+1 шаге 
        private List<List<List<int>>> _a2;              // Матрица составов партий требований фиксированного типа
        private List<List<int>> _a;                     // Матрица составов партий требований на k шаге
        private readonly int _countType;                // Количество типов
        private readonly List<int> _countClaims;        // Начальное количество требований для каждого типа данных
        private int _f1;                                // Критерий текущего решения для всех типов
        private int _f1Buf;                             // Критерий текущего решения для всех типов
        private readonly bool _staticSolution;          // Признак фиксированных партий
        private List<int> _nTemp;
        private bool _typeSolutionFlag;


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
            if (ind2 < _a2[type].Count)
                result[type] = CopyVector(_a2[type][ind2]);
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
        /// НУжна для отладки вывода массива 
        /// </summary>
        /// <param name="m">входной лист</param>
        /// <returns>лист в виде строки</returns>
        private static string PrintList(List<int> m)
        {
            var result = "";
            foreach (var t in m)
            {
                result += t + ", ";
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
        public void GenetateSolutionForAllTypes(string fileName)
        {
            using (var file = new StreamWriter(fileName))
            {
                GenerateStartSolution();
                var shedule = new Shedule(_a);
                shedule.ConstructShedule();
                _f1 = shedule.GetTime();
                //MessageBox.Show(PrintA(A) + " Время обработки " + f1);
                _f1Buf = _f1;
                file.WriteLine(_f1Buf);
                var maxA = CopyMatrix(_a);
                _typeSolutionFlag = true;
                if (!_staticSolution)
                {
                    while (CheckType(_i))
                    {
                        // Буферезируем текущее решение для построение нового на его основе
                        _ai = CopyMatrix(_a);
                        if (_typeSolutionFlag)
                        {
                            _a1 = new List<List<List<int>>>();
                            for (var i = 0; i < _countType; i++)
                            {
                                _a1.Add(new List<List<int>>());
                                _a1[i].Add(new List<int>());
                                _a1[i][0] = CopyVector(_a[i]);
                            }
                            _typeSolutionFlag = false;
                        }

                        var tempA = CopyMatrix(_ai);
                        _abuf = CopyMatrix(_ai);
                        _f1Buf = _f1;

                        // Для каждого типа и каждого решения в типе строим новое решение и проверяем его на критерий
                        _a2 = new List<List<List<int>>>();
                        string s;
                        //file.WriteLine("окрестность 1 вида");
                        for (var i = 0; i < _countType; i++)
                        {
                            _a2.Add(new List<List<int>>());
                            if (_i[i] <= 0) continue;
                            _a2[i] = NewData(i);
                            for (var j = 0; j < _a2[i].Count; j++)
                            {
                                tempA = SetTempAFromA2(i, j);
                                shedule = new Shedule(tempA);
                                shedule.ConstructShedule();
                                var fBuf = shedule.GetTime();
                                s = PrintA(tempA);
                                //file.Write(s + " " + fBuf);
                                //MessageBox.Show(s + " Время обработки " + fBuf);                                    
                                if (fBuf < _f1Buf)
                                {
                                    _abuf = CopyMatrix(tempA);
                                    _typeSolutionFlag = true;
                                    _f1Buf = fBuf;
                                    //file.Write(" +");
                                }
                                //file.WriteLine();
                            }
                        }
                        if (!_typeSolutionFlag)
                        {
                            //file.WriteLine("комбинации типов");
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
                                                shedule = new Shedule(tempA);
                                                shedule.ConstructShedule();
                                                var fBuf = shedule.GetTime();
                                                s = PrintA(tempA);
                                                //file.Write(s + " " + fBuf);
                                                //MessageBox.Show(s + " Время обработки " + fBuf);
                                                if (fBuf < _f1Buf)
                                                {
                                                    _abuf = CopyMatrix(tempA);
                                                    _typeSolutionFlag = true;
                                                    _f1Buf = fBuf;
                                                    //file.Write(" +");
                                                }
                                                //file.WriteLine();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (_typeSolutionFlag)
                        {
                            //MessageBox.Show("Лучшее решение "+PrintA(Abuf) + " Время обработки " + f1Buf);
                            _a = CopyMatrix(_abuf);
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
                file.WriteLine(_f1);
                file.Close();
                MessageBox.Show("Решения найдены");
            }
        }


        /// <summary>
        /// Формирование перебора для всех возможных решений из А2
        /// </summary>
        /// <param name="ind">текущий индекс изменяемого решения для 1 типа</param>
        /// <param name="_n">Матрица номеров решений из А2</param>
        /// <param name="f">Файл для записей логов</param>
        ///Менят здесь для _\*РУСЛАН*/_
        private void GenerateCombination(int ind, List<int> _n, StreamWriter f)
        {
            for (int i = _countType - 1; i >= 0; i--)
            {
                for (int j = 0;j <_a2[i].Count; j++)
                {
                    _n[i]=j;
                    f.WriteLine(PrintList(_n));
                        GetSolution(_n, f);
                }        
            }
        }

        /// <summary>
        /// Подстановка данных из перебора и вычисление решения
        /// </summary>
        /// <param name="_n">Массив индексов решений из А2</param>
        /// <param name="f">Файл для записей логов</param>
        private void GetSolution(List<int> _n, StreamWriter f)
        {
            var tempA = CopyMatrix(_a);
            for (var j = 0; j < _countType; j++)
            {
                if (_n[j] >= 0)
                {
                    tempA[j] = CopyVector(SetTempAFromA2(j, _n[j])[j]);
                }
            }
            var shedule = new Shedule(tempA);
            shedule.ConstructShedule();
            var fBuf = shedule.GetTime();
            var s = PrintA(tempA);
            //f.Write(s + " - " + fBuf);
            //MessageBox.Show(s + " Время обработки " + fBuf);
            if (fBuf < _f1Buf)
            {
                _abuf = CopyMatrix(tempA);
                _typeSolutionFlag = true;
                _f1Buf = fBuf;
                //file.Write(" +");
            }
            //f.WriteLine();
        }


        /// <summary>
        /// Алгоритм формирования решения по составам паритй всех типов данных
        /// </summary>
        public int[] GenetateSolutionForAllTypesSecondAlgorithm()
        {
            var sets = new Sets(Form1.compositionSets, Form1.timeSets);
            var result = new[] { 0, 0 };
            using (var f = new StreamWriter("standartOutData.txt", true))
            {
                GenerateStartSolution();
                var shedule = new Shedule(_a);
                shedule.ConstructShedule();
                var r = shedule.RetyrnR();
                sets.GetSolution(r);
                var time = sets.GetCriterion();
                var _f1 = time;// shedule.GetTime();
                //MessageBox.Show(PrintA(A) + " Время обработки " + f1);
                _f1Buf = _f1;
                result[0] = _f1Buf;
                var maxA = CopyMatrix(_a);
                _typeSolutionFlag = true;
                if (!_staticSolution)
                {
                    while (CheckType(_i))
                    {
                        // Буферезируем текущее решение для построение нового на его основе
                        _ai = CopyMatrix(_a);
                        if (_typeSolutionFlag)
                        {
                            _a1 = new List<List<List<int>>>();
                            for (var i = 0; i < _countType; i++)
                            {
                                _a1.Add(new List<List<int>>());
                                _a1[i].Add(new List<int>());
                                _a1[i][0] = CopyVector(_a[i]);
                            }
                            _typeSolutionFlag = false;
                        }

                        var tempA = CopyMatrix(_ai);
                        _abuf = CopyMatrix(_ai);
                        _f1Buf = _f1;

                        // Для каждого типа и каждого решения в типе строим новое решение и проверяем его на критерий
                        _a2 = new List<List<List<int>>>();
                        string s;
                        //file.WriteLine("окрестность 1 вида");
                        for (var i = 0; i < _countType; i++)
                        {
                            _a2.Add(new List<List<int>>());
                            if (_i[i] <= 0) continue;
                            _a2[i] = NewData(i);
                            for (var j = 0; j < _a2[i].Count; j++)
                            {
                                tempA = SetTempAFromA2(i, j);
                                shedule = new Shedule(tempA);
                                shedule.ConstructShedule();
                                r = shedule.RetyrnR();
                                sets.GetSolution(r);
                                time = sets.CountReadySets();
                                var fBuf = time;// shedule.GetTime();
                                s = PrintA(tempA);
                                f.Write(s + " - " + fBuf);
                                //MessageBox.Show(s + " Время обработки " + fBuf);                                    
                                if (fBuf > _f1Buf)
                                {
                                    _abuf = CopyMatrix(tempA);
                                    _typeSolutionFlag = true;
                                    _f1Buf = fBuf;
                                    //file.Write(" +");
                                }
                                f.WriteLine();
                            }
                        }
                        if (!_typeSolutionFlag)
                        {
                            //file.WriteLine("комбинации типов");
                            List<int> _n = new List<int>();
                            _nTemp = new List<int>();
                            for (int i = 0; i < _countType; i++)
                            {
                                _nTemp.Add(0);
                                _n.Add(_a2[i].Count);
                                if (_n[i] == 0) _n[i] = -1;
                            }
                            GenerateCombination(0, _nTemp, f);
                        }
                        if (_typeSolutionFlag)
                        {
                            //MessageBox.Show("Лучшее решение "+PrintA(Abuf) + " Время обработки " + f1Buf);
                            _a = CopyMatrix(_abuf);
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
                        f.WriteLine("------------------");
                    }
                }
                result[1] = _f1;
                //MessageBox.Show("Решения найдены");
                f.Close();
            }
            return result;
        }
    }
}
