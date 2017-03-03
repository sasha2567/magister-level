using System;
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
                for (var k = 0; k < _r[0].Count; k++)
                {
                    var ind = ReturnRIndex(k);
                    if (ind < 0) continue;
                    _startProcessing[i].Add(new List<int>());
                    _endProcessing[i].Add(new List<int>());
                    for (var p = 0; p < _r[ind][k]; p++)//количество требований
                    {
                        _startProcessing[i][k].Add(0);
                        _endProcessing[i][k].Add(0);

                    }
                }
            } 
            for (var i = 0; i < L; i++)
            {
                var yy = 0;
                var zz = 0;
                var xx = 0;
                for (var j = 0; j < _r[0].Count; j++)
                {
                    var index = ReturnRIndex(j);
                    if (index < 0) continue;
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
            }
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

        private List<int> CopyList(IEnumerable<int> inList)
        {
            return inList.ToList();
        }

        public List<List<int>> ConstructShedule()
        {
            var tempTime = 9999999;
            CalculateShedule();
            var tempR = CopyMatrix(_r);
            tempTime = _timeConstructShedule;
            for (var i = _r[0].Count - 1; i > 0; i--)
            {
                ChangeColum(i - 1, i);
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
