using UnityEngine;

public class FireBall : MonoBehaviour
{
    public int damage;
    public GameObject impactEffect;

    PlayerMovement playerMovement;
    Enemy enemy;

    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            playerMovement.TakeDamage(damage);
        }
        if (collision.gameObject.name == "Enemy")
        {
            
            enemy.DestroyObject();
        }

        HitTarget();

    }

    void HitTarget ()
    {
        GameObject effectIns = (GameObject) Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        Destroy(gameObject);
    }
    /*
    private Transform target;

    public float speed = 70f;

    public void Seek(Transform _target)
    {
        target = _target;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) 
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = transform.position;
        float distanceThisFram = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFram )
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFram, Space.World);
    }
    void HitTarget ()
    {
        GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy (gameObject);
    }*/
}
