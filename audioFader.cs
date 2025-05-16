using UnityEngine;
using System.Collections;

public class audioFader : MonoBehaviour
{
    public AudioSource audioSource;
    public float duration = 2f;

    public void fadeIn(){
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeAudio(0f,1f));
    }

    public void fadeOut(){
        StartCoroutine(FadeAudio(audioSource.volume, 0f, stopAfter: true));
    }

    private IEnumerator FadeAudio(float startVolume, float targetVolume, bool stopAfter = false)
    {
        float currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (stopAfter && targetVolume == 0f)
        {
            audioSource.Stop();
        }
    }
}
