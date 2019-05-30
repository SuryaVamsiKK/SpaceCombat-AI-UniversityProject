using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldToScreen : MonoBehaviour
{
    public Vector3 offset;
    public Camera cam;

    void Start()
    {
        
    }

    void Update()
    {
        this.GetComponent<RectTransform>().anchoredPosition = cam.WorldToScreenPoint(transform.parent.parent.position) + offset; 
        this.GetComponent<Slider>().value = transform.parent.parent.GetComponent<Health>().health/100; 
    }
}
