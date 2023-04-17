using UnityEngine;
using TMPro;

public class Meteor : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] int health;

    [SerializeField] TMP_Text textHealth;
    [SerializeField] float jumpForce;

    float[] leftAndRight = new float[2] { -1f, 1f };

    bool isShowing;

    void Start()
    {
        UpdateHealthUI();
        isShowing = true;
        rb.gravityScale = 0f;

        float direction = leftAndRight[Random.Range (0,2)];
        float screenOffset = Game.Instance.screenWidth * 1.3f;
        transform.position = new Vector2(screenOffset * direction, transform.position.y);

        rb.velocity = new Vector2(-direction, 0f);
        Invoke("FallDown", Random.Range(screenOffset - 2.5f, screenOffset - 1f));
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
            Missiles.Instance.DestroyMissile(other.gameObject);

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
    public void Die()
    {
        Destroy (gameObject);
    }
    public void UpdateHealthUI()
    {
        textHealth.text = health.ToString();
    }
}
