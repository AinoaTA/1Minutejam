using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComodaMother : MonoBehaviour
{
    public void OpenComoda()
    {
        GameController.GetGameController().GetComoda().OpenComoda();
    }
}
