using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class memeberManager : MonoBehaviour
{ 
    public Animator character_animator;
    public GameObject Particle_Death;
    private Transform Boss;
    public int Health;
    public float MinDistanceOfEnemy, MaxDistanceOfEnemy, moveSpeed;
    public bool fight, member;
    private Rigidbody rb;
    private CapsuleCollider _capsuleCollider;

    void Start()
    {
        character_animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        Boss = GameObject.FindWithTag("boss").transform;

        Health = 5;
    }

    void Update()
    {
        var bossDistance = Boss.position - transform.position;

        if (!fight)
        {
            if (bossDistance.sqrMagnitude <= MaxDistanceOfEnemy * MaxDistanceOfEnemy)
            {
                PlayerManager.PlayerManagerInstance.attackToTheBoss = true;
                PlayerManager.PlayerManagerInstance.gameState = false;
            }

            if (PlayerManager.PlayerManagerInstance.attackToTheBoss && member)
            {
                Vector3 targetPosition = CalculateTargetPosition();
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                var stickManRotation = new Vector3(Boss.position.x, transform.position.y, Boss.position.z) - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(stickManRotation, Vector3.up), 10f * Time.deltaTime);

                character_animator.SetFloat("run", 1f);

                rb.velocity = Vector3.zero;
            }
        }

        if (bossDistance.sqrMagnitude <= MinDistanceOfEnemy * MinDistanceOfEnemy)
        {
            fight = true;

            var stickManRotation = new Vector3(Boss.position.x, transform.position.y, Boss.position.z) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(stickManRotation, Vector3.up), 10f * Time.deltaTime);

            character_animator.SetBool("fight", true);

            MinDistanceOfEnemy = MaxDistanceOfEnemy;

            rb.velocity = Vector3.zero;
        }
        else
        {
            fight = false;
        }
    }

    private Vector3 CalculateTargetPosition()
    {
        // Determine a position around the boss based on the member's index and total members
        int totalMembers = PlayerManager.PlayerManagerInstance.player.childCount - 1; // Assuming player is a Transform containing all stickmen
        int memberIndex = transform.GetSiblingIndex(); // Assuming each member is a sibling in the hierarchy

        float angle = 360f / totalMembers * memberIndex;
        Vector3 offset = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * MinDistanceOfEnemy;
        return Boss.position + offset;
    }

    public void ChangeTheAttackMode()
    {
        character_animator.SetFloat("attackmode", Random.Range(0, 3));
    }
}
