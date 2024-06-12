using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro CounterTxt;
    [SerializeField] private GameObject stickMan;

    [Range(0f,1f)] [SerializeField] private float DistanceFactor, Radius;

    public Transform enemy;
    public bool Attack;

    void Start()
    {
        for (int i = 0; i < Random.Range(20,120); i++)

        {
           Instantiate(stickMan, transform.position, new Quaternion(0f, 180f, 0f, 1f), transform);
        }

        CounterTxt.text = (transform.childCount - 1).ToString();

        FormatStickMan();

        
    }

     public void FormatStickMan()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);
            
            var NewPos = new Vector3(x,-0.9263f,z);

            transform.transform.GetChild(i).localPosition = NewPos;
        }
    }

    void Update()
    {

         if (Attack && transform.childCount > 1)
        {
            var enemyDirection = enemy.position - transform.position;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation,quaternion.LookRotation(enemyDirection,Vector3.up),
                    Time.deltaTime * 3f);
                if (enemy.childCount > 1)
                {
                    var distance = enemy.GetChild(1).position - transform.GetChild(i).position;

                    if (distance.magnitude < 1.5f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            enemy.GetChild(1).position,Time.deltaTime * 2f);
                    } 
                }
              
            }
        }
        
    }

    public void AttackThem(Transform enemyForce)
    {
        enemy = enemyForce;
        Attack = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("run",true);
        }
    }
}
