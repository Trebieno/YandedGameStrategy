using UnityEngine;

namespace CodeBase.SystemGame
{

    public class RTSCamera : MonoCache
    {
        public float moveSpeed = 10f; // �������� ����������� ������
        public float scrollSpeed = 10f; // �������� �����������/���������
        public float minZoom = 5f; // ����������� �������� ����
        public float maxZoom = 20f; // ������������ �������� ����
        public float edgeScrollWidth = 10f; // ������ ������� ��������� �� ����� ������
        public float inputLerpSpeed = 10f; // �������� ������������ �����
        private Vector2 _currentInputDirection;

        [SerializeField] private bool _isMoveCursor = false;

        public override void OnTick()
        {
            MoveCamera();
            ScrollCamera();
        }
        private void MoveCamera()
        {
            // �������� ���� � ����������
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            // ������� ������ ����������� �� ������ �����
            Vector2 targetInputDirection = new Vector2(horizontalInput, verticalInput);
            // ������������� ������� ����������� �����
            _currentInputDirection = Vector2.Lerp(_currentInputDirection, targetInputDirection, Time.deltaTime * inputLerpSpeed);
            // ����������� ������, ���� ��� ����� ������ 1
            if (_currentInputDirection.magnitude > 1)
            {
                _currentInputDirection.Normalize();
            }
            // �������� ������ � ������ �����
            if (_currentInputDirection.magnitude > 0.01f)
            {
                Vector3 moveDirection = new Vector3(_currentInputDirection.x, _currentInputDirection.y, 0);
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            }

            if (!_isMoveCursor)
                return;

            // �������� ������ ��� ��������� ������� �� ���� ������
            if (Input.mousePosition.y >= Screen.height - edgeScrollWidth)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
            }
            if (Input.mousePosition.y <= edgeScrollWidth)
            {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
            }
            if (Input.mousePosition.x >= Screen.width - edgeScrollWidth)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
            }
            if (Input.mousePosition.x <= edgeScrollWidth)
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
            }
        }
        private void ScrollCamera()
        {
            // ����������� � ��������� ������ � ������� �������� ����
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                Camera.main.orthographicSize -= scrollInput * scrollSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
            }
        }
    }
}

