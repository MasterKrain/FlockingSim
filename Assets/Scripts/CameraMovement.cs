using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Flocking
{
	public class CameraMovement : MonoBehaviour
	{
		[SerializeField] private float _movementSpeed;
		[SerializeField] private float _ascensionSpeed;

		private float _deadzone = .01f;

		private void Update()
		{
			Vector3 pos = transform.position;

			float xMove = Input.GetAxisRaw("Horizontal") * _movementSpeed * Time.deltaTime;
			float zMove = Input.GetAxisRaw("Vertical") * _movementSpeed * Time.deltaTime;
			bool yInput = Input.GetKey(KeyCode.Space);

			pos += new Vector3(xMove, yInput ? _ascensionSpeed * Time.deltaTime : 0.0f, zMove);

			transform.position = pos;
		}
	}
}