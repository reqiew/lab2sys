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
                int speed = random.Next(1, 5); // Скорость от 1 до 4
                distance += speed;

                // Обновляем прогресс-бар на UI потоке
                this.Invoke((Action)(() =>
                {
                    horseProgressBars[horseIndex].Value = Math.Min(distance, 100);
                }));

                Thread.Sleep(100); // Задержка для эмуляции времени
            }

            // Сохраняем результат
            results.Add(horseIndex + 1); // Номер лошади
        }
        private void ShowResults()
        {
            string message = "Результаты гонки:\n";
            for (int i = 0; i < results.Count; i++)
            {
                message += $"Лошадь {results[i]} финишировала!\n";
            }
            MessageBox.Show(message, "Результаты", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
