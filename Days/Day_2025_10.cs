using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_10 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    struct Machine
    {
        public int size;
        public List<int> goalState;
        public List<List<int>> buttons;
        public List<int> outage;
    }
    protected override string part_1()
    {
        List<Machine> machines = new List<Machine>();
        foreach (string instruction in _input.Split('\n'))
        {
            List<string> inps = instruction.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            int machineSize = inps[0].Length - 2;
            List<int> goal = inps[0].Trim(new char[] { '[', ']' }).ToCharArray().Select((c, i) => c == '#' ? i : -1).ToList();
            goal.RemoveAll(n => n < 0);
            List<int> jolt = inps.Last().Substring(1, inps.Last().Length - 2).Split(',').Select(c =>  int.Parse(c)).ToList();

            inps.RemoveAt(0);
            inps.RemoveAt(inps.Count - 1);
            List<List<int>> buttons = new List<string>(inps).Select(x => x.Substring(1, x.Length -2).Split(',').Select(n => int.Parse(n)).ToList()).ToList() ;
            // sort to opti ?

            machines.Add(new Machine() { goalState = goal, buttons = buttons, outage = jolt, size = machineSize });
        }

        // Idea there is no point to do a button twice, so we'll do a recursive func, trying to switch a button and see if we can reach the goal with the rest
        double result = 0;
        foreach (Machine mac in machines)
        {
            result += recMatchState(mac.goalState, new List<int>(), mac.buttons, mac.size);

            if (result > 10)
                break;
        }


        return result.ToString();
    }

    double recMatchState(List<int> goal, List<int> currentState, List<List<int>> remainingButtons, int size)
    {
        if (goal.Count == currentState.Count)
        {
            // test equality
            if (goal.All(n => currentState.Contains(n)))
                return 0;
        }

        if (remainingButtons.Count == 0)
            return 1000;

        // stop case, a wrong state spot is no longer in remainingButtons
        List<int> remains = remainingButtons.SelectMany(x => x).ToList();
        for (int i = 0; i < size; i++)
        {
            if (!remains.Contains(i))
            {
                if (goal.Contains(i)  != currentState.Contains(i))
                    return 1000;
            }
        }

        // test if there is the perfect button to finish
        List<int> toPush = new List<int>(goal);
        foreach (int n in currentState)
        {
            if (toPush.Contains(n))
                toPush.Remove(n);
            else
                toPush.Add(n);
        }
        foreach (List<int>  btns in remainingButtons)
        {
            if (btns.Count != toPush.Count)
                continue;

            if (toPush.All(n => btns.Contains(n)))
                return 1;
        }



        double res = 100000;
        //for (int i = 0; i < remainingButtons.Count; i++) 
        //{
            List<int> button = remainingButtons[0];
            
            List<int> curState = new List<int>(currentState);
            foreach (int b in button)
            {
                if (curState.Contains(b))
                    curState.Remove(b);
                else
                    curState.Add(b);
            }
            curState.Sort();

            List<List<int>> tailButtons = new List<List<int>>(remainingButtons);
            tailButtons.RemoveAt(0);
            
        // Test with the first button switch and without the button switch, with all next buttons
            res = Math.Min(recMatchState(goal, curState, tailButtons, size), recMatchState(goal, currentState, tailButtons, size)) + 1;
        //}

        return res;
    }

    protected override string part_2()
    {
        foreach (string instruction in _input.Split('\n'))
        {

        }
        
        return base.part_2();
    }
}
