using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{

    [SerializeField] GameObject normalObj;
    [SerializeField] GameObject breakObj;

    private void Start()
    {
        normalObj.SetActive(true);
        breakObj.SetActive(false);
    }
    public void Break()
    {
        normalObj.SetActive(false);
        breakObj.SetActive(true);
    }
}
