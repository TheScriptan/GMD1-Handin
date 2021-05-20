using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> _availableStates;
    
    public BaseState CurrentState { get; private set; }

    public void SetStates(Dictionary<Type, BaseState> states)
    {
        _availableStates = states;
    }

    private void Update()
    {
        if (CurrentState == null)
        {
            CurrentState = _availableStates.Values.First();
        }
        
        var nextState = CurrentState.Tick();

        if (nextState != null && nextState != CurrentState?.GetType())
        {
            SwitchToNextState(nextState);
        }
    }

    private void SwitchToNextState(Type nextState)
    {
        CurrentState?.OnStateExit();
        CurrentState = _availableStates[nextState];
        CurrentState?.OnStateEnter();
    } 
}
