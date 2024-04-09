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

    //�࿡ ��Ʈ�ι̳� �߰�
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

    //���� �� á�ٸ� �� �����
    void CheckBoardAndRowClear()
    {
        bool isClear = false; // �ϼ��� ���� �ִ��� üũ
        int clearedRows = 0; //�ϼ��� ���� ����

        foreach(Transform row in this.transform)
        {
            //�ϼ��� ���� �ڽ� ������Ʈ�� ��� ����
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

    //�� ������
    void RowsDown()
    {
        for(int i = 1; i< this.transform.childCount; i++)
        {
            Transform col = this.transform.Find(i.ToString());

            //�� �� ����
            if (col.childCount == 0) continue;

            int emptyCol = 0; //�� ĭ ���������� �����ϴ� ����
            int j = i - 1; //���� �� �Ʒ��� �� ���� �����ϴ� �� Ȯ���ϴ� ����

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
