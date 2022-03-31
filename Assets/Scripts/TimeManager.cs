using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float slowdownFactor = 0.75f;
    [SerializeField] float slowdownDuration = 1f;

    void Update()
    {
        Time.timeScale += (1f / slowdownDuration) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void DoSlowMOtion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = 0.02f * slowdownFactor;
    }

}
