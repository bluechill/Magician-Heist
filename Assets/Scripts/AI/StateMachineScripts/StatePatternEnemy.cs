using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Director;

public class StatePatternEnemy : MonoBehaviour {
    public float SearchDuration = 10f;
    public float StandOver = 5f;
    public float KODuration = 10f;
    public float patrolSpeed = 2.0f;
    public float searchSpeed = 3.5f;
    public bool cooldown = false;
    public bool cooldown_invoke = false;

    public FieldOfView fov;

    public Transform[] patrolPoints;
    public Transform aiPointsOfInterst;
    public Transform originPoint;

    public AnimationClip guardWalk;

    private int destinationPoint = 0;
    public NavMeshAgent agent;
    private Animator animator;
    private bool animating = true;
    private bool animating_ko = false;

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public SearchState searchState;
    [HideInInspector] public SitState sitState;
    [HideInInspector] public AttackState attackState;
    bool idling = true;
    public bool knockout = false;
    private void Awake() {
        patrolState = new PatrolState(this);
        searchState = new SearchState(this);
        attackState = new AttackState(this);
        sitState = new SitState(this);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.autoBraking = false;
        agent.updateRotation = false;

        agent = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start () {
		if (this.gameObject.tag == "SitGuard") {
            currentState = sitState;
        }
        else {
            currentState = patrolState;
        }
    }
	void Cooldown()
    {
        cooldown = false;
        cooldown_invoke = false;
    }
	// Update is called once per frame
	void Update () {
        if (cooldown && !cooldown_invoke)
        {
            cooldown_invoke = true;
            Invoke("Cooldown", 4f);
        }
        if (knockout && animating_ko)
        {
            agent.velocity = Vector3.zero;
            return;
        }
        print(currentState);
        currentState.UpdateState();
        if (agent.velocity.magnitude > 0f && idling) {
            animator.Play("Guard Walking");
            idling = false;
        }

        if (agent.velocity.x < 0.0f)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));


        }
        else if (agent.velocity.x > 0.0f)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        } else if(!idling)
        {
            animator.Play("Guard Idling");
            idling = true;
        }
        if (knockout && !animating_ko)
        {
            animating_ko = true;
            animator.Play("Guard Knockout");
            idling = false;
            Invoke("Wakeup", 4f);

        }
        if (agent.isOnOffMeshLink && agent.currentOffMeshLinkData.linkType != OffMeshLinkType.LinkTypeManual)
            agent.CompleteOffMeshLink();
        else if (agent.isOnOffMeshLink) {
            var obj = agent.currentOffMeshLinkData.offMeshLink;

            if (obj.GetComponent<Door>() != null) {
                bool open = obj.GetComponent<Door>().open;

				if (!open) {
				}
                    //obj.GetComponent<Door>().OpenDoor();
                else
                    agent.ResetPath();
            }
        }
    }

    private void OnTriggerEnter(Collider coll) {
        currentState.OnTriggerEnter(coll);
    }
    public void Knockout()
    {
        knockout = true;
    }
    public void Wakeup()
    {
        animating_ko = false;
        knockout = false;
    }
}
