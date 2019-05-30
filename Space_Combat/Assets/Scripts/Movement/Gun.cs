using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public EType type;
    public GameObject Bullet;
    public float fireRate;
    bool shootable = true; 

    void Update()
    {
        if (type == EType.Player)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
    }

    IEnumerator ShootingYield(float fireRate)
    {
        yield return new WaitForSeconds(fireRate);
        shootable = true;
    }

    void Fire()
    {
        GameObject g = Instantiate(Bullet, transform.position, transform.rotation);
    }

    public void Shoot()
    {
        if (shootable)
        {
            shootable = false;
            Fire();
            StartCoroutine(ShootingYield((100 - fireRate) / 100));
        }
    }
}
