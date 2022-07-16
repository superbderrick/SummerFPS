using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 1000f;
    
    public void TakeDamage(float amount)
    {
        Debug.Log("amount " + amount);
        health -= amount;
        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Die ");
        Destroy(gameObject);
    }

}
