using UnityEngine;

public class MainGun : MonoBehaviour
{
    public float damage = 500f;
    public float range = 100f;
    public Camera fpsCam;
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Health enemyHealth = hit.transform.GetComponent<Health>();
            if(enemyHealth != null)
            {
                Debug.Log("Shoot ");
                enemyHealth.TakeDamage(damage);
            }
        }
    }

}
