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
        if (anim != null)
        {
            anim.SetFloat("moveX", 0f);
            anim.SetFloat("moveY", 0f);
        }
    }

    public void ResumeMovement()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (isPaused || isWaiting || currentTargetTransform == null)
        {
            if (anim != null)
            {
                anim.SetFloat("moveX", 0f);
                anim.SetFloat("moveY", 0f);
            }
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
            Vector2 moveDir = directionVector.normalized;
            
            if (anim != null)
            {
                anim.SetFloat("moveX", moveDir.x);
                anim.SetFloat("moveY", moveDir.y);
            }

            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPosition, 
                5f * Time.deltaTime
            );
        }
        else
        {
            if (anim != null)
            {
                anim.SetFloat("moveX", 0f);
                anim.SetFloat("moveY", 0f);
            }
            StartCoroutine(ProcessArrival());
        }
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
        if (anim != null)
        {
            anim.SetFloat("moveX", 0f);
            anim.SetFloat("moveY", 0f);
        }

        if (currentTargetData != null)
        {
            if (currentTargetData.wait)
            {
                yield return new WaitForSeconds(3f);
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