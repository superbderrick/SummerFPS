using Photon.Pun;

using UnityEngine;

public class SingleShotGun : Gun
{
	[SerializeField] Camera fpsCamera;
	public float range = 100f;
	public float damage = 500f;
	public float impactForce = 30f;
	
	PhotonView PV;

	void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	public override void Use()
	{
		Debug.Log("Use called");
		Shoot();
	}

	void Shoot()
	{
		Debug.Log("Shoot called");
		
		RaycastHit hit;
		if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
		{
			Debug.Log("Raycast");
			// Debug.Log(hit.transform.name);
			//
			// Health target = hit.transform.GetComponent<Health>();
			// if(target != null)
			// {
			// 	Debug.Log("Raycast22");
			// 	target.TakeDamage(damage);
			// }


		}
		
		
		// Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		// ray.origin = cam.transform.position;
		// if(Physics.Raycast(ray, out RaycastHit hit))
		// {
		// 	hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
		// 	PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
		// }
	}

	[PunRPC]
	void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
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
