using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {  get; private set; }
    [SerializeField] private AudioClip bounceClip;
    [SerializeField] private AudioClip clickClip;

    [SerializeField] private AudioSource effectsSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    public void PlayBounce()
    {
        effectsSource.PlayOneShot(bounceClip);
    }
    public void PlayClick() 
    {
        effectsSource.PlayOneShot(clickClip);
    }
}
