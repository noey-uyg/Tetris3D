using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    public float fallSpeed = 1f; // 테트로미노가 떨어지는 속도
    private float timeToFall = 0f;

    public Camera mainCamera; // 주 카메라
    public Board board;
    public Vector3 boardSize = new Vector3(10, 20, 10); // 보드의 크기

    void Update()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        timeToFall += Time.deltaTime;

        // 일정 시간마다 테트로미노를 자동으로 아래로 이동
        if (timeToFall >= fallSpeed)
        {
            MoveDown();
            timeToFall = 0f;
        }

        // 사용자 입력을 받아 테트로미노를 제어
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
            while (MoveDown()) // 아래로 이동 시도 반복
            {
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate();
        }

    }

    //왼쪽으로 이동
    public void MoveLeft()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        Vector3 moveVector = -mainCamera.transform.right; // 카메라의 오른쪽 벡터의 반대 방향
        if (IsWithinBoard(moveVector))
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);
            transform.position += new Vector3(Mathf.RoundToInt(moveVector.x), Mathf.RoundToInt(moveVector.y), Mathf.RoundToInt(moveVector.z));
        }
    }

    //오른쪽으로 이동
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

    //뒤로 이동
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

    //앞으로 이동
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

        while (MoveDown()) // 아래로 이동 시도 반복
        {
        }
    }

    //아래 이동
    public bool MoveDown()
    {
        Vector3 moveVector = Vector3.down;
        if (IsWithinBoard(moveVector))
        {
            transform.position += new Vector3(Mathf.RoundToInt(moveVector.x), Mathf.RoundToInt(moveVector.y), Mathf.RoundToInt(moveVector.z));
            return true;
        }
        else //이동 실패 시 파티클 생성 및 보드에 쌓기
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.Down);
            GameObject blockSFX = PoolManager.instance.ActivateObj(1);
            blockSFX.transform.position = this.transform.position;
            board.AddBoard(this.transform);
            return false;
        }
    }

    //위쪽으로 회전
    public void Rotate()
    {
        if (mainCamera.GetComponent<Swipe>().isRotate || GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Click);

        // 카메라의 오른쪽 벡터를 기준으로 회전
        Vector3 cameraRight = mainCamera.transform.right;

        // 회전 전의 자식 객체들의 위치 저장
        Vector3[] originalLocalPositions = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            originalLocalPositions[i] = transform.GetChild(i).localPosition;
        }

        // 회전 적용
        transform.RotateAround(transform.position, cameraRight, 90f);

        // 회전 후의 자식 객체들의 위치 저장
        Vector3[] rotatedLocalPositions = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            rotatedLocalPositions[i] = transform.GetChild(i).localPosition;
        }

        // 보드 내에 있는지 확인
        for (int i = 0; i < transform.childCount; i++)
        {
            // 회전 전 위치와 회전 후 위치의 차이 계산
            Vector3 positionDiff = rotatedLocalPositions[i] - originalLocalPositions[i];

            if (!IsWithinBoard(positionDiff))
            {
                // 회전 전의 위치로 되돌리기
                transform.RotateAround(transform.position, cameraRight, -90f);
                return;
            }
        }
    }

    // 위치가 보드 내에 있는지 확인
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

            //밑에 테트로미노가 쌓여있는지 확인
            Transform go = board.transform.Find(y.ToString());

            if (go != null && go.Find(x.ToString() + "*" + z.ToString())) return false;
        }
        return true;
    }

}