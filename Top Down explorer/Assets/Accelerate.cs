using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Accelerate : MonoBehaviour
{
    [SerializeField] private float speed = 100;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(transform.forward * speed);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(Vector3.up*10);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
             transform.Rotate(Vector3.up*-10);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce(-transform.forward * speed);
        }
    }
}