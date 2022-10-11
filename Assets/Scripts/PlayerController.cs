using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField] //private으로 해도 수정 가능함
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;
    [SerializeField]
    private float jumpForce;

    //상태 변수
    private bool run = false;
    private bool isGround = true;
    private bool isCrouch=false;

    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider;

    //민감도
    [SerializeField]
    private float lookSensitivity;


    //카메라 각도 제한 
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX=0f;

    //기타 컴포넌트
    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;
    public float rotationSpeed = 360f;


    // Start is called before the first frame update
    void Start()//스크립트가 처음 실행 될때
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update() 
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {   /*
            if (isCrouch) //앉은상태 점프 해제
            {
                Crouch();
            }*/
            myRigid.velocity = transform.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
            applySpeed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
            applySpeed = walkSpeed;
        }
        //
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        //
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity; //현재 위치에서 민감도만큼 곱해주기
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//카메라의 최대값과 최소값 유지
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);







    }

}
