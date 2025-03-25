using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public TMP_Text scoreNumber, Score;
    public int score;
    //public int healthScore;
    
    void Start()
    {
        //healthScore = 10;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Rock")
        {
            score++;
            Destroy(hit.gameObject);
            Debug.Log(score + " health lost");
            //Score.text = score + " health lost";
        }
        Destroy(hit.gameObject);
    }
}

