using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newAlgorithm
{
    public class GaaSecondLevel
    {
        public List<List<int>> GetGaaSecondLevelGroup(List<List<int>> Group)
        {
            var rand = new Random();
            var size = Group.Select(c => c.Count);
            var result = new List<List<int>>()
            {
                new List<int>(),
                new List<int>(),
                new List<int>()
            };
            for(var i = 0;i<rand.Next(3,6);i++)
            {
                var number = rand.Next(0, size.Sum());
                if (number < size.ToArray()[0])
                    result[0].Add(Group[0][number]);
                if (number > size.ToArray()[0] && number < size.ToArray()[1])
                    result[1].Add(Group[1][number - size.ToArray()[0]]);
                if (number > size.ToArray()[1] && number < size.ToArray()[2])
                    result[2].Add(Group[2][number - size.ToArray()[0] - size.ToArray()[1]]);
            }
            return result;
        }

    }
}
