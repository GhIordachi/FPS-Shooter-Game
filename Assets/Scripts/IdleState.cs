using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    PursueTargetState pursueTargetState;

    [Header("Detection Parameters")]
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] float detectionRadius = 5;
    [SerializeField] float minimumDetectionAngle = -50f;
    [SerializeField] float maximumDetectionAngle = 50f;
    [SerializeField] float characterEyeLevel = 1.6f;
    [SerializeField] LayerMask ignoreForLineOfSightDetection;

    //This states makes the zombie idle until it finds the player in his range

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget != null)
        {
            SoundManager.instance.PlaySound("zombie_pursue");
            return pursueTargetState;
        }
        else
        {
            FindThePlayer(zombieManager);
            return this;
        }
    }

    private void FindThePlayer(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        for(int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            if (player != null)
            {
                Vector3 targetDirection = transform.position - player.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                if(viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
                {
                    RaycastHit hit;
                    Vector3 playerStartPoint = new Vector3(player.transform.position.x,characterEyeLevel,player.transform.position.z);
                    Vector3 zombieStartPoint = new Vector3(transform.position.x, characterEyeLevel, transform.position.z); ;

                    if(Physics.Linecast(playerStartPoint,zombieStartPoint, out hit, ignoreForLineOfSightDetection))
                    {

                    }
                    else
                    {
                        zombieManager.currentTarget = player;
                    }
                }
            }
        }
    }
}
