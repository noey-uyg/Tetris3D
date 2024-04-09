using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public GameObject tetromino; // ��Ʈ�ι̳� ������Ʈ
    public Board board; // ����
    public Vector3 boardSize = new Vector3(10, 20, 10); // ������ ũ��


    private void Update()
    {
        if (tetromino == null) return;

        if(tetromino.transform.childCount == 0)
        {
            DeleteShadowBlocks();
        }

        // ��Ʈ�ι̳� ��Ʈ�ѷ��� ��ġ�� ȸ������ �״�� ����
        transform.position = tetromino.transform.position;
        transform.rotation = tetromino.transform.rotation;

        while (MoveDown()) // �Ʒ��� �̵� �õ� �ݺ�
        {
        }
    }


    bool MoveDown()
    {
        Vector3 moveVector = Vector3.down;
        if (IsWithinBoard(moveVector))
        {
            transform.position += new Vector3(Mathf.RoundToInt(moveVector.x), Mathf.RoundToInt(moveVector.y), Mathf.RoundToInt(moveVector.z));
            return true;
        }

        return false;
    }

    // ��ġ�� ���� ���� �ִ��� Ȯ��
    bool IsWithinBoard(Vector3 position)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform node = transform.GetChild(i).transform;
            int x = Mathf.RoundToInt(node.position.x + position.x);
            int y = Mathf.RoundToInt(node.position.y + position.y);
            int z = Mathf.RoundToInt(node.position.z + position.z);

            if (x < 0 || x >= boardSize.x) return false;
            if (y < 0 || y > boardSize.y) return false;
            if (z < 0 || z >= boardSize.z) return false;

            //�ؿ� ��Ʈ�ι̳밡 �׿��ִ��� Ȯ��
            Transform go = board.transform.Find(y.ToString());

            if (go != null && go.Find(x.ToString() + "*" + z.ToString())) return false;
        }
        return true;
    }

    //�׸��� ��� ��Ȱ��ȭ
    void DeleteShadowBlocks()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
