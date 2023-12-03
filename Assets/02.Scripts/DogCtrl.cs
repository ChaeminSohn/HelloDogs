using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class DogCtrl : MonoBehaviour { 
    public enum State
    {
        STAND, SIT, LIE, LIEDOWN, WALK, RUN, EAT
    }

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
    private readonly int hashIdleIndex = Animator.StringToHash("idleIndex");


    public GameObject startingPoint;
    private Rig headRig;
    public Transform followTarget;
    public GameObject head;
    public GameObject body;
    void Awake()
    {
        m_Camera = Camera.main;
        anim = GetComponent<Animator>();

        headRig = headRigObj.GetComponentInChildren<Rig>();
        StartCoroutine(DogAction());
        //startingPoint.transform.position = this.transform.position;
        anim.SetBool(hashStand, true);
        anim.SetInteger(hashIdleIndex, -1);
    }

    private void Update()
    {
        lookTarget.transform.position = m_Camera.transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GiveOrder("SitDown");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GiveOrder("Lie");
        }
        else if(Input.GetMouseButton(1)) 
        {
            GiveOrder("Fetch");
        }
    }

    public void GiveOrder(string order)
    {
        switch (order)
        {
            case "SitDown":
                //if (anim.GetInteger(hashIdleIndex) == -2)
                   // return;
                anim.SetInteger(hashIdleIndex, 0);
                state = State.SIT;
                break;

            case "Lie":
                anim.SetInteger(hashIdleIndex, Random.Range(1, 7));
                state = State.LIE;
                break;
            case "Fetch":
                state = State.RUN;
                anim.SetInteger(hashIdleIndex, -2);
                followTarget = GameObject.FindGameObjectWithTag("Ball").transform;
                anim.SetBool(hashidle, false);
                headRig.weight = 0.0f;
                anim.SetBool(hashRun, true);
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
                    anim.SetBool(hashidle, true);
                    
                    break;
                case State.SIT:
                    if (!anim.GetBool(hashSit))
                    {
                        anim.SetBool(hashidle, false);
                        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) 
                        { 
                             yield return new WaitForSeconds(1.0f);
                        }
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
                        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                        {
                            yield return new WaitForSeconds(1.0f);
                        }
                        anim.SetBool(hashLie, true);
                        anim.SetBool(hashidle, true);
                        anim.SetBool(hashSit, false);
                        anim.SetBool(hashLieDown, false);
                        anim.SetBool(hashStand, false);
                    }
                    if (timeCnt >= 10.0f)
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
                        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                        {
                            yield return new WaitForSeconds(1.0f);
                        }
                        anim.SetBool(hashLieDown, true);
                        anim.SetBool(hashidle, true);
                        anim.SetBool(hashSit, false);
                        anim.SetBool(hashLie, false);
                        anim.SetBool(hashStand, false);
                    }
                    if (timeCnt >= 10.0f)
                    {
                        anim.SetInteger(hashIdleIndex, Random.Range(1, 13));
                        if (anim.GetInteger(hashIdleIndex) < 7)
                            state = State.LIE;
                        timeCnt = 0;
                    }
                    break;
                case State.WALK:
                    float walk_distance = Vector3.Distance(followTarget.transform.position, this.transform.position);
                    transform.LookAt(followTarget.transform);

                    if (walk_distance >= 0.1f)
                    {
                        transform.position = Vector3.Lerp(transform.position, followTarget.position, Time.deltaTime * walkSpeed);
                    }
                    else
                    {
                        anim.SetBool(hashWalk, false);
                        state = State.STAND;
                    }
                    break;
                case State.RUN:
                    float run_distance = Vector3.Distance(followTarget.transform.position, this.transform.position);
                    transform.LookAt(followTarget.transform);

                    if (run_distance >= 1f)
                    {
                        transform.position = Vector3.Lerp(transform.position, followTarget.position, Time.deltaTime * runSpeed);
                    }
                    else
                    {
                        state = State.WALK;
                        followTarget = startingPoint.transform;
                        anim.SetBool(hashRun, false);
                        anim.SetBool(hashWalk, true);
                    }
                    break;
                default:
                    break;

            }
        
            timeCnt += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FollowTarget()
    {
        float distance = Vector3.Distance(followTarget.transform.position, this.transform.position);

        if(distance >= 1.0f)
        {
            transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, Time.deltaTime * runSpeed);
        }
        else
        {
            state = State.WALK;
        }
    }

   

}
