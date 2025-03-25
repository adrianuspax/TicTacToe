using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockControl : MonoBehaviour
{
    public enum Player
    {
        none = 0,
        x = 1,
        o = 2
    }

    [Serializable]
    public struct BlockData
    {
        public enum EvenOrOdd
        {
            unknown = -1,
            even = 0,
            odd = 1
        }

        [SerializeField] private int index;
        [SerializeField] private Player player;

        public BlockData(int index, Player player)
        {
            if (index < 0 || index > 8)
                index = -1;

            this.index = index;
            this.player = player;
        }

        public static bool IsIntoRange(int? index) => index != null && (index < 0 || index > 8);
        public readonly EvenOrOdd GetEvenOrOdd() => (EvenOrOdd)(index % 2);
        public readonly bool IsZero() => index == 0;
        public readonly int Index => index;
        public readonly Player Player => player;
        public readonly bool IsPlayed => ((int)player) > 0;
    }

    [Serializable]
    public class Args : EventArgs
    {
        [SerializeField] private BlockData blockData;

        public Args(BlockData blockData)
        {
            this.blockData = blockData;
        }

        public BlockData BlockData => blockData;
    }

    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private BlockData blockData;
    public static event EventHandler<Args> PlayHandler;
    private static Player _currentPlayer;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        button = GetComponentInChildren<Button>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        button.onClick.AddListener(Play);
        tmp.text = string.Empty;
        _currentPlayer = Player.none;
    }

    public void Play()
    {
        if (blockData.IsPlayed)
            return;

        _currentPlayer = _updateCurrentPlayer();
        tmp.text = _getText();
        blockData = new(blockData.Index, _currentPlayer);
        PlayHandler?.Invoke(this, new Args(blockData));
        return;

        Player _updateCurrentPlayer()
        {
            return _currentPlayer switch
            {
                Player.x => Player.o,
                Player.o => Player.x,
                _ => Player.x,
            };
        }

        string _getText()
        {
            return _currentPlayer switch
            {
                Player.x => "X",
                Player.o => "O",
                _ => "?",
            };
        }
    }

    public BlockData GetData() => blockData;
    public int Index => blockData.Index;
}
