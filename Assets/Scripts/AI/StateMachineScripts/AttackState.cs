using UnityEngine;
using System.Collections;


public class AttackState : IEnemyState {
    private readonly StatePatternEnemy enemy;
    private float standOverTimer; //How long the guard stands over the player after knockout

    public AttackState(StatePatternEnemy statePatternEnemy) {
        enemy = statePatternEnemy;
    }

    public void UpdateState() {
        standOverTimer += Time.deltaTime;
        if (standOverTimer >= enemy.StandOver) {
            ToPatrolState();
        }
    }
    public void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.CompareTag("Player")) { }
            
    }
    public void ToPatrolState() {
        enemy.currentState = enemy.patrolState;
        enemy.agent.speed = enemy.patrolSpeed;
    }
    public void ToSitState() {
        enemy.currentState = enemy.sitState;
    }

    public void ToSearchState() {
        enemy.currentState = enemy.searchState;
    }
    public void ToAttackState() {
        Debug.Log("Can't Transistion to Same State");
    }

    public void ToKOState() { }
}
