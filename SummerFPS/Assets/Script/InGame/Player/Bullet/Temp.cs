using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour
{
    private bool canShoot = true;
    [SerializeField, Range(0, 1f)] private float fireRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TaskOnClick(){
        StartCoroutine(Fire());
    }
    
    private IEnumerator Fire()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        // animator.SetTrigger("Shoot");
        // source.Play();
        var shot = ShotPool.Instance.Get();
        //particles.Emit(1);
        shot.transform.position = transform.position;
        shot.transform.rotation = transform.rotation;
        shot.gameObject.SetActive(true);
        canShoot = true;


    }
}
