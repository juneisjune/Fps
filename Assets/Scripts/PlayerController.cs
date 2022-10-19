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
    private float rotationLimit;
    private float currentCameraRotationX = 0f;
    

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
        }//this ����
        //jump
        if (Input.GetKey(KeyCode.Space) && Ground)
        {
            myRigid.velocity = transform.up * jumpForce;
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
        //run
        //��Ȯ�ϰ� � �۾��� �Ұ�����
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
        


        //����
        float yRotation = Input.GetAxisRaw("Mouse Y"); 
        float cameraRotationX = yRotation * lookSensitivity; //���� ��ġ���� �ΰ�����ŭ �����ֱ�
        //currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -rotationLimit, rotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        
        //�¿�
        float xRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationX = new Vector3(0f, xRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationX));

    }

}
