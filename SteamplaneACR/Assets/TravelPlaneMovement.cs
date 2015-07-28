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
/*	[SerializeField]
	Path[]
		Paths;*/
	// Update is called once per frame
	//int curPath = 0;

	void Update ()
	{
		/*float oldLocation = Driver.location;

		if (Input.GetMouseButton (0)) {
			Driver.path = Paths [1];
			//Driver.location = oldLocation;
		}
		if (Input.GetMouseButton (1)) {
			Driver.path = Paths [0];
			//Driver.location = oldLocation;
		}*/
		Driver.location += Speed * Time.deltaTime;
	}
}
