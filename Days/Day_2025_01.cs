using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_01 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    protected override string part_1()
    {
        long result = 0;
        int curPos = 50;
        foreach (string instruction in _input.Split('\n'))
        {
            bool isRight = instruction[0] == 'R';
            int count = int.Parse(instruction.Substring(1));

            curPos += isRight ? count : -count;
            curPos %= 100;

            if (curPos == 0)
                result++;

        }

        return result.ToString();
    }

    protected override string part_2()
    {
        long result = 0;
        long curPos = 50;
        foreach (string instruction in _input.Split('\n'))
        {
            bool isRight = instruction[0] == 'R';
            int count = int.Parse(instruction.Substring(1));

            long prevRes = result;
            long prevPos = curPos;

            // Method 1
            /* 
            {
                curPos += isRight ? count : -count;

                if (curPos > 99)
                    result += curPos / 100;
                else if (curPos < 0)
                    result += curPos / 100 + (prevPos == 0 ? 0 : 1);
                else if (curPos == 0)
                    result++;

                curPos = (curPos + 100000000) %100;
            }
            */


            /*
            // Method 2
            {
                curPos += isRight ? count : -count;

                if (curPos > 99)
                {
                    while (curPos > 99)
                    {
                        curPos -= 100;
                        result++;
                    }
                }
                else if (curPos < 0)
                {
                    while (curPos < 0)
                    {
                        curPos += 100;
                        result++;
                    }
                    if (prevPos == 0 && curPos != 0)
                        result--;
                }
                else if (curPos == 0)
                    result++;
            }
            */


            // Method 3
            {
                result += count / 100;
                count %= 100;

                curPos += isRight ? count : -count;

                if (curPos > 99)
                {
                    result++;
                    curPos -= 100;
                }
                else if (curPos < 0)
                {
                    curPos += 100;
                    if (prevPos != 0 )
                        result++;
                }
                else if (curPos == 0)
                    result++;
            }
        }

        return result.ToString();
    }
}
