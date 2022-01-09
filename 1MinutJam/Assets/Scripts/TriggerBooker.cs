using UnityEngine;

public class TriggerBooker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Books") && GameController.GetGameController().GetPlayer().CanAttach())
        {
            InteractableObject Object = other.gameObject.GetComponent<InteractableObject>();
            GameController.GetGameController().AddInteractable(Object);
            GameController.GetGameController().GetBookerCounter().currentCounter++;
            GameController.GetGameController().GetBookerCounter().UpdateCanvas();
            Object.Placed = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractableObject>().Placed)
        {
            InteractableObject Object = other.gameObject.GetComponent<InteractableObject>();
            GameController.GetGameController().RemoveInteractable(Object);
            GameController.GetGameController().GetBookerCounter().currentCounter--;
            GameController.GetGameController().GetBookerCounter().UpdateCanvas();
            Object.Placed = false;      
        }
    }
}
