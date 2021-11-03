using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera camera1;

    // Update is called once per frame
    private void Start()
    {
        camera1 = Camera.main;
    }

    void Update()
    {
        transform.LookAt(camera1.transform.position);
        // transform.localEulerAngles = new Vector3(180, 0, 0);
    }
}
