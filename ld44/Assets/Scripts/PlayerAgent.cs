using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : MonoBehaviour {
    [System.NonSerialized] public PlayerController Player;

    public void OnPlayerTriggerEnter(Collider other, Collider thisCollider) {
//        Debug.Log($"{gameObject.name} entered trigger: {other}. Original collider (this): {thisCollider}.");

        Player.AgentOnTriggerEnter(this, other);
    }
    
    public void OnPlayerTriggerStay(Collider other, Collider thisCollider) {
//        Debug.Log($"{gameObject.name} in trigger: {other}. Original collider (this): {thisCollider}.");

        Player.AgentOnTriggerStay(this, other);
    }
    
    public void OnPlayerTriggerExit(Collider other, Collider thisCollider) {
//        Debug.Log($"{gameObject.name} exited trigger: {other}. Original collider (this): {thisCollider}.");

        Player.AgentOnTriggerExit(this, other);
    }

    public void DoDamage(int damage)
    {
        Player.DoDamage(damage);
    }
}
