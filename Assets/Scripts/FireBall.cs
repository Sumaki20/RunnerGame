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
        enemy = GameObject.FindObjectOfType<Enemy>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            playerMovement.TakeDamage(damage);
        }
        if (collision.gameObject.name == "Enemy")
        {
            //enemy.TakeDamage(damage); HitAnything()
        }

        HitTarget();

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject, 10);
        }
    }

    void HitTarget ()
    {
        GameObject effectIns = (GameObject) Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 0.3f);

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
