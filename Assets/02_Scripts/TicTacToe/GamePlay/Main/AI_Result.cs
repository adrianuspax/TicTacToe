using System;
using UnityEngine;

namespace TicTacToe.GamePlay.Main
{
    /// <inheritdoc/>
    public partial class AI
    {
        [Serializable]
        public struct Result
        {
            public Main.Result main;
            [NonReorderable] public int[] indexes;

            public Result(Main.Result result = Main.Result.none, params int[] indexes)
            {
                main = result;
                this.indexes = indexes;
            }
        }
    }
}
