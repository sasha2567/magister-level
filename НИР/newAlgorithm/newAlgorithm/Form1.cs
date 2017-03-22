using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newAlgorithm
{
    public partial class Form1 : Form
    {
        int _selectionType = 2;
        int _l, _maxS, _maxT;
        int _countType, _countBatches;
        List<List<List<int>>> _temptS = new List<List<List<int>>>();
        List<List<int>> _temptT = new List<List<int>>();

        public Form1()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            _countType = (int)numericUpDown1.Value;
            _countBatches = Convert.ToInt32(countBatchesTB.Text);
            var listCountButches = new List<int>();
            for (var ii = 0; ii < _countType; ii++)
            {
                listCountButches.Add(_countBatches);
            }

            _l = Convert.ToInt32(LTB.Text);
            _maxS = Convert.ToInt32(timeSwitchingTB.Text);
            _maxT = Convert.ToInt32(timeTreatmentingTB.Text);
            _temptS = new List<List<List<int>>>();
            _temptT = new List<List<int>>();
            RandomTime();
            PrintTime();
        }

        private List<int> Copy(IEnumerable<int> _in)
        {
            return _in.ToList();
        }

        private List<List<int>> СоставыПартий(IReadOnlyList<List<int>> _in)
        {
            var a = new List<List<int>>();
            var count = 0;
            a.Add(new List<int>());
            a[count] = Copy(_in[0]);
            count++;
            foreach (var t in _in)
            {
                for (var j = 1; j < t.Count; j++)
                {
                    do
                    {
                        a.Add(new List<int>());
                        a[count] = Copy(a[count - 1]);
                        a[count][0]--;
                        a[count][j]++;
                        count++;
                    }
                    while (a[count - 1][0] > a[count - 1][j]);
                }
            }
            return a;
        }

        private void RandomTime()
        {
            var count = 0;
            for (var i = 0; i < _l; i++)
            {
                _temptT.Add(new List<int>());
                _temptS.Add(new List<List<int>>());
                for (var j = 0; j < _countType; j++)
                {
                    var temp = (count % 2 == 0) ? 2 : _maxT;
                    _temptT[i].Add(temp);
                    _temptS[i].Add(new List<int>());
                    for (var k = 0; k < _countType; k++)
                    {
                        temp = (count % 2 == 0) ? 2 : _maxS;
                        _temptS[i][j].Add(temp);
                    }
                    count++;
                }
            }
        }

        private void PrintTime()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.RowCount = _temptT.Count;
            dataGridView1.ColumnCount = _temptT[0].Count;
            for (var i = 0; i < _temptT.Count; i++)
            {
                for (var j = 0; j < _countType; j++)
                {
                    dataGridView1[j, i].Value = _temptT[i][j];
                }
            }
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.RowCount = _temptS.Count * _temptS[0].Count;
            dataGridView2.ColumnCount = _temptS[0].Count;
            for (var k = 0; k < _temptS.Count; k++)
                for (var i = 0; i < _countType; i++)
                    for (var j = 0; j < _countType; j++)
                    {
                        var str = k * _temptS[0].Count + i;
                        dataGridView2[j, str].Value = _temptS[k][i][j];
                    }
        }

        private void GetTime()
        {
            for (var i = 0; i < _temptT.Count; i++)
            {
                for (var j = 0; j < _countType; j++)
                {
                    _temptT[i][j] = Convert.ToInt32(dataGridView1[j, i].Value);
                }
            }
            for (var k = 0; k < _temptS.Count; k++)
                for (var i = 0; i < _countType; i++)
                    for (var j = 0; j < _countType; j++)
                    {
                        var str = k * _temptS[0].Count + i;
                        _temptS[k][i][j] = Convert.ToInt32(dataGridView2[j, str].Value);
                    }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _countType = (int)numericUpDown1.Value;
            _countBatches = Convert.ToInt32(countBatchesTB.Text);
            var listCountButches = new List<int>();
            for (var ii = 0; ii < _countType; ii++)
            {
                listCountButches.Add(_countBatches);
            }

            _l = Convert.ToInt32(LTB.Text);
            _maxS = Convert.ToInt32(timeSwitchingTB.Text);
            _maxT = Convert.ToInt32(timeTreatmentingTB.Text);
            GetTime();            
            Shedule.L = _l;
            Shedule.Switching = _temptS;
            Shedule.Treatment = _temptT;
            var firstLevel = new FirstLevel(_countType, listCountButches, checkBox1.Checked);
            firstLevel.GenetateSolutionForAllTypes("outputFirstAlgorithm.txt");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Shedule.L = _l;
            Shedule.Switching = _temptS;
            Shedule.Treatment = _temptT;
            _countType = (int)numericUpDown1.Value;
            _countBatches = Convert.ToInt32(countBatchesTB.Text);
            var listCountButches = new List<int>();
            for (var ii = 0; ii < _countType; ii++)
            {
                listCountButches.Add(_countBatches);
            }
            var gaa = new GAA(_countType, listCountButches, checkBox1.Checked);
            gaa.SetXrom((int)numericUpDown2.Value);
            gaa.calcFitnessList();
            int s;
           var result= gaa.getSelectionPopulation(_selectionType,out s);

            using (var file = new StreamWriter("outputGAA.txt",true))
            {
                int i = 0;
                foreach (var elem in gaa.nabor)
                {
                   
                    foreach (var elem2 in elem.GenList)
                    {
                        foreach (var elem3 in elem2)
                        {
                            file.Write(elem3+" ");
                        }
                        file.WriteLine();
                    }
                    file.WriteLine("_________________________");
                    file.WriteLine(gaa._fitnesslist[i]);
                    file.WriteLine("_________________________");
                    i++;
                }

                file.WriteLine("***************************");
                file.WriteLine(result);
                file.WriteLine("***************************");

            }
            MessageBox.Show("Данные успешно записаны", "Учпешное завершение", MessageBoxButtons.OK);
        }
            
        private void Change()
        {
            try
            {
                _countType = (int)numericUpDown1.Value;
                _l = Convert.ToInt32(LTB.Text);
                _maxS = Convert.ToInt32(timeSwitchingTB.Text);
                _maxT = Convert.ToInt32(timeTreatmentingTB.Text);
                _temptS = new List<List<List<int>>>();
                _temptT = new List<List<int>>();
                RandomTime();
                PrintTime();
            }
            catch 
            {
                return;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Change();
        }

        private void LTB_TextChanged(object sender, EventArgs e)
        {
            Change();
        }

        private void timeSwitchingTB_TextChanged(object sender, EventArgs e)
        {
            Change();
        }

        private void timeTreatmentingTB_TextChanged(object sender, EventArgs e)
        {
            Change();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _countType = (int)numericUpDown1.Value;
            _countBatches = Convert.ToInt32(countBatchesTB.Text);
            var listCountButches = new List<int>();
            for (var ii = 0; ii < _countType; ii++)
            {
                listCountButches.Add(_countBatches);
            }

            _l = Convert.ToInt32(LTB.Text);
            GetTime();
            Shedule.L = _l;
            Shedule.Switching = _temptS;
            Shedule.Treatment = _temptT;
            var firstLevel = new FirstLevel(_countType, listCountButches, checkBox1.Checked);
            firstLevel.GenetateSolutionForAllTypesSecondAlgorithm("outputSecondAlgorithm.txt");
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            _selectionType = 2;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _selectionType = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _selectionType = 1;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            _selectionType = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int[] ni = { 8, 12, 16, 24, 32 };
            int[] time = { 2, 4, 8, 16, 32 };
            int[] l = { 5, 10 };
            int[] n = { 5, 10 };
            const string file = "testFile";
            const string s = ".txt";
            string count = "";
            for (var i = 3; i < ni.Length; i++)
            {
                foreach (var t in n)
                {
                    foreach (var t1 in l)
                    {
                        for (var m = 0; m < time.Length; m++)
                        {
                            for (var p = 0; p < time.Length; p++)
                            {
                                _countType = t;
                                _l = t1;
                                _maxS = time[p];
                                _maxT = time[m];
                                _temptS = new List<List<List<int>>>();
                                _temptT = new List<List<int>>();
                                RandomTime();
                                PrintTime();
                                _countBatches = ni[i];
                                var listCountButches = new List<int>();
                                for (var ii = 0; ii < _countType; ii++)
                                {
                                    listCountButches.Add(_countBatches);
                                }

                                GetTime();
                                Shedule.L = _l;
                                Shedule.Switching = _temptS;
                                Shedule.Treatment = _temptT;
                                var firstLevel = new FirstLevel(_countType, listCountButches, false);
                                count = _countBatches + "_" + _countType + "_" + _l + "_" + _maxS + "_" + _maxT;
                                firstLevel.GenetateSolutionForAllTypesSecondAlgorithm(file + count + s);
                            }
                        }
                    }
                    var tt = 0;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
            for (int p = 2; p <= 32; p *= 2)
            {
                timeTreatmentingTB.Text = Convert.ToString(p);
                for (int i = 2; i <= 32; i *= 2)
                {
                    timeSwitchingTB.Text = Convert.ToString(i);

                    Shedule.L = _l;
                    Shedule.Switching = _temptS;
                    Shedule.Treatment = _temptT;
                    _countType = (int)numericUpDown1.Value;
                    _countBatches = Convert.ToInt32(countBatchesTB.Text);
                    var listCountButches = new List<int>();
                    for (var ii = 0; ii < _countType; ii++)
                    {
                        listCountButches.Add(_countBatches);
                    }
                    var gaa = new GAA(_countType, listCountButches, checkBox1.Checked);
                    gaa.SetXrom((int)numericUpDown2.Value);
                    gaa.calcFitnessList();
                    int s;
                    var result = gaa.getSelectionPopulation(_selectionType, out s);

                    using (var file = new StreamWriter("outputGAA.txt", true))
                    {
                        int k = 0;
                        foreach (var elem in gaa.nabor)
                        {

                            foreach (var elem2 in elem.GenList)
                            {
                                foreach (var elem3 in elem2)
                                {
                                    file.Write(elem3 + " ");
                                }
                                file.WriteLine();
                            }
                            file.WriteLine("_________________________");
                            file.WriteLine(gaa._fitnesslist[i]);
                            file.WriteLine("_________________________");
                            k++;
                        }

                        file.WriteLine("***************************");
                        file.WriteLine(result);
                        file.WriteLine("***************************");

                    }
                    using (var file = new StreamWriter("outputGAASimpleResult.txt", true))
                    {
                        file.WriteLine(result);
                    }
                }
                using (var file = new StreamWriter("outputGAASimpleResult.txt", true))
                {
                    file.WriteLine("_____________________");
                }
            }
            MessageBox.Show("Данные успешно записаны", "Учпешное завершение", MessageBoxButtons.OK);
        }


    }
}
