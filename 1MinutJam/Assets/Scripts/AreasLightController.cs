using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreasLightController : MonoBehaviour
{

    private void Awake()
    {
        GameController.GetGameController().SetAreasLight(this);
    }
    private void Update()
    {

    }

    public void OpenHelp()
    {

        for (int i = 0; i < GameController.GetGameController().AllObjectsInteractables.Count; i++)
        {
            if (!GameController.GetGameController().AllObjectsInteractables[i].Placed)
                GameController.GetGameController().AllObjectsInteractables[i].HelpLight.gameObject.SetActive(true);
        }

    }

    public void CloseHelp()
    {

        for (int i = 0; i < GameController.GetGameController().AllObjectsInteractables.Count; i++)
        {
            GameController.GetGameController().AllObjectsInteractables[i].HelpLight.gameObject.SetActive(false);

        }

    }
}
