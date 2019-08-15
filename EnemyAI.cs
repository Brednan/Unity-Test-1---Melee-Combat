using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    Animator enemyAnim;

    Transform player;
    Player _playerScript;

    private bool canAttack;

    private float speed;
    private float playerDistance;

    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;
        playerDistance = 34f;
        speed = 1.8f;
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        player = target.transform;
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _playerScript.enemyHasBeenHit = false;
}

    // Update is called once per frame
    void Update()
    {
        PlayerAttack();
        DestroyThis();
    }
    void PlayerAttack()
    {
        transform.LookAt(player);
        if(Vector3.Distance(transform.position, player.position) > playerDistance)
        {
            if(canAttack == true)
            {
                transform.Translate(Vector3.forward * speed);
                enemyAnim.Play("Run");
            }
        }
        else if(Vector3.Distance(transform.position, player.position) < playerDistance)
        {
            if(canAttack == true)
            {
                StartCoroutine("EnemyAttackCoolDown");
                StartCoroutine("EnemyAttackRoutine");
            }
        }
        else
        {
            enemyAnim.Play("Idle");
        }

    }
    public void DestroyThis()
    {
        if(_playerScript.enemyHasBeenHit == true)
        {
            GameObject.Destroy(this.gameObject);
        }

    }
    private IEnumerator EnemyAttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(enemyAnim.GetCurrentAnimatorClipInfo(0).Length);
        canAttack = true;
        yield break;
    }
    private IEnumerator EnemyAttackRoutine()
    {

        yield return new WaitForSeconds(0.3f);
        enemyAnim.Play("Attack");
        Debug.Log("EnemyHit");
        _playerScript.PlayerDeath();
        yield break;
    }
}
