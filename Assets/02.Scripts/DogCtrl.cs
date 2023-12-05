using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class DogCtrl : MonoBehaviour { 
    public enum State
    {
        STAND, SIT, LIE, LIEDOWN, WALK, RUN, EAT
    }
    [SerializeField]
    private GameObject defaultPlane;
    [SerializeField]
    private GameObject tennisBall;
    [SerializeField]
    private GameObject mouseHold;
    private Camera m_Camera;
    public State state = State.STAND;
    public float runSpeed = 3.0f;
    public float walkSpeed = 1.0f;
    public bool isAlive = true;
    public GameObject headRigObj;
    public GameObject lookTarget;
    private Animator anim;

    private readonly int hashidle = Animator.StringToHash("isIdle");
    private readonly int hashStand = Animator.StringToHash("isStand");
    private readonly int hashSit = Animator.StringToHash("isSit");
    private readonly int hashLie = Animator.StringToHash("isLie");
    private readonly int hashLieDown = Animator.StringToHash("isLieDown");
    private readonly int hashWalk = Animator.StringToHash("isWalk");
    private readonly int hashRun = Animator.StringToHash("isRun");    
    private readonly int hashEat = Animator.StringToHash("isEat");
    private readonly int hashIdleIndex = Animator.StringToHash("idleIndex");

    private GameObject mainPlane;
    private GameObject startingPoint;
    private Rig headRig;
    public GameObject followTarget;
    private Transform followTargetTr;
    private GameObject dogBowl;
    public GameObject head;
    public GameObject body;
    void Awake()
    {
        m_Camera = Camera.main;
        anim = GetComponent<Animator>();
        headRig = headRigObj.GetComponentInChildren<Rig>();
        startingPoint = GameObject.FindGameObjectWithTag("StartPoint");
        startingPoint.transform.position = this.transform.position;
        anim.SetBool(hashidle, true);   
        anim.SetBool(hashStand, true);
        anim.SetInteger(hashIdleIndex, -1);
        headRig.weight = 1.0f;
        mainPlane = Instantiate(defaultPlane, this.transform.position, Quaternion.identity);
        StartCoroutine(DogAction());
    }

    private void Update()
    {
        lookTarget.transform.position = m_Camera.transform.position;
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GiveOrder("SitDown");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GiveOrder("Lie");
        }
        else if(Input.GetMouseButton(1)) 
        {
            //GiveOrder("Fetch");
        }*/
        mainPlane.transform.position = transform.position;
    }

    public void GiveOrder(string order)
    {
        switch (order)
        {
            case "Stand":
                if (anim.GetInteger(hashIdleIndex) == -2)
                    return;
                anim.SetInteger(hashIdleIndex, -1);
                state = State.STAND;
                break;
            case "SitDown":
                if (anim.GetInteger(hashIdleIndex) == -2)
                    return;
                anim.SetInteger(hashIdleIndex, 0);
                state = State.SIT;
                break;

            case "Lie":
                if (anim.GetInteger(hashIdleIndex) == -2)
                    return;
                anim.SetInteger(hashIdleIndex, Random.Range(1, 7));
                state = State.LIE;
                break;
            case "Fetch":
                state = State.RUN;
                anim.SetInteger(hashIdleIndex, -2);
                followTarget = GameObject.FindGameObjectWithTag("Ball");
                followTargetTr = followTarget.transform;
                anim.SetBool(hashidle, false);
                headRig.weight = 0.0f;
                anim.SetBool(hashRun, true);
                break;
            case "Eat":
                if (anim.GetInteger(hashIdleIndex) == -2)
                    return;
                dogBowl = GameObject.FindGameObjectWithTag("Bowl");
                if (dogBowl != null)
                {
                    state = State.EAT;
                    anim.SetInteger(hashIdleIndex, -2);
                    anim.SetBool(hashidle, false);
                    anim.SetBool(hashEat, true);
                    headRig.weight = 0.0f;
                }
                break;
        }
    }
    IEnumerator DogAction()
    {
        float timeCnt = 0;
        while (isAlive)
        {
            switch (state)
            {
                case State.STAND:
                    if (!anim.GetBool(hashStand))
                    {
                        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) {
                            yield return new WaitForSeconds(0.1f);
                        }
                        anim.SetBool(hashidle, false);
                 
                        anim.SetBool(hashStand, true);
                        anim.SetBool(hashidle, true);
                        anim.SetBool(hashLie, false);
                        anim.SetBool(hashLieDown, false);
                        anim.SetBool(hashSit, false);
                    }
                    if (timeCnt >= 30.0f)
                    {
                        anim.SetInteger(hashIdleIndex, -1);
                        timeCnt = 0;
                    }
                    break;
                case State.SIT:
                    if (!anim.GetBool(hashSit))
                    {
                        anim.SetBool(hashidle, false);
                  
                        anim.SetBool(hashSit, true);
                        anim.SetBool(hashidle, true);
                        anim.SetBool(hashLie, false);
                        anim.SetBool(hashLieDown, false);
                        anim.SetBool(hashStand, false);
                    }
                    if (timeCnt >= 30.0f)
                    {
                        anim.SetInteger(hashIdleIndex, 0);
                        timeCnt = 0;
                    }
                    break;

                case State.LIE:
                    if (!anim.GetBool(hashLie))
                    {
                        anim.SetBool(hashidle, false);
               
                        anim.SetBool(hashLie, true);
                        anim.SetBool(hashidle, true);
                        anim.SetBool(hashSit, false);
                        anim.SetBool(hashLieDown, false);
                        anim.SetBool(hashStand, false);
                    }
                    if (timeCnt >= 30.0f)
                    {
                        anim.SetInteger(hashIdleIndex, Random.Range(1,13));
                        if (anim.GetInteger(hashIdleIndex) >= 7)
                            state = State.LIEDOWN;
                        timeCnt = 0;
                    }

                    break;

                case State.LIEDOWN:
                    if (!anim.GetBool(hashLieDown))
                    {
                        anim.SetBool(hashidle, false);
               
                        anim.SetBool(hashLieDown, true);
                        anim.SetBool(hashidle, true);
                        anim.SetBool(hashSit, false);
                        anim.SetBool(hashLie, false);
                        anim.SetBool(hashStand, false);
                    }
                    if (timeCnt >= 30.0f)
                    {
                        anim.SetInteger(hashIdleIndex, Random.Range(1, 13));
                        if (anim.GetInteger(hashIdleIndex) < 7)
                            state = State.LIE;
                        timeCnt = 0;
                    }
                    break;
                case State.WALK:
                    GameObject ballInstance = Instantiate(tennisBall, mouseHold.transform.position, Quaternion.identity);
                    ballInstance.transform.parent = mouseHold.transform;
                    if (!anim.GetBool(hashWalk))
                    {
                        followTarget = startingPoint;
                        followTargetTr = followTarget.transform;
                        anim.SetBool(hashWalk, true);
                    }
                    float walk_distance = Vector3.Distance(followTargetTr.position, this.transform.position);
                    transform.LookAt(followTargetTr);

                    while (walk_distance >= 0.2f)
                    {
                        transform.position = Vector3.Lerp(transform.position, followTargetTr.position, Time.deltaTime * walkSpeed);
                        yield return new WaitForSeconds(0.1f);
                        walk_distance = Vector3.Distance(followTargetTr.position, this.transform.position);
                    }
                    Destroy(ballInstance);  
                    anim.SetBool(hashWalk, false);
                    anim.SetInteger(hashIdleIndex, -1);
                    anim.SetBool(hashidle, true);
                    state = State.STAND;
                    GameObject.FindGameObjectWithTag("ButtonControl")?.
                        GetComponentInChildren<ButtonCtrl>()?.BallButtonOn();
                    headRig.weight = 1.0f;
                    GameManager.intimacy += 0.1f;
                    break;
                case State.RUN:
                    float run_distance = Vector3.Distance(followTargetTr.position, this.transform.position);
                    transform.LookAt(followTargetTr);

                    while (run_distance >= 0.2f)
                    {
                        transform.position = Vector3.Lerp(transform.position, followTargetTr.position, Time.deltaTime * runSpeed);
                        yield return new WaitForSeconds(0.1f);
                        run_distance = Vector3.Distance(followTargetTr.position, this.transform.position);
                    }
                    anim.SetBool(hashRun, false);
                    Destroy(followTarget);
                    state = State.WALK;
                    yield return new WaitForSeconds(3.0f);
                    break;
                case State.EAT:
                    float distance = Vector3.Distance(dogBowl.transform.position, this.transform.position);
                    transform.LookAt(dogBowl.transform);
                    while (distance >= 0.2f)
                    {
                        anim.SetBool(hashWalk, true);
                        transform.position = Vector3.Lerp(transform.position, dogBowl.transform.position, Time.deltaTime * walkSpeed);
                        yield return new WaitForSeconds(0.1f);
                        distance = Vector3.Distance(dogBowl.transform.position, this.transform.position);
                    }
                    anim.SetBool(hashWalk, false);
                    yield return new WaitForSeconds(6.0f);
                    anim.SetBool(hashEat, false);
                    transform.LookAt(startingPoint.transform);
                    distance = Vector3.Distance(startingPoint.transform.position, this.transform.position);
                    while (distance >= 0.2f)
                    {
                        transform.position = Vector3.Lerp(transform.position, startingPoint.transform.position, Time.deltaTime * walkSpeed);
                        yield return new WaitForSeconds(0.1f);
                        distance = Vector3.Distance(startingPoint.transform.position, this.transform.position);
                    }
                    
                    headRig.weight = 1.0f;
                    anim.SetBool(hashWalk, false);
                    anim.SetInteger(hashIdleIndex, -1);
                    anim.SetBool(hashidle, true);
                    
                    state = State.STAND;
                    GameManager.intimacy += 0.2f;
                    break;
                default:
                    break;

            }
        
            timeCnt += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
