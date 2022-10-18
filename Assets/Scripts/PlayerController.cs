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



        float _xRotation = Input.GetAxisRaw("Mouse Y"); //GetAxisRaw �� -1 0 1 ������ ���� �ϳ��� ��ȯ�ȴ�. Ű���� ���� ������ �� ��� ����
                                                        //GetAis �� -1.0f���� 1.0f ������ ������ ��ȯ�Ѵ�. �ε巯�� �̵��� �ʿ��� ��쿡 ����Ѵ�.
        float _cameraRotationX = _xRotation * lookSensitivity; //���� ��ġ���� �ΰ�����ŭ �����ֱ�
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//ī�޶��� �ִ밪�� �ּҰ� ����
                                //Mathf�� �ϳ��� Ŭ������ ���� �� �� ���߿� �Ϲ������� �ʿ��ѻﰢ �Լ� , �α� �Լ�, ��Ÿ �Լ��� ����� �Ϲ����� ���� �Լ� �÷����� �����մϴ�.
                                //Clamp�� �ּ� �ִ밪�� �����Ͽ� Float���� �����̿��� ���� ���� �ʵ��� ���� �ش�.
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        //transform�� �ش� ������Ʈ�� ���� ���� ������Ʈ�� Tranform ������Ʈ�� ����
        //loclaEulerAngles�� Transform ���� ������ ���� �ٸ�  Tranform�� �ڽ����� ������ �� �θ��� ������� ȸ���� Degree���� Vector3�� �����ϴ� ��������

        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
                                        //

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

    }

}
