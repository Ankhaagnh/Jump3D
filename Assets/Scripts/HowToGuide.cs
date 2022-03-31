using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToGuide : MonoBehaviour
{
    public GameObject text;
    private Animator anim;
    private int guideIndex = 0;
    private Vector2 prevMousePosition, nextMousePosition;
    private float xOriginDif = 0;
    public static bool firstTime = true;
    private void Start()
    {
        anim = GetComponent<Animator>();
        if (!firstTime)
            gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTime = false;
            if (guideIndex == 0) {
                guideIndex = 1;
                anim.SetInteger("GuideIndex", guideIndex);
            }
            else {
                prevMousePosition = Input.mousePosition;
            }
        }else
        if (Input.GetMouseButton(0))
        {
            text.SetActive(false);
            nextMousePosition = Input.mousePosition;
            xOriginDif = nextMousePosition.x - prevMousePosition.x;

            if (xOriginDif > 20 && guideIndex == 1)
            {
                guideIndex = 2;
                anim.SetInteger("GuideIndex", guideIndex);
            }
            else
            if (xOriginDif != 0 && xOriginDif < -20 && guideIndex == 2)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
