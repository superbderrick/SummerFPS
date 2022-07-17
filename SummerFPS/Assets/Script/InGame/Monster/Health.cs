using System;
using Com.LGUplus.Homework.Minifps.Utills;
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
        monsterHP.text = CommonUtils.GetStringMessage("Monster HP : " , health.ToString());
    }

    void Die()
    {
        Debug.Log("Die ");
        Destroy(gameObject);
    }
    
    [PunRPC]
    public void TakeHitRPC(int amount)
    {
        
        health -= amount;

        monsterHP.text = CommonUtils.GetStringMessage("Monster HP : " , health.ToString());
        
        if(health <= 0)
        {
            Debug.Log("die " + health);
            Die();
        }
    }

}
