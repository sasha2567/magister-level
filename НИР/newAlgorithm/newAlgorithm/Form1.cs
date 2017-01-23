﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newAlgorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<int> copy(List<int> _in)
        {
            List<int> result = new List<int>();
            foreach (int elem in _in)
            {
                result.Add(elem);
            }
            return result;
        }

        private List<List<int>> составыПартий(List<List<int>> _in){
            List<List<int>> A = new List<List<int>>();
            int count = 0;
            A.Add(new List<int>());
            A[count] = copy(_in[0]);
            count++;
            for (int i = 0; i < _in.Count; i++)
            {
                for (int j = 1; j < _in[i].Count; j++)
                {
                    do
                    {
                        A.Add(new List<int>());
                        A[count] = copy(A[count - 1]);
                        A[count][0]--;
                        A[count][j]++;
                        count++;
                    }
                    while (A[count - 1][0] > A[count - 1][j]);       
                }
            }
            return A;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int countType = (int)numericUpDown1.Value;
            int countBatches = Convert.ToInt32(textBox1.Text);
            List<int> listCountButches = new List<int>();
            //var A1 = new List<List<List<int>>>();
            //var temp = new List<List<int>>();
            //temp.Add(new List<int>());
            //temp.Add(new List<int>());
            //temp[0].Add(countBatches - 2);
            //temp[0].Add(2);
            for (int ii = 0; ii < countType; ii++)
            {
                //A1.Add(составыПартий(temp));
                listCountButches.Add(countBatches);
            }
            FirstLevel firstLevel = new FirstLevel(countType, listCountButches);
            firstLevel.GenetateSolutionForAllTypes();
            //var mass = new List<int>();
            //for (int ii = 0; ii < countType; ii++)
            //{
            //    mass.Add(0);
            //}
            //for (int i = 0; i < countType - 1; i++)
            //{
            //    for (int j = i + 1; j < countType; j++)
            //    {
            //        for (mass[i] = 1; mass[i] < A1[i].Count; mass[i]++)
            //        {
            //            for (mass[j] = 1; mass[j] < A1[j].Count; mass[j]++)
            //            {
            //                MessageBox.Show("Составы партий " + (i + 1) + " типа : " + A1[i][mass[i]][0] + "; " + A1[i][mass[i]][1] + "Составы партий " + (j + 1) + " типа : " + A1[j][mass[j]][0] + "; " + A1[j][mass[j]][1]);
            //            }
            //        }
            //    }   
            //}
        }
    }
}
