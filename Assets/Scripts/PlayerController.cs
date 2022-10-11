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
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {   /*
            if (isCrouch) //�������� ���� ����
            {
                Crouch();
            }*/
            myRigid.velocity = transform.up * jumpForce;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            isRun= true;
            applySpeed = runSpeed;
        }
    }

}
