using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;




    // 총알 개수 텍스트에 반영
    [SerializeField]
    private Text text_Bullet1;
    [SerializeField]
    private Text text_Bullet2;
    [SerializeField]
    private Text text_Bullet3;

    // Update is called once per frame
    void Update()
    {
        currentGun = theGunController.GetGun();
        text_Bullet1.text = currentGun.carryBulletCount.ToString();
        text_Bullet2.text = currentGun.reloadBulletCount.ToString();
        text_Bullet3.text = currentGun.currentBulletCount.ToString();
    }

    

}
