using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // Add this line to use LINQ

public class bossManager : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();
    public Animator BossAnimator;
    public static bossManager BossManagerCls;
    private float attackMode;
    public bool LockOnTarget, BossIsAlive;
    private Transform target;
    public Slider HealthBar;
    public TextMeshProUGUI Health_bar_amount;
    public int Health;
    public GameObject Particle_Death;
    public float maxDistance, minDistance;

    public delegate void BossDeathDelegate();
    public static event BossDeathDelegate OnBossDeath;

    void Start()
    {
        BossManagerCls = this;
        
        var enemy = GameObject.FindGameObjectsWithTag("add");

        foreach (var stickMan in enemy)
            Enemies.Add(stickMan);

        BossAnimator = GetComponent<Animator>();

        BossIsAlive = true;

        HealthBar.value = HealthBar.maxValue = Health = 200;

        Health_bar_amount.text = Health.ToString();
    }

    void Update()
    {
        foreach (var stickMan in Enemies)
        {
            var stickManDistance = stickMan.transform.position - transform.position;

            if (stickManDistance.sqrMagnitude < maxDistance * maxDistance && !LockOnTarget)
            {
                target = stickMan.transform;
                BossAnimator.SetBool("fight", true); // need to define.

                transform.position = Vector3.MoveTowards(transform.position, target.position, 1f * Time.deltaTime);
            }

            if (stickManDistance.sqrMagnitude < minDistance * minDistance)
                LockOnTarget = true;
        }

        if (LockOnTarget)
        {
            var bossRotation = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(bossRotation, Vector3.up), 10f * Time.deltaTime);

            for (int i = 0; i < Enemies.Count; i++)
                if (!Enemies[i].GetComponent<memeberManager>().member)
                    Enemies.RemoveAt(i);
        }

        if (Enemies.Count == 0)
        {
            BossAnimator.SetBool("fight", false);
            BossAnimator.SetFloat("attackmode", 4f);
        }

        if (Health <= 0 && BossIsAlive)
        {
            OnBossDeath?.Invoke(); 
            gameObject.SetActive(false);
            BossIsAlive = false;
        }
    }
}
