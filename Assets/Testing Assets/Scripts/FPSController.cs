using System;
using Player;
using UnityEngine;

// movement script requires CharacterController component
namespace Testing_Assets.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : FPSPlayer
    {
        public float speed = 12f;
        public float gravity = -9.81f;
        public Transform groundCheck;
        public Camera cam;
        public float jumpHeight = 2f;
        public float mouseSensitivity = 100f;
        public float range = 100f;
    
        private CharacterController controller;
        private Vector3 velocity;
        private bool isGrounded = false;
        private float xRot = 0f;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            // making sure the mouse doesn't leave the game screen
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (health <= 0)
            {
                Debug.Log("You're dead");
                Destroy(gameObject);
            }

            // mouse look
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f); // making sure we can only go up to 90 degrees
            transform.GetChild(0).localRotation = Quaternion.Euler(xRot, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, LayerMask.GetMask("ground"));
        
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            // movement
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
       
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                shoot();
            }
        }

        public override void takeDamage(int damage)
        {
            throw new NotImplementedException();
        }

        public override void shoot()
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, transform.forward, out hit, range))
            {
                Debug.Log("hit");
            }
        }
    }
}