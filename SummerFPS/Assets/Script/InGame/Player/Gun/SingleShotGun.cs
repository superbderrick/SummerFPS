using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField]
    Camera cam;

    PhotonView PV;
    [SerializeField, Range(0, 1f)]
    private float fireRate;
    [SerializeField]
    private int power;
    public Transform spawnPoint;

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
        yield return new WaitForSeconds(fireRate);
        PV.RPC("TakeHitRPC", RpcTarget.All);
    }

    [PunRPC]
    public IEnumerator TakeHitRPC()
    {
        var shot = ShotPool.Instance.Get();

        shot.transform.position = spawnPoint.position;
        shot.transform.rotation = transform.rotation;
        shot.gameObject.SetActive(true);

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            MonsterManager enemyMonsterManager = hit.transform.GetComponent<MonsterManager>();
            if (enemyMonsterManager != null)
            {
                enemyMonsterManager.TakeDamage(power);
            }
        }

        yield return null;
    }
}