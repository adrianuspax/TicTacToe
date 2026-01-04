using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace TicTacToe.GamePlay.Main
{
    /// <summary>
    /// Represents the Artificial Intelligence for the Tic-Tac-Toe game.
    /// </summary>
    [Serializable]
    public partial class AI
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AI"/> class.
        /// </summary>
        /// <param name="player">The input type of the human player.</param>
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

        [SerializeField] private Block.Input human; //The input type representing the human player.
        [SerializeField] private Block.Input ai; // The input type representing the AI player.
        [SerializeField] private int move;
        [SerializeField, NonReorderable] private List<int> bestMoves;
        public static event UnityAction NotifyHandler;
        private readonly int[,] winConditions = new int[,] // Defines the winning combinations on the Tic-Tac-Toe board.
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Lines
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
            {0, 4, 8}, {2, 4, 6}             // Diags
        };
        /// <summary>
        /// Checks the board for a winner or a draw.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <returns>The result of the game (win, lose, draw, or none).</returns>
        public Result CheckForWinner(Block.Data[] board)
        {
            var length = winConditions.GetLength(0);

            for (var i = 0; i < length; i++)
            {
                var a = winConditions[i, 0];
                var b = winConditions[i, 1];
                var c = winConditions[i, 2];

                if (board[a].Input != Block.Input.blank && board[a].Input == board[b].Input && board[b].Input == board[c].Input)
                {
                    var result = new Result(Main.Result.youLose, a, b, c);
                    if (board[a].Input != ai)
                        result.main = Main.Result.youWin;

                    return result;
                }
            }

            var isMovesLeft = IsMovesLeft(board);
            if (isMovesLeft)
                return new() { main = Main.Result.none };
            else
                return new() { main = Main.Result.draw };
        }
        /// <summary>
        /// Determines the best possible move for the AI.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <returns>The index of the best move.</returns>
        public int GetBestMove(Block.Data[] board)
        {
            var bestScore = int.MinValue;
            bestMoves = new List<int>();

            for (var i = 0; i < board.Length; i++)
            {
                if (board[i].Input == Block.Input.blank)
                {
                    board[i].Input = ai;
                    var score = Minimax(board, 0, false);
                    board[i].Input = Block.Input.blank;
                    Safety.Debug.Log("AI Move Index: " + i + " Score: " + score);
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

            if (bestScore > 0)
                NotifyHandler?.Invoke();

            move = -1;

            if (bestMoves.Count > 0)
            {
                var randomIndex = Random.Range(0, bestMoves.Count);
                move = bestMoves[randomIndex];
            }

            return move;
        }
        /// <summary>
        /// The Minimax algorithm to find the best move.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <param name="depth">The current depth of the recursion.</param>
        /// <param name="isMaximizing">True if the current move is for the maximizing player (AI), false otherwise.</param>
        /// <returns>The score of the move.</returns>
        private int Minimax(Block.Data[] board, int depth, bool isMaximizing)
        {
            int best;
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
                best = int.MinValue;

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
            }
            else
            {
                best = int.MaxValue;

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
            }

            return best;
        }
        /// <summary>
        /// Evaluates the current board state and returns a score.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <returns>A score of 10 for an AI win, -10 for a human win, and 0 otherwise.</returns>
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
        /// <summary>
        /// Checks if there are any moves left on the board.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <returns>True if there are available moves, false otherwise.</returns>
        private bool IsMovesLeft(Block.Data[] board)
        {
            for (var i = 0; i < board.Length; i++)
                if (board[i].Input == Block.Input.blank)
                    return true;

            return false;
        }
    }
}