using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    #region Singleton
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    public GameObject gameOverPanel;
    public GameObject gameMainPanel;
    public GameObject[] nextBlocks;
    public GameObject[] saveBlocks;
    public GameObject boardWall;
    public GameObject startParticle;

    public bool isGameStart = false;
    public bool isGameOver = false;
    public bool isPause = false;
    public bool isClick = false;

    public int baseScorePerLine = 10;

    public int curTet;
    public int nextTet;
    public int saveTet= -1;
    public int totalScore = 0;
    public int comboCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Resolution();
    }

    private void Start()
    {
        SoundManager.Instance.PlayBgm(true);
    }

    public void Resolution()
    {
        int setWidth = 1080;
        int setHeight = 1920;

        Screen.SetResolution(setWidth, setHeight, true);
        Screen.orientation = ScreenOrientation.Portrait;
    }

    //게임 오버 시 호출
    public void GameOver()
    {
        AdmobManager.Instance.ShowFrontAd();
        gameOverPanel.SetActive(true);
        isGameOver = true;
    }

    // 행을 지울 때마다 호출되는 함수
    public void ClearRow(int clearedRows)
    {
        // 콤보 개수 증가
        comboCount++;

        int comboScore = baseScorePerLine * (int)Math.Pow(Math.Pow(2, clearedRows - 1), comboCount);
        totalScore += comboScore;
    }

    // 행을 지울 수 없을 때 호출되는 함수
    public void ResetCombo()
    {
        comboCount = 1; // 콤보 개수 초기화
    }

    public void NextBlock()
    {
        for (int i = 0; i < nextBlocks.Length; i++)
        {
            if (i == nextTet)
            {
                nextBlocks[i].SetActive(true);
            }
            else
            {
                nextBlocks[i].SetActive(false);
            }
        }
    }

    public void SaveBlock()
    {
        for (int i = 0; i < saveBlocks.Length; i++)
        {
            if (i == saveTet)
            {
                saveBlocks[i].SetActive(true);
            }
            else
            {
                saveBlocks[i].SetActive(false);
            }
        }
    }
    
    public void GameStart()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);

        isGameStart = !isGameStart;
        isGameOver = false;
        isPause = false;
        isClick = false;

        saveTet = -1;
        curTet = 0;
        nextTet = 0;
        saveTet = -1;
        totalScore = 0;
        comboCount = 0;

        SaveBlock();
        gameMainPanel.SetActive(!gameMainPanel.activeSelf);
        boardWall.SetActive(!boardWall.activeSelf);
        startParticle.SetActive(!startParticle.activeSelf);
        gameOverPanel.SetActive(false);
        PoolManager.instance.DeactivateAllObjects();
    }

    public void GameQuit()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);

        Application.Quit();
    }
}
