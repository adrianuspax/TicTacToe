using System;
using UnityEngine;

namespace TicTacToe.GamePlay.Block
{
    /// <summary>
    /// Block Data Struct
    /// </summary>
    [Serializable]
    public struct Data
    {
        [SerializeField] private int index; // The index of the block on the board.
        [SerializeField] private Input input; //The input value (e.g., X or O) of the block.
        /// <summary>
        /// Block Data Constructor
        /// </summary>
        /// <param name="index">Index of block from Tic Tac Toe</param>
        /// <param name="input">Current Player Inputed in the block</param>
        public Data(int index = -1, Input input = Input.blank)
        {
            if (index < 0 || index > 8)
                index = -1;

            this.index = index;
            this.input = input;
        }
        /// <summary>
        /// Return the index of the block
        /// </summary>
        public int Index
        {
            readonly get => index; set => index = value;
        }
        /// <summary>
        /// Return the player input of the block
        /// </summary>
        public Input Input
        {
            readonly get => input; set => input = value;
        }
        /// <summary>
        /// return if the block is already inputted
        /// </summary>
        public readonly bool IsInputted => ((int)input) > 0;
    }
}
