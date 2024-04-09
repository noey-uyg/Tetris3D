using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public Material[] tetMet;
    public GameObject tilePrefab;
    public Transform tetrominoNode;
    public Transform shadowNode;
    public GameObject spawnPoint;

    public bool createTet = false;

    private void Start()
    {
        GameManager.Instance.nextTet = Random.Range(0, 7);
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        //노드에 블록이 없다면 새로운 테트로미노 생성
        if (tetrominoNode.childCount == 0 && !createTet)
        {
            CreateTetromino(GameManager.Instance.nextTet);
        }
    }

    //타일 생성
    void CreateTile(Transform parent, Vector2 position, int matIdx)
    {
        GameObject tile = PoolManager.instance.ActivateObj(0);
        tile.transform.parent = parent;
        tile.transform.localPosition = position;

        tile.GetComponent<Renderer>().material = tetMet[matIdx];
    }

    //테트로미노 생성
    void CreateTetromino(int index)
    {
        createTet = true; // 테트로미노가 생성중인지 체크하기
        GameManager.Instance.curTet = index;

        //회전 초기화
        tetrominoNode.rotation = Quaternion.identity;
        tetrominoNode.position = spawnPoint.transform.position;

        if (tetrominoNode.childCount > 0)
        {
            ClearNode();
        }
        else
        {
            GameManager.Instance.nextTet = Random.Range(0, 7);
        }

        switch (index)
        {
            // I : 하늘색
            case 0:
                CreateTile(tetrominoNode, new Vector2(-2f, 0.0f), 0);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), 0);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), 0);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), 0);
                break;

            // J : 파란색
            case 1:
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), 1);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), 1);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), 1);
                CreateTile(tetrominoNode, new Vector2(-1f, 1.0f), 1);
                break;

            // L : 귤색
            case 2:
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), 2);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), 2);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), 2);
                CreateTile(tetrominoNode, new Vector2(1f, 1.0f), 2);
                break;

            // O : 노란색
            case 3:
                CreateTile(tetrominoNode, new Vector2(0f, 0f), 3);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), 3);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), 3);
                CreateTile(tetrominoNode, new Vector2(1f, 1f), 3);
                break;

            // S : 녹색
            case 4:
                CreateTile(tetrominoNode, new Vector2(-1f, -1f), 4);
                CreateTile(tetrominoNode, new Vector2(0f, -1f), 4);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), 4);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), 4);
                break;

            // T : 자주색
            case 5:
                CreateTile(tetrominoNode, new Vector2(-1f, 0f), 5);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), 5);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), 5);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), 5);
                break;

            // Z : 빨간색
            case 6:
                CreateTile(tetrominoNode, new Vector2(-1f, 1f), 6);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), 6);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), 6);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), 6);
                break;
        }

        GameManager.Instance.NextBlock();
        CreateShadowBlocks();
        createTet = false;
    }

    // 그림자 블록 생성
    void CreateShadowBlocks()
    {
        shadowNode.position = spawnPoint.transform.position;
        // 테트로미노와 동일한 위치 및 형태로 생성되어야 함
        for (int i = 0; i < shadowNode.childCount; i++)
        {
            shadowNode.GetChild(i).localPosition = new Vector2(tetrominoNode.GetChild(i).localPosition.x,
                                                tetrominoNode.GetChild(i).localPosition.y);

            shadowNode.GetChild(i).gameObject.SetActive(true);
        }
    }

    //블록 저장
    public void SaveTetromino()
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Down);
        //저장된 테트로미노가 없는 경우
        if (GameManager.Instance.saveTet == -1)
        {
            GameManager.Instance.saveTet = GameManager.Instance.curTet;
            CreateTetromino(GameManager.Instance.nextTet);
        }
        else
        {
            int temp = GameManager.Instance.curTet;
            GameManager.Instance.curTet = GameManager.Instance.saveTet;
            GameManager.Instance.saveTet = temp;
            CreateTetromino(GameManager.Instance.curTet);
        }
        GameManager.Instance.SaveBlock();
    }

    void ClearNode()
    {
        while (tetrominoNode.childCount > 0)
        {
            Transform tile = tetrominoNode.GetChild(0);
            tile.gameObject.SetActive(false);
            tile.parent = PoolManager.instance.gameObject.transform;
        }
    }
}

