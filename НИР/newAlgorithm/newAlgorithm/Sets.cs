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
        public Sets(List<List<int>> composition, List<int> time)
        {
            _types = composition.Count;
            _composition = composition;
            _time = time;
            _readySets = new List<List<Kit>>();
            for (int i = 0; i < _types; i++)
            {
                _readySets.Add(new List<Kit>());
                _readySets[i].Add(new Kit(composition[i]));
            }
        }

        public int CountReadySets()
        {
            int res = 0;
            foreach (var row in _readySets)
            {
                foreach (var elem in row)
                {
                    if (elem.IsSetAllComposition()) res++;
                }
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        protected void AddKit(int type)
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
        protected void AddBatches(SheduleElement sheduleElement)
        {
            foreach (var row in _readySets)
            {
                foreach(var elem in row)
                {
                    if (!elem.IsSetAllComposition())
                    {
                        sheduleElement = elem.AddBatch(sheduleElement.getValue(), sheduleElement.getType(), sheduleElement.getTime());
                    }
                    if (sheduleElement.getValue() == 0)
                    {
                        return;
                    }
                }
            }
            if (sheduleElement.getValue() > 0)
            {
                AddKit(0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shedule"></param>
        public void GetSolution(List<SheduleElement> shedule)
        {
            foreach (var element in shedule)
            {
                AddBatches(element);
            }
        }
    }
}
