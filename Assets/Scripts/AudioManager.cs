using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip coin;
    public AudioClip candy;
    public AudioClip carryUp;
    public AudioClip ghost;
    public AudioClip pumper;

    protected AudioSource m_as;

    private void Awake()
    {
        if (!Instance) {
            Instance = this;
        }

        m_as = GetComponent<AudioSource>();
    }

    public void PlayCoin()
    {
        m_as.PlayOneShot(coin);
    }

    public void PlayCandy()
    {
        m_as.PlayOneShot(candy);
    }

    public void PlayCarryUp()
    {
        m_as.PlayOneShot(carryUp);
    }

    public void PlayGhost()
    {
        m_as.PlayOneShot(ghost);
    }

    public void PlayPumper()
    {
        m_as.PlayOneShot(pumper);
    }

    public void PlayCandyWithDelay(float time)
    {
        StartCoroutine(OnPlayCandy(time));
    }

    IEnumerator OnPlayCandy(float time)
    {
        yield return new WaitForSeconds(time);
        PlayCandy();
    }
}
