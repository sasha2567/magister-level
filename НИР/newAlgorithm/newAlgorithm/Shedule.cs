using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newAlgorithm
{
    class Shedule
    {
        private List<List<int>> R = new List<List<int>>();
        public static List<List<int>> TTreatment;
        public static List<List<List<int>>> TSwitching;
        private int timeConstructShedule;
        public static int L;
        private List<List<List<int>>> StartProcessing;
        private List<List<List<int>>> EndProcessing;
        public static int maxTimeSwitching = 4;
        public static int maxTimeTreatment = 4;
        


        public Shedule(List<List<int>> r)
        {
            R = r;
        }

        private void SetTime()
        {
            TSwitching = new List<List<List<int>>>();
            TTreatment = new List<List<int>>();
            Random rand = new Random();
            for (int i = 0; i < L; i++)
            {
                TTreatment.Add(new List<int>());
                TSwitching.Add(new List<List<int>>());
                for (int j = 0; j < R.Count; j++)
                {
                    //int otnosh = 2;
                    TTreatment[i].Add(rand.Next(2, maxTimeTreatment));
                    TSwitching[i].Add(new List<int>());
                    for (int k = 0; k < R.Count; k++)
                    {
                        TSwitching[i][j].Add(rand.Next(2, maxTimeSwitching));
                    }
                }
            }
        }

        private void CalculateShedule()
        {
            StartProcessing = new List<List<List<int>>>();
            EndProcessing = new List<List<List<int>>>();
            for (int i = 0; i < L; i++)//количество приборов
            {
                StartProcessing.Add(new List<List<int>>());
                EndProcessing.Add(new List<List<int>>());
                for (int k = 0; k < R[0].Count; k++)
                {
                    int ind = ReturnRIndex(k);
                    if (ind >= 0)
                    {
                        StartProcessing[i].Add(new List<int>());
                        EndProcessing[i].Add(new List<int>());
                        for (int p = 0; p < R[ind][k]; p++)//количество требований
                        {
                            StartProcessing[i][k].Add(0);
                            EndProcessing[i][k].Add(0);

                        }
                    }
                }
            } 
            int yy, zz, xx;
            for (int i = 0; i < L; i++)
            {
                yy = 0;
                zz = 0;
                xx = 0;
                for (int j = 0; j < R[0].Count; j++)
                {
                    int index = ReturnRIndex(j);
                    if (index >= 0)
                    {
                        for (int k = 0; k < R[index][j]; k++)
                        {
                            int timeToSwitch = TSwitching[i][xx][index];
                            if (index == xx && j != 0)
                                timeToSwitch = 0;
                            if (i > 0)
                            {
                                StartProcessing[i][j][k] = Math.Max(EndProcessing[i][yy][zz] + timeToSwitch, EndProcessing[i - 1][j][k]);
                            }
                            else 
                            { 
                                StartProcessing[i][j][k] = EndProcessing[i][yy][zz] + timeToSwitch; 
                            }
                            EndProcessing[i][j][k] = StartProcessing[i][j][k] + TTreatment[i][index];
                            timeConstructShedule = EndProcessing[i][j][k];
                            yy = j;
                            zz = k;
                            xx = index;
                        }
                    }
                }
            }
        }

        private int ReturnRIndex(int j)
        {
            for (int i = 0; i < R.Count; i++)
            {
                if (R[i][j] > 0)
                    return i;
            }
            return -1;
        }

        private List<List<int>> CopyMatrix(List<List<int>> inMatrix)
        {
            List<List<int>> ret = new List<List<int>>();
            for (int i = 0; i < inMatrix.Count; i++)
            {
                ret.Add(new List<int>());
                for (int j = 0; j < inMatrix[i].Count; j++)
                {
                    ret[i].Add(inMatrix[i][j]);
                }
            }
            return ret;
        }

        private void ChangeColum(int ind1, int ind2)
        {
            int indd1 = 0, indd2 = 0, temp;
            for (int i = 0; i < R.Count; i++)
            {
                if (R[i][ind1] > 0)
                {
                    indd1 = i;
                }
                if (R[i][ind2] > 0)
                {
                    indd2 = i;
                }
            }
            temp = R[indd1][ind1];
            R[indd1][ind1] = 0;
            R[indd2][ind1] = R[indd2][ind2];
            R[indd2][ind2] = 0;
            R[indd1][ind2] = temp;
        }

        private List<int> CopyList(List<int> inList)
        {
            List<int> ret = new List<int>();
            for (int i = 0; i < inList.Count; i++)
            {
                ret.Add(inList[i]);
            }
            return ret;
        }

        public List<List<int>> ConstructShedule()
        {
            List<List<int>> tempR = new List<List<int>>();
            int tempTime = 9999999;
            switch (R[0].Count)
            {
                case 1:
                    CalculateShedule();
                    break;
                case 2:
                    CalculateShedule();
                    tempR = CopyMatrix(R);
                    tempTime = timeConstructShedule;
                    ChangeColum(0, 1);
                    CalculateShedule();
                    if (tempTime < timeConstructShedule)
                    {
                        R = tempR;
                        timeConstructShedule = tempTime;
                    }
                    break;
                default:
                    CalculateShedule();
                    tempR = CopyMatrix(R);
                    tempTime = timeConstructShedule;
                    for (int i = R[0].Count - 1; i > 0; i--)
                    {
                        ChangeColum(i - 1, i);
                        CalculateShedule();
                        if (tempTime < timeConstructShedule)
                        {
                            R = tempR;
                            timeConstructShedule = tempTime;
                        }
                    }
                    break;
            }
            return R;
        }

        public int GetTime()
        {
            return timeConstructShedule;
        }
    }
}
