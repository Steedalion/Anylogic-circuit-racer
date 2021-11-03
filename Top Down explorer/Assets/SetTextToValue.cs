using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SetTextToValue : MonoBehaviour
{
    public Text text;
    public string preText;
    public NavMeshAgent agent;


    // Update is called once per frame
    void Update()
    {
        text.text = preText + agent.speed.ToString("0.00");
    }
}
