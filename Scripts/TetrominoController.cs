using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    public float fallSpeed = 1f; // ��Ʈ�ι̳밡 �������� �ӵ�
    private float timeToFall = 0f;

    public Camera mainCamera; // �� ī�޶�
    public Board board;
    public Vector3 boardSize = new Vector3(10, 20, 10); // ������ ũ��

    void Update()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        timeToFall += Time.deltaTime;

        // ���� �ð����� ��Ʈ�ι̳븦 �ڵ����� �Ʒ��� �̵�
        if (timeToFall >= fallSpeed)
        {
            MoveDown();
            timeToFall = 0f;
        }

        // ����� �Է��� �޾� ��Ʈ�ι̳븦 ����
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveBack();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveFoward();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
        else if(Input.GetKeyDown(KeyCode.Space)) 
        {
            while (MoveDown()) // �Ʒ��� �̵� �õ� �ݺ�
            {
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate();
        }

    }

    //�������� �̵�
    public void MoveLeft()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        Vector3 moveVector = -mainCamera.transform.right; // ī�޶��� ������ ������ �ݴ� ����
        if (IsWithinBoard(moveVector))
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
            transform.position += new Vector3(Mathf.RoundToInt(moveVector.x), Mathf.RoundToInt(moveVector.y), Mathf.RoundToInt(moveVector.z));
        }
    }

    //���������� �̵�
    public void MoveRight()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        Vector3 moveVector = mainCamera.transform.right;
        if (IsWithinBoard(moveVector))
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
            transform.position += new Vector3(Mathf.RoundToInt(moveVector.x), Mathf.RoundToInt(moveVector.y), Mathf.RoundToInt(moveVector.z));
        }
    }

    //�ڷ� �̵�
    public void MoveBack()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        Vector3 rotation = mainCamera.transform.rotation.eulerAngles;
        Vector3 moveVector = Vector3.zero;

        if (rotation.y == 0)
        {
            moveVector = new Vector3(0, 0, 1);
        }
        else if(rotation.y == 90)
        {
            moveVector = new Vector3(1, 0, 0);
        }
        else if(rotation.y == 180)
        {
            moveVector = new Vector3(0, 0, -1);
        }
        else if(rotation.y == 270)
        {
            moveVector = new Vector3(-1, 0, 0);
        }

        if (IsWithinBoard(moveVector))
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
            transform.position += new Vector3(Mathf.RoundToInt(moveVector.x), Mathf.RoundToInt(moveVector.y), Mathf.RoundToInt(moveVector.z));
        }
    }

    //������ �̵�
    public void MoveFoward()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        Vector3 rotation = mainCamera.transform.rotation.eulerAngles;
        Vector3 moveVector = Vector3.zero;

        if (rotation.y == 0)
        {
            moveVector = new Vector3(0, 0, -1);
        }
        else if (rotation.y == 90)
        {
            moveVector = new Vector3(-1, 0, 0);
        }
        else if (rotation.y == 180)
        {
            moveVector = new Vector3(0, 0, 1);
        }
        else if (rotation.y == 270)
        {
            moveVector = new Vector3(1, 0, 0);
        }

        if (IsWithinBoard(moveVector))
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
            transform.position += new Vector3(Mathf.RoundToInt(moveVector.x), Mathf.RoundToInt(moveVector.y), Mathf.RoundToInt(moveVector.z));
        }
    }

    public void InfiniteDown()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        while (MoveDown()) // �Ʒ��� �̵� �õ� �ݺ�
        {
        }
    }

    //�Ʒ� �̵�
    public bool MoveDown()
    {
        Vector3 moveVector = Vector3.down;
        if (IsWithinBoard(moveVector))
        {
            transform.position += new Vector3(Mathf.RoundToInt(moveVector.x), Mathf.RoundToInt(moveVector.y), Mathf.RoundToInt(moveVector.z));
            return true;
        }
        else //�̵� ���� �� ��ƼŬ ���� �� ���忡 �ױ�
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Down);
            GameObject blockSFX = PoolManager.instance.ActivateObj(1);
            blockSFX.transform.position = this.transform.position;
            board.AddBoard(this.transform);
            return false;
        }
    }

    //�������� ȸ��
    public void Rotate()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);

        // ī�޶��� ������ ���͸� �������� ȸ��
        Vector3 cameraRight = mainCamera.transform.right;

        // ȸ�� ���� �ڽ� ��ü���� ��ġ ����
        Vector3[] originalLocalPositions = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            originalLocalPositions[i] = transform.GetChild(i).localPosition;
        }

        // ȸ�� ����
        transform.RotateAround(transform.position, cameraRight, 90f);

        // ȸ�� ���� �ڽ� ��ü���� ��ġ ����
        Vector3[] rotatedLocalPositions = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            rotatedLocalPositions[i] = transform.GetChild(i).localPosition;
        }

        // ���� ���� �ִ��� Ȯ��
        for (int i = 0; i < transform.childCount; i++)
        {
            // ȸ�� �� ��ġ�� ȸ�� �� ��ġ�� ���� ���
            Vector3 positionDiff = rotatedLocalPositions[i] - originalLocalPositions[i];

            if (!IsWithinBoard(positionDiff))
            {
                // ȸ�� ���� ��ġ�� �ǵ�����
                transform.RotateAround(transform.position, cameraRight, -90f);
                return;
            }
        }
    }

    // ��ġ�� ���� ���� �ִ��� Ȯ��
    public bool IsWithinBoard(Vector3 position)
    {
        for(int i = 0; i < transform.childCount; i++)
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

}