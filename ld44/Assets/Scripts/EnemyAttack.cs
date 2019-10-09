using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    private EnemyController controller;
    private float attackStartTime = 0.0f;
    private int damageApplied = 0;
    private bool attackStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponentInParent<EnemyController>();
        
        attackStartTime = 0.0f;
        damageApplied = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    private void OnTriggerEnter(Collider other) {
//        Debug.Log($"Triggered: {other}");
        
        PlayerAgent playerAgent = other.gameObject.GetComponentInParent<PlayerAgent>();
        if (playerAgent != null)
        {
            playerAgent.OnPlayerTriggerEnter(this.GetComponent<Collider>(), other);

            if (controller.IsAttacking)
            {
                attackStartTime = Time.time;
                damageApplied = 0;

                attackStarted = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
//        Debug.Log($"In Trigger: {other}");
        
        PlayerAgent playerAgent = other.gameObject.GetComponentInParent<PlayerAgent>();
        if (playerAgent != null) {
            playerAgent.OnPlayerTriggerStay(this.GetComponent<Collider>(), other);

            if (controller.IsAttacking)
            {
                if (!attackStarted)
                {
                    attackStartTime = Time.time;
                    damageApplied = 0;

                    attackStarted = true;
                }

                int damageExpected = (int) ((Time.time - attackStartTime) * (float)controller.player.DamageAmount) + 1;
                if (damageExpected != damageApplied)
                {
                    playerAgent.DoDamage(damageExpected - damageApplied);

                    damageApplied = damageExpected;
                }
            }
            else
            {
                attackStartTime = 0.0f;
                damageApplied = 0;
                attackStarted = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
//        Debug.Log($"Left Trigger: {other}");
        
        PlayerAgent playerAgent = other.gameObject.GetComponentInParent<PlayerAgent>();
        if (playerAgent != null)
        {
            playerAgent.OnPlayerTriggerExit(this.GetComponent<Collider>(), other);
            
            attackStartTime = 0.0f;
            damageApplied = 0;
            attackStarted = false;
        }
    }
}
