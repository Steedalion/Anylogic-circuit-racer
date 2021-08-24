using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetAnimatorSpeed : MonoBehaviour
{
    public NavMeshAgent agent;
    private Animator anim;
    private static readonly int Speed = Animator.StringToHash("speed");

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat(Speed, agent.speed);
    }
}
