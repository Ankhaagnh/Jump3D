using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager instance;
    public float tramplineRadius = 1.8f;

    [SerializeField] private int laneCount = 250;
    [SerializeField] GameObject normalTrampline;
    [SerializeField] GameObject bottomTrampline;
    [SerializeField] GameObject fan;
    [SerializeField] GameObject finishLine;
    [SerializeField] GameObject m_disctructableTrampline;
    [SerializeField] GameObject m_disctructableTramplineForRoad;
    [SerializeField] LineRenderer line;
    [SerializeField] private List<Transform> allTramplines = new List<Transform>();

    Vector4 limitsForBottomTrampline = Vector4.zero;
    public GameObject DisctructableTrampline { get { return m_disctructableTrampline; }}
    public List<Transform> AllTramplines { get { return allTramplines; } }
    private float amp = 0;
    private float ampNext = 0;
    private float vel = 0;
    private float vel1 = 0;
    private float ht = 0;
    private float htNext = 0;
    public static bool bottomTramplineGenerated = false;
    private bool generatingTrampline;
    private void Awake()
    {
        instance = this;
    }
    private void FixedUpdate()
    {
        if (generatingTrampline)
            return;
        generatingTrampline = true;
        GenerateTramplines();
        if (!bottomTramplineGenerated) {
            GenerateBottomTrampline();
        }
    }
    public Transform GetNextTrampline(Transform trampline) {
        for (int i=0; i<allTramplines.Count-1; i++) {
            if (trampline == allTramplines[i]) {
                return allTramplines[i + 1];
            }
        }
        return null;
    }

    public Transform GetNearestTrampline(Transform trampline)
    {
        float dist = 1000;
        int nearestPlatformIndex = 0;
        for (int i = 0; i < allTramplines.Count - 1; i++)
        {
            float distance = Vector3.Distance(trampline.position, allTramplines[i].position);
            if (distance < dist&& trampline!=allTramplines[i])
            {
                dist = distance;
                nearestPlatformIndex = i;
            }
        }
        return allTramplines[nearestPlatformIndex];
    }
    public Transform GetNearestNextTrampline(Transform trampline)
    {
        float dist = 1000;
        int nearestPlatformIndex = 0;
        int prevPlatformIndex = -1;
        for (int i = 0; i < allTramplines.Count; i++)
        {
            float distance = Vector3.Distance(trampline.position, allTramplines[i].position);
            if (trampline == allTramplines[i]) {
                prevPlatformIndex = i;
            }
            if (distance < dist && trampline != allTramplines[i]&&prevPlatformIndex>=0&&prevPlatformIndex<i)
            {
                dist = distance;
                nearestPlatformIndex = i;
            }
        }
        return allTramplines[nearestPlatformIndex];
    }

    public int GetNumberOfTrumpoline()
    {
        return allTramplines.Count;
    }
    bool CheckBottomTramplineOverLap(Vector3 pos) {
        Vector3 tempPos = pos;
        for (int i = 0; i < DontDestroy.instance.allBottomLaneTramplines.Count; i++) {
            if (Vector3.Distance(DontDestroy.instance.allBottomLaneTramplines[i].position, pos) < 4) {
                return true;
            }
        }
        return false;
    }
    public void BlastTrampline()
    {
        if(m_disctructableTrampline.activeSelf)
            m_disctructableTrampline.GetComponent<DistructableTrampline>().Blast();
    }
    void GenerateBottomTrampline()
    {
        bottomTramplineGenerated = true;
        for (int i = 0; i < 30; i++) {
          var temp =  Instantiate(bottomTrampline, bottomTrampline.transform.position,
                        bottomTrampline.transform.rotation, DontDestroy.instance.transform) as GameObject;
            temp.transform.position = GeneratePosition();
            DontDestroy.instance.allBottomLaneTramplines.Add(temp.transform);
        }
    }
    Vector3 GeneratePosition() {
        Vector3 temp = new Vector3(Random.Range(limitsForBottomTrampline.z, limitsForBottomTrampline.x), bottomTrampline.transform.position.y, Random.Range(limitsForBottomTrampline.w, limitsForBottomTrampline.y));
        if (CheckBottomTramplineOverLap(temp)) {
            temp =  GeneratePosition();
        }
        return temp;
    }
    void UpdateBottomTramplineLimit(Vector3 pos, ref Vector4 limit)
    {
        if (bottomTramplineGenerated)
            return;
        if (pos.x > 0&&pos.x>=limit.x) {
            limit.x = pos.x;
        }
        if (pos.z > 0 && pos.z >= limit.y)
        {
            limit.y = pos.z;
        }
        if (pos.x <= 0 && pos.x <= limit.z)
        {
            limit.z = pos.x;
        }
        
        if (pos.z <= 0 && pos.z <= limit.w)
        {
            limit.w = pos.z;
        }
    }
    void GenerateTramplines() {
        htNext = 0;
        ampNext = 0;
        amp = 0;
        ht = 0;
        vel = 0;
        vel1 = 0;
        float RangeIndex = Random.Range(150, 350);
        laneCount += GameManager.currentLevel * 10;
        line.positionCount = laneCount;
        Vector3 lastPos = Vector3.zero;
        GameObject temp = Instantiate(normalTrampline, new Vector3((Mathf.Sin((ht) * Time.deltaTime) * amp), -10 * Time.deltaTime, (Mathf.Cos((-ht) * Time.deltaTime) * amp)), normalTrampline.transform.rotation) as GameObject;
        UpdateBottomTramplineLimit(temp.transform.position, ref limitsForBottomTrampline);
        allTramplines.Add(temp.transform);
        temp.GetComponent<Trampline>().noPoint = true;
        lastPos = temp.transform.position;
        int instantiatedDistTramps = 0;
        for (int i = 0; i < laneCount; i++) {
            ht = Mathf.SmoothDamp(ht, htNext, ref vel, 2f);
            amp = Mathf.SmoothDamp(amp, ampNext, ref vel1, 0.2f);
            line.SetPosition(i, new Vector3((Mathf.Sin((ht) * Time.deltaTime) * amp), -i * 10 * Time.deltaTime, (Mathf.Cos((-ht) * Time.deltaTime) * amp)));
            if (i % 10 == 0)
            {
                htNext = Random.Range(250, 350);
                ampNext = Random.Range(25, 35);
            }
            if (Vector3.Distance(line.GetPosition(i), lastPos) >4.7f&&i<laneCount-4)
            {
                Vector3 direction = Vector3.zero;
                direction = lastPos - line.GetPosition(i);
                direction.y = 0;
                if (Random.Range(0, 10) == 1) {
                    instantiatedDistTramps++;
                    temp = Instantiate(m_disctructableTramplineForRoad, new Vector3((Mathf.Sin(ht * Time.deltaTime) * amp), -i * 10 * Time.deltaTime, (Mathf.Cos(-ht * Time.deltaTime) * amp)), Quaternion.LookRotation(direction)) as GameObject;
                    temp.transform.rotation = Quaternion.LookRotation(direction);
                }
                else
                    temp = Instantiate(normalTrampline, new Vector3((Mathf.Sin(ht * Time.deltaTime) * amp), -i * 10 * Time.deltaTime, (Mathf.Cos(-ht * Time.deltaTime) * amp)), Quaternion.LookRotation(direction)) as GameObject;
                UpdateBottomTramplineLimit(temp.transform.position, ref limitsForBottomTrampline);
                allTramplines.Add(temp.transform);
                lastPos = temp.transform.position;
            }
            if(Vector3.Distance(line.GetPosition(i), lastPos) > 2.5f  && i % 20 == 0&& i<line.positionCount-line.positionCount/5)
            {
                Vector3 direction = Vector3.zero;
                direction = lastPos - line.GetPosition(i);
                direction.y = 0;
                temp = Instantiate(fan, new Vector3((Mathf.Sin(ht * Time.deltaTime) * amp), -i * 10 * Time.deltaTime, (Mathf.Cos(-ht * Time.deltaTime) * amp)), Quaternion.LookRotation(direction)) as GameObject;
                lastPos = temp.transform.position;
            }
            if (i == laneCount - 1) {
                Vector3 direction = Vector3.zero;
                direction = line.GetPosition(i)- line.GetPosition(i - 5);
                direction.y = 0;
                finishLine.transform.position = line.GetPosition(i);
                finishLine.transform.rotation = Quaternion.LookRotation(direction);
                allTramplines.Add(finishLine.transform);
                finishLine.SetActive(true);
            }
        }
        for (int i = allTramplines.Count-1; i >=0; i--) {
            if (allTramplines[i].gameObject.GetComponent<Trampline>())
                allTramplines[i].gameObject.GetComponent<Trampline>().index = allTramplines.Count - (i + 1);
            else
            if (allTramplines[i].gameObject.GetComponent<DistructableTrampline>())
                allTramplines[i].gameObject.GetComponent<DistructableTrampline>().index = allTramplines.Count - (i + 1);
        }

        foreach (Transform trampline in allTramplines)
        {
            if (trampline.GetComponent<Trampline>()) {
                Trampline ins = trampline.GetComponent<Trampline>();
                int randomNumber = Random.Range(1, 5);
                if (randomNumber == 1 && ins != null && trampline != allTramplines[0])
                {
                    ins.isMoving = true;
                }
            }
        }
    }
  
}
