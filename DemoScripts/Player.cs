using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    #region Variables
     public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
 
 
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
 
 
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
 
    public bool canMove = true;
 
    
    CharacterController characterController;

    public float radius = 5f; 
    private List<Vector3> points;
    public float NoPoints = 10f;
    public Terrain terrain;
    private TerrainMorpher tm; 

    public Camera cam;
    #endregion


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        tm = terrain.transform.GetComponent<TerrainMorpher>();
        points = new List<Vector3>();
    }
 
    void Update()
    {
 
        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
 
        #endregion
 
        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
 
        #endregion
 
        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);
 
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

        #region Raycast on click
        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {   
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit)) 
            {   
                CalculatePointsInSphere(radius, hit.point);
                foreach (var point in points)
                {   
                    tm.deformInFront(point);
                }
            }      
        }*/
        #endregion
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerCamera.gameObject.SetActive(true);
            cam.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerCamera.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
        }
    }
    void CalculatePointsInSphere(float radius, Vector3 origin)
    {
        for (int i = 0; i < NoPoints; i++)
        {
            for (int j = 0; j < NoPoints; j++)
            {
                for (int k = 0; k < NoPoints; k++)
                {
                    // Calculate the normalized position in 3D space
                    float x = i / NoPoints * 2f * radius - radius; 
                    float y = j / NoPoints * 2f * radius - radius; 
                    float z = k / NoPoints * 2f * radius - radius; 

                    if (x * x + y * y + z * z <= radius * radius)
                    {
                        Vector3 point = new Vector3(x, y, z) + origin;
                        points.Add(point);
                    }
                }
            }
        }
    }
}
