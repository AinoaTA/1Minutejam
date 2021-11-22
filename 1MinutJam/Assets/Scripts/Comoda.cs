using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Comoda : MonoBehaviour
{
    private int counter = 0;
    public int currentCounter = 0;

    public bool Opened = false;
    public TMP_Text text;

    public Animator anim;

    private void Awake()
    {
        GameController.GetGameController().SetComoda(this);
    }
    private void Start()
    {
        for (int i = 0; i < GameController.GetGameController().AllObjectsInteractables.Count; i++)
        {
            if (GameController.GetGameController().AllObjectsInteractables[i].tag == "Clothes")
                counter++;
        }
    }

    private void Update()
    {
        UpdateCanvas();


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Clothes") && GameController.GetGameController().GetPlayer().CanAttach())
        {
            InteractableObject Object = other.gameObject.GetComponent<InteractableObject>();
            GameController.GetGameController().AddInteractable(Object);
            currentCounter++;
            UpdateCanvas();
            Object.transform.SetParent(gameObject.transform);
            Object.Placed = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractableObject>().Placed)
        {
            InteractableObject Object = other.gameObject.GetComponent<InteractableObject>();
            GameController.GetGameController().RemoveInteractable(Object);
            currentCounter--;
            UpdateCanvas();
            Object.Placed = false;
        }
    }

    public void OpenComoda()
    {
        anim.SetBool("Open", true);
        Opened = true;
    }
    private void UpdateCanvas()
    {
        text.text = currentCounter + "/" + counter;
    }
}
