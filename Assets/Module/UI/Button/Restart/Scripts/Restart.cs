using UnityEngine.Events;

namespace TicTacToe.UI.Button
{
    using Button = UnityEngine.UI.Button;

    public class Restart : Input.Button.Default
    {
        public static event UnityAction Handler; // Alertar: Inserir antes do Start

        public void AddListener(UnityAction call)
        {
            Button.onClick.AddListener(call);
        }

        protected override void Behaviour()
        {
            base.Behaviour();
            Handler?.Invoke();
        }
    }
}
