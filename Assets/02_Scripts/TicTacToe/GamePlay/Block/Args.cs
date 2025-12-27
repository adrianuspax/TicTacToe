using System;
using UnityEngine;

namespace TicTacToe.GamePlay.Block
{
    /// <summary>
    /// Arguments for Play Handler
    /// </summary>
    [Serializable]
    public class Args : EventArgs
    {
        [SerializeField] private Data data; // The data associated with the block involved in the event.
        /// <summary>
        /// Arguments Constructor
        /// </summary>
        /// <param name="data"></param>
        public Args(Data data)
        {
            this.data = data;
        }
        /// <summary>
        /// Return the data of the block
        /// </summary>
        public Data Data => data;
    }
}
