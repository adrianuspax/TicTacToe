using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.GamePlay.Block
{
    using ASPax.Attributes.Drawer;
    using ASPax.Attributes.Drawer.SpecialCases;
    using ASPax.Attributes.Meta;
    using ASPax.Extensions;
    using ASPax.Utilities;
    /// <summary>
    /// Tick Tac Toe Block Control Behaviour
    /// </summary>
    public class Control : MonoBehaviour, Common.IAttributable
    {
        /// <summary>
        /// Player input Enum
        /// </summary>
        public enum Input
        {
            none = 0,
            x = 1,
            o = 2
        }
        /// <summary>
        /// Block Data Struct
        /// </summary>
        [Serializable]
        public struct Data
        {
            /// <summary>
            /// Even or Odd Enum
            /// </summary>
            public enum EvenOrOdd
            {
                unknown = -1,
                even = 0,
                odd = 1
            }

            [SerializeField, ReadOnly] private int index;
            [SerializeField, ReadOnly] private Input input;
            /// <summary>
            /// Block Data Constructor
            /// </summary>
            /// <param name="index">Index of block from Tic Tac Toe</param>
            /// <param name="input">Current Player Inputed in the block</param>
            public Data(int index, Input input)
            {
                if (index < 0 || index > 8)
                    index = -1;

                this.index = index;
                this.input = input;
            }
            /// <summary>
            /// Get Even or Odd
            /// </summary>
            public readonly EvenOrOdd GetEvenOrOdd() => (EvenOrOdd)(index % 2);
            /// <summary>
            /// Check if the block index is Zero
            /// </summary>
            public readonly bool IsZero() => index == 0;
            /// <summary>
            /// Return the index of the block
            /// </summary>
            public int Index
            {
                readonly get => index; set => index = value;
            }
            /// <summary>
            /// Return the player input of the block
            /// </summary>
            public Input Input
            {
                readonly get => input; set => input = value;
            }
            /// <summary>
            /// return if the block is already inputted
            /// </summary>
            public readonly bool IsInput => ((int)input) > 0;
        }
        /// <summary>
        /// Arguments for Play Handler
        /// </summary>
        [Serializable]
        public class Args : EventArgs
        {
            [SerializeField] private Data data;
            /// <summary>
            /// Arguments Constructor
            /// </summary>
            /// <param name="data"></param>
            public Args(Data data)
            {
                this.data = data;
            }
            /// <summary>
            /// Return the data of the block
            /// </summary>
            public Data Data => data;
        }
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private Image image;
        [SerializeField, ReadOnly] private Button button;
        [SerializeField, ReadOnly] private TextMeshProUGUI tmp;

        [Header(Header.variables, order = 0)]
        [SerializeField, ReadOnly] private Data data;
#if UNITY_EDITOR
        /// <summary>
        /// Reset to default values.
        /// </summary>
        [Button("Reset", SButtonEnableMode.Editor)]
        private void Reset()
        {
            SetIndex();
        }
        /// <summary>
        /// Method that can be called from the context menu in the Inpector for function tests
        /// </summary>
        [Button("Set Index", SButtonEnableMode.Editor)]
        private void SetIndex()
        {
            var chars = gameObject.name.Where(char.IsDigit).ToArray();
            var digits = new string(chars);
            var isParsable = int.TryParse(digits, out var i);

            if (isParsable) data = new() { Index = i };
        }
#endif
        /// <summary>
        /// Play Handler invoked into <see cref="SetInput"/>
        /// </summary>
        public static event EventHandler<Args> PlayHandler;
        private static Input _inputted;
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
            button.onClick.AddListener(SetInput);
            tmp.text = string.Empty;
            _inputted = Input.none;
        }
        /// <summary>
        /// Assignment of components and variables
        /// </summary>
        [Button("Components Assignment", SButtonEnableMode.Editor)]
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
            if (data.IsInput) return;

            _inputted = _updateInputted();
            tmp.text = _getText();
            data = new(data.Index, _inputted);
            var e = new Args(data);
            PlayHandler?.Invoke(this, e);
            return;

            Input _updateInputted()
            {
                return _inputted switch
                {
                    Input.x => Input.o,
                    Input.o => Input.x,
                    _ => Input.x,
                };
            }

            string _getText()
            {
                return _inputted switch
                {
                    Input.x => "X",
                    Input.o => "O",
                    _ => "?",
                };
            }
        }
        /// <summary>
        /// Get the block data
        /// </summary>
        public Data GetData() => data;
        /// <summary>
        /// Return the index of the block
        /// </summary>
        public int Index => data.Index;
    }
}
