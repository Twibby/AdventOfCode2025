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
        foreach (string instruction in _input.Split(','))
        {
            string[] bounds = instruction.Split('-');
            Debug.Log(bounds[0].Length + " - " +  bounds[1].Length);

            // assume bounds have 1digit difference max
            if (bounds[0].Length % 2 == 0)
            {
                Debug.Log("Deal with " + bounds[0]);
                string pref = bounds[0].Substring(0, bounds[0].Length / 2);
                Debug.Log("pref " + pref);
            }

            if (bounds[1].Length % 2 == 0)
            {
                Debug.Log("Deal with " + bounds[1]);
            }

            for (int i = bounds[0].Length; i <= bounds[1].Length; i++)
            {
                if (i % 2 == 1)
                    continue;


            }
        }

        return base.part_1();
    }

    protected override string part_2()
    {
        foreach (string instruction in _input.Split('\n'))
        {

        }
        
        return base.part_2();
    }
}
