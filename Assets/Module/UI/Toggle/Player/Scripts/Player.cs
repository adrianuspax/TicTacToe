using ASPax.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace TicTacToe.UI.Toggle
{
    using Toggle = UnityEngine.UI.Toggle;

    public class Player : Input.Toggle.Default
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

            Toggle.onValueChanged.AddListener(Save);
            Toggle.isOn = Load();
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

        public void Save(bool isOn)
        {
            var value = isOn.ToInt();
            PlayerPrefs.SetInt(nameof(Player), value);
        }

        public bool Load()
        {
            var value = PlayerPrefs.GetInt(nameof(Player), 0);
            return value.ToBool();
        }
    }
}
