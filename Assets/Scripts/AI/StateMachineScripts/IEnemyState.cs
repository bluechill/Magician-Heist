using UnityEngine;
using System.Collections;


public interface IEnemyState {
    void UpdateState();
    void OnTriggerEnter(Collider coll);
    void ToPatrolState();
    void ToSitState();
    void ToSearchState();
    void ToAttackState();
    void ToKOState();
}
