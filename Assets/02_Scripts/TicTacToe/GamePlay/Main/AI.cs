using System;
using UnityEngine;

namespace TicTacToe.GamePlay.Main
{
    [Serializable]
    public class AI
    {
        public AI(Block.Input player)
        {
            if (player == Block.Input.blank)
            {
                Debug.LogError("AI player cannot be blank!");
                return;
            }

            human = player;

            ai = player switch
            {
                Block.Input.x => Block.Input.o,
                Block.Input.o => Block.Input.x,
                _ => Block.Input.o,
            };
        }

        [SerializeField] private Block.Input human;
        [SerializeField] private Block.Input ai;
        [SerializeField] private Result result;

        private readonly int[,] winConditions = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Lines
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
            {0, 4, 8}, {2, 4, 6}             // Diags
        };

        public int GetBestMove(Block.Data[] board)
        {
            var bestScore = int.MinValue;
            var moveIndex = -1;

            for (var i = 0; i < board.Length; i++)
            {
                if (board[i].Input == Block.Input.blank)
                {
                    board[i].Input = ai;
                    var score = Minimax(board, 0, false);
                    board[i].Input = Block.Input.blank;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        moveIndex = i;
                    }
                }
            }

            return moveIndex;
        }


        private int Minimax(Block.Data[] board, int depth, bool isMaximizing)
        {
            var score = Evaluate(board);

            if (score == 10)
                return score - depth;

            if (score == -10)
                return score + depth;

            var isMovesLeft = IsMovesLeft(board);
            if (!isMovesLeft)
                return 0;

            if (isMaximizing)
            {
                var best = int.MinValue;

                for (var i = 0; i < board.Length; i++)
                {
                    if (board[i].Input == Block.Input.blank)
                    {
                        board[i].Input = ai;
                        var b = Minimax(board, depth + 1, false);
                        best = Mathf.Max(best, b);
                        board[i].Input = Block.Input.blank;
                    }
                }

                return best;
            }
            else
            {
                var best = int.MaxValue;

                for (var i = 0; i < board.Length; i++)
                {
                    if (board[i].Input == Block.Input.blank)
                    {
                        board[i].Input = human;
                        var b = Minimax(board, depth + 1, true);
                        best = Mathf.Min(best, b);
                        board[i].Input = Block.Input.blank;
                    }
                }

                return best;
            }
        }

        private int Evaluate(Block.Data[] board)
        {
            for (var i = 0; i < winConditions.GetLength(0); i++)
            {
                var a = winConditions[i, 0];
                var b = winConditions[i, 1];
                var c = winConditions[i, 2];

                if (board[a].Input == board[b].Input && board[b].Input == board[c].Input)
                {
                    if (board[a].Input == ai)
                        return 10;

                    if (board[a].Input == human)
                        return -10;
                }
            }

            return 0;
        }

        private bool IsMovesLeft(Block.Data[] board)
        {
            for (var i = 0; i < board.Length; i++)
                if (board[i].Input == Block.Input.blank)
                    return true;

            return false;
        }
    }
}