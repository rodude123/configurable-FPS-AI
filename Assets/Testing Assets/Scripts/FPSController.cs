using System;
using Assets.Scripts.Player;
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

		private void Start()
		{
			controller = GetComponent<CharacterController>();
			// making sure the mouse doesn't leave the game screen
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void Update()
		{
			// mouse look
			var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
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
			var x = Input.GetAxis("Horizontal");
			var z = Input.GetAxis("Vertical");
			var move = transform.right * x + transform.forward * z;
			controller.Move(move * speed * Time.deltaTime);

			velocity.y += gravity * Time.deltaTime;
			controller.Move(velocity * Time.deltaTime);

			if (Input.GetButtonDown("Jump") && isGrounded)
			{
				velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
			}

			if (Input.GetButtonDown("Fire1"))
			{
				Shoot();
			}
		}

		public override void TakeDamage(float damage)
		{
			health -= damage;
			if (health > 0)
			{
				return;
			}
			health = 0;
		}

		public override void Shoot()
		{
			if (!Physics.Raycast(cam.transform.position, transform.forward, out var hit, range))
			{
				return;
			}

			if (!hit.transform.root.CompareTag($"Enemy"))
			{
				return;
			}
			hit.transform.root.GetComponent<Assets.Scripts.Enemy.Enemy>().TakeDamage(20);
		}
	}
}