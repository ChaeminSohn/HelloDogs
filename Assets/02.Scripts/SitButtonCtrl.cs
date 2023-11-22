using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SitButtonCtrl : MonoBehaviour
{
    public GameObject dogObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dogObj == null) {
            dogObj = GameObject.FindGameObjectWithTag("modelObject_Script");
        }
    }

    void OnButtonClick()
    {
        dogObj.GetComponentInChildren<DogCtrl>()?.GiveOrder("SitDown");
    }
}
