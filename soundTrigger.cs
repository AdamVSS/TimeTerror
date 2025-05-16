using UnityEngine;
using System.Collections;

public class soundTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 2f;
    private bool hasPlayed = false;
    private Coroutine currentFade;

    void OnTriggerEnter(Collider other)
    {
        if(!hasPlayed && other.CompareTag("Player")){
            audioSource.Play();
            hasPlayed = true;
        }
    }
}
