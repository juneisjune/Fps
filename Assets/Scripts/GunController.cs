using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    private Gun currentGun;
    private float currentFireRate;

    public ParticleSystem particle;
    private AudioSource audioSource;

    // Update is called once per frame
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            currentGun.particle.Play();
            Play(currentGun.fire_Sound);
           
            print("ÃÑ¾Ë ¹ß»ç");
        }

    }
    private void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
