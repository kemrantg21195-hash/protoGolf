using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public GameObject cloud; 
    public ParticleSystem rain; 
    public Transform[] targets; 
    
    [Header("Настройки облака")]
    public float speed = 5f;
    public float cloudHeight = 5f; 
    
    int currentTarget = 0;
    bool moving = false; 

    void Start()
    {
       
        cloud.transform.position = targets[0].position + new Vector3(0, cloudHeight, 0); 
        rain.Play();
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (moving == false) 
            {
                moving = true;
                rain.Stop(); 
                
                currentTarget = currentTarget + 1;
                
              
                if (currentTarget >= targets.Length)
                {
                    currentTarget = 0;
                }
            }
        }

       
        if (moving == true)
        {
            
            Vector3 targetPos = targets[currentTarget].position + new Vector3(0, cloudHeight, 0);
            
            cloud.transform.position = Vector3.MoveTowards(cloud.transform.position, targetPos, speed * Time.deltaTime);

            if (Vector3.Distance(cloud.transform.position, targetPos) < 0.1f)
            {
                moving = false;
                rain.Play(); 
            }
        }
    }
}