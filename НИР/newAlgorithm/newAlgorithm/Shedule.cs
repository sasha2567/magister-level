﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newAlgorithm
{
    class Shedule
    {
        private List<List<int>> _r;
        public static List<List<int>> Treatment;
        public static List<List<List<int>>> Switching;
        private int _timeConstructShedule;
        public static int L;
        private List<List<List<int>>> _startProcessing;
        private List<List<List<int>>> _endProcessing;
        private List<SheduleElement> _rWithTime;

        public Shedule(List<List<int>> r, int l)
        {
            this._r = r;
            L = l;
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
            var maxColumn = 0;
            for (var j = 0; j < summ; j++)
            {
                result.Add(new List<int>());
                for (var i = 0; i < m.Count; i++)
                {
                    result[j].Add(0);
                }
            }
            for (var i = 0; i < m.Count; i++)
            {
                if (m[i].Count > maxColumn)
                {
                    maxColumn = m[i].Count;
                }
            }
            var ind = 0;
            for (var j = 0; j < maxColumn; j++)
            {
                for (var i = 0; i < m.Count; i++)
                {
                    if (m[i].Count > j)
                    {
                        result[ind][i] = m[i][j];
                        ind++;
                    }
                }
            }
            return result;
        }

        public List<SheduleElement> RetyrnR()
        {
            _rWithTime = new List<SheduleElement>();
            for (int i = 0; i < _endProcessing[_endProcessing.Count - 1].Count; i++)
            {
                var ind = ReturnRIndex(i);
                _rWithTime.Add(new SheduleElement(_r[i][ind], ind, _endProcessing[_endProcessing.Count - 1][i]));
            }
            return _rWithTime;
        }
        
        public Shedule(List<List<int>> r)
        {
            _r = GenerateR(r);
        }

        private void CalculateShedule()
        {
            _startProcessing = new List<List<List<int>>>();
            _endProcessing = new List<List<List<int>>>();
            for (var i = 0; i < L; i++)//количество приборов
            {
                _startProcessing.Add(new List<List<int>>());
                _endProcessing.Add(new List<List<int>>());
                for (var k = 0; k < _r.Count; k++)//количество партий
                {
                    try
                    {
                        var ind = ReturnRIndex(k);
                        _startProcessing[i].Add(new List<int>());
                        _endProcessing[i].Add(new List<int>());
                        for (var p = 0; p < _r[k][ind]; p++)//количество требований
                        {
                            _startProcessing[i][k].Add(0);
                            _endProcessing[i][k].Add(0);
                        }
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
            }
            /**/
            var yy = 0;
            var zz = 0;
            var xx = 0;
            for (var i = 0; i < L; i++)
            {
                for (var j = 0; j < _r.Count; j++)
                {
                    var index = ReturnRIndex(j);


                    for (var k = 0; k < _r[j][index]; k++)
                    {
                        var timeToSwitch = (index == xx && j != 0) ? 0 : Switching[0][xx][index];
                        if (i > 0)
                        {
                            _startProcessing[i][j][k] = Math.Max(_endProcessing[i][yy][zz] + timeToSwitch, _endProcessing[i - 1][j][k]);
                        }
                        else
                        {
                            _startProcessing[i][j][k] = _endProcessing[i][yy][zz] + timeToSwitch;
                        }
                        _endProcessing[i][j][k] = _startProcessing[i][j][k] + Treatment[i][index];
                        _timeConstructShedule = _endProcessing[i][j][k];
                        yy = j;
                        zz = k;
                        xx = index;
                    }
                }
                yy = 0;
                zz = 0;
                xx = 0;
            }
            /**/
            /*
            for (var j = 0; j < _r[0].Count; j++)
            {
                var index = ReturnRIndex(j);
                var type = j == 0 ? index : ReturnRIndex(j - 1);
                var timeToSwitch = (type == index && j != 0) ? 0 : Switching[0][type][index];
                var timeToTreament = Treatment[0][index];
                if (j > 0)
                {
                    var last = _r[type][j - 1] - 1;
                    _startProcessing[0][j][0] = _endProcessing[0][j - 1][last];
                }
                for (var k = 0; k < _r[index][j]; k++)
                {
                    if (k == 0)
                    {
                        _startProcessing[0][j][k] += timeToSwitch;
                    }
                    else
                    {
                        _startProcessing[0][j][k] += _endProcessing[0][j][k - 1];
                    }
                    _endProcessing[0][j][k] = _startProcessing[0][j][k] + timeToTreament;
                }
            }
            for (var i = 1; i < L; i++)
            {
                for (var j = 0; j < _r[0].Count; j++)
                {
                    var index = ReturnRIndex(j);
                    var type = j == 0 ? index : ReturnRIndex(j - 1);
                    var timeToSwitch = (type == index && j > 0) ? 0 : Switching[i][type][index];
                    var timeToTreament = Treatment[i][index];
                    for (var k = 0; k < _r[index][j]; k++)
                    {
                        if (k == 0)
                        {
                            if (j > 0)
                            {
                                var last = _r[type][j - 1] - 1;
                                _startProcessing[i][j][k] += Math.Max(_endProcessing[i][j - 1][last] + timeToSwitch,
                                    _endProcessing[i - 1][j][k]);
                            }
                            else
                            {
                                _startProcessing[i][j][k] += _endProcessing[i - 1][j][k] + timeToSwitch;
                            }
                            
                        }
                        else
                        {
                            _startProcessing[i][j][k] += Math.Max(_endProcessing[i][j][k - 1], _endProcessing[i - 1][j][k]);
                        }
                        _endProcessing[i][j][k] = _startProcessing[i][j][k] + timeToTreament;
                        _timeConstructShedule = _endProcessing[0][j][k];
                    }
                }
            }*/
        }

        private int ReturnRIndex(int j)
        {
            for (var i = 0; i < _r[j].Count; i++)
            {
                if (_r[j][i] > 0)
                    return i;
            }
            return -1;
        }

        private static List<List<int>> CopyMatrix(IReadOnlyList<List<int>> inMatrix)
        {
            var ret = new List<List<int>>();
            for (var i = 0; i < inMatrix.Count; i++)
            {
                ret.Add(new List<int>());
                for (var j = 0; j < inMatrix[i].Count; j++)
                {
                    ret[i].Add(inMatrix[i][j]);
                }
            }
            return ret;
        }

        private void ChangeColum(int ind1, int ind2)
        {
            var indd1 = 0;
            var indd2 = 0;
            for (var i = 0; i < _r.Count; i++)
            {
                if (_r[i][ind1] > 0)
                {
                    indd1 = i;
                }
                if (_r[i][ind2] > 0)
                {
                    indd2 = i;
                }
            }
            var temp = _r[indd1][ind1];
            _r[indd1][ind1] = 0;
            _r[indd2][ind1] = _r[indd2][ind2];
            _r[indd2][ind2] = 0;
            _r[indd1][ind2] = temp;
        }

        public List<List<int>> ConstructShedule()
        {
            var tempTime = 9999999;
            CalculateShedule();
            var tempR = CopyMatrix(_r);
            tempTime = _timeConstructShedule;
            for (var i = 0; i < _r[0].Count - 1; i++)
            {
                for (var j = i + 1; j < _r[0].Count; j++)
                ChangeColum(i, j);
                CalculateShedule();
                if (tempTime >= _timeConstructShedule) continue;
                _r = tempR;
                _timeConstructShedule = tempTime;
            }
            return _r;
        }

        public int GetTime()
        {
            return _timeConstructShedule;
        }
    }
}
