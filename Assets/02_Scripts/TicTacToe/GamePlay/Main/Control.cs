using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TicTacToe.GamePlay.Main
{
    /// <summary>
    /// Tic Tac Toe GamePlay Control Behaviour
    /// </summary>
    public class Control : MonoBehaviour
    {
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField] private Block.Input player;
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.variables, order = 2)]
        [SerializeField, ReadOnly] private AI.Result result; // The result of the game.
        [Space(-10, order = 0)]
        [Header(Header.components, order = 1)]
        [SerializeField, ReadOnly] private GridLayoutGroup gridLayoutGroup; // The grid layout group for the Tic-Tac-Toe board.

        [Header(Header.scripts, order = 0)]
        [SerializeField, ReadOnly] private AI ai; // The AI instance for the game.
        [SerializeField, NonReorderable, ReadOnly] private Block.Control[] blocks; // An array of block controls representing the cells of the board.
        [SerializeField, NonReorderable, ReadOnly] private Block.Data[] data; // An array of block data representing the state of the board.
        /// <inheritdoc/>
        private void Awake()
        {
            ComponentsAssignment();
        }
        /// <inheritdoc/>
        private void OnEnable()
        {
            Block.Control.PlayHandler += OnPlayable;
            Input.Button.Restart.Handler += ResetGame;
        }
        /// <inheritdoc/>
        private void Start()
        {
            result = new();

            if (player == Block.Input.blank)
            {
                player = Block.Input.x;
                Debug.LogWarning("Player cannot be blank! Player will be X!");
            }

            ai = new(player);

            if (player == Block.Input.o)
                AIInput();
        }
        /// <inheritdoc/>
        private void OnDisable()
        {
            Block.Control.PlayHandler -= OnPlayable;
            Input.Button.Restart.Handler -= ResetGame;
        }
        /// <inheritdoc/>
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
            var isEnd = ResultBehaviour();
            if (isEnd)
                return;
            if (e.Data.Input == player)
                AIInput();
        }

        private bool ResultBehaviour()
        {
            result = ai.CheckForWinner(data);

            return result.main switch
            {
                Main.Result.draw => _draw(),
                Main.Result.youLose => _youLose(),
                Main.Result.youWin => _youWin(),
                _ => _none(),
            };

            bool _draw()
            {
                SetBlocksInteractable(false);
                return true;
            }

            bool _youLose()
            {
                SetBlocksInteractable(false);
                _beahviour();
                return true;
            }

            bool _youWin()
            {
                SetBlocksInteractable(false);
                _beahviour();
                return true;
            }

            bool _none()
            {
                return false;
            }

            void _beahviour()
            {
                for (var i = 0; i < result.indexes.Length; i++)
                {
                    blocks[result.indexes[i]].SetColorText(Color.indianRed);
                }
            }
        }
        /// <summary>
        /// Initiates the AI's turn after a specified delay.
        /// </summary>
        /// <param name="delay">The delay in seconds before the AI makes a move.</param>
        public void AIInput(float delay = 0f)
        {
            if (delay < 0f)
                delay = 0f;

            var routine = AIInput(data, delay);
            StartCoroutine(routine);
        }
        /// <summary>
        /// Coroutine for handling the AI's turn.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <param name="delay">The delay in seconds before the AI makes a move.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        public IEnumerator AIInput(Block.Data[] board, float delay)
        {
            if (delay <= 0f)
                yield return new WaitForEndOfFrame();
            else
                yield return new WaitForSeconds(delay);

            var bestSlotIndex = ai.GetBestMove(board);
            if (bestSlotIndex == -1)
                yield break;

            blocks[bestSlotIndex].SetInput();
        }

        public void SetBlocksInteractable(bool value)
        {
            foreach (var block in blocks)
                block.SetInteractable(value);
        }

        public void ResetGame()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        /// <summary>
        /// Return all blocks
        /// </summary>
        /// <remarks>Read only</remarks>
        public Block.Control[] Blocks => blocks;
        /// <summary>
        /// Gets the input type of the human player.
        /// </summary>
        public Block.Input Player => player;
        /// <summary>
        /// Gets the current result of the game.
        /// </summary>
        public AI.Result Result => result;
    }
}
