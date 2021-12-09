using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AutoRunTests : MonoBehaviour
{
    public DataCollector data;
    public Transform[] goals;
    public GameObject agentPrefab;
    public List<GameObject> Agents;
    public int[] agentCounts;
    public int timesPerAgent;
    public int timeCount;
    public int repetitions;
    public int Test = 0;
    public int index = 0;
    string textfile = Application.streamingAssetsPath + "/Runs/test.txt";
    
    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Runs/");
        timeCount = timesPerAgent * agentCounts[Test];
        for (int i = 0; i < agentCounts[Test]; i++)
        {
            agentPrefab.transform.position = goals[Mathf.FloorToInt(Random.Range(0f, goals.Length))].position;
            agentPrefab.GetComponent<NavMeshController>().goal = goals;
            agentPrefab.GetComponent<NavMeshController>().data = data;
            
            Agents.Add(Instantiate(agentPrefab));
        }
    }
    public void WriteToFile(float time,int collisions)
    {
        
        if (!File.Exists(textfile))
        {
            File.WriteAllText(textfile, "RESULTS: \n\n");
            File.AppendAllText(textfile, "\nAgentCount: " + agentCounts[Test] + "\n");
        }
        File.AppendAllText(textfile, time + " ; " + collisions + "\n");
    }

    public void NextTest()
    {
        
        if (index < repetitions-1)
        {
            index++;
        }
        else
        {
            index = 0;
            Test++;
            File.AppendAllText(textfile, "\nAgentCount: " + agentCounts[Test] + "\n");
        }
        foreach(GameObject agent in Agents)
        {
            Destroy(agent);
        }
        Agents.Clear();
        if (Test < agentCounts.Length) {
            for (int i = 0; i < agentCounts[Test]; i++)
            {
                agentPrefab.transform.position = goals[Mathf.FloorToInt(Random.Range(0f, goals.Length))].position;
                agentPrefab.GetComponent<NavMeshController>().goal = goals;
                agentPrefab.GetComponent<NavMeshController>().data = data;

                Agents.Add(Instantiate(agentPrefab));
            }
        }
    }
}
