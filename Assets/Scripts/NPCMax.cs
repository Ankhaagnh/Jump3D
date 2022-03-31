using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMax : Character
{
    [SerializeField] float moveSpeed;
    private bool touchForTheFirstTime;
    private void OnEnable()
    {
        onJump += Jump;
    }
    private void OnDisable()
    {
        onJump -= Jump;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameManager.stats == GameStats.Over)
            return;
        Vector3 lookat = Vector3.zero;
        if (FallingDown)
        {
            Transform nearestBottomTrampline = DontDestroy.instance.GetNearestBottomTrampline(transform.position);
            if (nearestBottomTrampline)
            {
                lookat = nearestBottomTrampline.transform.position - transform.position;
                lookat.y = 0;
                float distance = Vector2.Distance(new Vector2(nearestBottomTrampline.position.x, nearestBottomTrampline.position.z), new Vector2(transform.position.x, transform.position.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookat), 20 * Time.deltaTime);
                if (1f < distance)
                {
                    Body.position += transform.forward * Mathf.Clamp(0f, moveSpeed * 2.5f, distance);
                }
            }
        }
        else
        if (LastTrampline)
        {
            float tramplineRadius = 0;
            float possibleMinDistanceToTrampline = 0.2f;
            if (touchForTheFirstTime)
            {
                possibleMinDistanceToTrampline = 1.2f;
            }

            if (LastTrampline.GetComponent<Trampline>().isBottomTrampoline)
            {
                NextTrampline = PlatformManager.instance.GetNearestTrampline(LastTrampline);
                if (!NextTrampline.GetComponent<Trampline>().isBottomTrampoline)
                    tramplineRadius = PlatformManager.instance.tramplineRadius;
            }
            float distance = Vector2.Distance(new Vector2(NextTrampline.position.x, NextTrampline.position.z + tramplineRadius), new Vector2(transform.position.x, transform.position.z));

            if (NextTrampline)
            {
                lookat = NextTrampline.transform.position - transform.position;
                lookat.y = 0;
                if (possibleMinDistanceToTrampline < distance)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookat), 5 * Time.deltaTime);
            }
            if ((possibleMinDistanceToTrampline < distance && !LastTrampline.GetComponent<Trampline>().isBottomTrampoline)
                || (NextTrampline.position.y < transform.position.y && possibleMinDistanceToTrampline < distance))
            {
                Body.position += transform.forward * Mathf.Clamp(0f, moveSpeed, distance);
            }
            else
            {
                touchForTheFirstTime = true;
                Body.velocity = new Vector3(0, Body.velocity.y, 0);
            }
        }
    }
    public void Jump()
    {
        for (int i = PlatformManager.instance.AllTramplines.Count-1; i >0 ; i--) {
            Transform trans = FindFarestTargetCanTravel(PlatformManager.instance.AllTramplines[i], moveSpeed);
            if (trans) {
                NextTrampline = trans;
                touchForTheFirstTime = false;
                break;
            }
        }
    }
    public Transform FindFarestTargetCanTravel(Transform target, float speed) {
        float upDuration = 0.4f;
        float totalDurationToReachTarget = Mathf.Sqrt(((transform.position.y+4.5f-target.position.y+1.5f) * 2f) / 10f)+ upDuration;
        float platformDistance = Vector2.Distance(new Vector2(target.position.x, target.position.z), new Vector2(transform.position.x, transform.position.z));
        float travelDistance = speed * 60 * totalDurationToReachTarget;
        if (platformDistance < travelDistance) {
            return target;
        }
        else
            return null;
    }
}
