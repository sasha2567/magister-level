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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countType"></param>
        /// <param name="composition"></param>
        /// <param name="time"></param>
        public Sets(int countType, List<List<int>> composition, List<int> time)
        {
            _types = countType;
            _composition = composition;
            _time = time;
            _readySets = new List<List<Kit>>();
            for (int i = 0; i < _types; i++)
            {
                _readySets.Add(new List<Kit>());
                _readySets[i].Add(new Kit(composition[i]));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void AddKit(int type)
        {
            _readySets[type].Add(new Kit(_composition[type]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="type"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int AddBatches(int batch, int type, int time)
        {
            foreach (var row in _readySets)
            {
                foreach(var elem in row)
                {
                    if (batch > 0 && !elem.IsSetAllComposition())
                    {
                        batch = elem.AddBatch(batch, type, time);
                    }
                }
            }
            return batch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<int> SetsInWork()
        {
            var result = new List<int>();
            for (int i = 0; i < _types; i++)
            {
                result.Add(0);
            }
            for (int i = 0; i < _types; i++)
            {
                for (int j = 0; j < _readySets[i].Count; j++)
                {
                    if (!_readySets[i][j].IsSetAllComposition())
                    {
                        result[i] = j;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<int> ReadySets()
        {
            var result = new List<int>();
            for (int i = 0; i < _types; i++)
            {
                result.Add(0);
            }
            for (int i = 0; i < _types; i++)
            {
                for (int j = 0; j < _readySets[i].Count; j++)
                {
                    if (!_readySets[i][j].IsSetAllComposition())
                    {
                        result[i] = j - 1;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shedule"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int[] GetBatch(List<List<List<int>>> shedule, int index)
        {
            var value = 0;
            var time = 0;
            var ind = 0;
            for (int i = 0; i < shedule.Count; i++)
            {
                if (shedule[i][index].Count != 0)
                {
                    value = shedule[i][index][0];
                    time = shedule[i][index][1];
                    ind = i;
                    break;
                }
            }
            return new int[] {value, ind, time};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shedule"></param>
        public void GetSolution(List<List<List<int>>> shedule)
        {
            for (int i = 0; i < shedule[0].Count; i++)
            {
                var result = GetBatch(shedule, i);
                var value = result[0];
                var type = result[1];
                var time = result[2];
                if (value != 0)
                {
                    if (!CountReadySets())
                    {
                        AddBatches(value, type, time);
                    }
                }
            }
        }
    }
}
