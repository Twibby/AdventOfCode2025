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
        public List<Switch> switches;
        public List<int> joltage;
    }

    struct Switch
    {
        public List<int> buttons;

        public override string ToString()
        {
            return "(" + System.String.Join(',', buttons) +")";
        }
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
            List<Switch> switchs = new List<string>(inps).Select(x => new Switch() { buttons = x.Substring(1, x.Length - 2).Split(',').Select(n => int.Parse(n)).ToList() }).ToList() ;
            // sort to opti ?

            machines.Add(new Machine() { goalState = goal, switches = switchs, joltage = jolt, size = machineSize });
        }

        // Idea there is no point to do a button twice, so we'll do a recursive func, trying to switch a button and see if we can reach the goal with the rest
        double result = 0;
        foreach (Machine mac in machines)
        {
            Debug.LogWarning("Trying to reach state " + System.String.Join(',', mac.goalState));
            double res = recMatchState(mac.goalState, new List<int>(), mac.switches, mac.size, new List<Switch>());
            Debug.LogWarning("Used " + res.ToString() + " switches to reach goal");
            result += res;

            if (IsDebug && result > 20)
                break;
        }


        return result.ToString();
    }

    double recMatchState(List<int> goal, List<int> currentState, List<Switch> remainingSwitches, int size, List<Switch> used)
    {
        if (IsDebug)
            Debug.Log(new string('*', used.Count) + " [" + System.String.Join(' ', currentState) + "] with used : " + System.String.Join('+', used.Select(x => x.ToString())) + "\t | Remains " + remainingSwitches.Count);

        if (goal.Count == currentState.Count)
        {
            // test equality
            if (goal.All(n => currentState.Contains(n)))
                return 0;
        }

        if (remainingSwitches.Count == 0)
        {
            if (IsDebug)
                Debug.Log(new string('>', used.Count) + " No remaining switches, dead end");

            return 1000;
        }

        // stop case, a wrong state spot is no longer in remainingButtons
        List<int> remains = remainingSwitches.SelectMany(x => x.buttons).Distinct().ToList();
        for (int i = 0; i < size; i++)
        {
            if (!remains.Contains(i))
            {
                if (goal.Contains(i) != currentState.Contains(i))
                {
                    if (IsDebug)
                        Debug.Log(new string('>', used.Count) + " Missing button for position " + i + " to reach goal --> DEAD END | " + System.String.Join(",", remains));
                    return 1000;
                }
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
        foreach (Switch sw in remainingSwitches)
        {
            if (sw.buttons.Count != toPush.Count)
                continue;

            if (toPush.All(n => sw.buttons.Contains(n)))
            {
                if (IsDebug)
                    Debug.Log(new string('>', used.Count + 1) + " Found perfect switch " + sw.ToString() + " to finish  --> TOTAL OF " +(used.Count+1).ToString() + " steps");

                return 1;
            }
        }



        double res = 100000;
        //for (int i = 0; i < remainingButtons.Count; i++) 
        //{
            List<int> button = remainingSwitches[0].buttons;
            List<Switch> tailButtons = new List<Switch>(remainingSwitches);
            tailButtons.RemoveAt(0);

            List<int> withHeadState = new List<int>(currentState);
            foreach (int b in button)
            {
                if (withHeadState.Contains(b))
                    withHeadState.Remove(b);
                else
                    withHeadState.Add(b);
            }
            withHeadState.Sort();

            List<Switch> withHeadUsed = new List<Switch>(used);
            withHeadUsed.Add(remainingSwitches[0]);


        double resWithHead = recMatchState(goal, withHeadState, tailButtons, size, withHeadUsed);
        if (IsDebug)
            Debug.Log(new string('$', withHeadUsed.Count) + " Res WITHHH Head is currently " + resWithHead.ToString() + " " + System.String.Join('+', withHeadUsed.Select(x => x.ToString())));
        
        double resWithoutHead = 10000;
        if (resWithHead > 1)
        {
            resWithoutHead = recMatchState(goal, currentState, tailButtons, size, used);
            if (IsDebug)
                Debug.Log(new string('$', used.Count) + " Res WITHOUT Head is currently " + resWithoutHead.ToString() + " " + System.String.Join('+', used.Select(x => x.ToString())));
        }
        else if (IsDebug)
        {
            Debug.Log(new string('$', used.Count) + " Skipping Res WITHOUT Head since WITH Head was optimal");
        }

        // Test with the first button switch and without the button switch, with all next buttons
        res = Math.Min(1 + resWithHead, resWithoutHead);
        if (IsDebug)
            Debug.Log(new string('$', used.Count) + " Then Res is currently " + res.ToString());
        //}

        return res;
    }




    protected override string part_2()
    {
        List<Machine> machines = new List<Machine>();
        foreach (string instruction in _input.Split('\n'))
        {
            List<string> inps = instruction.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            int machineSize = inps[0].Length - 2;
            List<int> goal = inps[0].Trim(new char[] { '[', ']' }).ToCharArray().Select((c, i) => c == '#' ? i : -1).ToList();
            goal.RemoveAll(n => n < 0);
            List<int> jolt = inps.Last().Substring(1, inps.Last().Length - 2).Split(',').Select(c => int.Parse(c)).ToList();

            inps.RemoveAt(0);
            inps.RemoveAt(inps.Count - 1);
            List<Switch> switchs = new List<string>(inps).Select(x => new Switch() { buttons = x.Substring(1, x.Length - 2).Split(',').Select(n => int.Parse(n)).ToList() }).ToList();
            
            switchs.Sort((a, b) => b.buttons.Count.CompareTo(a.buttons.Count)); // try biggest first

            machines.Add(new Machine() { goalState = goal, switches = switchs, joltage = jolt, size = machineSize });
        }

        // Idea there is no point to do a button twice, so we'll do a recursive func, trying to switch a button and see if we can reach the goal with the rest
        double result = 0;
        foreach (Machine mac in machines)
        {
            Debug.LogWarning("Trying to reach state " + System.String.Join(',', mac.goalState));
            double res = recMatchStateP2(mac.joltage, mac.switches);
            Debug.LogWarning("Used " + res.ToString() + " switches to reach goal");
            result += res;

            if (IsDebug && result > 20)
                break;
        }


        return result.ToString();
    }

    double recMatchStateP2(List<int> currentState, List<Switch> remainingSwitches)
    {
        //if (IsDebug)
        //    Debug.Log(new string('*', used.Count) + " [" + System.String.Join(' ', currentState) + "] with used : " + System.String.Join('+', used.Select(x => x.ToString())) + "\t | Remains " + remainingSwitches.Count);

        bool isOK = true;
        for (int i = 0; i < currentState.Count; i++)
        {
            if (currentState[i] < 0)
                return 10000;  // Too much jolt

            isOK &= currentState[i] == 0;
        }
        if (isOK)
            return 0;   // All at 0, perfect number of press


        
        List<Switch> availableSwitches = new List<Switch>(remainingSwitches);
        for (int i = 0; i < currentState.Count; i++)
        {
            if (currentState[i] == 0)
            {
                availableSwitches.RemoveAll(x => x.buttons.Contains(i));
            }
        }
        
        if (availableSwitches.Count == 0)
        {
            //if (IsDebug)
            //    Debug.Log(new string('>', used.Count) + " No remaining switches, dead end");

            return 10000;
        }


        double res = 100000;
        List<int> headButtons = remainingSwitches[0].buttons;
        List<Switch> tailSwitches = new List<Switch>(availableSwitches);
        tailSwitches.RemoveAt(0);

        List<int> withHeadState = new List<int>(currentState);
        foreach (int b in headButtons)
        {
            withHeadState[b]--;
        }

        //List<Switch> withHeadUsed = new List<Switch>(used);
        //withHeadUsed.Add(remainingSwitches[0]);


        double resWithHead = recMatchStateP2(withHeadState, tailSwitches);
        //if (IsDebug)
        //    Debug.Log(new string('$', withHeadUsed.Count) + " Res WITHHH Head is currently " + resWithHead.ToString() + " " + System.String.Join('+', withHeadUsed.Select(x => x.ToString())));

        double resWithoutHead = 10000;
        if (resWithHead > 1)
        {
            resWithoutHead = recMatchStateP2(currentState, tailSwitches);
            //if (IsDebug)
            //    Debug.Log(new string('$', used.Count) + " Res WITHOUT Head is currently " + resWithoutHead.ToString() + " " + System.String.Join('+', used.Select(x => x.ToString())));
        }
        else if (IsDebug)
        {
            //Debug.Log(new string('$', used.Count) + " Skipping Res WITHOUT Head since WITH Head was optimal");
        }

        // Test with the first button switch and without the button switch, with all next buttons
        res = Math.Min(1 + resWithHead, resWithoutHead);
        //if (IsDebug)
        //    Debug.Log(new string('$', used.Count) + " Then Res is currently " + res.ToString());
        //}

        return res;
    }
}
