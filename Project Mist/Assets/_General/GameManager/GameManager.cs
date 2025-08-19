using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachineManager))]
public class GameManager : MonoBehaviour
{
    StateMachineManager stateMachine;
    GameState currGameState;
    
    public static Action OnLevelReset;
    public static Action OnLevelProgressed;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachineManager>();
        currGameState = (GameState)(stateMachine.currentState);
    }

    private void OnEnable()
    {
        OnLevelReset += ResetCurrentLevel;
        OnLevelProgressed += ProgressNextLevel;
    }

    private void OnDisable()
    {
        OnLevelReset -= ResetCurrentLevel;
        OnLevelProgressed -= ProgressNextLevel;
    }

    private void ResetCurrentLevel()
    {
        currGameState.ResetLevel();
    }

    private void ProgressNextLevel()
    {
        currGameState.ProgressToNextLevel();
    }
}
