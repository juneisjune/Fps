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
    private bool Ground = true;
    private bool Crouch=false;

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
        if (Input.GetKey(KeyCode.Space) && Ground)
        {
            myRigid.velocity = transform.up * jumpForce;
        }
        //run
        //명확하게 어떤 작업을 할것인지
        Ground = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            run = true;
            applySpeed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
            applySpeed = walkSpeed;
        }
        //crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch = !Crouch;
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



        float _xRotation = Input.GetAxisRaw("Mouse Y"); //GetAxisRaw 는 -1 0 1 세가지 값중 하나가 반환된다. 키보드 값을 눌렀을 때 즉시 반응
                                                        //GetAis 는 -1.0f부터 1.0f 까지의 범위가 반환한다. 부드러운 이동이 필요한 경우에 사용한다.
        float _cameraRotationX = _xRotation * lookSensitivity; //현재 위치에서 민감도만큼 곱해주기
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//카메라의 최대값과 최소값 유지
                                //Mathf는 하나의 클래스로 게임 및 앱 개발에 일반적으로 필요한삼각 함수 , 로그 함수, 기타 함수를 비롯한 일반적인 수학 함수 컬렉션을 제공합니다.
                                //Clamp는 최소 최대값을 설정하여 Float값이 범위이외의 값을 넘지 않도록 도와 준다.
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        //transform은 해당 컴포넌트가 붙은 게임 오프젝트의 Tranform 컴포넌트를 참조
        //loclaEulerAngles는 Transform 계층 구조에 의한 다른  Tranform의 자식으로 존재할 떄 부모의 상대적인 회전량 Degree값을 Vector3로 접근하는 프로퍼터

        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
                                        //

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

    }

}
