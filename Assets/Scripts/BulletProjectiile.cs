using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BulletProjectiile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitRed;
    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 10f;
        bulletRigidbody.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
       // if (other.GetComponent<BulletTarget>() ! = null)
       // {
            //Hit target
       // }
       
       // else
       // {
            //Hit something else
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
       // }
        Destroy(gameObject);
    }
}
