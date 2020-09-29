using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Camera childCamera;
    public Transform rotatingChild;

    [Header("Move")]
    public float moveSpeed;
    public float shiftMoveSpeed;
    public BoxCollider cameraConstraints;

    [Header("Zoom")]
    public float zoomMultiplier = 1f;
    public Vector2 zoomConstraints;

    [Header("Rotation")]
    public float rotSpeed = 3.5f;
    private float rotX;
    private float rotY;
    public Vector2 rotXConstraint;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rotatingChild = transform.GetChild(0);
        childCamera = rotatingChild.GetChild(0).GetComponent<Camera>();
    }

    void Update()
    {
        if (!GameManager.instance.backgroundPanel.activeSelf)
        {
            Move();
            Zoom();
            Rotate();
        }
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
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0){
            scrollInput *= zoomMultiplier;
            float zPosition = -childCamera.transform.localPosition.z - scrollInput;

            zPosition = Mathf.Clamp(zPosition, zoomConstraints.x, zoomConstraints.y);
            childCamera.transform.localPosition = new Vector3(0,0, -zPosition);
            //Debug.Log("Zoom: " + zPosition);
        }
    }

    private void Rotate(){
        if (Input.GetMouseButton(2))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            rotatingChild.Rotate(new Vector3(Input.GetAxis("Mouse Y") * rotSpeed, -Input.GetAxis("Mouse X") * rotSpeed, 0));
            rotX = Mathf.Clamp(rotatingChild.rotation.eulerAngles.x, rotXConstraint.x, rotXConstraint.y);
            rotY = rotatingChild.rotation.eulerAngles.y;
            rotatingChild.rotation = Quaternion.Euler(rotX, rotY, 0);
        }else{
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
