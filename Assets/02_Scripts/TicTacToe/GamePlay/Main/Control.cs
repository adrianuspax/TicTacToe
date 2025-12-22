using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.GamePlay.Main
{
    /// <summary>
    /// Tic Tac Toe GamePlay Control Behaviour
    /// </summary>
    public class Control : MonoBehaviour, Common.IAttributable
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField, NonReorderable, ReadOnly] private string[] board;
        [Space(-10, order = 0)]
        [Header(Header.components, order = 1)]
        [SerializeField, ReadOnly] private GridLayoutGroup gridLayoutGroup;

        [Header(Header.scripts, order = 0)]
        [SerializeField, ReadOnly] private TicTacToeAI ai;
        [SerializeField, NonReorderable, ReadOnly] private Block.Control[] blocks;
        [SerializeField, NonReorderable, ReadOnly] private Block.Control.Data[] data;
        /// <summary>
        /// Awake is called when an enabled script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            ComponentsAssignment();
        }
        ///<inheritdoc/>
        private IEnumerator Start()
        {
            int value = Random.Range(0, 9);
            if (value == 4) value = 0;
            yield return new WaitForSeconds(2f); //????????????????????????
            blocks[value].SetInput();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            Block.Control.PlayHandler += OnPlayable;
        }
        /// <summary>
        /// This function is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            Block.Control.PlayHandler -= OnPlayable;
        }
        /// <summary>
        /// Assignment of components and variables
        /// </summary>
        [Button("Components Assignment")]
        public void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref ai);

            if (blocks.IsNullOrEmpty() || data.Length == 0 || data == null)
            {
                gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();

                var transform = gridLayoutGroup.transform;
                blocks = new Block.Control[transform.childCount];
                data = new Block.Control.Data[blocks.Length];

                for (var i = 0; i < blocks.Length; i++)
                {
                    var block = transform.GetChild(i).GetComponent<Block.Control>();

                    if (block.Index == -1)
                    {
                        Debug.LogError("Block index error!");
                        continue;
                    }

                    blocks[block.Index] = block;
                    data[block.Index] = block.GetData();
                }
            }
        }
        /// <summary>
        /// Function used to be called when <see cref="Block.Control.PlayHandler"/> is invoked.
        /// </summary>
        /// <param name="sender">Sender Object<br/>Must receive <see cref="Block.Control"/> as object</param>
        /// <param name="e">Arguments to Handler</param>
        /// <remarks>Default arguments when using <see cref="System.EventHandler"/></remarks>
        public void OnPlayable(object sender, Block.Control.Args e)
        {
            data[e.Data.Index] = e.Data;

            if (e.Data.Input == Block.Control.Input.x) return;

            AITurn();
        }

        private void AITurn()
        {
            board = data.Select(s => s.Input.ToString()).ToArray();
            var bestSlotIndex = ai.GetBestMove(board);
            print(bestSlotIndex);
            if (bestSlotIndex == -1) return;

            blocks[bestSlotIndex].SetInput();
        }
        /// <summary>
        /// Return all blocks
        /// </summary>
        /// <remarks>Read only</remarks>
        public Block.Control[] Blocks => blocks;
    }
}
