using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool Placed=false;
    public Light HelpLight;
    AudioSource Audio;

    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
    }
    public void StartSound()
    {
        Audio.Play();
    }
}
