using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public List<InteractableObject> AllObjectsInteractables; //objetos sin guardar
    public List<InteractableObject> CurrentSavedObjectsInteractables = new List<InteractableObject>(); //objetos que vas guardando.

    static GameController GC;
    PlayerController PC;
    UIController HUD;
    BookerCounter Booker;
    Comoda Comoda;
    AreasLightController AreasLight;

    private void Awake()
    {
        GC = this;
    }
    private void Start()
    {
        AllObjectsInteractables = FindObjectsOfType<InteractableObject>().ToList();
    }

    //SETS
    public PlayerController SetPlayer(PlayerController player)
    {
        return PC = player;
    }

    public UIController SetHUD ( UIController forceUI)
    {
        return HUD = forceUI;
    }

    public BookerCounter SetBookerC(BookerCounter booker)
    {
        return Booker = booker;
    }
    public Comoda SetComoda(Comoda comoda)
    {
        return Comoda = comoda;
    }
   
    public AreasLightController SetAreasLight(AreasLightController areasLight)
    {
        return AreasLight = areasLight;
    }

    //--
    //GETS
    static public GameController GetGameController() => GC;
    public PlayerController GetPlayer() => PC;

    public UIController GetHud() => HUD;

    public BookerCounter GetBookerCounter()=> Booker;

    public Comoda GetComoda() => Comoda;

    public AreasLightController GetAreasLight()=> AreasLight;
   
    //Functions

    public void AddInteractable(InteractableObject interactableObject)
    {
        CurrentSavedObjectsInteractables.Add(interactableObject);
        HUD.UpdateTextObjects();

        if (ReviseCount())
            GetHud().Winner();
    }
    public bool ReviseCount()
    {
        return CurrentSavedObjectsInteractables.Count >= AllObjectsInteractables.Count;
    }

    public void RemoveInteractable(InteractableObject interactable)
    {
        CurrentSavedObjectsInteractables.Remove(interactable);
        HUD.UpdateTextObjects();
    }
}
