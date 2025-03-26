using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.GamePlay.Main
{
    /// <summary>
    /// Tic Tac Toe GamePlay Control Behaviour
    /// </summary>
    public class Control : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField, NonReorderable] private Block.Control[] blocks;
        [SerializeField, NonReorderable] private Block.Control.Data[] data;
        /// <summary>
        /// Awake is called when an enabled script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();

            var transform = gridLayoutGroup.transform;
            blocks = new Block.Control[transform.childCount];
            data = new Block.Control.Data[blocks.Length];

            for (int i = 0; i < blocks.Length; i++)
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
        /// Function used to be called when <see cref="BlockControl.PlayHandler"/> is invoked.
        /// </summary>
        /// <param name="sender">Sender Object<br/>Must receive <see cref="BlockControl"/> as object</param>
        /// <param name="e">Arguments to Handler</param>
        /// <remarks>Default arguments when using <see cref="System.EventHandler"/></remarks>
        public void OnPlayable(object sender, Block.Control.Args e)
        {
            data[e.Data.Index] = e.Data;
        }
        /// <summary>
        /// Return all blocks
        /// </summary>
        /// <remarks>Read only</remarks>
        public Block.Control[] Blocks => blocks;
    }
}

