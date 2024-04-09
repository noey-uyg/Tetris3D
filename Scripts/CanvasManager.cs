using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public FireBaseController fireBaseController;

    public Text scoreText;
    public Text finalScore;

    public InputField userNameInput;

    public GameObject setBtn;
    public GameObject infoBtn;
    public GameObject setPanel;
    public GameObject infoPanel;

    public GameObject topPanel;
    public GameObject blockPanel;
    public GameObject bottomPanel;
    public GameObject centerPanel;
    public GameObject authPanel;
    public GameObject deletePanel;

    public GameObject rankPanel;
    public Transform rankContents;

    public Image sound;
    public Sprite[] soundIMG;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameManager.Instance.totalScore.ToString();
        finalScore.text = scoreText.text;
    }

    public void OnDeletePanel()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        deletePanel.SetActive(!deletePanel.activeSelf);
    }

    public void OnMenuBarClick()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        setBtn.SetActive(!setBtn.activeSelf);
        infoBtn.SetActive(!infoBtn.activeSelf);
    }

    public void OnSetBtnClick()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        setPanel.SetActive(!setPanel.activeSelf);
        GameManager.Instance.isPause = !GameManager.Instance.isPause;
    }

    public void OnInfoBtnClick()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        infoPanel.SetActive(!infoPanel.activeSelf);
        GameManager.Instance.isPause = !GameManager.Instance.isPause;
    }

    public void GameStartCanvasON()
    {
        topPanel.SetActive(!topPanel.activeSelf);
        blockPanel.SetActive(!blockPanel.activeSelf);
        centerPanel.SetActive(!centerPanel.activeSelf);
        bottomPanel.SetActive(!bottomPanel.activeSelf);

        setPanel.SetActive(false);
    }

    public void AuthPanelOnOff(bool isActive)
    {
        authPanel.SetActive(!isActive);
    }

    public void SendScore()
    {
        fireBaseController.SendRank(userNameInput.text, finalScore.text);
    }

    public void AddRank(int rank, string username, string score)
    {
        GameObject scoreBox = PoolManager.instance.ActivateObj(3);
        scoreBox.transform.transform.SetParent(rankContents.transform, false);
        scoreBox.GetComponent<ScoreBox>().SetRank(rank, username, score);
    }

    public void OnRankPanel()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        rankPanel.SetActive(!rankPanel.activeSelf);

        if (!rankPanel.activeSelf)
        {
            while (rankContents.childCount > 0)
            {
                Transform scoreBox = rankContents.GetChild(0);
                scoreBox.gameObject.SetActive(false);
                scoreBox.transform.transform.SetParent(PoolManager.Instance.transform, false);
            }
        }
    }

    public void SoundImg()
    {
        if (SoundManager.Instance.bgmPlayer.mute)
        {
            sound.sprite = soundIMG[0];
        }
        else
        {
            sound.sprite = soundIMG[1];
        }
    }
}