using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_04 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    protected override string part_1()
    {
        List<string> rows = _input.Split('\n').ToList();


        double result = 0;
        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].Length; j++)
            {
                if (rows[i][j] != '@')
                    continue;

                int neighboursCount = 0;

                if (i > 0 && j > 0 && rows[i-1][j-1] == '@')
                    neighboursCount++;

                if (i > 0 && rows[i - 1][j] == '@')
                    neighboursCount++;

                if (i > 0 && j < rows[i-1].Length -1 && rows[i - 1][j + 1] == '@')
                    neighboursCount++;


                if (j > 0 && rows[i][j - 1] == '@')
                    neighboursCount++;

                if (j < rows[i].Length - 1 && rows[i][j + 1] == '@')
                    neighboursCount++;


                if (i < rows.Count -1 && j > 0 && rows[i + 1][j - 1] == '@')
                    neighboursCount++;

                if (i < rows.Count - 1 && rows[i + 1][j] == '@')
                    neighboursCount++;

                if (i < rows.Count - 1 && j < rows[i + 1].Length - 1 && rows[i + 1][j + 1] == '@')
                    neighboursCount++;


                if (neighboursCount < 4)
                    result++;

            }
        }

        return result.ToString();
    }

    protected override string part_2()
    {
        List<List<char>> rows = _input.Split('\n').Select(x => x.ToCharArray().ToList()).ToList();

        double result = 0;

        List<(int, int)> rollsRemoved = new List<(int, int)>();

        do
        {
            rollsRemoved = new List<(int, int)>();

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < rows[i].Count; j++)
                {
                    if (rows[i][j] != '@')
                        continue;

                    int neighboursCount = 0;

                    if (i > 0 && j > 0 && rows[i - 1][j - 1] == '@')
                        neighboursCount++;

                    if (i > 0 && rows[i - 1][j] == '@')
                        neighboursCount++;

                    if (i > 0 && j < rows[i - 1].Count - 1 && rows[i - 1][j + 1] == '@')
                        neighboursCount++;


                    if (j > 0 && rows[i][j - 1] == '@')
                        neighboursCount++;

                    if (j < rows[i].Count - 1 && rows[i][j + 1] == '@')
                        neighboursCount++;


                    if (i < rows.Count - 1 && j > 0 && rows[i + 1][j - 1] == '@')
                        neighboursCount++;

                    if (i < rows.Count - 1 && rows[i + 1][j] == '@')
                        neighboursCount++;

                    if (i < rows.Count - 1 && j < rows[i + 1].Count - 1 && rows[i + 1][j + 1] == '@')
                        neighboursCount++;


                    if (neighboursCount < 4)
                    {
                        result++;
                        rollsRemoved.Add((i, j));
                    }

                }
            }

            Debug.Log("Removing " + rollsRemoved.Count + " rolls");
            foreach ((int, int) roll in rollsRemoved)
            {
                rows[roll.Item1][roll.Item2] = 'x';
            }

        } while (rollsRemoved.Count > 0);

        return result.ToString();
    }
}
