using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrashContaines : MonoBehaviour
{
    public TMP_Text text;
    private int counter=0;
    private int currentCounter=0;
    private void Start()
    {
        for (int i = 0; i < GameController.GetGameController().AllObjectsInteractables.Count; i++)
        {
            if (GameController.GetGameController().AllObjectsInteractables[i].tag == "Trash")
                counter++;
        }

        UpdateCanvas();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash") && GameController.GetGameController().GetPlayer().CanAttach())
        {
            GameController.GetGameController().AddInteractable(other.gameObject.GetComponent<InteractableObject>());
            currentCounter++;
            UpdateCanvas();
            other.GetComponent<InteractableObject>().Placed = true;
        }
    }

    private void UpdateCanvas()
    {
        text.text = currentCounter + "/" + counter;
    }
}
