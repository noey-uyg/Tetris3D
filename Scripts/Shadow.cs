using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public GameObject tetromino; // 테트로미노 오브젝트
    public Board board; // 보드
    public Vector3 boardSize = new Vector3(10, 20, 10); // 보드의 크기


    private void Update()
    {
        if (tetromino == null) return;

        if(tetromino.transform.childCount == 0)
        {
            DeleteShadowBlocks();
        }

        // 테트로미노 컨트롤러의 위치와 회전값을 그대로 따라감
        transform.position = tetromino.transform.position;
        transform.rotation = tetromino.transform.rotation;

        while (MoveDown()) // 아래로 이동 시도 반복
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

    // 위치가 보드 내에 있는지 확인
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

            //밑에 테트로미노가 쌓여있는지 확인
            Transform go = board.transform.Find(y.ToString());

            if (go != null && go.Find(x.ToString() + "*" + z.ToString())) return false;
        }
        return true;
    }

    //그림자 블록 비활성화
    void DeleteShadowBlocks()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
