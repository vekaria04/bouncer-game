using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Change : MonoBehaviour
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
        GameObject[] objectsWithLvl2Tag = GameObject.FindGameObjectsWithTag("HenchmanLvl2");

        if (objectsWithLvl2Tag.Length == 0)
        {
            Destroy(gameObject);
        }
    }
}