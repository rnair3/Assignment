using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formationalmovement : MonoBehaviour {

    private float radiusOfSatisfaction;
    private float maxSpeed;
    private float turnSpeed;

    [SerializeField] Transform leader;              
    [SerializeField] float distanceOffset;
    [SerializeField] float rotationOffset;
    [SerializeField] float angleOffset;

    [SerializeField] Transform robot;
    [SerializeField] Rigidbody robotRb;
    

    public void Start()
    {
        radiusOfSatisfaction = 1f;
        maxSpeed = 7f;
        turnSpeed = 5f;
    }

    public void Update()
    {
        //A point on the forward facing vector from the leader's direction
        Vector3 projection = leader.forward * distanceOffset;

        //Rotate the point based on the character's rotation offset.
        Vector3 finalPosition = Quaternion.Euler(0f, rotationOffset, 0f) * projection;

        finalPosition += leader.position;
        Debug.DrawRay(robot.transform.position, finalPosition);
        if (Vector3.Distance(robot.position,finalPosition) > radiusOfSatisfaction)
        {
            //Remaining distance to reach the destination
            Vector3 towards = finalPosition - robot.position;

            //normalize the vector
            towards.Normalize();
            towards *= maxSpeed;
            robotRb.velocity = towards;

            //Rotate the player to face the destination point
            robot.rotation = Quaternion.Lerp(robot.rotation, Quaternion.LookRotation(towards), turnSpeed * Time.deltaTime);
            robot.eulerAngles = new Vector3(0f, angleOffset, 0f);
        }
        else
        {
            robotRb.velocity = Vector3.zero;
            robotRb.angularVelocity = Vector3.zero;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.name == "Cube")
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //}
}
