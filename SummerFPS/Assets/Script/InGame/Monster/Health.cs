using Photon.Pun;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 2000f;
    public PhotonView PhotonView;
    
    public void TakeDamage(float amount)
    {
        Debug.Log("TakeDamage " + amount);

        PhotonView.RPC("TakeHitRPC", RpcTarget.All, amount);
        
    }

    void Die()
    {
        Debug.Log("Die ");
        Destroy(gameObject);
    }
    
    [PunRPC]
    public void TakeHitRPC(float amount)
    {
        Debug.Log("damage " + amount);
        Debug.Log("current health " + health);
        
        health -= amount;
        
        if(health <= 0f)
        {
            Debug.Log("die " + health);
            Die();
        }
    }

}
