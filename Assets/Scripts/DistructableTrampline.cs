using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistructableTrampline : Trampline
{
    public Texture texture;
    public Trampline source;
    private MeshRenderer[] peaces;
    private bool letBlast =false;
    public Material referenceMaterial;
    void OnEnable()
    {
        letBlast = false;
        peaces = GetComponentsInChildren<MeshRenderer>();
        if (texturePool.Length > 0) {
            int i = Random.Range(0, texturePool.Length);
            texture = texturePool[i];
        }
        for (int i = 0; i < 29; i++) {
            peaces[i].material.mainTexture =texture;
        }

        StartCoroutine(LetBlast());
        if (indexShower)
            indexShower.text = index.ToString();
    }
    private void Start()
    {
        if (indexShower)
            indexShower.text = index.ToString();
    }
    IEnumerator LetBlast()
    {
        yield return new WaitForSeconds(0.5f);
        letBlast = true;
    }
    public override void OnCollisionEnter(Collision other) {
        base.OnCollisionEnter(other);
        if (other.gameObject.tag == "Player"&& letBlast) {
            Blast();
        }
    }
    public void Blast() {
        StartCoroutine(Blasting());
    }
    IEnumerator Blasting() {
        GetComponent<Animator>().SetBool("Blast", true);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetBool("Blast", false);
        gameObject.SetActive(false);
    }
    void OnDisable() {
        letBlast = false;
    }
}
