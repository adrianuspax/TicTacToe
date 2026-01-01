namespace TicTacToe.Input.Button
{
    using Button = UnityEngine.UI.Button;
    /// <summary>
    /// Restart Button Behaviour
    /// </summary>
    public class Default : Input.Default
    {
        protected Button Button => selectable as Button;
    }
}
