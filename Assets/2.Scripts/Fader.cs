using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour
{
    private AudioSource audioSource = null;
    private Image imageFader = null;    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        imageFader = GetComponent<Image>();
        
        imageFader.color = Color.black;
    }

    public async Task Fade(float alpha, float time)
    {
        int wait = (int)time * 1000;
        imageFader.CrossFadeAlpha(alpha, time, false);

        await Task.Delay(wait);

        imageFader.raycastTarget = (alpha == 1.0f);
         
    }

    public async Task Fade(float alpha, float time, AudioClip clip)
    {
        int wait = (int)time;
        if (clip.length > wait) wait = (int)clip.length;
        wait *= 1000;
        
        imageFader.CrossFadeAlpha(alpha, time, false);
        audioSource.PlayOneShot(clip);
        await Task.Delay(wait);
        imageFader.raycastTarget = (alpha == 1.0f);
    }
}
