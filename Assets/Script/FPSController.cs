using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class FPSController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float mouseSensitivity = 2f;
        public float jumpForce = 5f;
        public float gravity = 9.81f;
        public Transform cameraTransform;
        public CharacterController characterController;

        private float _verticalRotation = 0f;
        private Vector3? _lastPosition = null;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (!InputMamager.Instance.IsLock)
            {
                Move();
                Look();

                if (ExploreManager.Instance.PlayerSpeed > 0)
                {
                    ExploreManager.Instance.CheckEvent(Utility.ConvertToVector2Int(transform.position));
                    ExploreManager.Instance.CheckVisit(transform);
                    ExploreManager.Instance.CheckTreasure(transform);
                    ExploreManager.Instance.CheckDoor(transform);
                }
            }

            if (_lastPosition != null)
            {
                ExploreManager.Instance.PlayerSpeed = Vector3.Distance((Vector3)_lastPosition, transform.position);
            }
            _lastPosition = transform.position;
        }

        void Move()
        {
            if (InputMamager.Instance.IsLock) 
            {
                return;
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            if (Mathf.Abs(moveX) > 0 || Mathf.Abs(moveZ) > 0)
            {
                Vector3 move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
                characterController.Move(move * Time.deltaTime * moveSpeed);
            }
        }

        void Look()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            _verticalRotation -= mouseY;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);           
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.tag == "FOE")
            {
                ExploreEnemyController enemy = hit.transform.GetComponent<ExploreEnemyController>();
                ExploreManager.Instance.EnterBattle(enemy.File);
            }
        }
    }
}