using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeggarObstacle : MonoBehaviour
{
    bool Chasing = false, Resetting = false, OnCooldown = false;

    Vector3 InitialPosition;
    Quaternion InitialRotation;

    PlayerBalanceManager targetPlayer = null;

    [SerializeField]
    LayerMask PlayersLayers;

    float MaxDistance = 7f, TimeToStop = 10f, Cooldown = 20f, RotationSpeed = 4f, MovementSpeed = 7f;

    Collider[] NearbyPlayers = new Collider[1];

    Animator anim;

    // Use this for initialization
    void Start()
    {
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
        anim = GetComponent<Animator>();


    }

    private void OnDestroy()
    {
        AkSoundEngine.StopAll(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, MaxDistance, NearbyPlayers, PlayersLayers) > 0 && targetPlayer == null && !Resetting && !OnCooldown)
        {
            anim.SetBool("Activated", true);
            targetPlayer = NearbyPlayers[0].gameObject.GetComponent<PlayerObstacleManager>().GetPlayerController();
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
                Vector3 Destination = targetPlayer.transform.position + targetPlayer.GetHips().forward * 1f;
                Destination.y = transform.position.y;
                if (Vector3.Distance(transform.position, Destination) > 2f)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer >= 9 && collision.gameObject.layer <= 12)
        {
            AkSoundEngine.PostEvent("TrampImpact", gameObject);
            collision.gameObject.GetComponent<PlayerObstacleManager>().Granny(transform.position);
        }
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
        targetPlayer = null;
    }

    void EndCooldown()
    {
        OnCooldown = false;
    }
	

}
