using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    public Canvas Canvas;
    public Camera PlayerCamera;
    public GameLevelManager LevelManager;
    public Animator animator;
    public CharacterController controller;
    public Material PlayerMaterial;
    public Material EnemyMaterial;
    public Renderer MeshRenderer;
    public float RunSpeed = 5.0f;
    public float BackwardsScale = 0.7f;
    public float TurnSpeed = 180.0f;
    public float Gravity = -9.8f;
    public int PotionCost = 40;
    public int PotionTime = 30;
    public int DamageAmount = 5;

    private float _potionStartTime = 0.0f;
    public int PotionTimeElapsed
    {
        get
        {
            if (!_isPotionActive)
            {
                return PotionTime;
            }
            
            float elapsed = Time.time - _potionStartTime;
            if (elapsed > (float)PotionTime)
            {
                return PotionTime;
            }

            return (int)elapsed;
        }
    }

    private bool _isPotionActive = false;
    public bool IsPotionActive => _isPotionActive;

    void Start() {
        // Hack for development - without the Intro UI, we'll never leave the Intro state
        if (!Canvas.isActiveAndEnabled && (LevelManager.State == GameLevelManager.GameState.Intro)) {
            LevelManager.ChangeState(GameLevelManager.GameState.Playing);
        }

		PlayerAgent agent = GetComponent<PlayerAgent>();
		agent.Player = this;
    }

    public void OnForwardedPointerDown(InputForwarder source, PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left) {
            return;
        }
        
        Debug.Log($"Current state: {LevelManager.State}");
        
        int hitLayer = source.gameObject.layer;
        
        Debug.Log($"Hit {source.gameObject.name} on layer: {LayerMask.LayerToName(hitLayer)} ({hitLayer})");
    }

    private void LateUpdate() {
    }

    public void AgentOnTriggerEnter(PlayerAgent agent, Collider other) {
        if (!other.gameObject.activeInHierarchy) {
            return;
        }

        if (other.CompareTag("shop"))
        {
            Debug.Log("At a shop!", other);
            LevelManager.ChangeState(GameLevelManager.GameState.InStore);
        } else if (other.CompareTag("enemy"))
        {
            //
        } else if (other.CompareTag("Finish"))
        {
            LevelManager.ChangeState(GameLevelManager.GameState.Won);
        }
    }

    public void AgentOnTriggerStay(PlayerAgent agent, Collider other) {
        if (!other.gameObject.activeInHierarchy) {
            return;
        }

        if (other.CompareTag("shop"))
        {
//            Debug.Log("Still at shop!", other);
        } else if (other.CompareTag("player-damage"))
        {
//            GameCoordinator.Instance.Health -= 1;
        }
    }

    public void AgentOnTriggerExit(PlayerAgent agent, Collider other) {
        if (!other.gameObject.activeInHierarchy) {
            return;
        }

        if (other.CompareTag("shop"))
        {
            Debug.Log("Left a shop!", other);
        }
    }

    public void DoDamage(int damage)
    {
        if (LevelManager.State == GameLevelManager.GameState.Playing)
        {
            GameCoordinator.Instance.Health -= damage;

            if (GameCoordinator.Instance.Health <= 0)
            {
                GameCoordinator.Instance.Health = 0;
                LevelManager.ChangeState(GameLevelManager.GameState.Lost);
            }
        }
    }

    public void ApplyPotion()
    {
        if (GameCoordinator.Instance.Health > PotionCost)
        {
            GameCoordinator.Instance.Health -= PotionCost;
            _isPotionActive = true;
            _potionStartTime = Time.time;

            MeshRenderer.material = EnemyMaterial;

            StopCoroutine("EndPotion");
            StartCoroutine("EndPotion", (float)PotionTime);
        }
    }

    IEnumerator EndPotion(float t)
    {
        yield return new WaitForSeconds(t);

        _isPotionActive = false;
        MeshRenderer.material = PlayerMaterial;
    }

    void Update()
    {
        if (LevelManager.State == GameLevelManager.GameState.Playing)
        {
            Vector3 movement = Vector3.zero;

            if (Input.GetKey("up"))
            {
                animator.SetFloat("MovementSpeed", 1.0f);

                Vector3 fwd = this.transform.forward;
                fwd.y = 0.0f;
                fwd.Normalize();
                movement = fwd * Time.deltaTime * RunSpeed;
            }
            else if (Input.GetKey("down"))
            {
                animator.SetFloat("MovementSpeed", -1.0f * BackwardsScale);

                Vector3 fwd = this.transform.forward;
                fwd.y = 0.0f;
                fwd.Normalize();
                movement = fwd * Time.deltaTime * -RunSpeed * BackwardsScale;
            }
            else
            {
                animator.SetFloat("MovementSpeed", 0.0f);
            }

            if (Input.GetKey("left"))
            {
                this.transform.Rotate(this.transform.up, -TurnSpeed * Time.deltaTime);
            }

            if (Input.GetKey("right"))
            {
                this.transform.Rotate(this.transform.up, TurnSpeed * Time.deltaTime);
            }

            movement.y += Gravity * Time.deltaTime;
            controller.Move(movement);
        }
        else
        {
            animator.SetFloat("MovementSpeed", 0.0f);
        }
    }
}
