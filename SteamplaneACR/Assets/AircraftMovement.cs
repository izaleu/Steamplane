using UnityEngine;
using System.Collections;

//Controls the lateral and vertical movement of the aircraft.
public class AircraftMovement : MonoBehaviour
{
	[SerializeField]
	Transform
		TravelPlaneCenter, ReticlePlane, PlaneBody;

	[SerializeField]
	float
		LateralMoveSpeed, RotateSpeed, MaxRotationAngle, ResetRotationSpeed, LookSpeed, ReticleSpeed;

	float ViewportWidth, ViewportHeight;

	float AIAxis_Horizontal, AIAxis_Vertical;

	// Use this for initialization
	void Start ()
	{
		float reticleDistanceFromCamera = Vector3.Distance (ReticlePlane.position, Camera.main.transform.position);

		ViewportWidth = Vector2.Distance (Camera.main.ViewportToWorldPoint (new Vector3 (0.0f, 0.5f, reticleDistanceFromCamera)), Camera.main.ViewportToWorldPoint (new Vector3 (1.0f, 0.5f, reticleDistanceFromCamera)));
		ViewportHeight = Vector2.Distance (Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.0f, reticleDistanceFromCamera)), Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 1.0f, reticleDistanceFromCamera))) / 2;
	}

	void FixedUpdate ()
	{
		Reason ();

		#region Movement
		float Xpos = AIAxis_Horizontal * ViewportWidth / 10;
		float Ypos = AIAxis_Vertical;// / TravelPlaneBounds.size.y;

		float reticleX = AIAxis_Horizontal * ViewportWidth;
		float reticleY = AIAxis_Vertical * ViewportHeight;

		//Move reticle
		ReticlePlane.localPosition = Vector3.Lerp (ReticlePlane.localPosition, new Vector3 (reticleX, reticleY, ReticlePlane.localPosition.z), Time.deltaTime * ReticleSpeed);

		//Move aircraft
		transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3 (Xpos, Ypos, transform.localPosition.z), Time.deltaTime * LateralMoveSpeed);
		#endregion

		#region rotation

		//Make aircraft face reticle
		Vector3 midlookPos = Vector3.Lerp (transform.position, ReticlePlane.position, Time.deltaTime * LookSpeed);

		transform.LookAt (midlookPos);

		float Xrot = AIAxis_Horizontal * Mathf.Abs (AIAxis_Horizontal) * RotateSpeed;

		if (PlaneBody.localRotation.z > MaxRotationAngle) {
			Xrot = MaxRotationAngle;
		} else if (PlaneBody.localRotation.z < -MaxRotationAngle) {
			Xrot = -MaxRotationAngle;
		} else {
			PlaneBody.Rotate (Vector3.forward, Xrot);
		}

		//Body rotation during turns
		if (Mathf.Abs (AIAxis_Horizontal) <= 0.1f) {
			PlaneBody.localRotation = Quaternion.Lerp (PlaneBody.localRotation, Quaternion.identity, Time.smoothDeltaTime * ResetRotationSpeed);
		}
		#endregion

	}

	void OnCollisionEnter (Collision collider)
	{
		//Debug.Log (collider.gameObject.name);
		Vector3 AwayFromObstacle = transform.position - collider.transform.position;
		AIAxis_Horizontal = AwayFromObstacle.x;
		AIAxis_Vertical = AwayFromObstacle.y;
	}

	void OnCollisionStay (Collision collider)
	{
		//Debug.Log (collider.gameObject.name);
		Vector3 AwayFromObstacle = transform.position - collider.transform.position;
		AIAxis_Horizontal = AwayFromObstacle.x;
		AIAxis_Vertical = AwayFromObstacle.y;
	}
	void OnCollisionExit (Collision collider)
	{
		//Debug.Log (collider.gameObject.name);
		AIAxis_Horizontal = 0.0f;
		AIAxis_Vertical = 0.0f;
	}

	void Reason ()
	{

	}
}
