using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.GamePlay.Block
{
    /// <summary>
    /// Tick Tac Toe Block Control Behaviour
    /// </summary>
    public class Control : MonoBehaviour
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

            [SerializeField] private int index;
            [SerializeField] private Input input;
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
            public readonly int Index => index;
            /// <summary>
            /// Return the player input of the block
            /// </summary>
            public readonly Input Input => input;
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

        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI tmp;
        [SerializeField] private Data data;
        public static event EventHandler<Args> PlayHandler;
        private static Input _inputted;
        /// <summary>
        /// Awake is called when an enabled script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            image = GetComponentInChildren<Image>();
            button = GetComponentInChildren<Button>();
            tmp = GetComponentInChildren<TextMeshProUGUI>();
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
        /// Set the player input
        /// </summary>
        public void SetInput()
        {
            if (data.IsInput)
                return;

            _inputted = _updateInputted();
            tmp.text = _getText();
            data = new(data.Index, _inputted);
            PlayHandler?.Invoke(this, new Args(data));
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
