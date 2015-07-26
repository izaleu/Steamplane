using UnityEngine;
using System.Collections;
using WhiteCat;

public class TravelPlaneMovement : MonoBehaviour
{

	[SerializeField]
	float
		Speed;
	[SerializeField]
	PathDriver
		Driver;

	// Update is called once per frame
	void LateUpdate ()
	{
		Driver.location += Speed;
	}
}
