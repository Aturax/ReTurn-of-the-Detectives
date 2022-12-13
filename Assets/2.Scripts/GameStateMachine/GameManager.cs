using System.Threading.Tasks;
using UnityEngine;

public class GameManager : StateMachine
{
    public MainMenuState MainMenuState = null;
    public CityState CityState = null;
    public LocationState LocationState = null;
    public GameOverState GameOverState = null;
    
    [SerializeField] private Fader _fader = null;
    [SerializeField] private AudioSource _audioSource = null;

    private void Start()
    {
        LoadStateMachine();
        StartStateMachine();
    }

    public void LoadStateMachine()
    {
        MainMenuState.SetStateMachine(this);
        CityState.SetStateMachine(this);
        LocationState.SetStateMachine(this);
        GameOverState.SetStateMachine(this);        
        _fader.LoadFader();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public async Task Fade(float alpha, float time)
    {
        await _fader.Fade(alpha, time);
    }

    public async Task Fade(float alpha, float time, AudioClip clip)
    {
        await _fader.Fade(alpha, time, clip);
    }

    protected override State GetInitialState()
    {
        return MainMenuState;
    }
}