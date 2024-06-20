using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0 && BossIsAlive)
        {
            gameObject.SetActive(false);
            BossIsAlive = false;
        }
    }
}
