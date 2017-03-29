using UnityEngine;
using System.Collections;


public class PatrolState : IEnemyState {
    private readonly StatePatternEnemy enemy;
    private int nextWayPoint;
    private GameObject interactWith;
    public bool blowUp = false;

    public PatrolState(StatePatternEnemy statePatternEnemy) {
        enemy = statePatternEnemy;
    }

    public void UpdateState() {
        //int RNG = Random.Range(0, 15);
        //if (RNG > 14) {
        //    ItemInteraction(interactWith);
        //}
        //Look();
        Patrol();
    }
    public void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.layer == 9)
            coll.gameObject.GetComponent<PlayerScript>().points -= 100;
        else if (coll.gameObject.CompareTag("Item")) {
            if (coll.gameObject.GetComponent<Item>().thrown) { 
                coll.gameObject.GetComponent<Item>().current_player.GetComponent<PlayerScript>().points += 100;
                blowUp = true;
            }
        }
    }

    public void ToPatrolState() {
        Debug.Log("Can't Transistion to Same State");
    }
    public void ToSitState() {
        enemy.currentState = enemy.sitState;
    }

    public void ToSearchState() {
        enemy.currentState = enemy.searchState;
        enemy.agent.speed = enemy.searchSpeed;
    }

    public void ToAttackState() {
        enemy.currentState = enemy.attackState;
    }

    public void ToKOState() { }

    private void Look() {
        enemy.fov.FindVisibleTargets();
        if (enemy.fov.visibleTargets.Count != 0) {
            enemy.chaseTarget = enemy.fov.visibleTargets[0].transform;
            ToSearchState();
        }
    }

    private void Patrol() {

        enemy.agent.destination = enemy.patrolPoints[nextWayPoint].position;
        enemy.agent.Resume();
        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance * 2f && !enemy.agent.pathPending) {
            nextWayPoint = (nextWayPoint + 1) % enemy.patrolPoints.Length;
        } 
    }

    private void ItemInteraction(GameObject item) {

    }
}
