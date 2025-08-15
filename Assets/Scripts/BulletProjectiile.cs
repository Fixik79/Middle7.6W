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
        float speed = 30f;
        bulletRigidbody.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {     
            //Hit something else
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);       
        Destroy(gameObject);
    }
}
