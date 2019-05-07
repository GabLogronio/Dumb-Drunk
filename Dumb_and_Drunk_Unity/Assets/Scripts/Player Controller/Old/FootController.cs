using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FootController : MonoBehaviour {

    [Header("Movement parameters")]
    [SerializeField]
    float FootHeight, StepForce, RotationForce;

    [Header("RightFoot = true, LeftFoot = false")]
    [SerializeField]
    bool RightFoot;
    [SerializeField]
    bool CanBeMoved = false, CanAttach = true;

    [SerializeField]
    GameObject PlayerController, InstantiableJoint;
    GameObject Joint;
    FixedJoint FixedJoint;


    Rigidbody rb;

    void Start () {

        rb = GetComponent<Rigidbody>();

	}
	
	void Update () {
        //if (Joint == null) rb.isKinematic = false;
        //else rb.isKinematic = true;
    }

    public void Move(Vector3 direction, bool MovingBack)
    {
        if (CanBeMoved)
        {
            rb.velocity = Vector3.zero;

            direction = direction.normalized * 0.5f;
            Vector3 FinalDirection = PlayerController.transform.position - transform.position + direction;
            if (RightFoot) FinalDirection += PlayerController.transform.right * 0.15f;
            else FinalDirection -= PlayerController.transform.right * 0.15f;

            if (!MovingBack)
            {
                if (FootHeight - transform.position.y >= 0f) FinalDirection.y = FootHeight - transform.position.y;
                else FinalDirection.y = transform.position.y - FootHeight; 
            }
            else
            {
                if (FootHeight / 2f - transform.position.y >= 0f) FinalDirection.y = FootHeight / 2f - transform.position.y;
                else FinalDirection.y = transform.position.y - FootHeight / 2f;

            }

            rb.AddForce(FinalDirection * StepForce);

        }
    }

    public void Detach()
    {
        if (Joint != null) Destroy(Joint);
        
        //if (FixedJoint != null) Destroy(FixedJoint);
        CanBeMoved = true;
    }

    private void OnCollisionExit(Collision coll)
    {
        if (Joint == null && coll.gameObject.layer == 11)
        {
            PlayerController.GetComponent<PlayerInputController>().SetFoot(RightFoot, false);
        }
    }

    public void Release()
    {
        CanBeMoved = false;
    }

    public void Rotate(bool direction)
    {
        if (Joint != null)
        {
            //if(direction) rb.AddTorque(Vector3.up * RotationForce);
            //else rb.AddTorque( - Vector3.up * RotationForce);
            transform.Rotate(Vector3.up * Time.deltaTime * RotationForce);

        }
    }

    public void SetFootCanAttach(bool ToSet)
    {
        CanAttach = ToSet;
    }

    private void OnCollisionEnter(Collision coll)
    {
        
        if (coll.gameObject.layer == 11 && CanAttach && Joint == null)
        {
            PlayerController.GetComponent<PlayerInputController>().SetFoot(RightFoot, true);

            ContactPoint contact = coll.contacts[0];
            Joint = Instantiate(InstantiableJoint, new Vector3(contact.point.x, contact.point.y, contact.point.z), coll.gameObject.transform.rotation);
            Joint.GetComponent<HingeJoint>().connectedBody = rb;
            Joint.transform.parent = transform;

        }
        /*
        if (coll.gameObject.layer == 11 && CanAttach && FixedJoint == null)
        {
            PlayerController.GetComponent<PlayerInputController>().SetFoot(RightFoot, true);
            FixedJoint = gameObject.AddComponent<FixedJoint>();
            Rigidbody OtherRb = coll.gameObject.GetComponent<Rigidbody>();
            if (OtherRb != null) FixedJoint.connectedBody = OtherRb;

        }*/
        
    }

    public void FixFoot(bool ToSet)
    {
        if (ToSet) FixedJoint = gameObject.AddComponent<FixedJoint>();
        else if (FixedJoint != null) Destroy(FixedJoint);

    }

}
