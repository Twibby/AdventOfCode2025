using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_03 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    protected override string part_1()
    {
        double result = 0;
        foreach (string instruction in _input.Split('\n'))
        {
            // idea : find largest digit in instruction but last char, then find biggest digit after first one
            // Comment amora-style: i'm drunk and dont find clever way to do it, stfu

            char max1 = instruction.Substring(0, instruction.Length - 1).Max();
            int index1 = instruction.IndexOf(max1);

            char max2 = instruction.Substring(index1 + 1).Max();

            int jolt = int.Parse((max1.ToString() + max2.ToString()).ToString());
            result += jolt;
            Debug.Log(jolt);
        }

        return result.ToString();
    }

    public int count = 2;

    protected override string part_2()
    {
        double result = 0;
        foreach (string instruction in _input.Split('\n'))
        {
            // idea: same concept as part1 but substring must exclude 12 last chars first, then 11 , ... maybe wake up and try to do something smart..

            int index = 0;
            double jolt = 0;
            for (int i = count - 1; i >= 0; i--)
            {
                char max = instruction.Substring(index, instruction.Length - i - index).Max();
                jolt += System.Math.Pow(10, i) * int.Parse(max.ToString());

                index = instruction.IndexOf(max, index) + 1;
            }

            result += jolt;
        }

        return result.ToString();
    }
}
