using System;
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
        int l, maxS, maxT;
        int countType, countBatches;
        List<List<List<int>>> temptS = new List<List<List<int>>>();
        List<List<int>> temptT = new List<List<int>>();

        public Form1()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            countType = (int)numericUpDown1.Value;
            countBatches = Convert.ToInt32(countBatchesTB.Text);
            List<int> listCountButches = new List<int>();
            for (int ii = 0; ii < countType; ii++)
            {
                listCountButches.Add(countBatches);
            }

            l = Convert.ToInt32(LTB.Text);
            maxS = Convert.ToInt32(timeSwitchingTB.Text);
            maxT = Convert.ToInt32(timeTreatmentingTB.Text);
            temptS = new List<List<List<int>>>();
            temptT = new List<List<int>>();
            RandomTime();
            PrintTime();
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

        private List<List<int>> составыПартий(List<List<int>> _in)
        {
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

        private void RandomTime()
        {
            int count = 0;
            for (int i = 0; i < l; i++)
            {
                temptT.Add(new List<int>());
                temptS.Add(new List<List<int>>());
                for (int j = 0; j < countType; j++)
                {
                    int temp = (count % 2 == 0) ? 2 : maxT;
                    temptT[i].Add(temp);
                    temptS[i].Add(new List<int>());
                    for (int k = 0; k < countType; k++)
                    {
                        temp = (count % 2 == 0) ? 2 : maxS;
                        temptS[i][j].Add(temp);
                    }
                    count++;
                }
            }
        }

        private void PrintTime()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.RowCount = temptT.Count;
            dataGridView1.ColumnCount = temptT[0].Count;
            for (int i = 0; i < temptT.Count; i++)
            {
                for (int j = 0; j < countType; j++)
                {
                    dataGridView1[j, i].Value = temptT[i][j];
                }
            }
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.RowCount = temptS.Count * temptS[0].Count;
            dataGridView2.ColumnCount = temptS[0].Count;
            for (int k = 0; k < temptS.Count; k++)
                for (int i = 0; i < countType; i++)
                    for (int j = 0; j < countType; j++)
                    {
                        int str = k * temptS[0].Count + i;
                        dataGridView2[j, str].Value = temptS[k][i][j];
                    }
        }

        private void GetTime()
        {
            for (int i = 0; i < temptT.Count; i++)
            {
                for (int j = 0; j < countType; j++)
                {
                    temptT[i][j] = Convert.ToInt32(dataGridView1[j, i].Value);
                }
            }
            for (int k = 0; k < temptS.Count; k++)
                for (int i = 0; i < countType; i++)
                    for (int j = 0; j < countType; j++)
                    {
                        int str = k * temptS[0].Count + i;
                        temptS[k][i][j] = Convert.ToInt32(dataGridView2[j, str].Value);
                    }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            countType = (int)numericUpDown1.Value;
            countBatches = Convert.ToInt32(countBatchesTB.Text);
            List<int> listCountButches = new List<int>();
            for (int ii = 0; ii < countType; ii++)
            {
                listCountButches.Add(countBatches);
            }

            l = Convert.ToInt32(LTB.Text);
            maxS = Convert.ToInt32(timeSwitchingTB.Text);
            maxT = Convert.ToInt32(timeTreatmentingTB.Text);
            GetTime();            
            Shedule.L = l;
            Shedule.maxTimeSwitching = maxS;
            Shedule.maxTimeTreatment = maxT;
            Shedule.TSwitching = temptS;
            Shedule.TTreatment = temptT;
            FirstLevel firstLevel = new FirstLevel(countType, listCountButches);
            firstLevel.GenetateSolutionForAllTypes();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var gaa = new GAA();
            gaa.SetXrom(1);
            var S = gaa.ToArray();
            var shedule = new Shedule(S);
            shedule.ConstructShedule();
            var s = shedule.GetTime();

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            countType = (int)numericUpDown1.Value;
            l = Convert.ToInt32(LTB.Text);
            maxS = Convert.ToInt32(timeSwitchingTB.Text);
            maxT = Convert.ToInt32(timeTreatmentingTB.Text);
            temptS = new List<List<List<int>>>();
            temptT = new List<List<int>>();
            RandomTime();
            PrintTime();
        }

        private void LTB_TextChanged(object sender, EventArgs e)
        {
            countType = (int)numericUpDown1.Value;
            l = Convert.ToInt32(LTB.Text);
            maxS = Convert.ToInt32(timeSwitchingTB.Text);
            maxT = Convert.ToInt32(timeTreatmentingTB.Text);
            temptS = new List<List<List<int>>>();
            temptT = new List<List<int>>();
            RandomTime();
            PrintTime();
        }
    }
}
