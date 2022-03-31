using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    // Update is called once per frame
    void Update()
    {
        RotateAlonX();
    }
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().OnGameOver();
        }
    }
    private void RotateAlonX()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
    }
}
