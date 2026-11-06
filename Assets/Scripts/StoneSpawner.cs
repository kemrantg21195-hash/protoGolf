using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    public GameObject stonePref; 
    public int poolSize = 20;     

    private List<GameObject> pool; 
    private int currIndex = 0;  

    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject stone = Instantiate(stonePref);
            stone.SetActive(false); 
            pool.Add(stone);        
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) 
        {
            SpawnStone();
        }
    }

    void SpawnStone()
    {
        GameObject currStone = pool[currIndex];

        currStone.SetActive(true);
        currStone.transform.position = transform.position;

        Rigidbody rb = currStone.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        currIndex = currIndex + 1;
        if (currIndex >= poolSize)
        {
            currIndex = 0;
        }
    }
}