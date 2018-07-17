using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flocking
{
	public class CameraMovement : MonoBehaviour
	{
		private float m_Deadzone = .01f;

		[SerializeField] private float m_MovementSpeed;
		[SerializeField] private float m_AscensionSpeed;

		private void Update()
		{
			Vector3 pos = transform.position;

			float xMove = Input.GetAxisRaw("Horizontal") * m_MovementSpeed * Time.deltaTime;
			float zMove = Input.GetAxisRaw("Vertical") * m_MovementSpeed * Time.deltaTime;
			bool yInput = Input.GetKey(KeyCode.Space);

			pos += new Vector3(xMove, yInput ? m_AscensionSpeed * Time.deltaTime : 0.0f, zMove);

			transform.position = pos;
		}
	}
}