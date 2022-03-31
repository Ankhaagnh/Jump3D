using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Character[] characters;
    [SerializeField] Text[] texts;
    [SerializeField] Text[] finalUIText;
    [SerializeField] GameObject playerLeadingEffect;
    

    List<Character> characterList = new List<Character>();

    // Start is called before the first frame update
    void Start()
    {
        characters = GameObject.FindObjectsOfType<Character>();

        foreach (var character in characters)
        {
            characterList.Add(character);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SortCharactersByIndex();
        ToggleLeadingEffect();
        UpdateUItext();

        UpdateFinalUIText();
    }

    private void UpdateFinalUIText()
    {
        for (var i = 0; i < texts.Length; i++)
        {
            finalUIText[i].text = characterList[i].name;
        }
    }

    private void SortCharactersByIndex()
    {
        characterList = characterList.OrderBy(o=>o.lastTramplineIndex).ToList();
    } 
    
    private void UpdateUItext()
    {
        for (var i = 0; i < texts.Length; i++)
        {
            texts[i].text = characterList[i].name;
        }
    }

    private void ToggleLeadingEffect()
    {
        if(characterList[0].name == "You")
        {
            playerLeadingEffect.gameObject.SetActive(true);
        }else
        {
            playerLeadingEffect.gameObject.SetActive(false);
        }
    }
}
