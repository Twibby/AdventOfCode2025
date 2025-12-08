using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_07 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    protected override string part_1()
    {
        List<string> grid = _input.Split('\n').ToList();

        double result = 0;

        List<int> curPos = new List<int>() {  grid[0].IndexOf('S') };
        for (int i = 1; i < grid.Count; i++)
        {
            List<int> nextPos = new List<int>();
            foreach (int pos in curPos)
            {
                if (grid[i][pos] == '^')
                {
                    result++;
                    if (!nextPos.Contains(pos - 1) && pos > 0)
                        nextPos.Add(pos - 1);

                    if (!nextPos.Contains(pos + 1) && pos < grid[pos].Length -1)
                        nextPos.Add(pos + 1);
                }
                else if (!nextPos.Contains(pos))
                    nextPos.Add(pos);
            }

            curPos = new List<int>(nextPos);
        }

        return result.ToString();
    }

    protected override string part_2()
    {
        List<string> grid = _input.Split('\n').ToList();


        Dictionary<int, double> curPos = new Dictionary<int, double> { {grid[0].IndexOf('S'), 1} };
        for (int i = 1; i < grid.Count; i++)
        {
            Dictionary<int, double> nextPos = new Dictionary<int, double>();
            foreach (int pos in curPos.Keys)
            {
                if (grid[i][pos] == '^')
                {
                    if (pos > 0)
                    {
                        if (!nextPos.ContainsKey(pos - 1))
                            nextPos.Add(pos - 1, 0);

                        nextPos[pos - 1] += curPos[pos];
                    }

                    if (pos < grid[pos].Length + 1)
                    {
                        if (!nextPos.ContainsKey(pos + 1))
                            nextPos.Add(pos + 1, 0);

                        nextPos[pos + 1] += curPos[pos];
                    }
                }
                else
                {
                    if (!nextPos.ContainsKey(pos))
                        nextPos.Add(pos, 0);

                    nextPos[pos] += curPos[pos];
                }
            }

            curPos = new Dictionary<int, double>(nextPos);
        }

        return curPos.Values.Sum().ToString();
    }
}
