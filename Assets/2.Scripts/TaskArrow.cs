using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TaskArrow : MonoBehaviour
{
    [SerializeField] private Color _baseColor;
    private Image _image = null;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetBaseColor()
    {
        StopAllCoroutines();
        _image.CrossFadeAlpha(1.0f, 0.0f, true);
    }

    public void StartBlink()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        _image.CrossFadeAlpha(0.0f, 1.0f, true);
        yield return new WaitForSeconds(1f);
        _image.CrossFadeAlpha(1.0f, 1.0f, true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(Blink());
    }
}
