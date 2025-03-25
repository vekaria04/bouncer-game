using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Change : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            CheckObjectsAndDestroySelf();
        }
    }

    void CheckObjectsAndDestroySelf()
    {
        GameObject[] objectsWithLvl1Tag = GameObject.FindGameObjectsWithTag("HenchmanLvl1");

        if (objectsWithLvl1Tag.Length == 0)
        {
            Destroy(gameObject);
        }
    }
}