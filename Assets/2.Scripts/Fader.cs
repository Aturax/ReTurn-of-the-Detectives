using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Fader
{
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private Image imageFader = null;

    public void LoadFader()
    {
        imageFader.color = Color.black;
    }    

    public async Task Fade(float alpha, float time)
    {
        imageFader.CrossFadeAlpha(alpha, time, false);
        
        int wait = Mathf.RoundToInt(time * 1000);
        
        await Task.Delay(wait);
        imageFader.raycastTarget = (alpha == 1.0f);         
    }

    public async Task Fade(float alpha, float time, AudioClip clip)
    {
        imageFader.CrossFadeAlpha(alpha, time, false);
        audioSource.PlayOneShot(clip);

        int wait = clip.length > time ? Mathf.RoundToInt(clip.length * 1000) : Mathf.RoundToInt(time * 1000);
        
        await Task.Delay(wait);
        imageFader.raycastTarget = (alpha == 1.0f);
    }
}
