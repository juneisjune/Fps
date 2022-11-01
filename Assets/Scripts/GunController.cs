using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; //60초에 1씩 감소 
        }
    }

    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate<=0)
        {
            Fire();
        }
    }

    private void Fire()
    {

        if (currentGun.currentBulletCount > 0)
        {
            Shoot();
        }
        else
            Reload();
    }

    private void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;// 연사 속도 재계산
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        print("총발싸");

    }
    private void Reload()
    {
        if(currentGun.carryBulletCount>0)
        {
            /*currentGun.ani..SetTrigger("Reload");*/
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
        }
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
