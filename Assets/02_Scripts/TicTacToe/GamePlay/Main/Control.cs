using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.GamePlay.Main
{
    public enum Result
    {
        draw = -1,
        none = 0,
        youLose = 1,
        youWin = 2
    }
    /// <summary>
    /// Tic Tac Toe GamePlay Control Behaviour
    /// </summary>
    public class Control : MonoBehaviour, Common.IAttributable
    {
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField] private Block.Input player;
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField, ReadOnly] private Result result;
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] private GridLayoutGroup gridLayoutGroup;

        [Header(Header.scripts, order = 0)]
        [SerializeField, ReadOnly] private AI ai;
        [SerializeField, NonReorderable, ReadOnly] private Block.Control[] blocks;
        [SerializeField, NonReorderable, ReadOnly] private Block.Data[] data;
        /// <summary>
        /// Awake is called when an enabled script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        private void OnEnable()
        {
            Block.Control.PlayHandler += OnPlayable;
        }
        ///<inheritdoc/>
        private void OnDisable()
        {
            Block.Control.PlayHandler -= OnPlayable;
        }
        ///<inheritdoc/>
        private void Start()
        {
            result = Result.none;

            if (player == Block.Input.blank)
            {
                player = Block.Input.x;
                Debug.LogWarning("Player cannot be blank! Player will be X!");
            }

            ai = new(player);

            if (player == Block.Input.o)
                AIInput();
        }
        /// <summary>
        /// Assignment of components and variables
        /// </summary>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public void ComponentsAssignment()
        {
            if (blocks.IsNullOrEmpty() || data.Length == 0 || data == null)
            {
                gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();

                var transform = gridLayoutGroup.transform;
                blocks = new Block.Control[transform.childCount];
                data = new Block.Data[blocks.Length];

                for (var i = 0; i < blocks.Length; i++)
                {
                    var block = transform.GetChild(i).GetComponent<Block.Control>();

                    if (block.Index == -1)
                    {
                        Debug.LogError("Block index error!");
                        continue;
                    }

                    blocks[block.Index] = block;
                    data[block.Index] = block.Data;
                }
            }
        }
        /// <summary>
        /// Function used to be called when <see cref="Block.Control.PlayHandler"/> is invoked.
        /// </summary>
        /// <param name="sender">Sender Object<br/>Must receive <see cref="Block.Control"/> as object</param>
        /// <param name="e">Arguments to Handler</param>
        /// <remarks>Default arguments when using <see cref="System.EventHandler"/></remarks>
        public void OnPlayable(object sender, Block.Args e)
        {
            data[e.Data.Index] = e.Data;
            if (e.Data.Input == player)
                AIInput();
        }

        public void AIInput(float delay = 0f)
        {
            if (delay < 0f)
                delay = 0f;

            var routine = AIInput(data, delay);
            StartCoroutine(routine);
        }

        public IEnumerator AIInput(Block.Data[] board, float delay)
        {
            if (delay <= 0f) yield return new WaitForEndOfFrame();
            else yield return new WaitForSeconds(delay);

            var bestSlotIndex = ai.GetBestMove(board);
            if (bestSlotIndex == -1)
            {
                result = Result.draw;
                yield break;
            }

            blocks[bestSlotIndex].SetInput();
        }
        /// <summary>
        /// Return all blocks
        /// </summary>
        /// <remarks>Read only</remarks>
        public Block.Control[] Blocks => blocks;
        public Block.Input Player => player;
    }
}
