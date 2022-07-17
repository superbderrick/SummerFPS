using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView PV;
    
    [SerializeField, Range(0, 1f)] private float fireRate;
    
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void Use()
    {
        StartCoroutine(Fire());
    }
    
    private IEnumerator Fire()
    {
        // canShoot = false;
         yield return new WaitForSeconds(0.125f);
         PV.RPC("TakeHitRPC", RpcTarget.All);
    }
    
    [PunRPC]
    public IEnumerator TakeHitRPC()
    {
        var shot = ShotPool.Instance.Get();
        
        Debug.Log("transform.position derrick x " + transform.position.x);
        Debug.Log("transform.position derrick y " + transform.position.y);
        Debug.Log("transform.position derrick z " + transform.position.z);
        
        shot.transform.position = transform.position;
        shot.transform.rotation = transform.rotation;
        shot.gameObject.SetActive(true);
        
        
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            Health enemyHealth = hit.transform.GetComponent<Health>();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage(500);
            }
            
        }

        yield return null;

    }
    

}