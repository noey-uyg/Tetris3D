using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField] private float swipeThreshold = 50f;
    [SerializeField] private Transform rotationCenter;
    [SerializeField] private float moveSpeed = 5f; // �̵� �ӵ�

    private Vector3 targetPosition; // ��ǥ ��ġ

    public bool isRotate = false;


    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.isPause || !GameManager.Instance.isGameStart) return;

        // ��ġ �Ǵ� ���콺 �Է� ����
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

        // ȸ�� �߽��� �ٶ󺸵��� ī�޶��� �ü��� ������Ʈ
        transform.LookAt(rotationCenter.position);

        if (isRotate)
        {
            // �ε巴�� �̵�
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ��ǥ ��ġ�� ���� �������� �� �̵�, ȸ�� ����
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;

                isRotate = false;
            }
        }
    }

    void CheckSwipe()
    {
        // �������� �Ÿ� ���
        float deltaX = fingerUpPosition.x - fingerDownPosition.x;

        // ���� ��������
        if (Mathf.Abs(deltaX) > swipeThreshold)
        {
            isRotate = true;
            // ������ ��������
            if (deltaX > 0)
            {
                RotateCameraAroundCenter(90f);
            }
            // ���� ��������
            else
            {
                RotateCameraAroundCenter(-90f);
            }
        }
    }

    void RotateCameraAroundCenter(float angle)
    {
        // ȸ�� �߽��� �������� ȸ�� ���� ����
        Quaternion deltaRotation = Quaternion.Euler(0, angle, 0);

        // ȸ�� �߽��� �������� ī�޶� ȸ��
        Vector3 direction = transform.position - rotationCenter.position;
        direction = deltaRotation * direction;
        Vector3 newPosition = rotationCenter.position + direction;

        //ī�޶� ��Ʈ�ι̳��� �̵� �� ȸ���� �̿�Ǳ� ������ ������ ��Ȯ�� ������ �Ǻ��ϱ� ����
        targetPosition = new Vector3(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y), Mathf.RoundToInt(newPosition.z));
    }
}
