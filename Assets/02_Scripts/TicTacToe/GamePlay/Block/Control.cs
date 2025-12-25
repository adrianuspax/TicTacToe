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
    public partial class Control : MonoBehaviour, Common.IAttributable
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private Image image;
        [SerializeField, ReadOnly] private Button button;
        [SerializeField, ReadOnly] private TextMeshProUGUI tmp;

        [Header(Header.variables, order = 0)]
        [SerializeField, ReadOnly] private Data data;
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
        public static event EventHandler<Args> PlayHandler;
        private static Input _lastInput;
        /// <summary>
        /// Awake is called when an enabled script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            ComponentsAssignment();
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            SetIndex();
            button.onClick.AddListener(SetInput);
            tmp.text = string.Empty;
            _lastInput = Input.blank;
        }
        /// <summary>
        /// Assignment of components and variables
        /// </summary>
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
            PlayHandler?.Invoke(this, e);
            return;

            Input _updateInputted()
            {
                return _lastInput switch
                {
                    Input.x => Input.o,
                    Input.o => Input.x,
                    _ => Input.x,
                };
            }

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
        /// Get the block data
        /// </summary>
        public Data Data => data;
        /// <summary>
        /// Return the index of the block
        /// </summary>
        public int Index => data.Index;
    }
}
