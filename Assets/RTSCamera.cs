using UnityEngine;

namespace CodeBase.SystemGame
{

    public class RTSCamera : MonoCache
    {
        public float moveSpeed = 10f; // Скорость перемещения камеры
        public float scrollSpeed = 10f; // Скорость приближения/отдаления
        public float minZoom = 5f; // Минимальное значение зума
        public float maxZoom = 20f; // Максимальное значение зума
        public float edgeScrollWidth = 10f; // Ширина области прокрутки по краям экрана
        public float inputLerpSpeed = 10f; // Скорость интерполяции ввода
        private Vector2 _currentInputDirection;

        [SerializeField] private bool _isMoveCursor = false;

        public override void OnTick()
        {
            MoveCamera();
            ScrollCamera();
        }
        private void MoveCamera()
        {
            // Получаем ввод с клавиатуры
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            // Создаем вектор направления на основе ввода
            Vector2 targetInputDirection = new Vector2(horizontalInput, verticalInput);
            // Интерполируем текущее направление ввода
            _currentInputDirection = Vector2.Lerp(_currentInputDirection, targetInputDirection, Time.deltaTime * inputLerpSpeed);
            // Нормализуем вектор, если его длина больше 1
            if (_currentInputDirection.magnitude > 1)
            {
                _currentInputDirection.Normalize();
            }
            // Движение камеры с учетом ввода
            if (_currentInputDirection.magnitude > 0.01f)
            {
                Vector3 moveDirection = new Vector3(_currentInputDirection.x, _currentInputDirection.y, 0);
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            }

            if (!_isMoveCursor)
                return;

            // Движение камеры при наведении курсора на края экрана
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
            // Приближение и отдаление камеры с помощью колесика мыши
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                Camera.main.orthographicSize -= scrollInput * scrollSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
            }
        }
    }
}

