using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newAlgorithm
{
    class Sets
    {
        private readonly int _types;
        private readonly List<List<int>> _composition;
        private List<List<Kit>> _readySets;
        private readonly List<int> _time;

        public Sets(int countType, List<List<int>> composition, List<int> time)
        {
            _types = countType;
            _composition = composition;
            _time = time;
            _readySets = new List<List<Kit>>();
            for (int i = 0; i < _types; i++)
            {
                _readySets.Add(new List<Kit>());
            }
        }

        public bool CountReadySets()
        {
            foreach (var row in _readySets)
            {
                foreach (var elem in row)
                {
                    if (elem.IsSetAllComposition()) return true;
                }
            }
            return false;
                    
        }

        public void AddKit(int type)
        {
            _readySets[type].Add(new Kit(_composition[type]));
        }

        public int AddBatches(int batch, int type)
        {
            foreach (var row in _readySets)
            {
                foreach(var elem in row)
                {
                    if (batch > 0 && !elem.IsSetAllComposition())
                    {
                        batch = elem.AddBatch(batch, type);
                    }
                }
            }
            return batch;
        }

        public List<int> SetsInWork()
        {
            var res = new List<int>();
            for (int i = 0; i < _types; i++)
            {
                res.Add(0);
            }
            for (int i = 0; i < _types; i++)
            {
                for (int j = 0; j < _readySets[i].Count; j++)
                {
                    if (!_readySets[i][j].IsSetAllComposition())
                    {
                        res[i] = j;
                        break;
                    }
                }
            }
            return res;
        }

        private int[] GetBatch(List<List<int>> shedule, int index)
        {
            var value = 0;
            var ind = 0;
            for(int i = 0; i < shedule.Count; i++)
            {
                if (shedule[i][index] != 0)
                {
                    value = shedule[i][index];
                    ind = i;
                    break;
                }
            }
            return new int[] {value, ind};
        }

        public void GetSolution(List<List<int>> shedule)
        {
            for (int i = 0; i < shedule[0].Count; i++)
            {
                var result = GetBatch(shedule, i);
                var value = result[0];
                var type = result[1];
                if (value != 0)
                {
                    if (!CountReadySets())
                    {
                        AddBatches(value, type);
                    }
                }
            }
        }
    }
}
