using UnityEngine;
using System.Collections;
public class BulletPooled : MonoBehaviour
{

    [SerializeField] private float BulletSpeed;
    [SerializeField] private float lifetime;
    private Rigidbody rb;

    private void OnEnable() {
        StartCoroutine(BulledDie());
        rb = GetComponent<Rigidbody>();
                  rb.velocity =rb.transform.forward* BulletSpeed * Time.deltaTime*10;

    }

    private void FixedUpdate()
    {
    }

    private IEnumerator BulledDie()
    {
        yield return new WaitForSeconds(lifetime);
        ShotPool.Instance.ReturnToPool(this);
    }

}
