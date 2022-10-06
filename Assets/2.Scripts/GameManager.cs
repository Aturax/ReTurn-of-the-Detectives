using UnityEngine;

public class GameManager : MonoBehaviour
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
