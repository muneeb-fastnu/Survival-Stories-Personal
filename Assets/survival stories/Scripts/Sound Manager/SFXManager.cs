using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource extraAudioSource;
    public AudioClip[] xxxx;

    public AudioClip weaponHit;
    public AudioClip weaponKill;
    public AudioClip weaponKB;

    public AudioClip[] xxxxx;

    public AudioClip bareHit;
    public AudioClip bareKill;
    public AudioClip bareKB;

    public AudioClip[] xxxxxx;

    public AudioClip click;
    public AudioClip slide;

    public AudioClip woodChop;
    public AudioClip stoneMining;
    public AudioClip LevelUp;

    public AudioClip walkGrass;

    public static SFXManager instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    private void Start()
    {
        if (audioSource == null)
        {
            // If AudioSource is not assigned, try to get it from the GameObject
            audioSource = GetComponent<AudioSource>();

            // If AudioSource is still null, add a new one
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    

    public void PlayWeaponHit()
    {
        audioSource.clip = weaponHit;
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void PlayWeaponKill()
    {
        audioSource.clip = weaponKill;
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void PlayWeaponKB()
    {
        audioSource.clip = weaponKB;
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void PlayBareHit()
    {
        audioSource.clip = bareHit;
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void PlayBareKill()
    {
        audioSource.clip = bareKill;
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void PlayBareKB()
    {
        audioSource.clip = bareKB;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlayAttackNoWeapon()
    {

    }
    public void PlayKBWeapon()
    {

    }
    public void PlayKBNoWeapon()
    {

    }
    public void PlaClick()
    {

    }
    public void PlayLevelUp()
    {
        extraAudioSource.clip = LevelUp;
        extraAudioSource.PlayOneShot(extraAudioSource.clip);
    }
    public void PlaySlide()
    {
        audioSource.clip = slide;
        audioSource.PlayOneShot(audioSource.clip);
    }

    private Coroutine woodChopCoroutine;
    public void PlayWoodChop()
    {
        audioSource.clip = woodChop;
        audioSource.PlayOneShot(audioSource.clip);
    }
    private IEnumerator PlayWoodChopPeriodically()
    {
        while (true)
        {
            // Play the wood chop sound
            PlayWoodChop();

            // Wait for 2 seconds before playing the sound again
            yield return new WaitForSeconds(0.8f);
        }
    }
    public void StartWoodChopCoroutine()
    {
        // Check if the coroutine is already running before starting a new one
        if (woodChopCoroutine == null)
        {
            woodChopCoroutine = StartCoroutine(PlayWoodChopPeriodically());
        }
    }
    public void StopWoodChopCoroutine()
    {
        if (woodChopCoroutine != null)
        {
            StopCoroutine(woodChopCoroutine);
            woodChopCoroutine = null;
            audioSource.Stop();
        }
    }
    //--------------
    private Coroutine stoneMiningCoroutine;
    public void PlayStoneMining()
    {
        audioSource.clip = stoneMining;
        audioSource.PlayOneShot(audioSource.clip);
    }
    private IEnumerator PlayStoneMiningPeriodically()
    {
        while (true)
        {
            // Play the wood chop sound
            PlayStoneMining();

            // Wait for 2 seconds before playing the sound again
            yield return new WaitForSeconds(0.8f);
        }
    }
    public void StartStoneMiningCoroutine()
    {
        // Check if the coroutine is already running before starting a new one
        if (stoneMiningCoroutine == null)
        {
            stoneMiningCoroutine = StartCoroutine(PlayStoneMiningPeriodically());
        }
    }
    public void StopStoneMiningCoroutine()
    {
        if (stoneMiningCoroutine != null)
        {
            StopCoroutine(stoneMiningCoroutine);
            stoneMiningCoroutine = null;
            audioSource.Stop();
        }
    }
    //--------------
    private Coroutine walkGrassCoroutine;
    public void PlayWalkGrass()
    {
        audioSource.clip = walkGrass;
        audioSource.Play();
    }
    private IEnumerator PlayWalkGrassPeriodically()
    {
        while (true)
        {
            // Play the wood chop sound
            audioSource.volume = 1f;
            PlayWalkGrass();
            

            // Wait for 2 seconds before playing the sound again
            yield return new WaitForSeconds(0.8f);
            //audioSource.volume = 0.4f;
        }
    }
    public void StartWalkGrassCoroutine()
    {
        // Check if the coroutine is already running before starting a new one
        if (walkGrassCoroutine == null)
        {
            walkGrassCoroutine = StartCoroutine(PlayWalkGrassPeriodically());
        }
    }
    public void StopWalkGrassCoroutine()
    {
        if (walkGrassCoroutine != null)
        {
            StopCoroutine(walkGrassCoroutine);
            walkGrassCoroutine = null;
            audioSource.volume = 0.4f;
            audioSource.Pause();
        }
    }
}
