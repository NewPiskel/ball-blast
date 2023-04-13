using UnityEngine;
using TMPro;

public class Meteor : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int health;

    [SerializeField] TMP_Text textHealth;
    [SerializeField] float jumpForce;
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.Equals("cannon"))
        {

        }else if(other.Equals("missile"))
        {

        }
        else if (other.Equals("wall"))
        {
            float posX = transform.position.x;
            if (posX > 0)
            {

            }
            else
            {

            }else if (other.Equals("ground"))
            {

            }
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

    }
    public void HealthUpdateUI()
    {
        textHealth.text = health.ToString();
    }
}
