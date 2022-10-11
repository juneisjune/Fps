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
    private bool run = false;
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
    public float rotationSpeed = 360f;


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
    void Update() 
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {   /*
            if (isCrouch) //�������� ���� ����
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
        float _cameraRotationX = _xRotation * lookSensitivity; //���� ��ġ���� �ΰ�����ŭ �����ֱ�
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//ī�޶��� �ִ밪�� �ּҰ� ����
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);







    }

}
