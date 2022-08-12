
using Jinyoung.dev.summerfps.Utills;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public delegate void IntEventHandler(int value);

public class MonsterManager : MonoBehaviour
{
    public static IntEventHandler onHpChange;
    
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
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }
    
    [PunRPC]
    public void TakeHitRPC(int amount)
    {
        Health -= amount;
        onHpChange?.Invoke(Health);
        MonsterHPText.text = CommonUtils.GetStringMessage("Monster HP : " , Health.ToString());
        
        if(Health <= 0)
        {
            Debug.Log("die " + Health);
            Die();
        }
    }

}
