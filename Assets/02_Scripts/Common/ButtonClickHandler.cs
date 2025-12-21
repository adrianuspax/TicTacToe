
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Um script genérico para lidar com eventos de clique de botão na Unity.
/// Ele expõe um UnityEvent no Inspector, permitindo que você atribua
/// qualquer função pública de qualquer GameObject para ser executada no clique.
/// </summary>
public class ButtonClickHandler : MonoBehaviour
{
    /// <summary>
    /// O evento que será acionado quando o botão for clicado.
    /// Atribua as funções desejadas a este evento no Inspector da Unity.
    /// </summary>
    public UnityEvent OnClick = new UnityEvent();

    /// <summary>
    /// Este método público pode ser chamado pelo componente Button do Unity.
    /// Ele, por sua vez, invoca o evento OnClick.
    /// </summary>
    public void TriggerClickEvent()
    {
        OnClick.Invoke();
    }
}
