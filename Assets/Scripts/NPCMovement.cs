using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    private List<NPCStore.NumberGroup> groups;
    private Transform pointsParent;

    private int currentGroupIndex = 0;
    private bool isWaiting = false;
    private bool isPaused = false;

    private Transform currentTargetTransform;
    private NPCStore.NumberData currentTargetData;

    private Animator anim;
    private string lastDirection = "down";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(List<NPCStore.NumberGroup> allowedGroups, Transform parentTransform)
    {
        groups = allowedGroups;
        pointsParent = parentTransform;
        currentGroupIndex = 0;

        MoveToNextTarget();
    }

    public void PauseMovement()
    {
        isPaused = true;
        if (anim != null) anim.SetBool("isMoving", false);
    }

    public void ResumeMovement()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (isPaused || isWaiting || currentTargetTransform == null)
        {
            if (anim != null) anim.SetBool("isMoving", false);
            return;
        }

        Vector3 targetPosition = new Vector3(
            currentTargetTransform.position.x,
            currentTargetTransform.position.y,
            transform.position.z
        );

        Vector2 directionVector = targetPosition - transform.position;
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance >= 0.05f)
        {
            DetermineDirection(directionVector);
            SetAnimation();
            if (anim != null) anim.SetBool("isMoving", true);

            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPosition, 
                3f * Time.deltaTime
            );
        }
        else
        {
            if (anim != null) anim.SetBool("isMoving", false);
            StartCoroutine(ProcessArrival());
        }
    }

    private void DetermineDirection(Vector2 moveInput)
    {
        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            if (moveInput.x > 0) lastDirection = "right";
            else if (moveInput.x < 0) lastDirection = "left";
        }
        else
        {
            if (moveInput.y > 0) lastDirection = "up";
            else if (moveInput.y < 0) lastDirection = "down";
        }
    }

    private void SetAnimation()
    {
        if (anim == null) return;

        anim.SetBool("isLeft", lastDirection == "left");
        anim.SetBool("isRight", lastDirection == "right");
        anim.SetBool("isUp", lastDirection == "up");
        anim.SetBool("isDown", lastDirection == "down");
    }

    private void MoveToNextTarget()
    {
        if (groups == null || groups.Count == 0)
        {
            currentTargetTransform = null;
            return;
        }

        if (currentGroupIndex >= groups.Count)
        {
            currentGroupIndex = 0;
        }

        NPCStore.NumberGroup currentGroup = groups[currentGroupIndex];

        if (currentGroup.numberList == null || currentGroup.numberList.Count == 0)
        {
            currentTargetTransform = null;
            return;
        }

        int randomIndex = Random.Range(0, currentGroup.numberList.Count);
        currentTargetData = currentGroup.numberList[randomIndex];

        string targetName = "Pos" + currentTargetData.number;
        currentTargetTransform = pointsParent.Find(targetName);

        if (currentTargetTransform == null)
        {
            currentGroupIndex = 0;
            MoveToNextTarget();
        }
    }

    private IEnumerator ProcessArrival()
    {
        isWaiting = true;
        if (anim != null) anim.SetBool("isMoving", false);

        if (currentTargetData != null)
        {
            if (currentTargetData.wait)
            {
                yield return new WaitForSeconds(5f);
            }

            if (currentTargetData.isPaused)
            {
                isPaused = true;
                yield return new WaitUntil(() => !isPaused);
            }

            currentGroupIndex = currentTargetData.number;
        }

        isWaiting = false;

        MoveToNextTarget();
    }
}