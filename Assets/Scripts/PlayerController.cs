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
    private bool isRun = false;
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
    void Update() //1초에 60번 정도실행됨
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        /*      if(isCrouch)
                  isCrouch = false;
              else
                  isCrouch = true;*/
        isCrouch = !isCrouch;

        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY=crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY=originPosY;
        }
        theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x,applyCrouchPosY,theCamera.transform.localPosition.z);
    }
 /*   IEnumerator CrouchCoroutine()
    {
        float _posY=theCamera.transform.localPosition.y;
        while(_posY!=applyCrouchPosY)
        {
            _posY = Mathf.Lerp();
        }
    }*/
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+0.1f);
    }
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space)&& isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift)) //when player pressing leftshift return true
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)) 
        {
            RunningCancel();
        }
    }
    private void Running()
    {
         isRun = true;
         applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRun=false;
        applySpeed=walkSpeed;
    }
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        //transform.position(현재위치)에서 velocity(속도)만큼 더해줌 이러면 순간이동함으로 deltaTime으로 나눠줌 Time.deltaTime=0.016
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); 
    }
    private void CharacterRotation() //좌우 마우스
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
    private void CameraRotation() //상하 마우스
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity; //현재 위치에서 민감도만큼 곱해주기
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//카메라의 최대값과 최소값 유지

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);    
    }
}
