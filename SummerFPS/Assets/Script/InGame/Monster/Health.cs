using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health = 2000;
    public PhotonView PhotonView;
    public Text monsterHP;
    public void TakeDamage(int amount)
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
    public void TakeHitRPC(int amount)
    {
        Debug.Log("damage " + amount);
        Debug.Log("current health " + health);
        
        health -= amount;

        monsterHP.text = "Monster HP : " + health;
        
        if(health <= 0)
        {
            Debug.Log("die " + health);
            Die();
        }
    }

}
