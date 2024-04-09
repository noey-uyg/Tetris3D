using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using System;


public class FireBaseController : MonoBehaviour
{
    public CanvasManager canvasManager;

    private FirebaseAuth auth;
    private FirebaseUser user;
    private Firebase.FirebaseApp app;
    private const int MAX_RANK_COUNT = 100;

    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                app = Firebase.FirebaseApp.DefaultInstance;
                FirebaseInit();
            }
            else
            {
                Debug.LogError("CheckAndFixDependenciesAsync Fail");
            }
        });
    }

    private void FirebaseInit()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;

        DatabaseReference rankDB = FirebaseDatabase.DefaultInstance.GetReference("Rank");
        rankDB.ValueChanged += ReceiveRank;
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;

        if(senderAuth != null)
        {
            user = senderAuth.CurrentUser;
            if(user != null )
            {
                Debug.Log(user.UserId);

                canvasManager.AuthPanelOnOff(true);
                GameManager.Instance.startParticle.SetActive(true);
                GameManager.Instance.gameMainPanel.SetActive(true);
            }
        }
    }

    //�α���
    public void SignIn()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
        SigninAnonymous();        
    }

    //�α׾ƿ�
    public void SignOut()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);

        auth.SignOut();
        canvasManager.AuthPanelOnOff(false);
        canvasManager.topPanel.SetActive(false);
        canvasManager.blockPanel.SetActive(false);
        canvasManager.bottomPanel.SetActive(false);
        canvasManager.centerPanel.SetActive(false);

        GameManager.Instance.gameMainPanel.SetActive(false);
        GameManager.Instance.boardWall.SetActive(false);
        GameManager.Instance.isGameStart = false;

        PoolManager.instance.DeactivateAllObjects();
    }

    //ȸ��Ż��
    public void DeleteUser()
    {
        if (user == null) return;

        user.DeleteAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCanceled) return;
                else if (task.IsFaulted) return;
                Debug.Log("User account deleted successfully.");
            });
    }

    //�Խ�Ʈ �α���
    private Task SigninAnonymous()
    {
        return auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Signin Fail");
                }
                else if(task.IsCompleted)
                {
                    Debug.Log("Siginin Complete");
                }
            });
    }

    //���� �о����
    public void ReadRankScore()
    {
        if (user == null) return;

        DatabaseReference rankDB = FirebaseDatabase.DefaultInstance.GetReference("Rank");
        rankDB.GetValueAsync().ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Read Fail");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapShot = task.Result;

                    List<KeyValuePair<string, DataSnapshot>> sortedList = new List<KeyValuePair<string, DataSnapshot>>();

                    foreach (var childSnapshot in snapShot.Children)
                    {
                        sortedList.Add(new KeyValuePair<string, DataSnapshot>(childSnapshot.Key, childSnapshot));
                    }

                    // ������������ ����
                    sortedList.Sort((a, b) => {
                        int scoreA = Convert.ToInt32(a.Value.Child("score").Value);
                        int scoreB = Convert.ToInt32(b.Value.Child("score").Value);
                        return scoreB.CompareTo(scoreA);
                    });

                    int i = 1;

                    // ���ĵ� �����͸� CanvasManager�� ����
                    foreach (var item in sortedList)
                    {
                        string userName = item.Value.Child("userName").Value.ToString();
                        string score = item.Value.Child("score").Value.ToString();

                        canvasManager.AddRank(i, userName, score);
                        i++;
                    }

                }
            });
    }

    //���� ������ ������
    public void SendRank(string userName, string score)
    {
        if (user == null) return;

        DatabaseReference rankDB = FirebaseDatabase.DefaultInstance.GetReference("Rank");
        string key = rankDB.Push().Key;

        Dictionary<string, object> updateRank = new Dictionary<string, object>();

        Dictionary<string, string> rankScore = new Dictionary<string, string>();

        rankScore.Add("score", score);
        rankScore.Add("userName", userName);

        updateRank.Add(key, rankScore);

        rankDB.UpdateChildrenAsync(updateRank).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Update Score");
                }
            });
    }

    //���� ����
    public void ReceiveRank(object sender, ValueChangedEventArgs e)
    {
        if (user == null) return;

        DataSnapshot snapShot = e.Snapshot;

        // ��� ��ŷ �����͸� ���� ����Ʈ
        List<KeyValuePair<string, DataSnapshot>> allRankings = new List<KeyValuePair<string, DataSnapshot>>();

        foreach (var childSnapshot in snapShot.Children)
        {
            allRankings.Add(new KeyValuePair<string, DataSnapshot>(childSnapshot.Key, childSnapshot));
        }

        // ������ �������� ������������ ����
        allRankings.Sort((a, b) =>
        {
            int scoreA = Convert.ToInt32(a.Value.Child("score").Value);
            int scoreB = Convert.ToInt32(b.Value.Child("score").Value);
            return scoreB.CompareTo(scoreA);
        });

        // ���� MAX_RANK_COUNT���� ������ �̿��� �����ʹ� ����
        for (int i = MAX_RANK_COUNT; i < allRankings.Count; i++)
        {
            string keyToRemove = allRankings[i].Key;
            RemoveRankData(keyToRemove);
        }
    }

    //���� 100�� ���� �����
    private void RemoveRankData(string key)
    {
        DatabaseReference rankDB = FirebaseDatabase.DefaultInstance.GetReference("Rank");

        rankDB.Child(key).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Removed rank data with key: {key}");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"Failed to remove rank data with key: {key}");
            }
        });
    }
}
