using System;
using System.Threading;
using UnityEngine;

namespace Game
{
    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            WaitingForEnemyTurn,
            TakingTurn,
            Busy,
        }


        private State _state;
        private float _timer;


        private void Awake()
        {
            _state = State.WaitingForEnemyTurn;
        }
        private void Start()
        {
            TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        }

        void Update()
        {
            if (TurnSystem.Instance.IsPlayerTurn())
                return;

            switch (_state)
            {
                case State.WaitingForEnemyTurn:
                    break;
                case State.TakingTurn:
                    _timer -= Time.deltaTime;
                    if(_timer <= 0)
                    {
                        _state = State.Busy;
                        TakeEnemyAIAction(SetStateTakingTurn);
                        TurnSystem.Instance.NextTurn();
                    }
                    break;
                case State.Busy:
                    break;
            }
        }

        private void TurnSystem_OnTurnChange(object sender, EventArgs eventArgs)
        {
            if(!TurnSystem.Instance.IsPlayerTurn())
            {
                _state = State.TakingTurn;
                _timer = 2f;
            }
        }

        private void SetStateTakingTurn()
        {
            _timer = 0.5f;
            _state = State.TakingTurn;
        }
        private void TakeEnemyAIAction(Action onComplete)
        {
            
        }
    }
}
















