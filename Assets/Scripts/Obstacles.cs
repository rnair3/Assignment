using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {

    //SerializeField] GameObject obstacle;
    private GameObject cube;

    // Use this for initialization
    void Start () {
    //    cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
      //  cube.transform.position = new Vector3(0f, -0.7f,0f);
    }
	
	// Update is called once per frame
	void Update () {

        //Detect right click
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                //To set the position of the obstacle at the click point
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
                cube.layer = LayerMask.NameToLayer("Unwalkable");
            }
        }


    }
}
