﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newAlgorithm
{
    public class GAA
    {
        Random rand = new Random();
        int N = 10;
        List<Xromossomi> nabor = new List<Xromossomi>();
        public class Xromossomi
        {
            Random rand = new Random();
            int N = 10;
            static int size = 10;
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
                nabor.Add(nach());
            }
            xor(size);

            return nabor;
        }

        void xor(int size)
        {
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
                nabor1[i].GenA = nabor[massA[i]].GenA;
                nabor1[i].GenB = nabor[massA[i]].GenB;
                nabor1[i].GenC = nabor[massA[i]].GenC;
            }
            nabor = nabor1;
        }

        public List<List<int>> ToArray()
        {
            List<List<int>> arr= new List<List<int>>();
            int index=-1;
           
            foreach(var hromosoma in nabor){
                index++;
                arr.Add(new List<int>());
                for (int i = 0; i < hromosoma.GenA.Count; i++)
                {                   
                    arr[index].Add(hromosoma.GenA[i]);                
                }
                index++;
                arr.Add(new List<int>());
                //2 строка
                for (int i = 0; i < hromosoma.GenB.Count; i++)
                {                   
                    arr[index].Add(hromosoma.GenB[i]);                   
                }
                index++;
                arr.Add(new List<int>());
                //3 строка
                for (int i = 0; i < hromosoma.GenC.Count; i++)
                {
                    arr[index].Add(hromosoma.GenC[i]);
                }
            }
            return arr;

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
