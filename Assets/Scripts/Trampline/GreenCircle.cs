using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCircle : MonoBehaviour
{
    [SerializeField] GameObject greenCircle = null;
    [SerializeField] float initiationDistance = 5f;
    GameObject player;

    void Awake()
    {
        player =  GameObject.FindWithTag("Player");
    }
    
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= initiationDistance)
        {
            greenCircle.SetActive(true);
        }
    }
}
