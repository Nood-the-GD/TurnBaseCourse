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

        #region Unity functions
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
                        if(TryTakeEnemyAIAction(SetStateTakingTurn))
                        {
                            _state = State.Busy;
                        }
                        else
                        {
                            // No more enemy have action can take
                            TurnSystem.Instance.NextTurn();
                        }
                    }
                    break;
                case State.Busy:
                    break;
            }
        }
        #endregion

        #region Event functions
        private void TurnSystem_OnTurnChange(object sender, EventArgs eventArgs)
        {
            if(!TurnSystem.Instance.IsPlayerTurn())
            {
                _state = State.TakingTurn;
                _timer = 2f;
            }
        }
        #endregion

        #region Get set
        private void SetStateTakingTurn()
        {
            _timer = 0.5f;
            _state = State.TakingTurn;
        }
        #endregion
        
        #region Take action
        private bool TryTakeEnemyAIAction(Action onComplete)
        {
            foreach(Unit enemy in UnitManager.Instance.GetEnemyUnitList())
            {
                if(TryTakeEnemyAIAction(enemy, onComplete))
                {
                    return true;
                }
            }
            return false;
        }
        private bool TryTakeEnemyAIAction(Unit enemy, Action onComplete)
        {
            EnemyAIAction bestEnemyAIAction = null;
            BaseAction bestBaseAction = null;
            foreach(BaseAction baseAction in enemy.GetBaseActionArray())
            {
                if(!enemy.CanSpendActionPointToTakeAction(baseAction))
                {
                    continue;
                }

                if(bestEnemyAIAction == null)
                {
                    bestEnemyAIAction = baseAction.GetBestAIAction();
                    bestBaseAction = baseAction;
                }    
                else
                {
                    EnemyAIAction testEnemyAIAction = baseAction.GetBestAIAction();
                    if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                    {
                        bestEnemyAIAction = testEnemyAIAction;
                        bestBaseAction = baseAction;
                    }
                }
            }

            if(bestEnemyAIAction != null && enemy.TrySpendActionPointsToTakeAction(bestBaseAction))
            {
                bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onComplete);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
















