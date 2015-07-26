using UnityEngine;
using System.Collections;

public class MovementOnPlane : MonoBehaviour
{
	Bounds
		TravelPlaneBounds;

	[SerializeField]
	Transform
		TravelPlaneCenter, ReticlePlane, PlaneBody;

	[SerializeField]
	GameObject
		TravelPlaneQuad;

	[SerializeField]
	float
		LateralMoveSpeed, RotateSpeed, MaxRotationAngle, ResetRotationSpeed, LookSpeed, ReticleSpeed;

	// Use this for initialization
	void Start ()
	{

		//build the bounds
		TravelPlaneBounds = new Bounds ();
		TravelPlaneBounds.Encapsulate (TravelPlaneQuad.GetComponent<Renderer> ().bounds);
		Debug.Log (TravelPlaneBounds.size.x);
	}

	void FixedUpdate ()
	{
		#region Movement
		float Xpos = Input.GetAxis ("Horizontal") / TravelPlaneBounds.size.x;
		float Ypos = Input.GetAxis ("Vertical") / TravelPlaneBounds.size.y;

		float reticleX = Input.GetAxis ("Horizontal");
		float reticleY = Input.GetAxis ("Vertical");
		//Move reticle
		ReticlePlane.localPosition = Vector3.Lerp (ReticlePlane.localPosition, new Vector3 (reticleX, reticleY, ReticlePlane.localPosition.z), Time.deltaTime * ReticleSpeed);

		//Move aircraft
		transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3 (Xpos, Ypos, transform.localPosition.z), Time.deltaTime * LateralMoveSpeed);

		//Make aircraft face reticle

		Vector3 midlookPos = Vector3.Lerp (transform.position, ReticlePlane.position, Time.deltaTime * LookSpeed);
		transform.LookAt (midlookPos);
		#endregion
		
		#region rotation
		float Xrot = Input.GetAxis ("Horizontal") * Mathf.Abs (Input.GetAxis ("Horizontal")) * RotateSpeed;

		if (PlaneBody.localRotation.z > MaxRotationAngle) {
			Xrot = MaxRotationAngle;
		} else if (PlaneBody.localRotation.z < -MaxRotationAngle) {
			Xrot = -MaxRotationAngle;
		} else {
			PlaneBody.Rotate (Vector3.forward, Xrot);
		}

		if (Mathf.Abs (Input.GetAxis ("Horizontal")) <= 0.1f) {
			PlaneBody.localRotation = Quaternion.Lerp (PlaneBody.localRotation, Quaternion.identity, Time.smoothDeltaTime * ResetRotationSpeed);
		}
		#endregion

	}
}
