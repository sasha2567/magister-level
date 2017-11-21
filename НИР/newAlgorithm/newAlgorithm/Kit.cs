using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newAlgorithm
{
    class Kit
    {
        private readonly List<int> _composition;
        private List<int> _readyComposition;
        private int _time;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="composition"></param>
        public Kit(List<int> composition)
        {
            _composition = composition;
            _readyComposition = new List<int>();
            for (int i = 0; i < composition.Count; i++)
            {
                _readyComposition.Add(0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="type"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int AddBatch(int batch, int type, int time)
        {
            _readyComposition[type] += batch;
            if (ChechCompositionType(type))
            {
                var difference = _readyComposition[type] - _composition[type];
                _readyComposition[type] = _composition[type];
                return difference;
                _time = difference * time / batch;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSetAllComposition()
        {
            for(int i = 0; i < _composition.Count; i++)
            {
                if (_readyComposition[i] != _composition[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool ChechCompositionType(int type)
        {
            return _readyComposition[type] > _composition[type] ? true : false;
        }
    }
}
