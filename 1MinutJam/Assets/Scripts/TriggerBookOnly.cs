using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class TriggerBookOnly : MonoBehaviour
//{
//    InteractableObject interactable;

//    private void Start()
//    {
//        interactable = GetComponent<InteractableObject>();
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        print(collision.collider.name);
//        if(collision.collider.CompareTag("Booker") && GameController.GetGameController().GetPlayer().CanAttach())
//        {
//            print("a");
//            GameController.GetGameController().AddInteractable(interactable);
//            GameController.GetGameController().GetBookerC().currentCounter++;
//            GameController.GetGameController().GetBookerC().UpdateCanvas();
//            interactable.Placed = true;
//        }
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        if (collision.collider.CompareTag("Booker") && GameController.GetGameController().GetPlayer().CanAttach())
//        {
//            GameController.GetGameController().RemoveInteractable(interactable);
//            GameController.GetGameController().GetBookerC().currentCounter--;
//            GameController.GetGameController().GetBookerC().UpdateCanvas();
//            interactable.Placed = false;
//        }
//    }
//    //private void OnTriggerEnter(Collider other)
//    //{
//    //    if (other.CompareTag("Booker") && GameController.GetGameController().GetPlayer().CanAttach())
//    //    {
//    //        GameController.GetGameController().AddInteractable(interactable);
//    //        GameController.GetGameController().GetBookerC().currentCounter++;
//    //        GameController.GetGameController().GetBookerC().UpdateCanvas();
//    //        interactable.Placed = true;
//    //    }
//    //}

//    //private void OnTriggerExit(Collider other)
//    //{
//    //    GameController.GetGameController().RemoveInteractable(interactable);
//    //    GameController.GetGameController().GetBookerC().currentCounter++;
//    //    GameController.GetGameController().GetBookerC().UpdateCanvas();
//    //    interactable.Placed = true;
//    //}
//}
