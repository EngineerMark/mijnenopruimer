using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Camera childCamera;

    [Header("Move")]


    [Header("Zoom")]


    [Header("Rotation")]
    public float rotSpeed = 3.5f;
    private float rotX;
    private float rotY;
    public Vector2 rotXConstraint;

    void Start()
    {
        childCamera = transform.GetChild(0).GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * rotSpeed, -Input.GetAxis("Mouse X") * rotSpeed, 0));
            rotX = Mathf.Clamp(transform.rotation.eulerAngles.x, rotXConstraint.x, rotXConstraint.y);
            rotY = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
    }
}
