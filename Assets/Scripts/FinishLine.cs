using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponent<Player>().OnGameComplete();
        }else
        if (other.tag == "NPC")
        {
            other.GetComponent<Character>().Finish();
        }
    }
}
