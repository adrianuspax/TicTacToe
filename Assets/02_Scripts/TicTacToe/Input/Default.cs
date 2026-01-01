using ASPax.Attributes.Drawer;
using ASPax.Attributes.Drawer.SpecialCases;
using ASPax.Attributes.Meta;
using ASPax.Extensions;
using ASPax.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.Input
{
    /// <summary>
    /// Restart Button Behaviour
    /// </summary>
    public class Default : MonoBehaviour
    {
        [Header(Header.READONLY, order = 0), HorizontalLine]
        [Space(-10, order = 1)]
        [Header(Header.components, order = 2)]
        [SerializeField, ReadOnly] protected Selectable selectable;
        [SerializeField, NonReorderable, ReadOnly] protected Image[] images;
        [SerializeField, NonReorderable, ReadOnly] protected TextMeshProUGUI[] tmps;
        /// <inheritdoc/>
        [Button(nameof(ComponentsAssignment), SButtonEnableMode.Editor)]
        public virtual void ComponentsAssignment()
        {
            this.GetComponentIfNull(ref selectable);
            this.GetComponentsInAllChildrenIfNull(ref images);
            this.GetComponentsInAllChildrenIfNull(ref tmps);
        }
        /// <inheritdoc/>
        protected virtual void Awake()
        {
            ComponentsAssignment();
        }
    }
}
