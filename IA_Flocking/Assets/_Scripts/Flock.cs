using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager flockManager;
    float speed;
    bool turning = false;

    private void Start()
    {
        // Atribui � vari�vel �speed� um valor aleat�rio entre os valores m�nimo e m�ximo determinados no FlockManager.
        speed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed);
    }
    private void Update()
    {
        Bounds bounds = new Bounds(flockManager.transform.position, flockManager.swimLimits * 2);
        RaycastHit hit = new RaycastHit();
        Vector3 direction = flockManager.transform.position - transform.position;

        /*
            Caso o peixe saia dos limites estabelecidos ou esteja perto de colidir com o pilar,
            desvia sua movimenta��o para desviar do pilar ou voltar para a �rea limite.
        */
        if (bounds.Contains(transform.position) == false)
        {
            turning = true;
            direction = flockManager.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            turning = false;
        }
        if (turning)
        {
            transform.rotation = Quaternion.Slerp
            (
                transform.rotation,
                Quaternion.LookRotation(direction),
                flockManager.rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            // Introduz varia��o na velocidade dos peixes.
            if (Random.Range(0, 100) < 10)
            {
                speed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed);
            }

            /*
                Introduz varia��o na movimenta��o dos peixes.
                Alterna aleat�riamente entre aplicar as regras de rota��o e manter movimento na dire��o atual.
            */
            if (Random.Range(0, 100) < 20)
            {
                ApplyRules();
            }
        }

        // Move o objeto no eixo Z (Local) em determinada velocidade.
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    private void ApplyRules()
    {
        GameObject[] arrayGameObjects = flockManager.allFish;

        Vector3 posFlockCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;

        float groupSpeed = 0.01f;
        float distanceToOther;
        float groupSize = 0;

        /*
            Agrupa os peixes com base na 'neighbourDistance' definida no 'FlockManager'.
        */
        foreach (GameObject fish in arrayGameObjects)
        {
            if (fish != this.gameObject)
            {
                distanceToOther = Vector3.Distance(fish.transform.position, this.transform.position); // Calcula a dist�ncia entre o objeto analisado no foreach e este objeto.

                /*
                    Se a dist�ncia entre este objeto e o objeto analisado foi menor do que a definida no 'flockManager'
                    Aplica a movimenta��o deste objeto ao redor do centro do grupo formado.
                */
                if (distanceToOther <= flockManager.neighbourDistance)
                {
                    posFlockCentre += fish.transform.position;
                    groupSize++;

                    /*
                        Caso a dist�ncia entre este objeto e o objeto analisado no foreach for menor do que 1,
                        desvia o movimento para evitar colis�o.
                    */
                    if (distanceToOther < 1f)
                    {
                        vAvoid = vAvoid + (this.transform.position - fish.transform.position);
                    }

                    Flock flockOtherFish = fish.GetComponent<Flock>();
                    groupSpeed = groupSpeed + flockOtherFish.speed;
                }
            }
        }

        /*
            Define a posi��o central do grupo formado. 
        */
        if (groupSize > 0)
        {
            posFlockCentre = posFlockCentre / groupSize;
            speed = groupSpeed / groupSize;

            Vector3 direction = (posFlockCentre + vAvoid) - this.transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp
                (
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    flockManager.rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}
