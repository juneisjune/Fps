using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //���ǵ� ���� ����
    [SerializeField] //private���� �ص� ���� ������
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;
    [SerializeField]
    private float jumpForce;

    //���� ����
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch=false;

    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider;

    //�ΰ���
    [SerializeField]
    private float lookSensitivity;


    //ī�޶� ���� ���� 
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX=0f;

    //��Ÿ ������Ʈ
    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()//��ũ��Ʈ�� ó�� ���� �ɶ�
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update() //1�ʿ� 60�� ���������
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

        //transform.position(������ġ)���� velocity(�ӵ�)��ŭ ������ �̷��� �����̵������� deltaTime���� ������ Time.deltaTime=0.016
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); 
    }
    private void CharacterRotation() //�¿� ���콺
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
    private void CameraRotation() //���� ���콺
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity; //���� ��ġ���� �ΰ�����ŭ �����ֱ�
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//ī�޶��� �ִ밪�� �ּҰ� ����

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);    
    }
}
