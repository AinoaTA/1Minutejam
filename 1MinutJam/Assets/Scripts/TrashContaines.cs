using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashContaines : MonoBehaviour
{
    [SerializeField] private List<GameObject> TrashCollector;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            print("Muy bien, era basura!");
            TrashCollector.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}
