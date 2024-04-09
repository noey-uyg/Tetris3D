using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField] private float swipeThreshold = 50f;
    [SerializeField] private Transform rotationCenter;
    [SerializeField] private float moveSpeed = 5f; // 이동 속도

    private Vector3 targetPosition; // 목표 위치

    public bool isRotate = false;


    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        // 터치 또는 마우스 입력 감지
        if (!isRotate && Input.GetMouseButtonDown(0))
        {
            fingerDownPosition = Input.mousePosition;
            GameManager.Instance.isClick = true;
        }

        if (!isRotate && GameManager.Instance.isClick && Input.GetMouseButtonUp(0))
        {
            fingerUpPosition = Input.mousePosition;
            CheckSwipe();
            GameManager.Instance.isClick = false;
        }

        // 회전 중심을 바라보도록 카메라의 시선을 업데이트
        transform.LookAt(rotationCenter.position);

        if (isRotate)
        {
            // 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 위치와 거의 도달했을 때 이동, 회전 종료
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;

                isRotate = false;
            }
        }
    }

    void CheckSwipe()
    {
        // 스와이프 거리 계산
        float deltaX = fingerUpPosition.x - fingerDownPosition.x;

        // 수평 스와이프
        if (Mathf.Abs(deltaX) > swipeThreshold)
        {
            isRotate = true;
            // 오른쪽 스와이프
            if (deltaX > 0)
            {
                RotateCameraAroundCenter(90f);
            }
            // 왼쪽 스와이프
            else
            {
                RotateCameraAroundCenter(-90f);
            }
        }
    }

    void RotateCameraAroundCenter(float angle)
    {
        // 회전 중심을 기준으로 회전 각도 설정
        Quaternion deltaRotation = Quaternion.Euler(0, angle, 0);

        // 회전 중심을 기준으로 카메라를 회전
        Vector3 direction = transform.position - rotationCenter.position;
        direction = deltaRotation * direction;
        Vector3 newPosition = rotationCenter.position + direction;

        //카메라가 테트로미노의 이동 및 회전에 이용되기 때문에 정수로 정확한 조건을 판별하기 위함
        targetPosition = new Vector3(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y), Mathf.RoundToInt(newPosition.z));
    }
}
