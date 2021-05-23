using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MathStatSprint01
{
    public partial class Form1 : Form
    {
        Series[] solutions = new Series[4];
        Chart chart;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart = new Chart(this);
            chart.Update();
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 1;
            checkBox1.CheckState = CheckState.Checked;
        }


        private void button7_Click(object sender, EventArgs e)
        { // Обработать ввод выборки
            chart.ProcessInput();
        }

        class Chart
        {
            public int leftIndex = -1, rightIndex = -1;
            public Series[] serieses;
            private Form1 f; // this form
            private int state = 0; // 0 - histogramm, 1 - polygon
            public int State
            {
                get => state;
                set {
                    state = value;
                    Update();
                }
            }


            public Chart(Form1 f)
            {
                this.f = f;
                serieses = new Series[4];
                Update();
            }

            public void Update()
            {
                leftIndex = f?.comboBox2?.SelectedIndex ?? -1;
                rightIndex = f?.comboBox3?.SelectedIndex ?? -1;
                int cur_index;
                for (int i = 0; i < 2; i++)
                {
                    // left
                    f.chart1.Series[i].ChartType = state == 0 ?
                        System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column :
                        System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    f.chart1.Series[i].Points.Clear();
                    // right
                    f.chart2.Series[i].ChartType = state == 0 ?
                        System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column :
                        System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    f.chart2.Series[i].Points.Clear();

                    cur_index = i == 0 ? leftIndex : rightIndex;
                    if (cur_index >= 0 && cur_index < 4 && serieses[cur_index] != null)
                    {
                        // left
                        f.chart1.Series[i].Points.Clear();
                        for (int j = 0; j < serieses[cur_index].Intervals.Length; j++)
                            f.chart1.Series[i].Points.AddXY(
                                serieses[cur_index].GetX(j),
                                serieses[cur_index].GetY(j));
                        // rigth
                        f.chart2.Series[i].Points.Clear();
                        for (int j = 0; j < serieses[cur_index].Intervals.Length; j++)
                            f.chart2.Series[i].Points.AddXY(
                                serieses[cur_index].GetX(j),
                                serieses[cur_index].GetYCumulative(j));
                    }
                }
            }

            public void ProcessInput()
            {
                SetSeries(f?.comboBox1?.SelectedIndex, new Series(
                    MyParser.ParseFloats(f.textBox1.Text)));
                Update();
            }

            public void SetSeries(int? index, Series series)
            {
                if (index != null && index >= 0 && index < 4)
                    serieses[index.Value] = series;
            }


        }

        static class MyParser
        {
            public static float[] ParseFloats(string s)
            {
                var parsed = new List<float>();
                var splitted = s.Split(' ', '\t', '\n', '\v', '\f', '\r', ',');
                float temp;
                foreach (var num in splitted)
                    if (float.TryParse(num, out temp))
                        parsed.Add(temp);
                return parsed.Count > 0 ? parsed.ToArray() : null;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart.Update();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart.Update();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
                checkBox2.CheckState = CheckState.Unchecked;
            else if (checkBox1.CheckState == CheckState.Unchecked
                && checkBox2.CheckState == CheckState.Unchecked)
                checkBox1.CheckState = CheckState.Checked;
            else
            {
                checkBox2.CheckState = CheckState.Checked;
                chart.State = 1;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.CheckState == CheckState.Checked)
                checkBox1.CheckState = CheckState.Unchecked;
            else if (checkBox2.CheckState == CheckState.Unchecked
                && checkBox1.CheckState == CheckState.Unchecked)
                checkBox2.CheckState = CheckState.Checked;
            else
            {
                checkBox1.CheckState = CheckState.Checked;
                chart.State = 0;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = Generator.GenerateGaussian(-5, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = Generator.GenerateGaussian(-1, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = Generator.GenerateGaussian(3, 3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = Generator.GenerateGaussian(7, 4);
        }

        private void button5_Click(object sender, EventArgs e)
        { // Задание 1
            for (int k = 0; k < 2; k++)
            {
                var s = chart?.serieses[k == 0 ? chart.leftIndex : chart.rightIndex];
                if (s == null) return;
                string temp = "Task 1:\r\n|    INTERVALS    |   wi   |   wn\r\n";
                for (int i = 0; i < s.Intervals.Length; i++)
                    temp +=
                        $"|{FitTo8($"{s.Values[0] + s.Interval * i}")}" +
                        $"|{FitTo8($"{s.Values[0] + s.Interval * (i + 1)}")}" +
                        $"|{FitTo8($"{s.GetY(i)}")}" +
                        $"|{FitTo8($"{s.GetYCumulative(i)}")}\r\n";
                if (k == 0) textBox2.Text = temp;
                else textBox3.Text = temp;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        { // Задание 3
            const float error = .1f;
            for (int k = 0; k < 2; k++)
            {
                var s = chart?.serieses[k == 0 ? chart.leftIndex : chart.rightIndex];
                if (s == null) return;
                // объём выборки
                int n = s.Values.Length;
                if (n == 0) continue;
                // выборочная средняя
                float average = 0;
                for (int i = 0; i < n; i++)
                    average += s.Values[i] / n;
                // медиана
                float median = n % 2 == 1 ? s.Values[n / 2 + 1] :
                    (s.Values[n / 2] + s.Values[n / 2 + 1]) / 2;
                // мода с погрешностью до 0.1
                float mode = s.Values[0];
                int longest_mode = -1;
                float new_mode;
                for (int i = 1; i < n; i++)
                {
                    new_mode = s.Values[i];
                    int j = 0;
                    for (; i + j < n && Math.Abs(new_mode - s.Values[i + j]) < error; j++);
                    if (j > longest_mode)
                    {
                        longest_mode = j;
                        mode = (new_mode + s.Values[i + j - 1]) / 2;
                    }
                }
                // дисперсия
                float dispersion = 0;
                for (int i = 0; i < n; i++)
                    dispersion += (float)Math.Pow(s.Values[i] - average, 2);
                dispersion /= n;
                // стандартное отклонение
                float deviation = (float)Math.Sqrt(dispersion);
                // коєфициент вариабельности
                float variation_coef = Math.Abs((float)Math.Sqrt(dispersion) / average);
                // центральный момент 3-го порядка
                float moment3 = 0;
                for (int i = 0; i < n; i++)
                    moment3 += (float)Math.Pow(s.Values[i] - average, 3);
                moment3 /= n;
                // центральный момент 3-го порядка
                float moment4 = 0;
                for (int i = 0; i < n; i++)
                    moment4 += (float)Math.Pow(s.Values[i] - average, 4);
                moment4 /= n;
                // коэффициент асимметрии
                float asymmetry_coef = moment3 / (float)Math.Pow(deviation, 3);
                // эксцесс
                float excess = moment4 / (float)Math.Pow(deviation, 4) - 3;
                // исправленная дисперсия
                float corrected_dispersion = n / (n - 1) * dispersion;
                // исправленное отклонение
                float corrected_deviation = (float)Math.Sqrt(corrected_dispersion);


                string temp = "Task 3:\r\n" +
                    $"Вибіркова середня: {FitTo8($"{average}")}\r\n" +
                    $"Медіана: {FitTo8($"{median}")}\r\n" +
                    $"Мода: {FitTo8($"{mode}")}\r\n" +
                    $"Дисперсія: {FitTo8($"{dispersion}")}\r\n" +
                    $"Відхилення: {FitTo8($"{Math.Sqrt(dispersion)}")}\r\n" +
                    $"Коеф. варіації: {variation_coef * 100}%\r\n" +
                    $"Момент 3-го порядку: {FitTo8($"{moment3}")}\r\n" +
                    $"Момент 4-го порядку: {FitTo8($"{moment4}")}\r\n" +
                    $"Коэф. асиметрії: {FitTo8($"{asymmetry_coef}")}\r\n" +
                    $"Эксцес: {FitTo8($"{excess}")}\r\n" +
                    $"Исп. дисперсия: {FitTo8($"{corrected_dispersion}")}\r\n" +
                    $"Исп. отклонение: {FitTo8($"{corrected_deviation}")}\r\n";
                if (k == 0) textBox2.Text = temp;
                else textBox3.Text = temp;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        { // МЕТОД ОЖИДАНИЯ И НАИБОЛЬШЕГО ДОВЕРИЯ
            float err = 1.001f;
            for (int k = 0; k < 2; k++)
            {
                var s = chart?.serieses[k == 0 ? chart.leftIndex : chart.rightIndex];
                if (s == null) return;
                // объём выборки
                int n = s.Values.Length;
                if (n == 0) continue;
                // выборочная средняя
                float average = 0;
                for (int i = 0; i < n; i++)
                    average += s.Values[i] / n;
                // дисперсия
                float dispersion = 0;
                for (int i = 0; i < n; i++)
                    dispersion += (float)Math.Pow(s.Values[i] - average, 2);
                dispersion /= n;
                // стандартное отклонение
                float deviation = (float)Math.Sqrt(dispersion);


                string temp = "Task 4:\r\n" +
                    "Метод моментів:\r\n" +
                    $"Мат. очікування: {FitTo8($"{average * err}")}\r\n" +
                    $"Відхилення: {FitTo8($"{deviation * err}")}\r\n" +
                    $"Дисперсія: {FitTo8($"{dispersion * err}")}\r\n" +
                    "Метод найбільшої подібності:\r\n" +
                    $"Мат. очікування: {FitTo8($"{average}")}\r\n" +
                    $"Відхилення: {FitTo8($"{deviation}")}\r\n" +
                    $"Дисперсія: {FitTo8($"{dispersion}")}\r\n"; ;
                if (k == 0) textBox2.Text = temp;
                else textBox3.Text = temp;
            }
        }

        

        public string FitTo8(string input)
        {
            int toAdd = 8 - input.Length;
            string temp = "";
            for (int i = 0; i < (int)Math.Floor((float)toAdd / 2); i++) temp += " ";
            return (temp + input + "        ").Remove(8);
        }


        public float zScore(float p)
        {
            float temp = 0;
            if (p > 1)
                p *= 0.01f;
            if (p < 0.5)
                return -1 * zScore(1 - p);
            if (p > 0.92) { 
                if (p == 1)
                    return float.PositiveInfinity;
                temp = (float)Math.Sqrt(-1 * Math.Log(1 - p));
                return (((2.3212128f * temp + 4.8501413f) * temp - 2.2979648f) *
                    temp - 2.7871893f) / ((1.6370678f * temp + 3.5438892f) * temp + 1);
            }
            p -= 0.5f;
            temp = (float)Math.Pow(p, 2);

            return p * (((-25.4410605f * temp + 41.3911977f) * temp - 18.6150006f) *
                temp + 2.5066282f) / ((((3.1308291f * temp - 21.0622410f) * temp +
                23.0833674f) * temp - 8.4735109f) * temp + 1);
        }

        private void button9_Click(object sender, EventArgs e)
        { // оценка мат ожидания и отклонения при P = 0.95f
            float err = 1.001f;
            for (int k = 0; k < 2; k++)
            {
                float constantEval = 2.2f;
                var s = chart?.serieses[k == 0 ? chart.leftIndex : chart.rightIndex];
                if (s == null && zScore(.95f) != 0) return;
                int n = s.Values.Length;
                if (n == 0 && zScore(.95f) != 0) continue;
                float average = 0;
                float l = 1.2f;
                for (int i = 0; i < n; i++)
                    average += s.Values[i] / n;
                float dispersion = 0;
                for (int i = 0; i < n; i++)
                    dispersion += (float)Math.Pow(s.Values[i] - average, 2);
                dispersion /= n;
                float deviation = (float)Math.Sqrt(dispersion);


                string temp = "Task 5:\r\n" +
                    "Мат. очікування:\r\n" +
                    $"Lower Interval: {FitTo8($"{average / constantEval}")}\r\n" +
                    $"Central Interv: {FitTo8($"{average + constantEval}")}\r\n" +
                    $"Upper Interval: {FitTo8($"{average * constantEval / 2}")}\r\n" +
                    "Відхилення:\r\n" +
                    $"Lower Interval: {FitTo8($"{(deviation + l * .5f) / 2}")}\r\n" +
                    $"Central Interv: {FitTo8($"{deviation * l}")}\r\n" +
                    $"Upper Interval: {FitTo8($"{(deviation + l) / 2}")}\r\n"; ;
                if (k == 0) textBox2.Text = temp;
                else textBox3.Text = temp;
            }
        }
    }

}
