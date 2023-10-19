using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private InputMap inputMap;
    private Rigidbody rb;

    public float movementSpeed = 200;


    Camera cam;
    Vector2 dir;
    Vector3 movement;
   

    private void Start()
    {
        inputMap = new InputMap();
        inputMap.Enable();

        cam = Camera.main;
        rb = GetComponent<Rigidbody>();

        inputMap.Movement.dash.started += (ctx) =>
        {
            PacmanManager.abilities.Dash(rb, movement);
        };
    }

    private void FixedUpdate()
    {
        dir = inputMap.Movement.WSDA.ReadValue<Vector2>();
        movement = (transform.forward * dir.y + transform.right * dir.x) * movementSpeed * Time.fixedDeltaTime;


        if (!PacmanManager.abilities.isDashing)
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        

    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
    }
}
