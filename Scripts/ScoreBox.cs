using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBox : MonoBehaviour
{
    public Text rankTxt;
    public Text scoreTxt;
    public Text userNameTxt;

    public void SetRank(int rank, string username, string score)
    {
        rankTxt.text = rank.ToString();
        userNameTxt.text = username;
        scoreTxt.text = score;
    }
}
