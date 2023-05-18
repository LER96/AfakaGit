using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThrough : MonoBehaviour
{
    [SerializeField] bool isActive=true;
    [SerializeField] List<GameObject> objectsToHide;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(isActive);
        if (other.transform.tag == "Player")
        {
            isActive = false;
            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(isActive);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            isActive = true;
            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(isActive);
            }
        }
    }
}
