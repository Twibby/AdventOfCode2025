using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_09 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    protected override string part_1()
    {
        List<Vector2> redTiles = new List<Vector2>();
        foreach (string instruction in _input.Split('\n'))
        {
            float[] pos = instruction.Split(',').Select(x => float.Parse(x)).ToArray();
            redTiles.Add(new Vector2(pos[0], pos[1]));
        }

        double maxArea = 0;
        for (int i = 0; i < redTiles.Count;i ++)
        {
            for (int j = i+1; j < redTiles.Count;j ++)
            { 
                maxArea = System.Math.Max(maxArea, (System.Math.Abs((double)(redTiles[i].x - redTiles[j].x)) +1)  * (System.Math.Abs((double)(redTiles[i].y - redTiles[j].y)) +1) );
            }
        }

        return maxArea.ToString();
    }

    protected override string part_2()
    {
        List<Vector2> redTiles = new List<Vector2>();
        foreach (string instruction in _input.Split('\n'))
        {
            float[] pos = instruction.Split(',').Select(x => float.Parse(x)).ToArray();
            redTiles.Add(new Vector2(pos[0], pos[1]));
        }

        //bool isAlternate = true;
        //for (int i = 0; i < redTiles.Count -1; i++)
        //{
        //    if (isAlternate && redTiles[i].x != redTiles[i + 1].x)
        //        return i + " " + redTiles[i - 1] + " " + redTiles[i] + " " + redTiles[i + 1];
        //    if (!isAlternate && redTiles[i].y != redTiles[i + 1].y)
        //        return i + " " + redTiles[i - 1] + " " + redTiles[i] + " " + redTiles[i + 1];

        //    isAlternate = !isAlternate;
        //}

        //return "OK";



        // Idea for each segment,
        // First try to prolongate it if next segment is same direction  (NOPE SEEMS TO NEVER HAPPEN, ALWAYS AN ANGLE)
        // Then take two neighbours, and look if they are on the same side
        // If yes, compute area and compare to max
        //double maxArea = 0;
        //bool isOnSameX = false;
        //for (int i = 0; i < redTiles.Count; i++)
        //{
        //    isOnSameX = !isOnSameX;

        //    // segment is i---i+1   isOnX means the segment have same X so we must compare X diff with prev and next red tiles
        //    Vector2 curA = redTiles[i];
        //    Vector2 curB = redTiles[i < redTiles.Count - 1 ? i + 1 : 0];
        //    Vector2 prev = i > 0 ? redTiles[i - 1] : redTiles.Last();
        //    Vector2 next = redTiles[(i + 2) % redTiles.Count];

        //    double diffPrev = isOnSameX ? prev.x - curA.x : prev.y - curA.y ;
        //    double diffNext = isOnSameX ? next.x - curA.x : next.y - curA.y;
        //    if (Math.Sign(diffPrev) != Math.Sign(diffNext))
        //    { // Probably useless but let's compute thin rectangle
        //        maxArea = Math.Max(maxArea, 1+Math.Abs(isOnSameX ? (curB.y - curA.y) : (curB.x - curA.x)));
        //        continue;
        //    }

        //    double maxDiff = 1 + Math.Max(Math.Abs(diffPrev), Math.Abs(diffNext));
        //    double segDiff = 1 + (isOnSameX ? (curB.y - curA.y) : (curB.x - curA.x));

        //    maxArea = Math.Max(maxArea, maxDiff * segDiff);


        //    // We should also consider bigger diff for a big rectangle, if there is no point inside of it
        //    /*   N-------....   Next
        //     *   |    
        //     *   |    P--...    Prev
        //     *   |    |
        //     *   B----A         */

        //}
        // 1540957  <<

        double maxArea = 0;
        // dumb way : generate all rectangle and test if there are points inside ??
        for (int i = 0; i < redTiles.Count; i++)
        {
            for (int j = i + 1; j < redTiles.Count; j++)
            {
                //maxArea = System.Math.Max(maxArea, (System.Math.Abs((double)(redTiles[i].x - redTiles[j].x)) + 1) * (System.Math.Abs((double)(redTiles[i].y - redTiles[j].y)) + 1));

                Vector2 curA = redTiles[i];
                Vector2 curB = redTiles[j];
                float xMin = Mathf.Min(curA.x, curB.x);
                float xMax = Mathf.Max(curA.x, curB.x);
                float yMin = Mathf.Min(curA.y, curB.y);
                float yMax = Mathf.Max(curA.y, curB.y);

                if (xMax < xMin || yMax < yMin)
                {
                    // No point inside possible, probably a case of "thin" rectangle, comupte area
                    maxArea = Math.Max(maxArea, (xMax - xMin + 1) * (yMax - yMin + 1));
                    continue;
                }

                bool hasPointsInside = false;
                foreach (Vector2 point in redTiles)
                {
                    if (point.x > xMin && point.x < xMax && point.y > yMin && point.y < yMax)
                    {
                        hasPointsInside = true;
                        break;
                    }
                }

                if (!hasPointsInside)
                {
                    // Valid rectangle
                    maxArea = Math.Max(maxArea, (xMax - xMin + 1) * (yMax - yMin + 1));
                }

            }
        }
        // 4591195600  too high

        return maxArea.ToString();
    }
}
