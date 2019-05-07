using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeggarObstacle : MovingObstacle
{
    [SerializeField]
    bool Chasing = false, Resetting = false, OnCooldown = false;

    Vector3 InitialPosition;
    [SerializeField]
    Quaternion InitialRotation;

    [SerializeField]
    LayerMask PlayersLayers;

    [SerializeField]
    float MaxDistance, TimeToStop = 5f, Cooldown = 20f, RotationSpeed = 4f;

    Collider[] NearbyPlayers = new Collider[7];

    Animator anim;

    // Use this for initialization
    void Start()
    {
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
        anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, 6f, NearbyPlayers, PlayersLayers) > 0 && target == null && !Resetting && !OnCooldown)
        {
            anim.SetBool("Activated", true);
            target = NearbyPlayers[0].gameObject.GetComponent<PlayerObstacleManager>().GetPlayerController();
            MovementSpeed = 1.5f;
            Chase();

        }

        if (Chasing)
        {
            if (Vector3.Distance(InitialPosition, transform.position) > MaxDistance)
            {
                StopChasing();

            }
            else
            {
                Vector3 Destination = target.transform.position + target.transform.forward * 2f;
                Destination.y = transform.position.y;
                if (Vector3.Distance(transform.position, Destination) > 1.25f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Destination, Time.deltaTime * MovementSpeed);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Destination - transform.position), Time.deltaTime * RotationSpeed);
                    anim.SetBool("NearPlayer", false);

                }
                else
                {
                    anim.SetBool("NearPlayer", true);

                }

            }

        }
        else if (Resetting)
        {
            transform.position = Vector3.MoveTowards(transform.position, InitialPosition, Time.deltaTime * MovementSpeed);
            if (Vector3.Distance(InitialPosition, transform.position) > 0.01f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(InitialPosition - transform.position), Time.deltaTime * RotationSpeed);
            else
            {
                Resetting = false;
                OnCooldown = true;
                Invoke("EndCooldown", Cooldown);
            } 

        }
        else
        {
            anim.SetBool("Activated", false);
            transform.position = InitialPosition;
            transform.rotation = Quaternion.Slerp(transform.rotation, InitialRotation, Time.deltaTime * RotationSpeed * 2f);

        }

    }

    protected override void Activate()
    {

    }

    void Chase()
    {
        Chasing = true;
        Invoke("StopChasing", TimeToStop);
    }

    void StopChasing()
    {
        anim.SetBool("NearPlayer", false);

        Chasing = false;
        Resetting = true;
        MovementSpeed = 1f;
        target = null;
    }

    void EndCooldown()
    {
        OnCooldown = false;
    }
	

}
