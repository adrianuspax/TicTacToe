using ASPax.Attributes.Drawer;
using ASPax.Utilities;
using UnityEngine;

namespace TicTacToe.Input.Toggle
{
    using Toggle = UnityEngine.UI.Toggle;
    /// <summary>
    /// Restart Button Behaviour
    /// </summary>
    public class Default : Input.Default
    {
        [Header(Header.MANAGEABLE, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.assets, order = 2)]
        [SerializeField] protected Sprite offSprite;
        [SerializeField] protected Sprite onSprite;

        protected virtual void Start()
        {
            Toggle.onValueChanged.AddListener(Behaviour);
        }

        protected virtual void Behaviour(bool isOn)
        {
            if (isOn)
            {
                if (onSprite != null)
                    Toggle.image.sprite = onSprite;
            }
            else
            {
                if (offSprite != null)
                    Toggle.image.sprite = offSprite;
            }
        }

        protected Toggle Toggle => selectable as Toggle;
    }
}
