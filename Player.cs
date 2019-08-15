using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Animator anim;

    [SerializeField]
    private float speed;
    private float attackSpeed;
    private Vector3 _spawnPoint;

    [SerializeField]
    Rigidbody rb;


    [SerializeField]
    RaycastHit hit;
    Camera cam;

    private EnemyAI _enemyScript;
    [SerializeField]
    GameObject _enemyPrefab;

    private Transform _enemy;
    public bool enemyHasBeenHit;

    private bool attack;
    // Start is called before the first frame update
    void Start()
    {
        attack = false;
        speed = 2.0f;
        attackSpeed = 1.0f;
        cam = Camera.main;
        GameObject _enemyPrefab = GameObject.FindGameObjectWithTag("Enemy");
        _enemy = _enemyPrefab.transform;
        _enemyScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();
        _spawnPoint = new Vector3(-16608, -6430.6f, -43578f);
    }
        // Update is called once per frame
        void Update()
        {

        CursorLock();
        Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
        if (!attack)
            {
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.W))
            {
                StartCoroutine("AttackRoutine");
                if (attack)
                {
                    anim.Play("Attack");
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                    {
                        if(hit.collider.tag == "Enemy")
                        {
                            Debug.Log("Hit");
                            Attack();
                        }

                    }

                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine("AttackRoutine");
                if (attack)
                {
                    anim.Play("Attack");
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            Debug.Log("Hit");
                            Attack();
                        }
                    }
                }
            }

                else if (Input.GetKey(KeyCode.W))
                {
                    Run();
                }
                else
                {
                    anim.Play("Idle");
                }
            }
            LookAround();

        }
         private void Run()
        {
            anim.Play("Run");
            transform.Translate(Vector3.forward * speed);
        }
         private void Attack()
        {
        StartCoroutine("EnemySpawnRoutine");
        }
        IEnumerator AttackRoutine()
        {
            attack = true;
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
            attack = false;
            yield break;
        }
         private void LookAround()
        {
            float _mouseX = Input.GetAxis("Mouse X");

            float _mouseY = Input.GetAxis("Mouse Y");

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + _mouseY, transform.localEulerAngles.y + _mouseX, transform.localEulerAngles.z);
        }
    private void CursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    IEnumerator EnemySpawnRoutine()
    {
        enemyHasBeenHit = true;
        yield return new WaitForSeconds(1.0f);
        enemyHasBeenHit = false;
        Instantiate(_enemyPrefab, new Vector3(-16526, -6432, -43730), Quaternion.identity);
        yield break;
    }
    public void PlayerDeath()
    {
        transform.position = _spawnPoint;
    }
}
