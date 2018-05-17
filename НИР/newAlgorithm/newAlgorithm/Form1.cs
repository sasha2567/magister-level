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

        public static List<List<int>> compositionSets;//майкрософт кодстайл говорит юзать верблюжий стиль https://msdn.microsoft.com/en-us/library/ms229043%28v=vs.100%29.aspx
        public static List<List<int>> timeSets;

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
                            file.Write(elem3 + " ");
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
            firstLevel.GenetateSolutionForAllTypesSecondAlgorithm();
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

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int[] N_komplect_type = { 2 };
            int[] N_komplect_for_type = { 2, 4 };
            int[] N_komplect_sostav = { 2, 4 };
            int[] n = { 5, 10 };
            int[] l = { 5, 10 };
            int[] time = { 2, 4, 8, 16, 32 };

            string file = "test/testFile_";
            using (var fileOut = new StreamWriter(file + "All.txt"))
            {
                foreach (var n_kom in N_komplect_type)
                {
                    compositionSets = new List<List<int>>();
                    timeSets = new List<List<int>>();
                    foreach(var n_kom_q in N_komplect_for_type)
                    {
                        for (int i = 0; i < n_kom; i++)
                        {
                            compositionSets.Add(new List<int>());
                            timeSets.Add(new List<int>());
                            for (int j = 0; j < n_kom_q; j++)
                            {
                                timeSets[i].Add(140);
                            }
                        }
                        foreach (var n_kom_s in N_komplect_sostav)
                        {
                            foreach (var t in n)
                            {
                                _countType = t;
                                for (int i = 0; i < n_kom; i++)
                                {
                                    for (var ind = 0; ind < _countType; ind++)
                                    {
                                        compositionSets[i].Add(n_kom_s);
                                    }
                                }

                                foreach (var _countLine in l)
                                {
                                    foreach (var t2 in time)
                                    {
                                        foreach (var t3 in time)
                                        {
                                            _temptS = new List<List<List<int>>>();
                                            _temptT = new List<List<int>>();
                                            _l = _countLine;
                                            _maxS = t3;
                                            _maxT = t2;
                                            RandomTime();
                                            PrintTime();
                                            GetTime();
                                            Shedule.L = _countLine;
                                            Shedule.Switching = _temptS;
                                            Shedule.Treatment = _temptT;
                                            var listCountButches = new List<int>();
                                            for (var ii = 0; ii < _countType; ii++)
                                            {
                                                listCountButches.Add(0);
                                            }
                                            for (var ii = 0; ii < _countType; ii++)
                                            {
                                                _countBatches = 0;
                                                for (int i = 0; i < n_kom; i++)
                                                {
                                                    _countBatches += compositionSets[i][ii] * n_kom_q;
                                                }
                                                listCountButches[ii] = _countBatches;
                                            }
                                            
                                            
                                            var firstLevel = new FirstLevel(_countType, listCountButches, checkBox1.Checked);
                                            var result = firstLevel.GenetateSolutionForAllTypesSecondAlgorithm();
                                            var first = Convert.ToInt32(result[0]);
                                            var top = Convert.ToInt32(result[1]);
                                            fileOut.WriteLine(first + "\t" + top);
                                        }
                                    }
                                    fileOut.WriteLine();
                                }
                            }

                        }
                    }
                }
                fileOut.Close();
            }
                        
            MessageBox.Show("Все сделяль, Насяника");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var massi = new[] { 8, 12, 16, 24, 32 };
            foreach (var intt in massi)
            {
                countBatchesTB.Text = intt.ToString();
                for (int z = 5; z <= 10; z += 5)
                {
                    numericUpDown1.Text = z.ToString();
                    for (int z1 = 5; z1 <= 10; z1 += 5)
                    {
                        LTB.Text = z1.ToString();
                        using (var file = new StreamWriter("outputGAASimpleResult.txt", true))
                        {
                            file.WriteLine("________________(CB = " + intt + ")");
                            file.WriteLine();
                            file.WriteLine("________________(L = " + LTB.Text + ")(N=" + numericUpDown1.Value + ")_____");
                        }
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
                                var countSourceKit = gaa.calcFitnessList();
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
                                        file.WriteLine("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=");
                                        file.WriteLine("Kit" + countSourceKit[i]);
                                        file.WriteLine("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=");
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
                    }
                }
            }
            MessageBox.Show("Данные успешно записаны", "Учпешное завершение", MessageBoxButtons.OK);
        }

        private void setsBtn_Click(object sender, EventArgs e)
        {
            var firstType = new List<int>();
            var secondType = new List<int>();
            var testTime = new List<int>();
            testTime.Add(15);
            testTime.Add(18);
            firstType.Add(3);
            firstType.Add(3);
            firstType.Add(3);
            secondType.Add(2);
            secondType.Add(3);
            secondType.Add(4);
            var testType = new List<List<int>>();
            testType.Add(firstType);
            testType.Add(secondType);
        }
    }
}
