namespace TicTacToe.GamePlay.Main
{
    /// <summary>
    /// Represents the possible outcomes of the game.
    /// </summary>
    public enum Result
    {
        /// <summary>
        /// The game ended in a draw.
        /// </summary>
        draw = -1,
        /// <summary>
        /// The game is still in progress.
        /// </summary>
        none = 0,
        /// <summary>
        /// The human player lost the game.
        /// </summary>
        youLose = 1,
        /// <summary>
        /// The human player won the game.
        /// </summary>
        youWin = 2
    }
}
