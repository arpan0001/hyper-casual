using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class bossManager : MonoBehaviour
{
    public Animator BossAnimator;
    public static bossManager BossManagerCls;
    public bool LockOnTarget, BossIsAlive;
    private Transform target;
    public Slider HealthBar;
    public TextMeshProUGUI Health_bar_amount;
    public int Health;
    public GameObject Particle_Death;
    public float maxDistance = 10f; 
    public float minDistance = 3f; 
    public delegate void BossDeathDelegate();
    public static event BossDeathDelegate OnBossDeath;


    void Start()
    {
        BossManagerCls = this;
        BossAnimator = GetComponent<Animator>();
        BossIsAlive = true;
        HealthBar.value = HealthBar.maxValue = Health = 200;
        Health_bar_amount.text = Health.ToString();
    }

    void Update()
    {
       HealthBar.transform.rotation = Quaternion.Euler( HealthBar.transform.rotation.x,0f, HealthBar.transform.rotation.y);

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

        if (Health <= 0 && BossIsAlive)
        {
            OnBossDeath?.Invoke();
            
            gameObject.SetActive(false);
            BossIsAlive = false;
            Instantiate(Particle_Death, transform.position, Quaternion.identity);
            SceneManager.LoadScene(1); 
        }
    }

   public void ChangeTheBossAttackMode()
    {
        Debug.Log("Changing boss attack mode...");
        BossAnimator.SetFloat("attackmode", Random.Range(2, 4));
    }
}