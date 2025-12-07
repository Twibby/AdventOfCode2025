using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_06 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    protected override string part_1()
    {
        List<List<double>> inputs = new List<List<double>>();
        foreach (string instruction in _input.Split('\n'))
        {
            if (instruction[0] is '+' or '*')
                break;

            inputs.Add(instruction.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Select(x => double.Parse(x)).ToList());
        }

        List<string> op = _input.Split('\n').ToList().Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
         
        double result = 0; 
        for (int i  = 0; i < op.Count; i++)
        {
            bool isAdd = op[i] == "+";
            double tmpRes = isAdd ? 0 : 1;
            foreach (List<double> numbers in inputs)
            {
                if (isAdd)
                    tmpRes += numbers[i];
                else
                    tmpRes *= numbers[i];
            }

            result += tmpRes;
        }




        return result.ToString();
    }

    protected override string part_2()
    {
        List<string> instructions = _input.Split('\n').ToList();
        string ops = instructions.Last();
        instructions.Remove(ops);

        double result = 0;
        bool isAdd = true;
        List<double> values = new List<double>();

        for (int i = 0; i < ops.Length; i++)
        {
            if (instructions.All(s => s[i] == ' ')) // end of problem
            {
                if (values.Count > 0)
                {
                    double tmp = isAdd ? 0 : 1;
                    foreach (double val in values) { tmp = isAdd ? tmp + val : tmp * val; }
                    result += tmp;
                }

                values = new List<double>();
            }
            else
            {
                double val = double.Parse(System.String.Join("", instructions.Select(s => s[i]).Where(c => c != ' ')));
                values.Add(val);

                if (ops[i] != ' ')
                    isAdd = ops[i] == '+';
            }
        }

        if (values.Count > 0)
        {
            double tmp = isAdd ? 0 : 1;
            foreach (double val in values) { tmp = isAdd ? tmp + val : tmp * val; }
            result += tmp;
        }

        return result.ToString();
    }
}
