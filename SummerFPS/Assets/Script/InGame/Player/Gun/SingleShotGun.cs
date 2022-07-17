using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView PV;
    private bool canShoot = true;
    [SerializeField, Range(0, 1f)] private float fireRate;

    private Vector3 ScreenCenter;
    public GameObject cross;
    
    
    
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }

    public override void Use()
    {
        Shoot();
        StartCoroutine(Fire());
    }
    
    private IEnumerator Fire()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        var shot = ShotPool.Instance.Get();
        shot.transform.position = cross.transform.position;
        Debug.Log("shot.transform.position x" +shot.transform.position.x);
        Debug.Log("shot.transform.position y " +shot.transform.position.x);
        Debug.Log("shot.transform.position z" +shot.transform.position.x);
        shot.transform.rotation = transform.rotation;
        shot.gameObject.SetActive(true);
        canShoot = true;
    }
    
    

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("raytranfrom name " + hit.transform);
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            Health enemyHealth = hit.transform.GetComponent<Health>();
            if(enemyHealth != null)
            {
                Debug.Log("Shoot ");
                enemyHealth.TakeDamage(500);
            }
            
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal , RaycastHit hit)
    {
        
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }
    

}