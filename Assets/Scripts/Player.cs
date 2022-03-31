using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Vector2 prevMousePosition, nextMousePosition;
    float xOriginDif = 0;
    [SerializeField] float speed = 2;
    [SerializeField] GameObject[] perfectLandingTrails;
    public Color normalColor, hittingColor;
    bool moving = false;
    public LayerMask mask;
    private LineRenderer line;
    private float timer = 0;
    void Awake() {
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            prevMousePosition = Input.mousePosition;
            nextMousePosition = Input.mousePosition;
        }
        else
        if (Input.GetMouseButton(0)) {
            if (!Landing)
            {
                moving = true;
            }
            else
                moving = false;
            nextMousePosition = Input.mousePosition;
            xOriginDif = nextMousePosition.x - prevMousePosition.x;
            prevMousePosition = Input.mousePosition;
            xOriginDif /= (Screen.width / 1050f);
        }
        else
        {
            moving = false;
            xOriginDif = 0;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameManager.stats == GameStats.Over)
            return;
        Vector3 lookat = Vector3.zero;
        if (moving) {
            transform.position+=Body.transform.forward*speed;
            timer = 0;
        }
        if (!moving&& LastTrampline)
        {
            timer += Time.fixedDeltaTime;
            if (timer > 2.0f&& LastTrampline!=null)
            {
                Transform nextTrampline = PlatformManager.instance.GetNextTrampline(LastTrampline);
                if (nextTrampline) {
                    lookat = nextTrampline.transform.position - LastTrampline.position;
                    lookat.y = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookat), 3 * Time.deltaTime);
                }
            }
        }
        else
            transform.rotation = (Quaternion.Euler(Body.rotation.eulerAngles + new Vector3(0, (xOriginDif * RotationRate * Time.deltaTime), 0)));

        line.SetPosition(0, transform.position + transform.up * 0.5f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out hit, 100, mask)){
            line.SetPosition(1, hit.point);
            line.startColor = hittingColor;
            line.endColor = hittingColor;
        }
        else {
            line.SetPosition(1, new Vector3(transform.position.x, -100, transform.position.z));
            line.startColor = normalColor;
            line.endColor = normalColor;
        }
    }
    public void OnGameComplete()
    {
        SceneController.instance.OnGameComplete();
        Anim.SetBool("Finish", true);
    }
    public void OnGameOver()
    {
        Anim.SetBool("Death", true);
        StartCoroutine(GameOvering());
    }
    IEnumerator GameOvering() {
        yield return new WaitForSeconds(1.8f);
        SceneController.instance.OnGameOver();
    }
    public void ToggleTrailEffect(bool condition)
    {
        foreach (GameObject trail in perfectLandingTrails)
        {
            trail.SetActive(condition);
        }
    }
}
