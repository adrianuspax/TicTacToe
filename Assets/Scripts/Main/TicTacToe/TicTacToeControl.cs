using UnityEngine;
using UnityEngine.UI;

public class TicTacToeControl : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField, NonReorderable] private BlockControl[] blocks;
    [SerializeField, NonReorderable] private BlockControl.BlockData[] data;
    /// Awake is called when an enabled script instance is being loaded.
    private void Awake()
    {
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();

        var transform = gridLayoutGroup.transform;
        blocks = new BlockControl[transform.childCount];
        data = new BlockControl.BlockData[blocks.Length];

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = transform.GetChild(i).GetComponent<BlockControl>();

            if (block.Index == -1)
            {
                Debug.LogError("Block index error!");
                continue;
            }

            blocks[block.Index] = block;
            data[block.Index] = block.GetData();
        }
    }
    /// This function is called when the object becomes enabled and active.
    private void OnEnable()
    {
        BlockControl.PlayHandler += OnPlayable;
    }
    /// This function is called when the behaviour becomes disabled.
    private void OnDisable()
    {
        BlockControl.PlayHandler -= OnPlayable;
    }

    public void OnPlayable(object sender, BlockControl.Args e)
    {
        data[e.BlockData.Index] = e.BlockData;
    }

    public BlockControl[] Blocks => blocks;
}
