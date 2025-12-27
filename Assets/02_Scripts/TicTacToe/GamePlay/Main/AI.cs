using System;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe.GamePlay.Main
{
    using Random = UnityEngine.Random;

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

        private readonly int[,] winConditions = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Lines
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
            {0, 4, 8}, {2, 4, 6}             // Diags
        };

        public Result CheckForWinner(Block.Data[] board)
        {
            var length = winConditions.GetLength(0);

            for (var i = 0; i < length; i++)
            {
                var a = winConditions[i, 0];
                var b = winConditions[i, 1];
                var c = winConditions[i, 2];

                if (board[a].Input != Block.Input.blank && board[a].Input == board[b].Input && board[b].Input == board[c].Input)
                    if (board[a].Input == ai)
                        return Result.youLose;
                    else
                        return Result.youWin;
            }

            var isMovesLeft = IsMovesLeft(board);
            if (isMovesLeft)
                return Result.none;
            else
                return Result.draw;
        }

        public int GetBestMove(Block.Data[] board)
        {
            var bestScore = int.MinValue;
            var bestMoves = new List<int>();

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
                        bestMoves.Clear();
                        bestMoves.Add(i);
                    }
                    else if (score == bestScore)
                    {
                        bestMoves.Add(i);
                    }
                }
            }

            var move = -1;
            if (bestMoves.Count > 0)
            {
                var randomIndex = Random.Range(0, bestMoves.Count); //new System.Random().Next(bestMoves.Count);
                move = bestMoves[randomIndex];
            }
            return move;
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