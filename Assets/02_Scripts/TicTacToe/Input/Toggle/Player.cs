using UnityEngine.Events;

namespace TicTacToe.Input.Toggle
{
    using Toggle = UnityEngine.UI.Toggle;

    public class Player : Default
    {
        public static event UnityAction<bool> Handler; // Alertar: Inserir antes do Start
        private const string ON = "First: You";
        private const string OFF = "First: AI";
        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            if (Handler != null)
                Toggle.onValueChanged.AddListener(Handler);
        }

        protected override void ToggleBehaviour(bool isOn)
        {
            base.ToggleBehaviour(isOn);
            tmps[0].text = isOn ? ON : OFF;
        }

        public void AddListener(UnityAction<bool> call)
        {
            Toggle.onValueChanged.AddListener(call);
        }
    }
}
