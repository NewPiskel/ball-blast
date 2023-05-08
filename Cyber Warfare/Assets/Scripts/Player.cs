/*using UnityEngine;

public class Player : MonoBehaviour
{
    Camera cam;
    Rigidbody2D rb; 
    
    [SerializeField] HingeJoint2D[] wheels;
    JointMotor2D motor;

    [SerializeField] float CannonSpeed;
     bool isMoving = false;

    Vector2 pos;
    float screenBounds;
    float velocityX;
  
    void Start()
    {
        cam = Camera.main;

        rb = GetComponent<Rigidbody2D> ();
        pos = rb.position;

        motor = wheels [0].motor; 


        screenBounds = Game.Instance.screenWidth - 0.56f;
    }

   
    void Update()
    {
        isMoving = Input.GetMouseButton(0);

        if (isMoving)
        {
            pos.x = cam.ScreenToWorldPoint(Input.mousePosition).x;
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition (Vector2.Lerp (rb.position, pos, CannonSpeed * Time.fixedDeltaTime));
        }else{
            rb.velocity = Vector2.zero;
        }

        velocityX = rb.GetPointVelocity(rb.position).x;
        if (Mathf.Abs (velocityX) > 0.0f && Mathf.Abs (rb.position.x) < screenBounds)
        {
            motor.motorSpeed = velocityX * 150f;
            MotorActivate (true);
        }
        else
        {
            motor.motorSpeed = 0f;
            MotorActivate (false);
        }
    } 

    void MotorActivate(bool isActive)
    {
        wheels [0].useMotor = isActive;
        wheels [1].useMotor = isActive;

        wheels [0].motor = motor;
        wheels [1].motor = motor;
    }
}*/
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 100;

    [Header("Audio")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField][Range(0, 1)] float deathSFXVolume = 0.75f;
    [SerializeField] AudioClip projectileSFX;
    [SerializeField][Range(0, 1)] float projectileSFXVolume;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileCoolDown = 0.1f;


    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
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
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
            FindObjectOfType<SceneLoader>().LoadGameOver();
            {
                //SceneManager.LoadScene(2);
            }
        }
    }

    private void Move()
    {
        float changeX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float newXPosition = Mathf.Clamp(transform.position.x + changeX, xMin, xMax);

        float changeY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        float newYPosition = Mathf.Clamp(transform.position.y + changeY, yMin, yMax);

        transform.position = new Vector2(newXPosition, newYPosition);
    }


    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            /*AudioSource.PlayClipAtPoint(projectileSFX, Camera.main.transform.position, projectileSFXVolume);*/
            yield return new WaitForSeconds(projectileCoolDown);

        }
    }
    public int GetHealth()
    {
        return health;
    }

}

