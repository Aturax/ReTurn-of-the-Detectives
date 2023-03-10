using System.Threading.Tasks;
using UnityEngine;

public class GameManager : StateMachine
{
    [field:SerializeField] public MainMenuState MainMenuState { get; private set; } = null;
    [field: SerializeField] public CityState CityState { get; private set; } = null;
    [field: SerializeField] public LocationState LocationState { get; private set; } = null;
    [field: SerializeField] public GameOverState GameOverState { get; private set; } = null;
    
    [SerializeField] private Fader _fader = null;
    [SerializeField] private AudioSource _audioSource = null;

    protected override State GetInitialState()
    {
        return MainMenuState;
    }

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
}