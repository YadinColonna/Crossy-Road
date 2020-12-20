using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Vector3 speed = Vector3.zero;
    public float lifetime = 10f;
    
    private float startTime;
    private void Start()
    {
        startTime = Time.time;
    }

    //Constantemente mover
    private void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition += speed * Time.deltaTime;
        transform.position = currentPosition;

        if (Time.time > startTime + lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
