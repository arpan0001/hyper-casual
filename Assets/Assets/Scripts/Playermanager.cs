using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public Transform player;
    private int numberOfStickmans, numberOfEnemyStickmans;
    [SerializeField] public TextMeshPro CounterTxt;
    [SerializeField] private GameObject stickMan;
    [SerializeField] public GameObject weapon1;
    [SerializeField] public GameObject weapon2;
    [SerializeField] public GameObject weapon3;
    [SerializeField] public GameObject weapon4;

    public bool w2Activated = false;
    public bool w3Activated = false;
    public bool w1Activated = false;
    public bool w4Activated = false;

    [Range(0f, 1f)] [SerializeField] private float DistanceFactor, Radius;

    public bool moveByTouch, gameState, attackToTheBoss;
    private Vector3 mouseStartPos, playerStartPos;
    public float playerSpeed, roadSpeed;
    private Camera camera;

    [SerializeField] private Transform road;
    [SerializeField] private Transform enemy;
    private bool attack;
    public static PlayerManager PlayerManagerInstance;

    [SerializeField] private AudioClip gameStartMusic;
    private AudioSource audioSource;

    void Start()
    {
        player = transform;

        // Calculate initial number of stickmen
        numberOfStickmans = transform.childCount - 4;
        UpdateStickmanCount();

        camera = Camera.main;

        PlayerManagerInstance = this;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!gameState)
        {
            return;
        }

        if (attack)
        {
            var enemyDirection = new Vector3(enemy.position.x, transform.position.y, enemy.position.z) - transform.position;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation =
                    Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 3f);
            }

            if (enemy.GetChild(1).childCount > 1)
            {
                for (int i = 1; i < transform.childCount; i++)
                {
                    var distance = enemy.GetChild(1).GetChild(0).position - transform.GetChild(i).position;

                    if (distance.magnitude < 1.5f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            new Vector3(enemy.GetChild(1).GetChild(0).position.x, transform.GetChild(i).position.y,
                                enemy.GetChild(1).GetChild(0).position.z), Time.deltaTime * 1f);
                    }
                }
            }
            else
            {
                attack = false;
                roadSpeed = 2f;

                FormatStickMan();

                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).rotation = Quaternion.identity;
                }

                enemy.gameObject.SetActive(false);
            }

            if (transform.childCount == 5)
            {
                enemy.transform.GetChild(1).GetComponent<EnemyManager>().StopAttacking();
                gameObject.SetActive(false);
                gameState = false;
                LoadTryAgainScene();
            }
        }
        else
        {
            MoveThePlayer();
        }

        if (gameState)
        {
            road.Translate(road.forward * Time.deltaTime * roadSpeed);
        }

        if (transform.childCount == 1)
        {
            if (enemy != null && enemy.transform.GetChild(1).childCount > 0)
            {
                enemy.transform.GetChild(1).GetComponent<EnemyManager>().StopAttacking();
            }
            gameState = false;
            LoadTryAgainScene();
        }
    }

    void MoveThePlayer()
    {
        if (Input.GetMouseButtonDown(0) && gameState)
        {
            moveByTouch = true;
            var plane = new Plane(Vector3.up, 0f);
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out var distance))
            {
                mouseStartPos = ray.GetPoint(distance + 1f);
                playerStartPos = transform.position;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            moveByTouch = false;
        }

        if (moveByTouch)
        {
            var plane = new Plane(Vector3.up, 0f);
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out var distance))
            {
                var mousePos = ray.GetPoint(distance + 1f);

                if (mousePos.x != Mathf.Infinity && mousePos.y != Mathf.Infinity && mousePos.z != Mathf.Infinity)
                {
                    var move = mousePos - mouseStartPos;
                    var control = playerStartPos + move;

                    if (numberOfStickmans > 50)
                        control.x = Mathf.Clamp(control.x, -0.7f, 0.7f);
                    else
                        control.x = Mathf.Clamp(control.x, -1f, 1f);

                    transform.position = new Vector3(Mathf.Lerp(transform.position.x, control.x, Time.deltaTime * playerSpeed), transform.position.y, transform.position.z);
                }
            }
        }
    }

    public void FormatStickMan()
    {
        for (int i = 1; i < player.childCount; i++)
        {
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);
            var newPos = new Vector3(x, -0.9263f, z);

            player.transform.GetChild(i).DOLocalMove(newPos, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void MakeStickMan(int number)
    {
        for (int i = numberOfStickmans; i < number; i++)
        {
            Instantiate(stickMan, transform.position, Quaternion.identity, transform);
        }

        UpdateStickmanCount();
        FormatStickMan();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gate"))
        {
            other.transform.parent.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            other.transform.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;

            var gateManager = other.GetComponent<GateManager>();

            if (gateManager != null)
            {
                if (gateManager.multiply)
                {
                    MakeStickMan(numberOfStickmans * gateManager.randomNumber);
                }
                else
                {
                    MakeStickMan(numberOfStickmans + gateManager.randomNumber);
                }
            }
        }

        if (other.CompareTag("enemy"))
        {
            enemy = other.transform;
            attack = true;

            roadSpeed = 0.5f;

            other.transform.GetChild(1).GetComponent<EnemyManager>().AttackThem(transform);

            StartCoroutine(UpdateTheEnemyAndPlayerStickMansNumbers());
        }

        if (other.CompareTag("nogate"))
        {
            if (weapon1 != null)
            {
                weapon1.SetActive(true);
                w1Activated = true;
                w3Activated = false;
                w2Activated = false;
                w4Activated = false;
            }
        }

        if (other.CompareTag("bombgate"))
        {
            if (weapon4 != null)
            {
                weapon4.SetActive(true);
                w4Activated = true;
                w3Activated = false;
                w2Activated = false;
                w1Activated = false;
            }
        }

        if (other.CompareTag("weapongate"))
        {
            if (weapon2 != null)
            {
                weapon2.SetActive(true);
                w2Activated = true;
            }
        }

        if (other.CompareTag("gun"))
        {
            if (weapon3 != null)
            {
                weapon3.SetActive(true);
                w2Activated = false;
                w3Activated = true;
                w4Activated = false;
            }
        }
    }

    IEnumerator UpdateTheEnemyAndPlayerStickMansNumbers()
    {
        numberOfEnemyStickmans = enemy.transform.GetChild(1).childCount - 1;
        numberOfStickmans = transform.childCount - 4;

        while (numberOfEnemyStickmans > 0 && numberOfStickmans > 0)
        {
            numberOfEnemyStickmans--;
            numberOfStickmans--;

            enemy.transform.GetChild(1).GetComponent<EnemyManager>().CounterTxt.text = numberOfEnemyStickmans.ToString();
            UpdateStickmanCount();

            if (numberOfStickmans == 0)
            {
                enemy.transform.GetChild(1).GetComponent<EnemyManager>().StopAttacking();
                gameState = false;

                Destroy(gameObject);
                LoadTryAgainScene();
                yield break;
            }

            yield return null;
        }

        if (numberOfEnemyStickmans == 0)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.identity;
            }

            enemy.GetComponent<EnemyManager>().StopAttacking();
        }
    }

    public void StartGame()
    {
        gameState = true;

        if (gameStartMusic != null && audioSource != null)
        {
            audioSource.clip = gameStartMusic;
            audioSource.Play();
        }
    }

   public void UpdateStickmanCount()
    {
        numberOfStickmans = transform.childCount - 4;
        CounterTxt.text = numberOfStickmans.ToString();
    }

    public void LoadTryAgainScene()
    {
        SceneManager.LoadScene(3);
    }
}
