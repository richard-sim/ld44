using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
//        Debug.Log($"Triggered: {other}");
        
        PlayerAgent agent = other.gameObject.GetComponentInParent<PlayerAgent>();
        if (agent != null) {
            agent.OnPlayerTriggerEnter(this.GetComponent<Collider>(), other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
//        Debug.Log($"In Trigger: {other}");
        
        PlayerAgent agent = other.gameObject.GetComponentInParent<PlayerAgent>();
        if (agent != null) {
            agent.OnPlayerTriggerStay(this.GetComponent<Collider>(), other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
//        Debug.Log($"Left Trigger: {other}");
        
        PlayerAgent agent = other.gameObject.GetComponentInParent<PlayerAgent>();
        if (agent != null) {
            agent.OnPlayerTriggerExit(this.GetComponent<Collider>(), other);
        }
    }
}
