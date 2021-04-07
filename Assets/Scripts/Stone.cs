using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public bool collectable = false;
    public string thrower;
    public float speed;
    private Vector3 direction;
    public GameManager gameManager;
    public float sDistance; //distance avant arrêt
    public Vector3 enemyPosition;
    public float nDistance; //distance entre caillou et ennemi

    void Start()
    {
        collectable = false;
        //gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        nDistance = Vector3.Distance(enemyPosition, transform.position);

        if (!collectable)
        {
            if (thrower == "player" || sDistance >= nDistance)
                transform.position += direction.normalized * speed * Time.deltaTime;
            else
                collectable = true;
        }
        else if (transform.position.z == 0)
        {
            transform.position += new Vector3(0, 0, 1);
        }      
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Character child;

        if (col.gameObject.TryGetComponent<Character>(out child))
        {
            if (collectable)
            {
                //ramassé
                if (col.gameObject.name == "Player" && child.munitions < child.maxMunitions)
                {
                    SoundManager.PlaySound("PickStone");
                    col.gameObject.GetComponent<PlayerCharacter>().PickUp();
                    gameManager.stonesList.Remove(gameObject);
                    GameObject.Destroy(gameObject);

                }
                if (col.gameObject.name.Contains("Enemy") && child.munitions < child.maxMunitions)
                {
                    col.gameObject.GetComponent<Enemies>().PickUp();
                    gameManager.stonesList.Remove(gameObject);
                    GameObject.Destroy(gameObject);
                }
            }

            else if (!collectable)
            {
                //dégâts
                if (col.gameObject.tag == "Player" && thrower != "player")
                {
                    col.gameObject.GetComponent<PlayerCharacter>().TakeDamages();
                    collectable = true;
                }
                if (col.gameObject.tag == "enemy" && thrower != "enemy")
                {
                    col.gameObject.GetComponent<Enemies>().TakeDamages();
                    collectable = true;
                    gameManager.score += 1;
                }
            }
        }

        if (col.gameObject.tag == "bord")
        {
            collectable = true;
        }   
    }

    public void Throw(Vector3 dir, string currentThrower)
    {
        direction = dir;
        thrower = currentThrower;
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }

    public void StopDistance(float didi, Vector3 nmiP)
    {
        sDistance = Random.Range(didi, 15f);
        enemyPosition = nmiP;
    }
}
