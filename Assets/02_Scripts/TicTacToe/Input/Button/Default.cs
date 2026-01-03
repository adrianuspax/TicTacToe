namespace TicTacToe.Input.Button
{
    using Button = UnityEngine.UI.Button;
    /// <summary>
    /// Restart Button Behaviour
    /// </summary>
    public class Default : Input.Default
    {
        protected virtual void Start()
        {
            Button.onClick.AddListener(Behaviour);
        }

        protected virtual void Behaviour()
        {
            return;
        }

        protected Button Button => selectable as Button;
    }
}
