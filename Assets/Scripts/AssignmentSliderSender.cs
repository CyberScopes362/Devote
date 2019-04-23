using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AssignmentSliderSender : MonoBehaviour, IPointerUpHandler
{
    public AssignmentTask assignmentTask;

    public void OnPointerUp(PointerEventData eventData)
    {
        assignmentTask.OnSliderValueChanged();
    }
}
