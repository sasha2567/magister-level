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
            for (int i = 0; i < _types; i++)
            {
                for (int j = 0; j < _readySets[i].Count; j++)
                {
                    if (batch > 0 && !_readySets[i][j].IsSetAllComposition())
                    {
                        batch = _readySets[i][j].AddBatch(batch, type);
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
                    }
                }
            }
            return res;
        }

        public void GetSolution(List<List<int>> shedule)
        {
            foreach (var row in shedule)
            {
                foreach (var elem in row)
                {
                    if (elem != 0)
                    {
                        if (!CountReadySets())
                        {

                        }
                    }
                }
            }
        }
    }
}
