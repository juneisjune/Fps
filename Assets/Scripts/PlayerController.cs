using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] //private으로 해도 수정 가능함
    private float walkSpeed;

    
    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()//스크립트가 처음 실행 될때
    {
        myRigid = GetComponet<Rigidbody>();
    }

    // Update is called once per frame
    void Update()//1초에 60번 정도실행됨
    {
       
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX; 
    }
}
