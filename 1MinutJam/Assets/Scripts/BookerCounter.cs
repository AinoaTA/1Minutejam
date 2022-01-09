using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookerCounter : MonoBehaviour
{
    private int counter = 0;
    public int currentCounter = 0;

    public TMP_Text text;

    private void Awake()
    {
        GameController.GetGameController().SetBookerC(this);    
    }
    private void Start()
    {
        for (int i = 0; i < GameController.GetGameController().AllObjectsInteractables.Count; i++)
        {
            if (GameController.GetGameController().AllObjectsInteractables[i].tag == "Books")
                counter++;
        }
        UpdateCanvas();
    }
    public void UpdateCanvas()
    {
        text.text = currentCounter + "/" + counter;
    }
}
