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
            Toggle.onValueChanged.AddListener(Save);
            //Necessário para inicializar o toggle com o valor salvo (Já que a propriedade `isOn` não dispara se o valor default for o mesmo!)
            var value = Load();
            Toggle.SetIsOnWithoutNotify(value); // Não dispara os listeners
            Toggle.onValueChanged.Invoke(value); // Dispara os listeners manualmente
        }

        protected override void Behaviour(bool isOn)
        {
            base.Behaviour(isOn);
            tmps[0].text = isOn ? ON : OFF;
            Handler?.Invoke(isOn);
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

        public bool IsOn => Toggle.isOn;
    }
}
