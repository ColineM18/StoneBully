using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public bool collectable = false;
    public string thrower;
    public float speed;
    public bool type;
    private Vector3 direction;
    public bool up;
    public bool shootDown;
    public GameManager gameManager;
    public float sDistance; //distance avant arrêt
    public Vector3 nmiPos;
    public float nDistance; //distance entre caillou et ennemi

    void Start()
    {
        collectable = false;
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }

    public void CollectableOrNot(bool na, string throwerOrigin)
    {
        type = na; //inutile?
        thrower = throwerOrigin;
    }

    public void ShootDir(bool UpOrDown)
    {
        shootDown = UpOrDown;
    }

    private void Update()
    {
        nDistance = Vector3.Distance(nmiPos, transform.position);

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
        if (collectable)
        {
          //ramassé
            if (col.gameObject.name == "Player" && col.gameObject.GetComponent<PlayerCharacter>().munitions < col.gameObject.GetComponent<PlayerCharacter>().maxMunitions)
            {
                SoundManager.PlaySound("PickStone");
                col.gameObject.GetComponent<PlayerCharacter>().PickUp();
                GameObject.Destroy(this.gameObject);

            }
            if (col.gameObject.name.Contains("Enemy") && col.gameObject.GetComponent<Enemies>().munitions < col.gameObject.GetComponent<Enemies>().maxMunitions)
            {
                col.gameObject.GetComponent<Enemies>().PickUp();
                GameObject.Destroy(this.gameObject);
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

        if (col.gameObject.tag == "bord")
        {
            collectable = true;
        }


    }

    public void Throw(Vector3 dir, string currentThrower)
    {
        direction = dir;
        thrower = currentThrower;
    }

    public void StopDistance(float didi, Vector3 nmiP)
    {
        sDistance = Random.Range(didi, 15f);
        nmiPos = nmiP;
    }
}
