using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayClip : MonoBehaviour
{
    public AudioClip MusicClip;

    AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = MusicClip;
        musicSource.Play();
    }

}
