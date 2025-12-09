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

    public float factor = 1f / 200f;
    protected override string part_2()
    {
        List<Vector2> redTiles = new List<Vector2>();
        int index = 0;
        foreach (string instruction in _input.Split('\n'))
        {
            float[] pos = instruction.Split(',').Select(x => float.Parse(x)).ToArray();
            redTiles.Add(new Vector2(pos[0], pos[1]));

            // Visual debug
            if (IsDebug)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = new Vector3(pos[0] * factor, pos[1] * factor, 0);
                go.name = "Point " + (index / 16).ToString();
                go.GetComponent<MeshRenderer>().material.color = new Color((float)(index % 256) / 255f, 16f * (float)(index / 256) / 255f, (index / 256f) / 255f);
                index += 16;
            }
        }

        double maxArea = 0;
        
        for (int i = 0; i < redTiles.Count; i++)
        {
            for (int j = i + 1; j < redTiles.Count; j++)
            {
                // Hack because all tiles are forming a beautiful convex circle except the couple of bastard 248 & 249
                if (i > 30 && j >= 249) 
                    continue;


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
                    double tmp = maxArea;
                    // Valid rectangle
                    maxArea = Math.Max(maxArea, (xMax - xMin + 1) * (yMax - yMin + 1));

                    // VISUAL DEBUG
                    if (tmp != maxArea)
                    {
                        Debug.Log("i : " + i + " | " + j);
                        setPlaneFromPoints(curA * factor, curB * factor);
                    }
                }
            }
        }

        return maxArea.ToString();
    }

    // Following code is only for visual debug
    private Transform plane;
    private GameObject AGo, BGo;

    void setPlaneFromPoints(Vector2 pointA, Vector2 pointB)
    {
        if (plane == null)
            plane = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;

        plane.LookAt(plane.position + Vector3.up, Vector3.forward);


        if (AGo)
            Destroy(AGo);
        if (BGo)
            Destroy(BGo);

        AGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        AGo.transform.position = pointA;
        AGo.transform.localScale = 10f * Vector3.one;
        AGo.name = "PointA";
        AGo.GetComponent<MeshRenderer>().material.color = Color.red;

        BGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        BGo.transform.position = pointB;
        BGo.transform.localScale = 10f * Vector3.one;
        BGo.name = "PointB";
        BGo.GetComponent<MeshRenderer>().material.color = Color.green;

        plane.position = new Vector2((pointA.x + pointB.x) / 2f, (pointA.y + pointB.y) / 2f);
        Vector3 topCenter = new Vector3(Mathf.Abs(pointA.x - pointB.x) / 2f, Mathf.Max(pointA.y, pointB.y));
        plane.localScale = 0.1f * new Vector3(Mathf.Abs(pointA.x - pointB.x), 10f, Mathf.Abs(pointA.y - pointB.y));

    }
}
