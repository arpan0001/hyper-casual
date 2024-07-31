using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class bossManager : MonoBehaviour
{
    public Animator BossAnimator;
    public static bossManager BossManagerCls;
    public bool LockOnTarget, BossIsAlive, DeathAnimationTriggered;
    private Transform target;
    public Slider HealthBar;
    public TextMeshProUGUI Health_bar_amount;
    public int Health;
    public GameObject Particle_Death;
    public GameObject BossDefeatedUI;
    public float maxDistance = 10f;
    public float minDistance = 3f;
    public delegate void BossDeathDelegate();
    public static event BossDeathDelegate OnBossDeath;

    void Start()
    {
        BossManagerCls = this;
        BossAnimator = GetComponent<Animator>();
        BossIsAlive = true;
        DeathAnimationTriggered = false;
        HealthBar.value = HealthBar.maxValue = Health = 200;
        Health_bar_amount.text = Health.ToString();
        BossDefeatedUI.SetActive(false); // Ensure UI is initially inactive
    }

    void Update()
    {
        HealthBar.transform.rotation = Quaternion.Euler(HealthBar.transform.rotation.x, 0f, HealthBar.transform.rotation.y);

        if (BossIsAlive)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in players)
            {
                var playerDistance = Vector3.Distance(player.transform.position, transform.position);

                if (playerDistance < maxDistance && !LockOnTarget)
                {
                    target = player.transform;
                    BossAnimator.SetBool("fight", true);

                    transform.position = Vector3.MoveTowards(transform.position, target.position, 1f * Time.deltaTime);
                }

                if (playerDistance < minDistance)
                    LockOnTarget = true;
            }

            if (LockOnTarget && target != null)
            {
                var bossRotation = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(bossRotation, Vector3.up), 10f * Time.deltaTime);
            }

            if (players.Length == 0)
            {
                BossAnimator.SetBool("fight", false);
                BossAnimator.SetFloat("attackmode", 4f);
            }

            if (Health <= 0 && !DeathAnimationTriggered)
            {
                TriggerBossDeath();
            }
        }
    }

    private void TriggerBossDeath()
    {
        OnBossDeath?.Invoke();
        DeathAnimationTriggered = true;
        BossAnimator.SetTrigger("Death"); // Trigger the death animation
        StartCoroutine(HandleBossDeath());
    }

    private IEnumerator HandleBossDeath()
    {
        // Wait for the length of the death animation
        yield return new WaitForSeconds(BossAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Additional 4 seconds wait before deactivating the boss
        yield return new WaitForSeconds(4f);
        
        // Deactivate the boss game object
        gameObject.SetActive(false);
        BossIsAlive = false;

        // Instantiate death particle effect
        Instantiate(Particle_Death, transform.position, Quaternion.identity);

        // Activate the BossDefeatedUI
        BossDefeatedUI.SetActive(true);
    }

    public void ChangeTheBossAttackMode()
    {
        Debug.Log("Changing boss attack mode...");
        BossAnimator.SetFloat("attackmode", Random.Range(2, 4));
    }
}
