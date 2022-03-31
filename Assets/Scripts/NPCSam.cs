using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSam : Character
{
    [SerializeField] float moveSpeed;
    private bool touchForTheFirstTime;
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameManager.stats == GameStats.Over)
            return;
        Vector3 lookat = Vector3.zero;
        if (FallingDown)
        {
            Transform nearestBottomTrampline = DontDestroy.instance.GetNearestBottomTrampline(transform.position);
            if (nearestBottomTrampline) {
                lookat = nearestBottomTrampline.transform.position - transform.position;
                lookat.y = 0;
                float distance = Vector2.Distance(new Vector2(nearestBottomTrampline.position.x, nearestBottomTrampline.position.z), new Vector2(transform.position.x, transform.position.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookat), 20 * Time.deltaTime);
                if (1f < distance) {
                    Body.position += transform.forward * Mathf.Clamp(0f, moveSpeed*2.5f, distance);
                }
            }
        }else
        if (LastTrampline)
        {
            float tramplineRadius = 0;
            NextTrampline = null;
            float possibleMinDistanceToTrampline = 0.2f;
            if (touchForTheFirstTime) {
                possibleMinDistanceToTrampline = 1.2f;
            }
            if (NextTrampline == null) {
                touchForTheFirstTime = false;
                NextTrampline = PlatformManager.instance.GetNearestNextTrampline(LastTrampline);
            }
            if (LastTrampline.GetComponent<Trampline>().isBottomTrampoline) {
                NextTrampline = PlatformManager.instance.GetNearestTrampline(LastTrampline);
                if(!NextTrampline.GetComponent<Trampline>().isBottomTrampoline)
                    tramplineRadius = PlatformManager.instance.tramplineRadius;
            }
            float distance = Vector2.Distance(new Vector2(NextTrampline.position.x, NextTrampline.position.z + tramplineRadius), new Vector2(transform.position.x, transform.position.z));

            if (NextTrampline) {
                lookat = NextTrampline.transform.position - transform.position;
                lookat.y = 0;
                if(possibleMinDistanceToTrampline< distance)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookat), 5 * Time.deltaTime);
            }
            if ((possibleMinDistanceToTrampline < distance&&!LastTrampline.GetComponent<Trampline>().isBottomTrampoline)
                ||(NextTrampline.position.y<transform.position.y&& possibleMinDistanceToTrampline < distance))
            {
                Body.position += transform.forward * Mathf.Clamp(0f, moveSpeed, distance);
            }
            else {
                touchForTheFirstTime = true;
                Body.velocity = new Vector3(0, Body.velocity.y, 0);
            }
        }
    }
}
