using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] //private���� �ص� ���� ������
    private float walkSpeed;

    
    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()//��ũ��Ʈ�� ó�� ���� �ɶ�
    {
        myRigid = GetComponet<Rigidbody>();
    }

    // Update is called once per frame
    void Update()//1�ʿ� 60�� ���������
    {
       
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX; 
    }
}
