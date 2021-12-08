using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataCollector : MonoBehaviour
{
    public List<float> times = new List<float>();
    public AutoRunTests AutoRun;

    public int collisions = 0;

    public void Send(float num)
    {
        times.Add(num);
        if (times.Count >= AutoRun.timeCount)
        {
            AutoRun.WriteToFile(Average(), collisions);
            AutoRun.NextTest();
            times.Clear();
            collisions = 0;
        }
    }

    public float Average()
    {
        float sum = 0;
        foreach (float time in times)
        {
            sum += time;
        }
        return (sum / times.Count);
    }
    public void Collision()
    {
        collisions += 1;
    }
}
    
