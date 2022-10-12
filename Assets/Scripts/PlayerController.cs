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
    private bool Ground = true;
    private bool Crouch=false;

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
    void Update() 
    {
        //move
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * applySpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * applySpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * applySpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * applySpeed * Time.deltaTime);
        }//this ����
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && Ground)
        {
            myRigid.velocity = transform.up * jumpForce;
        }
        //run
        //��Ȯ�ϰ� � �۾��� �Ұ�����
        Ground = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        print(Ground);
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
            if(Crouch)
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
        //
        
       
      
/*        //
        float yRotate = Input.GetAxisRaw("Mouse X");
        float cameraRotateY = yRotate * lookSensitivity;
        Vector3 charRotateY = new Vector3(0f, _yRotate, 0f);

        float xRotation = Input.GetAxisRaw("Mouse Y");*/

    }

}
