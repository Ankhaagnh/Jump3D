using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy instance;
    public List<Transform> allBottomLaneTramplines = new List<Transform>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        if(instance!=this)
            Destroy(this.gameObject);
    }
    public Transform GetNearestBottomTrampline(Vector3 pos)
    {
        float dist = 1000;
        int nearestPlatformIndex = 0;
        for (int i = 0; i < allBottomLaneTramplines.Count - 1; i++)
        {
            float distance = Vector3.Distance(pos, allBottomLaneTramplines[i].position);
            if (distance < dist)
            {
                dist = distance;
                nearestPlatformIndex = i;
            }
        }
        return allBottomLaneTramplines[nearestPlatformIndex];
    }
}
