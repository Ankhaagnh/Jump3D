using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneController.instance.OnGameOver();
        }
        else
        if (other.tag != "Trampline") {
            other.gameObject.SetActive(false);
        }
    }
}
