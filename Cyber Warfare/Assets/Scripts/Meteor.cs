using UnityEngine;
using TMPro;

public class Meteor : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected int health;

    [SerializeField] protected TMP_Text textHealth;
    [SerializeField] protected float jumpForce;

    protected float[] leftAndRight = new float[2] { -1f, 1f };

    [HideInInspector] public bool isResultOfFission = true;

    protected bool isShowing;

    


    void Start()
    {
        UpdateHealthUI();
        isShowing = true;
        rb.gravityScale = 0f;

        if (isResultOfFission)
        {
            FallDown();
        }
        else
        {

            float direction = leftAndRight[Random.Range(0, 2)];
            float screenOffset = Game.Instance.screenWidth * 1.3f;
            transform.position = new Vector2(screenOffset * direction, transform.position.y);

            rb.velocity = new Vector2(-direction, 0f);
            Invoke("FallDown", Random.Range(screenOffset - 2.5f, screenOffset - 1f));
        }
    }

    void FallDown()
    {
        isShowing = false;
        rb.gravityScale = 1f;
        rb.AddTorque(Random.Range(-20f, 20f));
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("cannon"))
        {

            Debug.Log("gameover");
        }
        if (other.tag.Equals("missile"))
        {

            TakeDamage(1);
            /*Missiles.Instance.DestroyMissile(other.gameObject);*/

        }
        if (!isShowing && other.tag.Equals("wall"))
        {

            float posX = transform.position.x;
            if (posX > 0)
            {

                rb.AddForce(Vector2.left * 150f);
            }
            else
            {

                rb.AddForce(Vector2.right * 150f);
            }
            rb.AddTorque(posX * 4f);
        }
        if (other.tag.Equals("ground"))
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.AddTorque(-rb.angularVelocity * 4f);
        }


    }
    public void TakeDamage(int damage)
    {
        if (health > 1)
        {
            health -= damage;
        }
        else
        {
            Die();
        }
        UpdateHealthUI();
    }
    virtual protected void Die()
    {
        Destroy(gameObject);
    }
    protected void UpdateHealthUI()
    {
        textHealth.text = health.ToString();
    }
}

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] int health = 100;
    [SerializeField] int scoreValue = 10;

    [Header("Shooting Config")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 2f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectilespeed = 10f;

    [Header("Effects")]
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField][Range(0, 1)] float deathSFXVolume = 0.75f;
    [SerializeField] AudioClip projectileSFX;
    [SerializeField][Range(0, 1)] float projectileSFXVolume;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter < 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectilespeed);
        AudioSource.PlayClipAtPoint(projectileSFX, Camera.main.transform.position, projectileSFXVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer otherDamageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (otherDamageDealer == null) { return; }
        ProcessHit(otherDamageDealer);
    }

    private void ProcessHit(DamageDealer otherDamageDealer)
    {
        health -= otherDamageDealer.GetDamage();
        otherDamageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
