/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public Rigidbody MyRb;
    public float speed;
    public GameObject Explosion;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        MyRb.AddForce(transform.forward * speed);
        Destroy(this.gameObject, 4);
    }

    void OnTriggerEnter(Collider col)
    {
        GameObject go = Instantiate(Explosion, transform.position, transform.rotation);
        go.SetActive(true);
        Destroy(go, 1);
        Destroy(this.gameObject);

        if (col.CompareTag("Henchman"))
        {
            col.GetComponent<EnemyAiTutorial>().TakeDamage(damage);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody MyRb;
    public float speed;
    public GameObject Explosion;
    public int damage;
    private float bulletLifetime = 4f; // Lifetime of the bullet in seconds
    public int bossHealth = 10;


    // Start is called before the first frame update
    void Start()
    {
        MyRb.AddForce(transform.forward * speed);
        Destroy(gameObject, bulletLifetime); // Destroy the bullet after a certain lifetime
    }

    // Update is called once per frame
    void Update()
    {
        // Move the bullet in a straight line
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        // Check for collision along the bullet's path
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime))
        {
            // If the bullet hits something, instantiate an explosion
            GameObject go = Instantiate(Explosion, hit.point, Quaternion.identity);
            go.SetActive(true);
            Destroy(go, 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HenchmanLvl1") || collision.gameObject.CompareTag("HenchmanLvl2") || collision.gameObject.CompareTag("HenchmanLvl3"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            bossHealth--;
                if (bossHealth == 0) ;
            {
                Destroy(collision.gameObject);
            }
        }
    }


}