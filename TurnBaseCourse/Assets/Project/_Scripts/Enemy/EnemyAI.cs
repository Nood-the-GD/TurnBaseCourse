using System;
using UnityEngine;

namespace Game
{
    public class EnemyAI : MonoBehaviour
    {
        private float _timer;


        private void Start()
        {
            TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        }

        void Update()
        {
            if (TurnSystem.Instance.IsPlayerTurn())
                return;

            _timer -= Time.deltaTime;
            if(_timer <= 0)
            {
                TurnSystem.Instance.NextTurn();
            }
        }

        private void TurnSystem_OnTurnChange(object sender, EventArgs eventArgs)
        {
            _timer = 2f;
        }
    }
}
