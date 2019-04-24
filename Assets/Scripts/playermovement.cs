using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour {

    private float speed;                        //Speed with which the player moves
    private float radiusofSatisfactn;           //Radius within which the player can reach not the exact mouse click pixel
    [SerializeField] Transform player;          //Player Tranform
    [SerializeField] Rigidbody rb;              //Rigidbody corresponding to the player
    Vector3 towards;                            //Distance between player and mouse-click
    RaycastHit hit;                             //variable used to find the mouse click
    bool move;
    float turnSpeed;                            //The speed with which the player rotates

    // Use this for initialization
    void Start () {
        move = false;
        speed = 5f;
        radiusofSatisfactn = 1f;
        turnSpeed = 2f;
    }
	
	// Update is called once per frame
	void Update () {

        //RaycastHit hit;

        if (Input.GetMouseButton(0))
        {
            //To set up the variable that will give the position of the mouse through the camera
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                move = true;                //The player needs to move towards mouse-click point
                //   towards = hit.point;// - player.transform.position;
            }
        }

        if (move)
        {
            // Calculate vector from character to target
            towards = hit.point - player.position;

            //Apply orientation to the player object so that it faces the direction where the mouse was clicked
            player.rotation = Quaternion.Lerp(player.rotation, Quaternion.LookRotation(towards), turnSpeed * Time.deltaTime);

            // If we haven't reached the target yet
            if (towards.magnitude > radiusofSatisfactn)
            {
                // Normalize vector to get just the direction
                towards.Normalize();
                towards *= speed;

                // Move character
                rb.velocity = towards;
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                move = false;
            }
        }
    }
}
