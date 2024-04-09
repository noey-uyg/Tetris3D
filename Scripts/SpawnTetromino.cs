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

        //��忡 ����� ���ٸ� ���ο� ��Ʈ�ι̳� ����
        if (tetrominoNode.childCount == 0 && !createTet)
        {
            CreateTetromino(GameManager.Instance.nextTet);
        }
    }

    //Ÿ�� ����
    void CreateTile(Transform parent, Vector2 position, int matIdx)
    {
        GameObject tile = PoolManager.instance.ActivateObj(0);
        tile.transform.parent = parent;
        tile.transform.localPosition = position;

        tile.GetComponent<Renderer>().material = tetMet[matIdx];
    }

    //��Ʈ�ι̳� ����
    void CreateTetromino(int index)
    {
        createTet = true; // ��Ʈ�ι̳밡 ���������� üũ�ϱ�
        GameManager.Instance.curTet = index;

        //ȸ�� �ʱ�ȭ
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
            // I : �ϴû�
            case 0:
                CreateTile(tetrominoNode, new Vector2(-2f, 0.0f), 0);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), 0);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), 0);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), 0);
                break;

            // J : �Ķ���
            case 1:
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), 1);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), 1);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), 1);
                CreateTile(tetrominoNode, new Vector2(-1f, 1.0f), 1);
                break;

            // L : �ֻ�
            case 2:
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), 2);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), 2);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), 2);
                CreateTile(tetrominoNode, new Vector2(1f, 1.0f), 2);
                break;

            // O : �����
            case 3:
                CreateTile(tetrominoNode, new Vector2(0f, 0f), 3);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), 3);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), 3);
                CreateTile(tetrominoNode, new Vector2(1f, 1f), 3);
                break;

            // S : ���
            case 4:
                CreateTile(tetrominoNode, new Vector2(-1f, -1f), 4);
                CreateTile(tetrominoNode, new Vector2(0f, -1f), 4);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), 4);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), 4);
                break;

            // T : ���ֻ�
            case 5:
                CreateTile(tetrominoNode, new Vector2(-1f, 0f), 5);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), 5);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), 5);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), 5);
                break;

            // Z : ������
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

    // �׸��� ��� ����
    void CreateShadowBlocks()
    {
        shadowNode.position = spawnPoint.transform.position;
        // ��Ʈ�ι̳�� ������ ��ġ �� ���·� �����Ǿ�� ��
        for (int i = 0; i < shadowNode.childCount; i++)
        {
            shadowNode.GetChild(i).localPosition = new Vector2(tetrominoNode.GetChild(i).localPosition.x,
                                                tetrominoNode.GetChild(i).localPosition.y);

            shadowNode.GetChild(i).gameObject.SetActive(true);
        }
    }

    //��� ����
    public void SaveTetromino()
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Down);
        //����� ��Ʈ�ι̳밡 ���� ���
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

