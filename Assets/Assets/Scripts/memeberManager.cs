using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class memberManager : MonoBehaviour
{
    public Animator character_animator;
    public GameObject ch_blood;
    private Transform Boss;
    public int Health;
    public float MinDistanceOfEnemy, MaxDistanceOfEnemy, moveSpeed;
    public bool fight, member;
    private Rigidbody rb;
    private CapsuleCollider _capsuleCollider;
    private AudioSource audioSource;
    public AudioClip attackModeAudioClip;
    

    void Start()
    {
        character_animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();

        GameObject bossObj = GameObject.FindWithTag("boss");
        if (bossObj != null)
        {
            Boss = bossObj.transform;
            bossManager.OnBossDeath += HandleBossDeath;
        }

        Health = 50;
    }

    void Update()
    {
        if (Boss == null) return;

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
                transform.position = Vector3.MoveTowards(transform.position, Boss.position, moveSpeed * Time.deltaTime);

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

    private void OnDestroy()
    {
        if (Boss != null)
        {
            bossManager.OnBossDeath -= HandleBossDeath;
        }
    }

    private void HandleBossDeath()
    {
        character_animator.SetFloat("attackmode", 2f);
        PlayAttackModeAudio();
    }

    private void PlayAttackModeAudio()
    {
        if (character_animator.GetFloat("attackmode") == 2f && attackModeAudioClip != null)
        {
            audioSource.PlayOneShot(attackModeAudioClip);
            
        }
    }

    

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("damage"))
        {
            Health--;

            if (Health <= 0)
            {
                Instantiate(ch_blood, transform.position, Quaternion.identity);

                gameObject.SetActive(false);
                transform.parent = null;
            }
        }
        else if (other.collider.CompareTag("blue"))
        {
            Destroy(other.gameObject);
        }
    }
}
