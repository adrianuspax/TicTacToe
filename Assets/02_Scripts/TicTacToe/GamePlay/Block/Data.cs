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
        [SerializeField] private int index;
        [SerializeField] private Input input;
        /// <summary>
        /// Block Data Constructor
        /// </summary>
        /// <param name="index">Index of block from Tic Tac Toe</param>
        /// <param name="input">Current Player Inputed in the block</param>
        public Data(int index, Input input)
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
