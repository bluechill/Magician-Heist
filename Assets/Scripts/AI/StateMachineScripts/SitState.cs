using UnityEngine;
using System.Collections;


public class SitState : IEnemyState {
    private readonly StatePatternEnemy enemy;
    private float searchTimer;

    public SitState(StatePatternEnemy statePatternEnemy) {
        enemy = statePatternEnemy;
    }

    public void UpdateState() {
        Look();
        int RNG = Random.Range(0, 10);
        if (RNG > 9) {
            ToPatrolState();
        }
        else if (RNG > 6) {
            enemy.transform.eulerAngles = new Vector3(180f, 0);
        }
    }
    public void OnTriggerEnter(Collider coll) { }
    public void ToPatrolState() {
        enemy.currentState = enemy.patrolState;
    }
    public void ToSitState() {
        Debug.Log("Can't Transistion to Same State");
    }
    public void ToSearchState() {
        enemy.currentState = enemy.searchState;
    }

    public void ToAttackState() {
        enemy.currentState = enemy.attackState;
    }

    public void ToKOState() { }

    private void Look() {
        enemy.fov.FindVisibleTargets();
        if (enemy.fov.visibleTargets != null) {
            enemy.chaseTarget = enemy.fov.visibleTargets[0].transform;
            ToSearchState();
        }
    }
}
