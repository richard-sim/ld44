using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Animator animator; 
    public NavMeshAgent agent;
    public PlayerController player;

    private bool _isAttacking = false;

    public bool IsAttacking
    {
        get { return _isAttacking; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAttacking)
        {
            agent.SetDestination(player.transform.position);
        }

        animator.SetFloat("MovementSpeed", (!agent.hasPath || (agent.velocity.magnitude < 0.1f)) ? 0.0f : 1.0f);
    }
    
    private void OnTriggerEnter(Collider other) {
//        Debug.Log($"Triggered: {other}");
        
        PlayerAgent playerAgent = other.gameObject.GetComponentInParent<PlayerAgent>();
        if (playerAgent != null) {
            playerAgent.OnPlayerTriggerEnter(this.GetComponent<Collider>(), other);

            if (!player.IsPotionActive)
            {
                _isAttacking = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
//        Debug.Log($"In Trigger: {other}");
        
        PlayerAgent playerAgent = other.gameObject.GetComponentInParent<PlayerAgent>();
        if (playerAgent != null) {
            playerAgent.OnPlayerTriggerStay(this.GetComponent<Collider>(), other);

            if (!player.IsPotionActive)
            {
                _isAttacking = true;
            }
            else
            {
                _isAttacking = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
//        Debug.Log($"Left Trigger: {other}");
        
        PlayerAgent playerAgent = other.gameObject.GetComponentInParent<PlayerAgent>();
        if (playerAgent != null) {
            playerAgent.OnPlayerTriggerExit(this.GetComponent<Collider>(), other);

            _isAttacking = false;
        }
    }
}
