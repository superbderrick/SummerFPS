using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 2000f;
    public PhotonView PhotonView;
    public Text monsterHP;
    public void TakeDamage(float amount)
    {
        Debug.Log("TakeDamage " + amount);

        PhotonView.RPC("TakeHitRPC", RpcTarget.All, amount);
        
    }

    private void Start()
    {
        monsterHP.text = "Monster HP : " + health;
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

        monsterHP.text = "Monster HP : " + health;
        
        if(health <= 0f)
        {
            Debug.Log("die " + health);
            Die();
        }
    }

}
