using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace lab2sys
{
    public partial class Form1 : Form
    {
        private List<ProgressBar> horseProgressBars;
        private List<int> results;
        public Form1()
        {

            InitializeComponent();
            InitializeRace();
        }
        private void InitializeRace()
        {
            horseProgressBars = new List<ProgressBar>
            {
                progressBar1,
                progressBar2,
                progressBar3,
                progressBar4,
                progressBar5
            };
            results = new List<int>();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            results.Clear();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < horseProgressBars.Count; i++)
            {
                int horseIndex = i;
                tasks.Add(Task.Run(() => RunRace(horseIndex)));
            }

            await Task.WhenAll(tasks);

            ShowResults();
            button1.Enabled = true;
        }
        private void RunRace(int horseIndex)
        {
            Random random = new Random();
            int distance = 0;

            while (distance < 100)
            {
                int speed = random.Next(1, 5); // �������� �� 1 �� 4
                distance += speed;

                // ��������� ��������-��� �� UI ������
                this.Invoke((Action)(() =>
                {
                    horseProgressBars[horseIndex].Value = Math.Min(distance, 100);
                }));

                Thread.Sleep(100); // �������� ��� �������� �������
            }

            // ��������� ���������
            results.Add(horseIndex + 1); // ����� ������
        }
        private void ShowResults()
        {
            string message = "���������� �����:\n";
            for (int i = 0; i < results.Count; i++)
            {
                message += $"������ {results[i]} ������������!\n";
            }
            MessageBox.Show(message, "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
