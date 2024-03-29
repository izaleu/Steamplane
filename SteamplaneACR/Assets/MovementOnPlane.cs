﻿using UnityEngine;
using System.Collections;

public class MovementOnPlane : MonoBehaviour
{
	Bounds
		TravelPlaneBounds;

	[SerializeField]
	Transform
		TravelPlaneCenter, ReticlePlane, PlaneBody;

	[SerializeField]
	float
		LateralMoveSpeed, RotateSpeed, MaxRotationAngle, ResetRotationSpeed, LookSpeed, ReticleSpeed;

	float ViewportWidth, ViewportHeight;

	// Use this for initialization
	void Start ()
	{
		float reticleDistanceFromCamera = Vector3.Distance (ReticlePlane.position, Camera.main.transform.position);

		ViewportWidth = Vector2.Distance (Camera.main.ViewportToWorldPoint (new Vector3 (0.0f, 0.5f, reticleDistanceFromCamera)), Camera.main.ViewportToWorldPoint (new Vector3 (1.0f, 0.5f, reticleDistanceFromCamera)));
		ViewportHeight = Vector2.Distance (Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.0f, reticleDistanceFromCamera)), Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 1.0f, reticleDistanceFromCamera))) / 2;
	}

	void FixedUpdate ()
	{
		#region Movement
		float Xpos = Input.GetAxis ("Horizontal") * ViewportWidth / 10;
		float Ypos = Input.GetAxis ("Vertical");// / TravelPlaneBounds.size.y;

		float reticleX = Input.GetAxis ("Horizontal") * ViewportWidth;
		float reticleY = Input.GetAxis ("Vertical") * ViewportHeight;

		//Move reticle
		ReticlePlane.localPosition = Vector3.Lerp (ReticlePlane.localPosition, new Vector3 (reticleX, reticleY, ReticlePlane.localPosition.z), Time.deltaTime * ReticleSpeed);

		//Move aircraft
		transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3 (Xpos, Ypos, transform.localPosition.z), Time.deltaTime * LateralMoveSpeed);
		#endregion

		#region rotation

		//Make aircraft face reticle
		Vector3 midlookPos = Vector3.Lerp (transform.position, ReticlePlane.position, Time.deltaTime * LookSpeed);

		transform.LookAt (midlookPos);

		float Xrot = Input.GetAxis ("Horizontal") * Mathf.Abs (Input.GetAxis ("Horizontal")) * RotateSpeed;

		if (PlaneBody.localRotation.z > MaxRotationAngle) {
			Xrot = MaxRotationAngle;
		} else if (PlaneBody.localRotation.z < -MaxRotationAngle) {
			Xrot = -MaxRotationAngle;
		} else {
			PlaneBody.Rotate (Vector3.forward, Xrot);
		}

		//Body rotation during turns
		if (Mathf.Abs (Input.GetAxis ("Horizontal")) <= 0.1f) {
			PlaneBody.localRotation = Quaternion.Lerp (PlaneBody.localRotation, Quaternion.identity, Time.smoothDeltaTime * ResetRotationSpeed);
		}
		#endregion

	}
}
