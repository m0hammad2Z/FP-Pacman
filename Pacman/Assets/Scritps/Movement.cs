using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private InputMap inputMap;
    private Rigidbody rigidbody;

    [SerializeField] float movementSpeed = 200;

    Camera cam;
    Vector2 dir;

    private void Start()
    {
        inputMap = new InputMap();
        inputMap.Enable();

        cam = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        dir = inputMap.Movement.WSDA.ReadValue<Vector2>();
        Vector3 x = (transform.forward * dir.y + transform.right * dir.x) * movementSpeed * Time.deltaTime;
        rigidbody.velocity = x;

    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
    }
}
