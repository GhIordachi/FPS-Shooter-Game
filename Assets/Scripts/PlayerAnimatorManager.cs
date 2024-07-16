using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimatorManager : MonoBehaviour
{
    public Animator animator;
    PlayerManager playerManager;
    PlayerLocomotionManager playerLocomotionManager;

    RigBuilder rigBuilder;
    [Header("Hand IK Constraints")]
    public TwoBoneIKConstraint rightHandIK;
    public TwoBoneIKConstraint leftHandIK;

    [Header("Aiming Constraints")]
    public MultiAimConstraint spine01;
    public MultiAimConstraint spine02;
    public MultiAimConstraint head;

    float snappedHorizontal;
    float snappedVertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigBuilder = GetComponent<RigBuilder>();
        playerManager = GetComponent<PlayerManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    public void PlayAnimationWithOutRootMotion(string targetAnimation, bool isPerformingAction)
    {
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.SetBool("disableRootMotion", true);
        animator.applyRootMotion = false;
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void PlayAnimation(string targetAnimation, bool isPerformingAction)
    {
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.SetBool("disableRootMotion", true);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void HandleAnimatorValues(float horizontalMovement, float verticalMovement, bool isRunning)
    {
        if(horizontalMovement > 0)
        {
            snappedHorizontal = 1;
        }
        else if(horizontalMovement < 0)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }

        if (verticalMovement > 0)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        if(isRunning && snappedVertical > 0)
        {
            snappedVertical = 2;
        }

        animator.SetFloat("Horizontal", snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", snappedVertical, 0.1f, Time.deltaTime);
    }

    public void AssignHandIK(RightHandTarget rightTarget, LeftHandTarget leftTarget)
    {
        rightHandIK.data.target = rightTarget.transform;
        leftHandIK.data.target = leftTarget.transform;
        rigBuilder.Build();
    }

    public void ClearHandIK()
    {
        rightHandIK.data.targetPositionWeight = 0;
        rightHandIK.data.targetRotationWeight = 0;

        leftHandIK.data.targetPositionWeight = 0;
        leftHandIK.data.targetRotationWeight = 0;
    }

    public void RefreshHandIK()
    {
        rightHandIK.data.targetPositionWeight = 1;
        rightHandIK.data.targetRotationWeight = 1;

        leftHandIK.data.targetPositionWeight = 1;
        leftHandIK.data.targetRotationWeight = 1;
    }

    public void UpdateAimConstraints()
    {
        if (playerManager.isAiming)
        {
            spine01.weight = 0.9f;
            spine02.weight = 0.3f;
            head.weight = 0.7f;
        }
        else
        {
            spine01.weight = 0;
            spine02.weight = 0;
            head.weight = 0;
        }
    }

    private void OnAnimatorMove()
    {
        if (playerManager.disableRootMotion)
            return;

        Vector3 animatorDeltaPosition = animator.deltaPosition;
        animatorDeltaPosition.y = 0;

        Vector3 velocity = animatorDeltaPosition / Time.deltaTime;
        playerLocomotionManager.playerRigidbody.drag = 0;
        playerLocomotionManager.playerRigidbody.velocity = velocity;
        transform.rotation *= animator.deltaRotation;
    }
}
