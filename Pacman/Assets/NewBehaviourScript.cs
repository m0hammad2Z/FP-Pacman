using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public Rigidbody rb;

    private float time = 0.0f;
    private bool isMoving = false;
    private bool isJumpPressed = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        print("Time : " + Time.time);
        print("Delta Time : " + Time.deltaTime);
        print("fixed Time : " + Time.fixedDeltaTime);
        print("Smoot Time : " + Time.smoothDeltaTime);
    }




    
}
