using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float staticSpeed = 0.1f;
    public float smoothTime = 1;
    public Transform myCharacter;

    private Vector3 targetPosition;
    private float originalZDistance;
    private Vector3 currentSmoothSpeed;

    private void Start()
    {
        Vector3 originalPlayerPos = myCharacter.position;
        Vector3 originalCameraPos = this.transform.position;

        //Define la distancia maxima que puede haber con el personaje
        originalZDistance = originalPlayerPos.z - originalCameraPos.z;

        targetPosition = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 currentPlayerPos = myCharacter.position;

        //Distancia actual con el personaje 
        float playerDistance = currentPlayerPos.z - targetPosition.z;

        if (playerDistance > originalZDistance)
        {
            float diff = playerDistance - originalZDistance;

            targetPosition.z += diff;
        } else
        {
            //Cuando el jugador no este muy lejos de la camara
            //Agregar posicion en el eje z
            targetPosition.z += staticSpeed * Time.deltaTime;
        }
        
        Vector3 finalPos = Vector3.SmoothDamp(transform.position, targetPosition, ref currentSmoothSpeed, smoothTime);
        //Posicionar camara
        transform.position = finalPos;
    }
}
