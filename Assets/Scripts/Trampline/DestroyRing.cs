using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRing : MonoBehaviour
{
    void OnEnable ()
    {
        StartCoroutine(DestroyRingEffect());
    }

    IEnumerator DestroyRingEffect()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
