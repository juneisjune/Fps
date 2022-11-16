using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //현재 장착된 총
    [SerializeField]
    private Gun currentGun;
    //연사속도 계싼
    private float currentFireRate;

    private bool isReload = false;
    public bool isFineSightMode = false;

    // 본래 포지션 값.
    [SerializeField]
    private Vector3 originPos;
    //효과음
    private AudioSource audioSource;

    //충돌 정보 받아오기
    private RaycastHit hitInfo;

    [SerializeField]
    private Camera theCam;

    [SerializeField]
    private GameObject hitEffectPrefab;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //왼쪽 마우스
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            if (!isReload)
            {
                if (currentGun.currentBulletCount > 0)
                {
                    currentGun.currentBulletCount--;
                    currentFireRate = currentGun.fireRate; // 연사 속도 재계산.
                    PlaySE(currentGun.fire_Sound);
                    currentGun.muzzleFlash.Play();
                    Hit();
                    StopAllCoroutines();


                    print("총알 발사함");
                }
                else
                {
                    CancelFineSight();
                    StartCoroutine(ReloadCoroutine());
                }
            }
        }
       //오른쪽 마우스 정조준
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            isFineSightMode = !isFineSightMode;


            if (isFineSightMode)
            {
                StopAllCoroutines();
                StartCoroutine(FineSightActivateCoroutine());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(FineSightDeactivateCoroutine());
            }
        }
       //r 키 reload bullet
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
        GunFireRateCalc();
    }
    private void Hit() // 상하에 영향 없음
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo, currentGun.range))
        {
            GameObject particle = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(particle, 0.2f);
        }
    }
    //용어 선정 명확성 필요
    //
    //소리 재생
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            isFineSightMode = !isFineSightMode;

            if (isFineSightMode)
            {
                StopAllCoroutines();
                StartCoroutine(FineSightActivateCoroutine());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(FineSightDeactivateCoroutine());
            }
        }

    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

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


            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }

  
    //합쳐보기
    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }
    
    public Gun GetGun()
    {
        return currentGun;
    }
}
