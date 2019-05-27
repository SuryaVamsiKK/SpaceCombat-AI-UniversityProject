using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject Bullet;
        
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject g = Instantiate(Bullet, transform.position, transform.rotation);
        }
    }
}
