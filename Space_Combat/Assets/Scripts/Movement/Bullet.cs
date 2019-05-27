using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public float Damage;
    public float maxTime;

    private void Awake()
    {
        StartCoroutine(AutoDestroy());
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * Speed;
        //transform.Translate(transform.forward * Time.deltaTime * Speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PivotRefrence>() != null)
        {
            collision.gameObject.GetComponent<PivotRefrence>().HealthRefrence.health -= Damage;
            Destroy(this.gameObject);
            StopCoroutine(AutoDestroy());
        }
    }

    public IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(maxTime);
        Destroy(this.gameObject);
    }
}
