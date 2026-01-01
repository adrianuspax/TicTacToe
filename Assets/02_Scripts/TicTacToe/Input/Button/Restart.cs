using UnityEngine.Events;

namespace TicTacToe.Input.Button
{
    using Button = UnityEngine.UI.Button;

    public class Restart : Default
    {
        public static event UnityAction Handler; // Alertar: Inserir antes do Start
        /// <inheritdoc/>
        private void Start()
        {
            if (Handler != null)
                Button.onClick.AddListener(Handler);
        }

        public void AddListener(UnityAction call)
        {
            Button.onClick.AddListener(call);
        }
    }
}
