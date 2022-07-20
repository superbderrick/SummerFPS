using System;
using Com.LGUplus.Homework.Minifps.Utills;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManager : MonoBehaviour
{
    public int Health = 2000;
    public PhotonView PhotonView;
    public Text MonsterHPText;
    public void TakeDamage(int amount)
    {
        PhotonView.RPC("TakeHitRPC", RpcTarget.All, amount);
    }
    private void Start()
    {
        MonsterHPText.text = CommonUtils.GetStringMessage("Monster HP : " , Health.ToString());
    }

    void Die()
    {
        Debug.Log("Die ");
        Destroy(gameObject);
    }
    
    [PunRPC]
    public void TakeHitRPC(int amount)
    {
        Health -= amount;
        MonsterHPText.text = CommonUtils.GetStringMessage("Monster HP : " , Health.ToString());
        
        if(Health <= 0)
        {
            Debug.Log("die " + Health);
            Die();
        }
    }

}
