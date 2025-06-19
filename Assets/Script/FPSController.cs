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

        [Range(0.001f, 0.01f)]
        public float Amount = 0.002f;
        [Range(1f, 30f)]
        public float Freduency = 10f;
        [Range(10f, 100f)]
        public float Smooth = 10f;

        private float _verticalRotation = 0f;
        private Vector3? _lastPosition = null;
        private GameObject _obj = null;
        private TreasureTrigger _treasure = null;
        private LayerMask _triggerLayer; //Àð¾À©M¦aªOªº trigger

        private float _toggleSpeed = 3f;
        private Vector3 _startPosition;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _triggerLayer = ~(1 << LayerMask.NameToLayer("Trigger"));
            _startPosition = cameraTransform.localPosition;
        }

        void Update()
        {
            if (!InputMamager.Instance.IsLock)
            {
                Move();
                Look();
                Raycast();

                if (ExploreManager.Instance.PlayerSpeed > 0)
                {
                    ExploreManager.Instance.SetPlayerPosition(new Vector2(transform.position.x, transform.position.z));
                    StartHeadBob();
                }
                StopHeadBob();
            }

            if (_lastPosition != null)
            {
                ExploreManager.Instance.PlayerSpeed = Vector3.Distance((Vector3)_lastPosition, transform.position);
            }
            _lastPosition = transform.position;
        }

        void Move()
        {
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

        public void Raycast() 
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1, _triggerLayer))
            {
                if (!hit.collider.gameObject.Equals(_obj))
                {
                    _obj = hit.collider.gameObject;
                    if (hit.collider.tag == "Treasure")
                    {
                        _treasure = hit.collider.GetComponent<TreasureTrigger>();
                        _treasure.Outline.enabled = true;
                        ExploreManager.Instance.ShowTreasure(_treasure);
                    }
                    else if (hit.collider.tag == "Door")
                    {
                        if (_treasure != null)
                        {
                            _treasure.Outline.enabled = false;
                            _treasure = null;
                        }
                        ExploreManager.Instance.ShowDoor(hit.collider.GetComponent<DoorTrigger>());
                    }
                    else
                    {
                        if (_treasure != null)
                        {
                            _treasure.Outline.enabled = false;
                            _treasure = null;
                        }
                        ExploreManager.Instance.ClearObjectInfo();
                    }
                }
            }
            else
            {
                _obj = null;
                if (_treasure != null)
                {
                    _treasure.Outline.enabled = false;
                    _treasure = null;
                }
                ExploreManager.Instance.ClearObjectInfo();
            }
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.tag == "FOE")
            {
                ExploreEnemyController enemy = hit.transform.GetComponent<ExploreEnemyController>();
                ExploreManager.Instance.EnterBattle(enemy.File);
            }
        }

        private void StartHeadBob() 
        {
            Vector3 pos = Vector3.zero;
            pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Freduency) * Amount * 1.4f, Smooth * Time.deltaTime);
            pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Freduency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
            cameraTransform.localPosition += pos;
        }

        private void StopHeadBob() 
        {
            if(cameraTransform.localPosition == _startPosition) 
            {
                return;
            }
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, _startPosition, Time.deltaTime);
        }
    }
}