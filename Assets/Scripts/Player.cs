using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
        public float sensitivity;
        public float verticalLimit;
        public Transform cameraTransform;
        public float speed;
        public float speedMultiplier;
        public float gravity;
        public float jumpForce;
        public float jumpDelta;
        public float jumpDeltaDuration;
        public float jumpRayLength;

        public const float baseSpeed = 6f;

        Vector3 playerOrientation;
        Vector3 cameraOrientation;
        CharacterController characterController;
        bool isJumping;
        Camera playerCamera;
        [SerializeField] int health = 100;

        void Start()
        {
                characterController = GetComponent<CharacterController>();
                playerCamera = GetComponentInChildren<Camera>();
                playerOrientation = transform.localEulerAngles;
                cameraOrientation = cameraTransform.localEulerAngles;
                // LockCursor();

                Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
        }

        void OnDestroy()
        {
                Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
        }

        void OnSpeedChanged(float speed)
        {
                this.speed = baseSpeed * speed;
        }

        void Update()
        {
                Look();
                Move();
                Jump();
                Shoot();
        }

        void Look()
        {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                playerOrientation.y += mouseX * sensitivity;
                cameraOrientation.x -= mouseY * sensitivity;
                cameraOrientation.x = Mathf.Clamp(cameraOrientation.x, -verticalLimit, verticalLimit);

                transform.localEulerAngles = playerOrientation;
                cameraTransform.localEulerAngles = cameraOrientation;
        }

        void Move()
        {
                float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speedMultiplier * speed : speed;

                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector3 movement = new Vector3(horizontal * currentSpeed, 0f, vertical * currentSpeed);
                movement = Vector3.ClampMagnitude(movement, currentSpeed);
                movement *= Time.deltaTime;

                movement = transform.TransformDirection(movement);
                movement.y = gravity;

                characterController.Move(movement);
        }

        void Shoot()
        {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                        float xpos = playerCamera.pixelWidth / 2;
                        float ypos = playerCamera.pixelHeight / 2;
                        Vector3 pos = new Vector3(xpos, ypos, 0);

                        Ray ray = playerCamera.ScreenPointToRay(pos);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                                if (hit.transform.tag == "Enemy")
                                {
                                        Enemy enemy = hit.transform.GetComponent<Enemy>();
                                        if (enemy != null)
                                        {
                                                enemy.GetHit();
                                                Messenger.Broadcast(GameEvent.ENEMY_HIT);
                                        }
                                }
                                else
                                {
                                        StartCoroutine(SpawnSphere(hit.point));
                                }
                        }
                }
        }

        IEnumerator SpawnSphere(Vector3 pos)
        {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = pos;

                yield return new WaitForSeconds(1.5f);
                Destroy(sphere);
        }

        void LockCursor()
        {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
        }

        void Jump()
        {
                if (Input.GetAxis("Jump") > 0 && !isJumping)
                {
                        Ray ray = new Ray(transform.position, -transform.up);
                        if (Physics.Raycast(ray, jumpRayLength))
                                StartCoroutine(JumpAction());
                }
        }

        IEnumerator JumpAction()
        {
                isJumping = true;
                float oldGravity = gravity;
                gravity = jumpForce;

                while (gravity > oldGravity)
                {
                        gravity -= jumpDelta;
                        yield return new WaitForSeconds(jumpDeltaDuration);
                }
                isJumping = false;
        }

        public void GetDamaged(int damage)
        {
                health -= damage;
                if (health < 0)
                {
                        health = 0;
                        Debug.Log("Player is dead");
                }
                else
                {
                        Debug.Log($"New Health: {health}");
                }
        }

        void OnGUI()
        {
                int size = 12;
                int width = playerCamera.pixelWidth / 2 - size / 4;
                int height = playerCamera.pixelHeight / 2 - size / 2;

                GUI.Label(new Rect(width, height, size, size), "*");
        }
}
