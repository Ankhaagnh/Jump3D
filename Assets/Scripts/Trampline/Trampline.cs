using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampline : MonoBehaviour
{
    public int index = 0;
    public bool noPoint = false;
    public TextMesh indexShower;
    public Texture2D[] texturePool;
    [SerializeField] private float force;
    [SerializeField] GameObject perfectLandingEffect = null;
    [SerializeField] GameObject normalLandingEffect = null;
    [SerializeField] float movingSpeed = 0.4f;
    [SerializeField] float maxMovingRange = 0.8f;
    public bool isBottomTrampoline = false;
    private int counter = 0;
    
    Vector3 initialPosition;
    public bool isMoving = false;
    public bool isMovingRight = true;

    void Awake()
    {
        initialPosition = transform.position;
    }

    void Start()
    {
        if (texturePool.Length > 0) {
            int i = Random.Range(0, texturePool.Length);
            GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = texturePool[i];
        }
        if(indexShower)
            indexShower.text = index.ToString();
    }
    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Character>()) {
            other.gameObject.GetComponent<Character>().Jump(this.transform, force);
        }
        if (other.gameObject.tag == "Player") {
            if(!isBottomTrampoline)
            {
                SceneController.instance.AddSomeProgress(this); 
            }
            other.transform.GetComponent<Player>().ToggleTrailEffect(false);

            if(normalLandingEffect != null)
            {
                normalLandingEffect.SetActive(true);
            }
            
            if (counter == 0&& !noPoint) {
                if (0.3f > Vector2.Distance(new Vector2(other.transform.position.x, other.transform.position.z), new Vector2(this.transform.position.x, this.transform.position.z)))
                {
                    SceneController.instance.Encourage("PERFECT!!!", Color.green);
                    Instantiate(perfectLandingEffect, transform.position + new Vector3(0, 0.9f, 0), Quaternion.Euler(-90f, 0f,0f));

                    other.transform.GetComponent<Player>().ToggleTrailEffect(true);
                }
                else
            if (0.6f > Vector2.Distance(new Vector2(other.transform.position.x, other.transform.position.z), new Vector2(this.transform.position.x, this.transform.position.z)))
                {
                    SceneController.instance.Encourage("GOOD!!!", Color.cyan);
                }
            }
            force += 20;
            counter++;
            if (counter > 10000)
            {
                if (GetComponentInChildren<SkinnedMeshRenderer>()) {
                    PlatformManager.instance.DisctructableTrampline.GetComponent<DistructableTrampline>().texture = GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture;
                    PlatformManager.instance.DisctructableTrampline.GetComponent<DistructableTrampline>().source = this;
                    PlatformManager.instance.DisctructableTrampline.GetComponent<DistructableTrampline>().index = index;
                    PlatformManager.instance.DisctructableTrampline.SetActive(true);
                    PlatformManager.instance.DisctructableTrampline.GetComponent<DistructableTrampline>().indexShower.text = index.ToString();
                    PlatformManager.instance.DisctructableTrampline.transform.position = transform.position;
                    PlatformManager.instance.DisctructableTrampline.transform.rotation = transform.rotation;
                    gameObject.SetActive(false);
                }
            }
            if (PlatformManager.instance.DisctructableTrampline.GetComponent<DistructableTrampline>().source != this && PlatformManager.instance.DisctructableTrampline.activeSelf)
            {
                PlatformManager.instance.BlastTrampline();
            }
        }
        GetComponent<Animator>().SetBool("Bounce", true);
    }
    public void StopBouncingAnim(){
        GetComponent<Animator>().SetBool("Bounce", false);
    }
    
    void OnDisable()
    {
        StopBouncingAnim();
    }
    void Update()
    {
        if (!isMoving) return;
        if (isMovingRight)
        {
            MoveSides(Vector3.right, false);
        }else
        {
            MoveSides(Vector3.left, true);
        }
    }
    private void MoveSides(Vector3 vector3, bool condition)
    {
        transform.Translate(vector3 * movingSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position ,initialPosition) > maxMovingRange)
        {
            isMovingRight = condition;
        }
    }
}
