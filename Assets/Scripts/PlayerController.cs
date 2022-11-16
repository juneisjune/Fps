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
    private bool isWalk=false;
    private bool run = false;
    private bool Ground = true;
    private bool Crouch=false;

    //움직임 체크 변수
    private Vector3 lastPos;

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
    private float currentCameraRotationX = 0;


    //기타 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private Crosshair theCrossHair;

    // Start is called before the first frame update
    void Start()//스크립트가 처음 실행 될때
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theCrossHair = FindObjectOfType<Crosshair>();

        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update() 
    {
        MoveCheck();
        CharacterRotation();
        CameraRotation();
        //move
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * applySpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * applySpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * applySpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * applySpeed * Time.deltaTime);
        }//this 뺴기
         //jump

        if (Input.GetKey(KeyCode.Space))
        {
            print("space 눌러짐");
            myRigid.velocity = transform.up * jumpForce;
        }

        //crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //Crouch = !Crouch;
            theCrossHair.CrouchingAnimation(run);

            if (Crouch)
            {
                applySpeed = crouchSpeed;
                applyCrouchPosY = crouchPosY;
            }
            else
            {
                applySpeed = walkSpeed;
                applyCrouchPosY = originPosY;
            }
        }
        //run
        //명확하게 어떤 작업을 할것인지
        Ground = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            run = true;
            applySpeed = runSpeed;
            theCrossHair.RunningAnimation(run);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
            theCrossHair.RunningAnimation(run);

            applySpeed = walkSpeed;
        }
    }

    private void MoveCheck()
    {
        if(!run && !Crouch )
        {
            if (Vector3.Distance(lastPos,transform.position)>=0.01f) //전프레임과 현제 프레임 위치 값이 0.01보다 작으면 안겆는것
                isWalk = true;
            else
                isWalk = false;

            theCrossHair.WalkingAnimation(isWalk);

            lastPos = transform.position;
        }
        
    }
    private void CharacterRotation() 
    {
        // 좌우 캐릭터 회전
        print("좌우");
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity*5;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        Debug.Log(myRigid.rotation);
        Debug.Log(myRigid.rotation.eulerAngles);
    }

    private void CameraRotation()
        
    {
        print("상하");
        // 상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

}

