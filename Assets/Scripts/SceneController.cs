using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{   
    [SerializeField] private Text encourageText;
    [SerializeField] private float currentProgress;
    [SerializeField] private Image progressBar;
    [SerializeField] private Text currentLevel;
    [SerializeField] private Text nextLevel;
    [SerializeField] private GameObject gamePlay;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameCompletedScreen;
    [SerializeField] private Trampline lastTrampline;
    [SerializeField] private PlatformManager insPlatManager;

    public static SceneController instance;

    

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Time.timeScale = 0;
        currentLevel.text = GameManager.currentLevel.ToString();
        nextLevel.text = (GameManager.currentLevel+1).ToString();
        progressBar.fillAmount = currentProgress;
    }
    public void AddSomeProgress(Trampline trampline)
    {
        if (lastTrampline == null) {
            lastTrampline = trampline;
            return;
        }
        if (lastTrampline == trampline)
            return;
        int indexDiff = 0;
        if (trampline.index == -1) {
            indexDiff = (int)(Vector3.Distance(lastTrampline.transform.position, trampline.transform.position))/2;
        }else
            indexDiff = Mathf.Abs(Mathf.Abs(lastTrampline.index) - Mathf.Abs(trampline.index));
        if (indexDiff > 5) {
            currentProgress = GetProgressionPoint(trampline);
            Encourage("LONG JUMP!!!", Color.yellow);
        }
        else
            currentProgress = GetProgressionPoint(trampline);
        progressBar.fillAmount = currentProgress;
        if (currentProgress >= 1) {
            progressBar.color = Color.green;
        }
        lastTrampline = trampline;
    }
    public void Encourage(string word, Color color)
    {
        StartCoroutine(EncouragingPlayer(word, color));
    }
    IEnumerator EncouragingPlayer(string word, Color color) {
        if (!encourageText.gameObject.activeSelf) {
            encourageText.gameObject.SetActive(true);
            encourageText.text = word;
            encourageText.color = color;
            yield return new WaitForSeconds(1.5f);
            encourageText.gameObject.SetActive(false);
        }
    }
    public void OnGameStart()
    {
        Time.timeScale = 1;
        gamePlay.SetActive(true);
    }
    public void OnGameOver() {
        gamePlay.SetActive(false);
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void OnGameComplete()
    {
        gamePlay.SetActive(false);
        gameCompletedScreen.SetActive(true);
        Time.timeScale = 0;
    }


    private float GetProgressionPoint(Trampline trampoline)
    {
        float trumpolineCount =  insPlatManager.GetNumberOfTrumpoline() - 1;
        float progressionFloat = ((trumpolineCount - trampoline.index) / trumpolineCount);
        return progressionFloat;
    }
}
