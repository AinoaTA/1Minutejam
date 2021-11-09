using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> AllObjectsInteractables; //objetos sin guardar
    public List<GameController> CurrentSavedObjectsInteractables; //objetos que vas guardando.

    static GameController GC;
    PlayerController PC;
    //set
    public PlayerController SetPlayer(PlayerController player)
    {
        return PC = player;
    }

    private void Awake()
    {
        GC = this;
    }

    //Gets
    static public GameController GetGameController() => GC;
    public PlayerController GetPlayer() => PC;

    //Functions



}
