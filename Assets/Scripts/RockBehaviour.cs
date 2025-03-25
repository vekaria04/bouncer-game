using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RockBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Text message;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            Debug.Log("Object Dodged");
            Destroy(gameObject);
        }
    }
}
