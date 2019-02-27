using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayClip : MonoBehaviour
{
    public AudioClip MusicClip;

    AudioSource musicSource;

    bool StartToPlay = true; // This can be used in the future to stop the music

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = MusicClip;
        musicSource.Play();
    }

}
