using UnityEngine;

public class Stone : MonoBehaviour
{
    private GameManager gameManager;
    public bool collectable;
    public Character.CharacterType thrower;
    public float speed;
    private Vector3 direction;
    public Vector3 enemyPosition;
    public float stopDistance; //distance avant arrêt 
    public float separationDistance => Vector3.Distance(enemyPosition, transform.position);

    private void Update()
    {
        if (!collectable)
        {
            if (thrower == Character.CharacterType.Player || stopDistance >= separationDistance)
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
                if (col.gameObject.name == "Player" && child.munitions < gameManager.maxMunitions)
                {
                    SoundManager.PlaySound("PickStone");
                    col.gameObject.GetComponent<PlayerCharacter>().PickUp();
                    gameManager.stonesList.Remove(gameObject.GetComponent<Stone>());
                    GameObject.Destroy(gameObject);

                }
                if (col.gameObject.name.Contains("Enemy") && child.munitions < gameManager.maxMunitions)
                {
                    col.gameObject.GetComponent<Enemy>().PickUp();
                    gameManager.stonesList.Remove(gameObject.GetComponent<Stone>());
                    GameObject.Destroy(gameObject);
                }
            }

            else if (!collectable)
            {
                //dégâts
                if (col.gameObject.tag == "Player" && thrower == Character.CharacterType.Enemy)
                {
                    col.gameObject.GetComponent<PlayerCharacter>().TakeDamages();
                    collectable = true;
                }
                if (col.gameObject.tag == "enemy" && thrower == Character.CharacterType.Player)
                {
                    col.gameObject.GetComponent<Enemy>().TakeDamages();
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

    public void Throw(Vector3 dir, Character.CharacterType currentThrower)
    {
        direction = new Vector3(dir.x * Time.deltaTime, dir.y * Time.deltaTime, 0);
        thrower = currentThrower;
        gameManager = GameManager.instance;
        transform.parent = gameManager.transform;
        gameManager.stonesList.Add(this);
    }

    public void StopDistance(float distance, Vector3 enemyPosition)
    {
        //stop enemies stones after an amount of time, because it does not stop, compared to player's stone whose aim for the mouse position
        stopDistance = Random.Range(distance, 15f);
        this.enemyPosition = enemyPosition;
    }
}
