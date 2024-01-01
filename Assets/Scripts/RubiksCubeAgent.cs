using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RubiksCubeAgent : Agent
{
    public int move = 0;
    public List<float> idealCube;
    public CubeScript cubehandler;
    public TextMesh display;
    public Material sucess;
    public Material fail;
    public List<float> currentState;
    public Gradient failGradient;
    public static int numberOfSolves;
    public static int totalTries;
    public static int step;
    public override void OnEpisodeBegin()
    {
        move = 0;
        cubehandler.RebuildCube();
        cubehandler.Scramble(); 
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(cubehandler.TakeInput());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];
        if (action == 0) { cubehandler.LeftTurn(false); }
        else if (action == 1) { cubehandler.LeftTurn(true); }
        else if (action == 2) { cubehandler.RightTurn(false); }
        else if (action == 3) { cubehandler.RightTurn(true); }
        else if (action == 4) { cubehandler.UpTurn(false); }
        else if (action == 5) { cubehandler.UpTurn(true); }
        else if (action == 6) { cubehandler.DownTurn(false); }
        else if (action == 7) { cubehandler.DownTurn(true); }
        else if (action == 8) { cubehandler.FrontTurn(false); }
        else if (action == 9) { cubehandler.FrontTurn(true); }
        else if (action == 10) { cubehandler.BackTurn(false); }
        else if (action == 11) { cubehandler.BackTurn(true); }
        SetReward();
        if(gameObject.name.Contains("7"))
        {
            step++;
        }
    }
    void SetReward()
    {
        currentState = cubehandler.TakeInput();
        int piecesMatching = 0;
        for (int x = 0; x < 24; x++)
        {
            if (idealCube[x] - currentState[x] <= 0.5f)
            {
                piecesMatching++;
            }
        }
        if (piecesMatching == 24)
        {
            numberOfSolves++;
            totalTries++;
            SetReward(100);
            display.text = "Reward:" + 100.ToString();
            display.transform.parent.Find("Cube").GetComponent<Renderer>().material = sucess;
            EndEpisode();
            for(int x = 0; x < 24; x++)
            {
                print(idealCube[x]);
            }
        }
        else if (move == 17)
        {
            display.transform.parent.Find("Cube").GetComponent<Renderer>().material = fail;
        }
        else if (move == 18)
        {
            totalTries++;
            int reward = 0;
            //Calculate Piece Rewards
            reward += piecesMatching;
            //Calculate Face Rewards
            for (int x = 0; x < 6; x++)
            {
                int piecesMatchingOnFace = 0;
                for (int y = 0; y < 4; y++)
                {
                    if (currentState[x*4+y] == idealCube[x*4+y])
                    {
                        piecesMatchingOnFace++;
                    }
                }
                if(piecesMatchingOnFace == 4)
                {
                    reward += 10;
                }
            }
            SetReward(reward);
            display.text = "Reward:" + reward.ToString();
            display.transform.parent.Find("Cube").GetComponent<Renderer>().material = fail;
            display.transform.parent.Find("Cube").GetComponent<Renderer>().material.color = failGradient.Evaluate(reward / 84);
            display.transform.parent.Find("Cube").GetComponent<Renderer>().material.SetColor("_EmissionColor", failGradient.Evaluate(reward / 84));
            EndEpisode();
        }
        move++;
    }
}
