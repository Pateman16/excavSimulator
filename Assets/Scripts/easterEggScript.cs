using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class easterEggScript : MonoBehaviour {
    AudioSource music;

    void Start()
    {
        music = GetComponent<AudioSource>();
    }
    void OnTriggerExit(Collider other)
    {
        music.Play();

    }
}
