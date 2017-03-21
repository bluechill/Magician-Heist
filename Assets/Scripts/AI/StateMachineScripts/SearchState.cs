using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SearchState : IEnemyState {
    private readonly StatePatternEnemy enemy;
    private float searchTimer;
    public SearchState(StatePatternEnemy statePatternEnemy) {
        enemy = statePatternEnemy;
    }

    public void UpdateState() {
        Look();
        Search();
        Chase();
    }

    public void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.layer == 19)
            ToAttackState();
        if (coll.gameObject.layer == 9 && !enemy.cooldown)
        {
            enemy.cooldown = true;
            ToPatrolState();
        }
    }
    public void HitCooldown()
    {

    }
    public void ToPatrolState() {
        enemy.currentState = enemy.patrolState;
        enemy.agent.speed = enemy.patrolSpeed;
    }
    public void ToSitState() {
        Debug.Log("Can't Go From Search To Sit");
    }

    public void ToAttackState() {
        enemy.currentState = enemy.attackState;
        searchTimer = 0f;
    }

    public void ToSearchState() {
        searchTimer = 0f;
    }

    public void ToKOState() { }

    private void Look() {
        enemy.fov.FindVisibleTargets();
        if (enemy.fov.visibleTargets.Count != 0) {
            enemy.chaseTarget = enemy.fov.visibleTargets[0].transform;
            ToSearchState();
        }
    }

    private void Search() {
        searchTimer += Time.deltaTime;
        if(searchTimer >= enemy.SearchDuration) {
            ToPatrolState();
        }
    }

    private void Chase() {
        enemy.agent.destination = enemy.chaseTarget.transform.position;
        enemy.agent.Resume();
    }
}
