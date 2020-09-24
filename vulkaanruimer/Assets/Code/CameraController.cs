using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Camera childCamera;
    private Transform rotatingChild;

    [Header("Move")]
    public float moveSpeed;
    public float shiftMoveSpeed;
    public BoxCollider cameraConstraints;

    [Header("Zoom")]


    [Header("Rotation")]
    public float rotSpeed = 3.5f;
    private float rotX;
    private float rotY;
    public Vector2 rotXConstraint;

    void Start()
    {
        rotatingChild = transform.GetChild(0);
        childCamera = rotatingChild.GetChild(0).GetComponent<Camera>();
    }

    void Update()
    {
        Move();
        Zoom();
        Rotate();
    }

    private void Move(){
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = input.normalized;
        Vector3 dir = rotatingChild.forward * input.z + rotatingChild.right * input.x;
        dir.y = 0;
        dir = dir.normalized;
        Vector3 newPosition = transform.position + (dir * (Input.GetKey(KeyCode.LeftShift) ? shiftMoveSpeed : moveSpeed) * Time.deltaTime);
        newPosition = Vector3.Max(newPosition, cameraConstraints.bounds.min);
        newPosition = Vector3.Min(newPosition, cameraConstraints.bounds.max);
        newPosition.y = 0;
        transform.position = newPosition;
    }

    private void Zoom(){
        
    }

    private void Rotate(){
        if (Input.GetMouseButton(2))
        {
            rotatingChild.Rotate(new Vector3(Input.GetAxis("Mouse Y") * rotSpeed, -Input.GetAxis("Mouse X") * rotSpeed, 0));
            rotX = Mathf.Clamp(rotatingChild.rotation.eulerAngles.x, rotXConstraint.x, rotXConstraint.y);
            rotY = rotatingChild.rotation.eulerAngles.y;
            rotatingChild.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
    }
}
