using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public PoolManager poolManager;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject go = new GameObject((20 - i - 1).ToString());
            go.transform.localPosition = new Vector3(5, (20 - i - 1), 5);
            go.transform.parent = transform.transform;
        }
    }

    //행에 테트로미노 추가
    public void AddBoard(Transform tetromino)
    {
        while(tetromino.childCount > 0)
        {
            GameObject node = tetromino.GetChild(0).gameObject;

            int x = Mathf.RoundToInt(node.transform.position.x);
            int y = Mathf.RoundToInt(node.transform.position.y);
            int z = Mathf.RoundToInt(node.transform.position.z);

            if (y >= 20)
            {
                GameManager.Instance.GameOver();
                return;
            }

            node.transform.parent = transform.Find(y.ToString());
            node.name = x.ToString() + "*" + z.ToString();
        }

        CheckBoardAndRowClear();
    }

    //행이 꽉 찼다면 행 지우기
    void CheckBoardAndRowClear()
    {
        bool isClear = false; // 완성된 행이 있는지 체크
        int clearedRows = 0; //완성된 행의 개수

        foreach(Transform row in this.transform)
        {
            //완성된 행의 자식 오브젝트를 모두 제거
            if (row.childCount == 10 * 10) 
            {
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.BreakTile);
                clearedRows++; 
                while (row.childCount > 0)
                {
                    Transform tile = row.GetChild(0);
                    tile.gameObject.SetActive(false);
                    tile.parent = poolManager.transform;
                }

                row.DetachChildren();
                isClear = true;

                GameObject cubeSFX = poolManager.ActivateObj(2);
                cubeSFX.transform.position = row.position;
            }
        }

        if (isClear)
        {
            GameManager.Instance.ClearRow(clearedRows);
            RowsDown();
        }
        else
        {
            GameManager.Instance.ResetCombo();
        }
    }

    //행 내리기
    void RowsDown()
    {
        for(int i = 1; i< this.transform.childCount; i++)
        {
            Transform col = this.transform.Find(i.ToString());

            //빈 행 무시
            if (col.childCount == 0) continue;

            int emptyCol = 0; //몇 칸 내릴것인지 저장하는 변수
            int j = i - 1; //현재 행 아래에 빈 행이 존재하는 지 확인하는 변수

            while (j >= 0)
            {
                if(this.transform.Find(j.ToString()).childCount == 0)
                {
                    emptyCol++;
                }
                j--;
            }

            if (emptyCol > 0)
            {
                Transform tCol = this.transform.Find((i - emptyCol).ToString());

                while(col.childCount > 0)
                {
                    Transform tile = col.GetChild(0);
                    tile.parent = tCol;
                    tile.transform.position += new Vector3(0, -emptyCol, 0);
                }

                col.DetachChildren();
            }
        }
    }
}
