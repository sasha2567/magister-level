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
            int countBatches = Convert.ToInt32(countBatchesTB.Text);
            List<int> listCountButches = new List<int>();
            for (int ii = 0; ii < countType; ii++)
            {
                listCountButches.Add(countBatches);
            }
            int l, maxS, maxT;
            l = Convert.ToInt32(LTB.Text);
            maxS = Convert.ToInt32(timeSwitchingTB.Text);
            maxT = Convert.ToInt32(timeTreatmentingTB.Text);
            List<List<List<int>>> temptS = new List<List<List<int>>>();
            List<List<int>> temptT = new List<List<int>>();
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
            gaa.SetXrom();
            var shedule = new Shedule(gaa.ToArray());
            shedule.ConstructShedule();
            var s=shedule.GetTime();

        }
    }
}
