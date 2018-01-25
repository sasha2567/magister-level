using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newAlgorithm
{
    public class GAA
    {
        private readonly List<int> _i;                  // Вектор интерпритируемых типов данных
        private List<List<int>> _ai;                    // Буферизированная матрица составов партий требований на k+1 шаге 
        private List<List<List<int>>> _a1;              // Матрица составов партий требований на k+1 шаге 
        private List<List<List<int>>> _a2;              //false global fvg// Матрица составов партий требований фиксированного типа
        private List<List<int>> _a;                     // Матрица составов партий требований на k шаге
        protected readonly int _countType;              // Количество типов
        private readonly List<int> _countClaims;        // Начальное количество требований для каждого типа данных
        private int _f1;                                // Критерий текущего решения для всех типов
        private int _f1Buf;                             // Критерий текущего решения для всех типов
        private readonly bool _staticSolution;          // Признак фиксированных партий
        private List<List<List<int>>> Array;            // Состав пратий в виде массива
        Random rand = new Random();
        int N = 10;
        public List<int> _fitnesslist = new List<int>();
        public List<Xromossomi> nabor = new List<Xromossomi>();

        public GAA(int countType, List<int> countClaims, bool stat)
        {
            _countType = countType;
            _countClaims = countClaims;
            _staticSolution = stat;
            _i = new List<int>(_countType);
        }

        public GAA() { }

        public class Xromossomi
        {
            Random rand = new Random();
            int N = 10;
            static int size = 10;
            public List<List<int>> GenList = new List<List<int>>();
            public List<int> GenListOst = new List<int>();
            public List<int> GenA = new List<int>();
            public List<int> GenC = new List<int>();
            public List<int> GenB = new List<int>();
            public int ostA = size;
            public int ostB = size;
            public int ostC = size;
            public Xromossomi(int i)
            {
                ostA = i;
                ostB = i;
                ostC = i;
            }
            public Xromossomi() { }
        }



        public Xromossomi nachlist()
        {
            Xromossomi xrom = new Xromossomi();

            for (int i = 0; i < _countType; i++)
            {
                xrom.GenList.Add(new List<int>());
            }
            xrom.GenListOst.AddRange(_countClaims);

            for (int j = 0; j < xrom.GenList.Count; j++)
            {
                int buff = 0;
                for (int i = 0; i < N / 2 - 1; i++)
                {
                    if (xrom.GenListOst[j] == 2)
                    {
                        buff = 2;
                        xrom.GenListOst[j] = 0;
                    }
                    else
                        if (xrom.GenListOst[j] == 1)
                    {
                        xrom.GenList[j][xrom.GenList[j].Count - 1]++;
                        xrom.GenListOst[j] = 0;
                        buff = 0;
                    }
                    else
                            if (xrom.GenListOst[j] == 0)
                        buff = 0;
                    else
                        xrom.GenListOst[j] -= buff = rand.Next(2, xrom.GenListOst[j]);
                    xrom.GenList[j].Add(buff);
                }
                xrom.GenList[j].Add(xrom.GenListOst[j]);
            }
            return xrom;
        }

        Xromossomi nach()
        {


            Xromossomi xrom = new Xromossomi();
            int buff = 0;

            for (int i = 0; i < N / 2 - 1; i++)
            {
                if (xrom.ostA == 2)
                {
                    buff = 2;
                    xrom.ostA = 0;
                }
                else
                    if (xrom.ostA == 1)
                {
                    xrom.GenA[xrom.GenA.Count - 1]++;
                    xrom.ostA = 0;
                    buff = 0;
                }
                else
                        if (xrom.ostA == 0)
                    buff = 0;
                else
                    xrom.ostA -= buff = rand.Next(2, xrom.ostA);
                xrom.GenA.Add(buff);
            }
            xrom.GenA.Add(xrom.ostA);
            //
            int s = N % 3 == 2 ? N / 3 + 1 : N / 3;
            int t = N % 3 == 1 || N % 3 == 2 ? N / 3 + 1 : N / 3;
            buff = 0;
            for (int i = 0; i < N / 2 - 1; i++)
            {
                if (xrom.ostB == 0 && 10 == xrom.GenA.Sum() && xrom.GenA[2] != 0)
                {

                }

                if (xrom.ostB == 2)
                {
                    buff = 2;
                    xrom.ostB = 0;
                }
                else
                    if (xrom.ostB == 1)
                {
                    if (xrom.GenB.Count == 0)
                        xrom.GenA[(N / 3) - 1]++;
                    else
                        xrom.GenB[xrom.GenB.Count - 1]++;
                    xrom.ostB = 0;
                }
                else
                        if (xrom.ostB == 0)
                    buff = 0;
                else
                    xrom.ostB -= buff = rand.Next(2, xrom.ostB);
                xrom.GenB.Add(buff);
            }
            xrom.GenB.Add(xrom.ostB);

            ///
            for (int i = 0; i < N / 2 - 1; i++)
            {
                if (xrom.ostC == 2)
                {
                    buff = 2;
                    xrom.ostC = 0;
                }
                else
                    if (xrom.ostC == 1)
                {
                    xrom.GenC[xrom.GenC.Count - 1]++;
                    xrom.ostC = 0;
                }
                else
                        if (xrom.ostC == 0)
                    buff = 0;
                else
                    xrom.ostC -= buff = rand.Next(2, xrom.ostC);
                xrom.GenC.Add(buff);
            }

            if (xrom.ostC == 1)
            {
                xrom.GenC[xrom.GenC.Count - 1]++;
                xrom.ostC = 0;
            }
            else
                xrom.GenC.Add(xrom.ostC);

            return xrom;
        }

        public List<Xromossomi> SetXrom(int size)
        {
            for (int i = 0; i < size; i++)
            {
                var s = nachlist();
                if (!nabor.Contains(s))
                {
                    nabor.Add(nachlist());
                }
                else
                {
                    i--;
                }
            }
            //xor(size);

            return nabor;
        }
        public int getSelectionPopulation(int selection, out int s)
        {
            List<int> SortFitnessList = new List<int>(_fitnesslist);
            SortFitnessList.Sort();
            s = _fitnesslist.IndexOf(SortFitnessList[0]);
            return SortFitnessList[0];
        }



        void xor(int size, List<Xromossomi> nabr = null)
        {
            List<Xromossomi> naborInternal = new List<Xromossomi>(nabor);
            if (nabr != null)
            {
                naborInternal = new List<Xromossomi>(nabr);
            }


            int[] massA = new int[size];
            int[] massB = new int[size];
            int[] massC = new int[size];
            int buff;
            int v;
            for (int i = 0; i < size; i++)
            {
                massA[i] = i;
            }
            List<Xromossomi> nabor1 = new List<Xromossomi>();
            for (int i = 0; i < size; i++)
                nabor1.Add(new Xromossomi());

            for (int i = 0; i < size; i++)
            {
                buff = massA[v = rand.Next(size)];
                massA[v] = massA[v = rand.Next(size)];
                massA[v] = buff;
            }
            //
            for (int i = 0; i < size; i++)
            {
                buff = massB[v = rand.Next(size)];
                massB[v] = massB[v = rand.Next(size)];
                massB[v] = buff;
            }
            //
            for (int i = 0; i < size; i++)
            {
                buff = massC[v = rand.Next(size)];
                massC[v] = massC[v = rand.Next(size)];
                massC[v] = buff;
            }

            for (int i = 0; i < size; i++)
            {
                nabor1[i].GenA = naborInternal[massA[i]].GenA;
                nabor1[i].GenB = naborInternal[massA[i]].GenB;
                nabor1[i].GenC = naborInternal[massA[i]].GenC;
            }
            naborInternal = nabor1;
        }

        public List<List<int>> TestArray()
        {
            List<List<int>> a1 = new List<List<int>>();
            List<int> a = new List<int>();
            a.Add(10);
            a.Add(2);
            List<int> b = new List<int>();
            b.Add(10);
            b.Add(2);
            List<int> c = new List<int>();
            c.Add(10);
            c.Add(2);

            a1.Add(a);
            a1.Add(b);
            a1.Add(c);
            return a1;
        }

        public List<List<int>> GenerateR(IReadOnlyList<List<int>> m)
        {
            var result = new List<List<int>>();
            var summ = m.Sum(t => t.Count);
            for (var i = 0; i < _countType; i++)
            {
                result.Add(new List<int>());
                for (var j = 0; j < summ; j++)
                {
                    result[i].Add(0);
                }
            }
            var ind = 0;
            for (var i = 0; i < m.Count; i++)
            {
                for (var j = 0; j < m[i].Count; j++)
                {
                    result[i][ind] = m[i][j];
                    ind++;
                }
            }
            return result;
        }

        public int[] calcFitnessList() {
            List<int> FitnessList = new List<int>();
            var r = this.ToArrayList();
            int[] ni = { 8, 12, 16/*, 24, 32 */};
            int[] time = { 2, 4, 8, 16, 32 };
            int[] l = { 5, 10 };
            int[] n = { 5, 10 };
            int[,] compositionSetsForType = {
                {2, 2, 0, 2, 2, 0, 0, 0, 0, 0},
                {2, 0, 2, 2, 2, 0, 0, 0, 0, 0},
                {0, 2, 2, 2, 2, 0, 0, 0, 0, 0},
                {2, 2, 2, 0, 2, 0, 0, 0, 0, 0},
                {2, 2, 2, 2, 0, 0, 0, 0, 0, 0}
            };

            var CompositionSets = new List<List<int>>();
            var TimeSets = new List<int>();
            TimeSets.Add(95);
            TimeSets.Add(90);
            TimeSets.Add(95);
            TimeSets.Add(90);
            TimeSets.Add(95);
            for (int i = 0; i < 5; i++)
            {
                CompositionSets.Add(new List<int>());
                for (int j = 0; j < 10; j++)
                {
                    CompositionSets[i].Add(compositionSetsForType[i, j]);
                }
            }

            var test = new Sets(CompositionSets, TimeSets);
            List<int> CountKit = new List<int>();

            foreach (var elem in r)
            {
                var shedule = new Shedule(elem);
                shedule.ConstructShedule();
                FitnessList.Add(shedule.GetTime());
                test.GetSolution(shedule.RetyrnR());
                CountKit.Add(test.CountReadySets());
            }
            _fitnesslist = FitnessList;
            return CountKit.ToArray();
        }

        public List<List<List<int>>> ToArrayList()
        {
            List<List<List<int>>> r = new List<List<List<int>>>();
            foreach (var elem in nabor)
            {
                foreach (var elem2 in elem.GenList)
                {
                    elem2.Sort();
                    elem2.Reverse();
                    elem2.RemoveAll(e => e == 0);
                }

                r.Add(elem.GenList);
            }
            return r;
        }
        public List<List<List<int>>> ToArray()
        {
            List<List<List<int>>> arrResult = new List<List<List<int>>>();
            foreach (var hromosoma in nabor)
            {
                List<List<int>> arr = new List<List<int>>();
                int index = 0;
                arr.Add(new List<int>());
                for (int i = 0; i < hromosoma.GenA.Count; i++)
                {
                    if (hromosoma.GenA[i] > 0)
                        arr[index].Add(hromosoma.GenA[i]);
                }
                index++;
                arr.Add(new List<int>());
                //2 строка
                for (int i = 0; i < hromosoma.GenB.Count; i++)
                {
                    if (hromosoma.GenB[i] > 0)
                        arr[index].Add(hromosoma.GenB[i]);
                }
                index++;
                arr.Add(new List<int>());
                //3 строка
                for (int i = 0; i < hromosoma.GenC.Count; i++)
                {
                    if (hromosoma.GenC[i] > 0)
                        arr[index].Add(hromosoma.GenC[i]);
                }

                foreach (var elem in arr)
                {
                    elem.Sort();
                    elem.Reverse();
                }
                arrResult.Add(new  List<List<int>>(arr));
            }
            
            return arrResult;

        }
        void mutation()
        {
            Random rand = new Random();

            int i;
            foreach (var elem1 in nabor)
            {

                for (int i1 = 0; i1 < 1; i1++)
                {
                    if (elem1.GenA[i = rand.Next(elem1.GenA.Count - 1)] - 2 > 1)
                    {
                        elem1.GenA[i] -= 2;
                        elem1.GenA[rand.Next(elem1.GenA.Count - 1)] += 2;
                    }
                    if (elem1.GenB[i = rand.Next(elem1.GenB.Count - 1)] - 2 > 1)
                    {
                        elem1.GenB[i] -= 2;
                        elem1.GenB[rand.Next(elem1.GenB.Count - 1)] += 2;
                    }
                    if (elem1.GenC[i = rand.Next(elem1.GenC.Count - 1)] - 2 > 1)
                    {
                        elem1.GenC[i] -= 2;
                        elem1.GenC[rand.Next(elem1.GenC.Count - 1)] += 2;
                    }
                }
            };

        }
    }
}
