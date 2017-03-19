using UnityEngine;
using System.Collections;


public class KOState : IEnemyState {
    private readonly StatePatternEnemy enemy;
    private float KOTimer; //How long the guard stands over the player after knockout

    public KOState(StatePatternEnemy statePatternEnemy) {
        enemy = statePatternEnemy;
    }

    public void UpdateState() {
        KOTimer += Time.deltaTime;
        if (KOTimer >= enemy.KODuration) {
            ToPatrolState();
        }
    }
    public void OnTriggerEnter(Collider coll) {
        
    }
    public void ToPatrolState() {
        enemy.currentState = enemy.patrolState;
        enemy.agent.speed = enemy.patrolSpeed;
    }
    public void ToSitState() {
        Debug.Log("Can't go from KO to Sitting");
    }

    public void ToSearchState() {
        Debug.Log("Can't go from KO to Search");
    }
    public void ToAttackState() {
        Debug.Log("Can't go from KO to Attack");
    }

    public void ToKOState() {
        Debug.Log("Can't Transistion to Same State");
    }
}
