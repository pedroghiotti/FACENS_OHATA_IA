using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager flockManager;
    float speed;

    private void Start()
    {
        // Atribui � vari�vel �speed� um valor aleat�rio entre os valores m�nimo e m�ximo determinados no FlockManager.
        speed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed);
    }
    private void Update()
    {
        // Move o objeto no eixo Z em determinada velocidade.
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
}
