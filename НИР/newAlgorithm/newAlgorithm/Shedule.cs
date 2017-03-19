using System;
using System.Collections.Generic;
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

        public Shedule(List<List<int>> r)
        {
            _r = r;
        }

        private void CalculateShedule()
        {
            _startProcessing = new List<List<List<int>>>();
            _endProcessing = new List<List<List<int>>>();
            for (var i = 0; i < L; i++)//количество приборов
            {
                _startProcessing.Add(new List<List<int>>());
                _endProcessing.Add(new List<List<int>>());
                for (var k = 0; k < _r[0].Count; k++)//количество партий
                {
                    var ind = ReturnRIndex(k);
                    _startProcessing[i].Add(new List<int>());
                    _endProcessing[i].Add(new List<int>());
                    for (var p = 0; p < _r[ind][k]; p++)//количество требований
                    {
                        _startProcessing[i][k].Add(0);
                        _endProcessing[i][k].Add(0);
                    }
                }
            }
            var yy = 0;
            var zz = 0;
            var xx = 0;
            for (var i = 0; i < L; i++)
            {
                for (var j = 0; j < _r[0].Count; j++)
                {
                    var index = ReturnRIndex(j);


                    for (var k = 0; k < _r[index][j]; k++)
                    {
                        var timeToSwitch = Switching[i][xx][index];
                        if (index == xx && j != 0)
                            timeToSwitch = 0;
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
            /*for (var j = 0; j < _r[0].Count; j++)
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
                    var timeToSwitch = (type == index && j != 0) ? 0 : Switching[i][type][index];
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
            for (var i = 0; i < _r.Count; i++)
            {
                if (_r[i][j] > 0)
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
