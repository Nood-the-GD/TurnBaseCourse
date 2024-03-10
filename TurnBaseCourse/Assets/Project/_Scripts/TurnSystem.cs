using System;
using NOOD;
using UnityEngine;

namespace Game
{
    public class TurnSystem : MonoBehaviorInstance<TurnSystem>
    {
        public event EventHandler OnTurnChange;
        private int _turnNumber = 1;
        private bool _isPlayerTurn = true;

        #region Unit functions
        void Start()
        {
        }
        #endregion

        public void NextTurn()
        {
            _turnNumber++;
            _isPlayerTurn = !_isPlayerTurn;
            OnTurnChange?.Invoke(this, EventArgs.Empty);
        }

        public int GetTurnNumber()
        {
            return _turnNumber;
        }

        public bool IsPlayerTurn()
        {
            return _isPlayerTurn;
        }
    }
}
