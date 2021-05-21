using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathStatSprint01
{
    public class Series
    {
        private float[] series; // to initialize
        private List<float>[] intervals;

        public List<float>[] Intervals
        {
            get {
                Update();
                return intervals;
            }
        }

        public float[] Values
        {
            get => series;
            set {
                series = value;
                if (series != null) Array.Sort(series);
            }
        }

        public Series(float[] series)
        {
            Values = series;
            if (series?.Length != null && series.Length > 0) Update();
        }

        public void Update()
        {
            UpdateIntervals();
        }

        private void UpdateIntervals()
        {
            intervals = new List<float>[SturgessSize];
            for (int j = 0; j < SturgessSize; j++)
                intervals[j] = new List<float>();
            for (int i = 0, j = 0; i < ((Values?.Length) ?? 0); i++)
            {
                if (Values[i] >= Values[0] + Interval * (j + 1)
                    && j + 1 < intervals.Length)
                { j++; i--; continue; }
                intervals[j].Add(Values[i]);
            }
        }

        private int SturgessSize
        {
            get {
                if (series?.Length != null)
                    return (int)Math.Floor(1 + 3.322 * Math.Log10(series.Length));
                return 0;
            }
        }

        public float Interval => 
            (Values[series.Length - 1] - Values[0]) / SturgessSize;

        public float GetX(int index)
        {
            return series[0] + Interval * (.5f + index);
        }

        public float GetY(int index)
        {
            return (float)intervals[index].Count / Values.Length;
        }

        public float GetYCumulative(int index)
        {
            if (index == -1) return 0;
            return GetY(index) + GetYCumulative(index - 1);
        }

    }
}

/**
   7.5 6.1 7 6 7.4 6.8 6.3 7.5 7 7.5 7.6 
   10.6 6 8.2 7.1 9.6 8.5 9.2 8 8 8.7 
   9.8 8.3 8.5 9.5 6.3 5.8 7.2 7.5 6.5
 */