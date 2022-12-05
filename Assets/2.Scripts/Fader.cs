using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Fader
{
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private Image _imageFader = null;

    public void LoadFader()
    {
        _imageFader.color = Color.black;
    }    

    public async Task Fade(float alpha, float time)
    {
        _imageFader.CrossFadeAlpha(alpha, time, false);
        
        int wait = Mathf.RoundToInt(time * 1000);
        
        await Task.Delay(wait);
        _imageFader.raycastTarget = (alpha == 1.0f);         
    }

    public async Task Fade(float alpha, float time, AudioClip clip)
    {
        _imageFader.CrossFadeAlpha(alpha, time, false);
        _audioSource.PlayOneShot(clip);

        int wait = clip.length > time ? Mathf.RoundToInt(clip.length * 1000) : Mathf.RoundToInt(time * 1000);
        
        await Task.Delay(wait);
        _imageFader.raycastTarget = (alpha == 1.0f);
    }
}
