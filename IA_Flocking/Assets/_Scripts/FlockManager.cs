using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5, 5, 5);

    [Header("Configura��es do Cardume")]
    [Range(0, 5f)] public float minSpeed;
    [Range(0, 5f)] public int maxSpeed;

    private void Start()
    {
        // Inicializa o array �allFish� com o tamanho determinado na variavel �numFish�.
        allFish = new GameObject[numFish];

        // Popula o array �numFish� com inst�ncias criadas da prefab �fishPrefab� em posi��es aleat�rias dentro dos limites definidos em �swimLimits� e 
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));

            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().flockManager = this;
        }
    }
}
