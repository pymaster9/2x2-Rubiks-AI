using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayScript : MonoBehaviour
{
    public int totalSteps;
    float timeSinceStart;
    void FixedUpdate()
    {
        timeSinceStart += Time.deltaTime;
        string output = "Step: " + RubiksCubeAgent.step.ToString() + "/ " + totalSteps.ToString() + " (" + (((float)RubiksCubeAgent.step/(float)totalSteps)*100).ToString() + "% )";
        output += "\nTotal Episodes Completed: " + RubiksCubeAgent.totalTries.ToString();
        output += "\nTotal Solves: " + RubiksCubeAgent.numberOfSolves.ToString();
        if (RubiksCubeAgent.numberOfSolves > 0)
        {
            output += "\nSolve Percentage: " + (((float)RubiksCubeAgent.numberOfSolves / (float)RubiksCubeAgent.totalTries) * 100).ToString() + "%";
        }
        if (((float)RubiksCubeAgent.step / (float)totalSteps) > 0)
        {
            float ete = (timeSinceStart / ((float)RubiksCubeAgent.step / (float)totalSteps)) * (1 - ((float)RubiksCubeAgent.step / (float)totalSteps));
            output += "\nETE: " + ete + " seconds";
        }
        GetComponent<TextMeshProUGUI>().text =  output;
    }
}
