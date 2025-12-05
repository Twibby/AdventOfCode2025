using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_05 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    protected override string part_1()
    {
        string[] inputs = _input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        List<(double,double)> ranges = new List<(double,double)> ();
        foreach (string instruction in inputs[0].Split('\n'))
        {
            string[] bounds = instruction.Split('-');
            ranges.Add((double.Parse(bounds[0]), double.Parse(bounds[1])));
        }

        ranges.Sort(delegate ((double, double) a, (double, double) b)
        {
            return a.Item2.CompareTo(b.Item2);
        });


        double result = 0;
        foreach (string item in inputs[1].Split('\n'))
        {
            double itemId = double.Parse(item);

            foreach ((double,double) range in ranges)
            {
                if (itemId > range.Item2)
                    continue;

                if (itemId > range.Item1)
                {
                    result++;
                    break;
                }
            }
        }

        return result.ToString();
    }

    protected override string part_2()
    {
        string[] inputs = _input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        List<(double, double)> ranges = new List<(double, double)>();
        foreach (string instruction in inputs[0].Split('\n'))
        {
            string[] bounds = instruction.Split('-');
            ranges.Add((double.Parse(bounds[0]), double.Parse(bounds[1])));
        }

        ranges.Sort(delegate ((double, double) a, (double, double) b)
        {
            return a.Item1.CompareTo(b.Item1);
        });

        // ranges are sorted by mindBound
        // so at each range, we know we just have to compare with maxValue previously encountered and not count this interval
        double result = 0;
        double maxVal = -1;
        foreach ((double,double) range in ranges)
        {
            double toAdd = (1 + range.Item2 - range.Item1);
            if (maxVal >= range.Item1)
                toAdd -= 1 + maxVal - range.Item1;

            result += Math.Max(0, toAdd);
            maxVal = Math.Max(maxVal, range.Item2);
        }

        return result.ToString();
    }
}
