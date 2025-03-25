using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int health = 10;
    public TMP_Text healthOnScreen;
    public TMP_Text defeatMessage;
    public TMP_Text restoreHealth;
    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        healthOnScreen.text = "Health: "+ health.ToString();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health--;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("DoubleHealth"))
        {
            health = 20;
            Destroy(collision.gameObject);
            StartCoroutine(ShowMessage(restoreHealth));
        }
        if (health <= 0)
        {
            DestroyAllBullets();
            health = 10;
            transform.position = startingPosition;
            StartCoroutine(ShowMessage(defeatMessage));
        }
    }

    IEnumerator ShowMessage(TMP_Text message)
    {
        message.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        message.gameObject.SetActive(false);
    }

    void DestroyAllBullets()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.name.Contains("Bullet(Clone)"))
            {
                Destroy(obj);
            }
        }
    }


}
