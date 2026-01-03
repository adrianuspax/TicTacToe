using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.GamePlay.Block
{
    /// <summary>
    /// Tick Tac Toe Block Control Behaviour
    /// </summary>
    public partial class Control : MonoBehaviour
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private Image image; // The image component of the block.
        [SerializeField, ReadOnly] private Button button; //The button component of the block.
        [SerializeField, ReadOnly] private TextMeshProUGUI tmp; //The TextMeshProUGUI component for displaying text on the block.

        [Header(Header.variables, order = 0)]
        [SerializeField, ReadOnly] private Data data; //The data associated with this block.
        /// <summary>
        /// Method that can be called from the context menu in the Inpector for function tests
        /// </summary>
        [Button(nameof(SetIndex), SButtonEnableMode.Editor)]
        private void SetIndex()
        {
            var index = transform.GetSiblingIndex();
            data = new() { Index = index };
#if UNITY_EDITOR
            tmp.text = $"{index}";
#endif
        }
        /// <summary>
        /// Play Handler invoked into <see cref="SetInput"/>
        /// </summary>
        public static event EventHandler<Args> Handler;
        private static Input _lastInput; //Stores the last input made on any block.
        /// <inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        /// <inheritdoc/>
        private void Start()
        {
            SetIndex();
            button.onClick.AddListener(SetInput);
            tmp.text = string.Empty;
            _lastInput = Input.blank;
        }
        /// <inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            this.GetComponentInChildrenIfNull(ref image);
            this.GetComponentInChildrenIfNull(ref button);
            this.GetComponentInChildrenIfNull(ref tmp);
        }
        /// <summary>
        /// Set the player input
        /// </summary>
        public void SetInput()
        {
            if (data.IsInputted)
                return;

            _lastInput = _updateInputted();
            tmp.text = _getText();
            data = new(data.Index, _lastInput);
            var e = new Args(data);
            Handler?.Invoke(this, e);
            return;
            // Update the last input
            Input _updateInputted()
            {
                return _lastInput switch
                {
                    Input.x => Input.o,
                    Input.o => Input.x,
                    _ => Input.x,
                };
            }
            // Get the text representation of the input
            string _getText()
            {
                return _lastInput switch
                {
                    Input.x => "x",
                    Input.o => "o",
                    _ => "blank",
                };
            }
        }
        /// <summary>
        /// Sets the interactable state of the button, enabling or disabling user interaction
        /// </summary>
        /// <param name="value">A value indicating whether the button should be interactable.</param>
        public void SetInteractable(bool value)
        {
            button.interactable = value;
        }

        public void SetColorText(Color color)
        {
            tmp.color = color;
        }

        public void ResetData()
        {
            data = new();
            button.interactable = true;
            tmp.text = string.Empty;
            tmp.color = Color.black;
        }
        /// <summary>
        /// Get the block data
        /// </summary>
        public Data Data => data;
        /// <summary>
        /// Return the index of the block
        /// </summary>
        public int Index => data.Index;
    }
}
