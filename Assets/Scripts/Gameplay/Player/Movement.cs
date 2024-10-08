using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Vector2 moveDir;
    [SerializeField]
    private bool isTop;
    [SerializeField]
    private bool isGround;
    [SerializeField]
    private Vector2 lookDir;

    public float topOffset;  //distance above player's head to block jumping
    public float bottomOffset;   //distance below player's feet to enable jumping and falling
    public float moveSpeed;
    public float gravity;
    public float velocity;
    public float lookSensX;
    public float lookSensY;

    public Camera childCamera;
    public CharacterController characterController;
   
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        isGround = isGrounded();
        isTop = isTopBlocked();
        //rotate character by mouseY
        transform.Rotate(Vector3.up, lookDir.x * lookSensX * Time.deltaTime);
        
        //movement & gravity
        Vector3 moveDelta = transform.right * moveDir.x + transform.forward * moveDir.y;
        moveDelta *= moveSpeed * Time.deltaTime;
        if (isTopBlocked()){
            //  jump not allowed
            velocity = -1f;
        }
        
        if (!isGrounded()){
            //  falling or jumping
            velocity -= Time.deltaTime * gravity;
        }else if(velocity < 0){         
            //just landed
            velocity = -1f;
        }
        moveDelta += Vector3.up * velocity * Time.deltaTime;
        characterController.Move(moveDelta);

        //camera pitch & restrict the max degree
        float rotateDelta = lookDir.y * lookSensY * Time.deltaTime;
        float plannedRotate = childCamera.transform.localEulerAngles.x - rotateDelta;
        int range =  (int)(plannedRotate / 90);
        //camera's transform.localEulerAngles.x is 0 - 90 degree when look down, 270 - 360 when look up
        //1 means larger than 90 degree(look down), 2 means lower than 270(look up)
        if (range == 1){
            plannedRotate = 90f;
        }
        if (range == 2){
            plannedRotate = -90f;
        }
        childCamera.transform.localEulerAngles = new Vector3(plannedRotate, 0, 0);
    }

    public void OnMove(InputAction.CallbackContext callbackContext){
        moveDir = callbackContext.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        lookDir = callbackContext.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.started &&
            Physics.Raycast(transform.position, Vector3.down, bottomOffset + characterController.height / 2))
        {
            velocity = 5;
        }
    }

    bool isGrounded(){
        return Physics.Raycast(transform.position, Vector3.down, bottomOffset + characterController.height / 2);
    }
    bool isTopBlocked()
    {
        return Physics.Raycast(transform.position, Vector3.up, topOffset + characterController.height / 2);
    }
}
