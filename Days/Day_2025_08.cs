using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day_2025_08 : DayScript2025
{
    protected override string test_input()
    {
        return _input;
    }

    struct JBoxPair
    {
        public Vector3 boxA;
        public Vector3 boxB;
        public float distance;
    }

    struct Circuit
    {
        List<Vector3> jboxes;
    }

    protected override string part_1()
    {
        List<Vector3> jbox = new List<Vector3>();
        foreach (string instruction in _input.Split('\n'))
        {
            List<float> coords = instruction.Split(',').Select(x => float.Parse(x)).ToList();
            jbox.Add(new Vector3(coords[0], coords[1], coords[2]));
        }

        // Get 1000 closest pairs
        List<JBoxPair> pairs = new List<JBoxPair>();
        for (int i = 0; i < jbox.Count; i++) 
        {
            for (int j = i + 1; j < jbox.Count; j++)
            {
                float dist = Vector3.Distance(jbox[i], jbox[j]);

                if (dist is float.NaN)
                    continue;

                if (pairs.Count < 1000 || dist < pairs[999].distance)
                {
                    if (pairs.Count >= 1000)
                        pairs.RemoveAt(999);
                    
                    pairs.Add(new JBoxPair() { boxA = jbox[i], boxB = jbox[j], distance = dist });
                    pairs.Sort(delegate (JBoxPair a, JBoxPair b)
                    {
                        return a.distance.CompareTo(b.distance);
                    });
                }
            }
        }
        
        // Create basic circuits with those 1000 pairs
        List<List<Vector3>> circuits = new List<List<Vector3>>();
        for (int i = 0; i < pairs.Count; i++)
        {
            JBoxPair curPair = pairs[i];

            bool added = false;
            foreach (List<Vector3> circuit in circuits)
            {
                if (circuit.Contains(curPair.boxA) && circuit.Contains(curPair.boxB))
                {
                    added = true;
                    break;
                }

                if (circuit.Contains(curPair.boxA))
                {
                    circuit.Add(curPair.boxB);
                    added = true;
                    break;
                }

                if (circuit.Contains(curPair.boxB))
                {
                    circuit.Add(curPair.boxA);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                circuits.Add(new List<Vector3>() { curPair.boxA, curPair.boxB });
            }
        }

        // There still can be circuits to merge, for example if we have 
        // 1. A-B  2. C-D  and  a new pair B-C, 3 will be added to first circuit and we'll have A-B-C & C-D instead of A-B-C-D
        List<List<Vector3>> realCircuits = new List<List<Vector3>>();
        List<int> indexRemoved = new List<int>();
        for (int i = 0; i < circuits.Count; i++) 
        {
            if (indexRemoved.Contains(i))
                continue;

            List<Vector3> newCirc = new List<Vector3>();
            List<Vector3> toLink = new List<Vector3>(circuits[i]);
            while (toLink.Count > 0)
            {
                List<Vector3> newLinks = new List<Vector3>();
                foreach (Vector3 jb in toLink)
                {
                    if (newCirc.Contains(jb))
                        continue;

                    newCirc.Add(jb);

                    for (int j = i + 1; j < circuits.Count; j++)
                    {
                        if (indexRemoved.Contains(j))
                            continue;

                        if (circuits[j].Contains(jb))
                        {
                            // WE HAVE A MERGE TO DO!   --> Add new jbox to check if they are in multiple circuits
                            indexRemoved.Add(j);

                            newLinks.AddRange(circuits[j]);
                            newLinks.Remove(jb);
                        }
                    }
                }

                toLink = new List<Vector3>(newLinks);
            }

            realCircuits.Add(newCirc);
        }


        List<int> counts = realCircuits.Select(x => x.Count).ToList();
        counts.Sort();
        counts.Reverse();
        double res = counts[0] * counts[1] * counts[2];

        return res.ToString();
    }

    protected override string part_2()
    {
        List<Vector3> jbox = new List<Vector3>();
        foreach (string instruction in _input.Split('\n'))
        {
            List<float> coords = instruction.Split(',').Select(x => float.Parse(x)).ToList();
            jbox.Add(new Vector3(coords[0], coords[1], coords[2]));
        }
        int jBoxCount = jbox.Count;

        // Get all pairs, ordered by distance
        List<JBoxPair> pairs = new List<JBoxPair>();
        for (int i = 0; i < jbox.Count; i++)
        {
            for (int j = i + 1; j < jbox.Count; j++)
            {
                float dist = Vector3.Distance(jbox[i], jbox[j]);

                if (dist is float.NaN)
                    continue;

                pairs.Add(new JBoxPair() { boxA = jbox[i], boxB = jbox[j], distance = dist });
            }
        }

        pairs.Sort(delegate (JBoxPair a, JBoxPair b)
        {
            return a.distance.CompareTo(b.distance);
        });

        // Create circuits and check at each step if there is only one circuit containing all jboxes
        List<List<Vector3>> circuits = new List<List<Vector3>>();
        for (int pi = 0; pi < pairs.Count; pi++)
        {
            JBoxPair curPair = pairs[pi];

            bool added = false;
            Vector3 toCheck = Vector3.zero;
            foreach (List<Vector3> circuit in circuits)
            {
                if (circuit.Contains(curPair.boxA) && circuit.Contains(curPair.boxB))
                {
                    added = true;
                    break;
                }

                if (circuit.Contains(curPair.boxA))
                {
                    circuit.Add(curPair.boxB);
                    added = true;
                    toCheck = curPair.boxB;
                    break;
                }

                if (circuit.Contains(curPair.boxB))
                {
                    circuit.Add(curPair.boxA);
                    added = true;
                    toCheck = curPair.boxA;
                    break;
                }
            }

            if (!added)
            {
                circuits.Add(new List<Vector3>() { curPair.boxA, curPair.boxB });
            }
            else
            {   // Merge circuits

                // Note : Ideally, the merge should be done in the previous loop, when a match is found,
                // so you dont have to run all circuits again but just the following ones
                List<List<Vector3>> realCircuits = new List<List<Vector3>>();
                List<int> indexRemoved = new List<int>();
                for (int i = 0; i < circuits.Count; i++)
                {
                    if (indexRemoved.Contains(i))
                        continue;

                    List<Vector3> newCirc = new List<Vector3>();
                    List<Vector3> toLink = new List<Vector3>(circuits[i]);
                    while (toLink.Count > 0)
                    {
                        List<Vector3> newLinks = new List<Vector3>();
                        foreach (Vector3 jb in toLink)
                        {
                            if (newCirc.Contains(jb))
                                continue;

                            newCirc.Add(jb);

                            for (int j = i + 1; j < circuits.Count; j++)
                            {
                                if (indexRemoved.Contains(j))
                                    continue;

                                if (circuits[j].Contains(jb))
                                {
                                    // WE HAVE A MERGE TO DO!   --> Add new jbox to check if they are in multiple circuits
                                    indexRemoved.Add(j);

                                    newLinks.AddRange(circuits[j]);
                                    newLinks.Remove(jb);
                                }
                            }
                        }

                        toLink = new List<Vector3>(newLinks);
                    }

                    realCircuits.Add(newCirc);
                }
                circuits = new List<List<Vector3>>(realCircuits);
            }

            if (circuits.Count == 1 && circuits[0].Count == jBoxCount)
            {
                return ((double)curPair.boxA.x * (double)curPair.boxB.x).ToString();
            }
        }

        return "Not found";
    }
}
