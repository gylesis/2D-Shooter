using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Bot
{
    public class StateMachine
    {
        private IState _currentState;

        private readonly Dictionary<BotState, IState> _states;

        private bool _allowToChangeState = true;

        public Action<BotState> StateChanged { get; set; }
        
        public StateMachine(params IState[] states)
        {
            _states = states.ToDictionary(x => x.State);
        }

        public async void ChangeState(BotState state, float waitTime = 0)
        {
            if(_currentState?.State == state) return;

            while (_allowToChangeState == false) 
                await Task.Yield();

            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            
            _currentState?.Exit();

           // Debug.Log($"Changed state {state}");
           
            StateChanged?.Invoke(state);
            _currentState = _states[state];
            _currentState.Enter();
        }

        public async void LockChangingState(float lockTime)
        {
            _allowToChangeState = false;
            await Task.Delay(TimeSpan.FromSeconds(lockTime));
            _allowToChangeState = true;
        }

        public void Update()
        {
            _currentState?.Update();
        }
       
    }
}