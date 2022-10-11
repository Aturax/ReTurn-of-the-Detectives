using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private GameSM stateMachine = null;

    [Space]
    [SerializeField] Fader fader = null;

    void Start()
    {
        stateMachine.LoadStateMachine(fader);
        stateMachine.StartStateMachine();
    }
}
