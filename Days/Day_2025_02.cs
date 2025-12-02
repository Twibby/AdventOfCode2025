using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_02 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    protected override string part_1()
    {
        double result = 0;
        foreach (string instruction in _input.Split(','))
        {
            string[] bounds = instruction.Split('-');
            Debug.Log("Instruction : " + instruction + "\t | " + bounds[0].Length + " - " +  bounds[1].Length);

            // Compute "real" range
            double minBound, maxBound;
            if (bounds[0].Length % 2 == 0)
                minBound = double.Parse(bounds[0].Substring(0, bounds[0].Length / 2));
            else
                minBound = System.Math.Pow(10, bounds[0].Length/2);

            if (bounds[1].Length % 2 == 0)
                maxBound = double.Parse(bounds[1].Substring(0, bounds[1].Length / 2));
            else
                maxBound = System.Math.Pow(10, bounds[1].Length / 2) -1;

            Debug.Log("Range to check: " + minBound+ "|" +minBound + " - " + maxBound + "|" + maxBound);
            if (minBound > maxBound)
            {
                Debug.LogWarning("No value in range");
                continue;
            }

            double min = double.Parse(bounds[0]);
            double max = double.Parse(bounds[1]);

            double curVal;
            for (double i = minBound; i <= maxBound; i++)
            {
                string valStr = i.ToString() + i.ToString();
                curVal = double.Parse(valStr);
                if (curVal >= min && curVal <= max)
                {
                    result += curVal;
                    Debug.Log("Counted " + curVal);
                }

                if (curVal > max)
                    break;
            }
        }

        return result.ToString();
    }

    protected override string part_2()
    {
        double result = 0;
        foreach (string instruction in _input.Split(','))
        {
            string[] bounds = instruction.Split('-');
            Debug.LogWarning("Instruction : " + instruction + "\t | " + bounds[0].Length + " - " + bounds[1].Length);

            double min = double.Parse(bounds[0]);
            double max = double.Parse(bounds[1]);

            for (double val = min; val <= max; val++)
            {
                string valStr = val.ToString();
                for (int k = 1; k <= val.ToString().Length /2; k++) //look for possible pattern
                {
                    if (valStr.Length % k != 0) // if not multiple, can't be a pattern
                        continue;

                    string tmp = string.Concat(Enumerable.Repeat(valStr.Substring(0, k), valStr.Length / k));
                    if (tmp.CompareTo(valStr) == 0)
                    {
                        result += val;
                        Debug.Log("Counted " + val);
                        break; 
                    }
                }
            }

        }

        return result.ToString();
    }
}
