using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCtrl : MonoBehaviour { 
    public enum State
    {
        IDLE, SIT, LIE, WALK, RUN, EAT
    }

    public State state = State.IDLE;
    public bool isAlive = true;
    private Animator anim;
    private readonly int hashSit = Animator.StringToHash("isSit");
    void Awake()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(DogAction());
    }
    
    public void GiveOrder(string order)
    {
        switch (order)
        {
            case "SitDown":
                state = State.SIT;
                break;
        }
    }
    IEnumerator DogAction()
    {
        while (isAlive)
        {
            switch (state)
            {
                case State.IDLE:
                    break;
                case State.SIT:
                    anim.SetBool(hashSit, true);
                    break;
                case State.WALK:
                    break;
                case State.RUN:
                    break;
                default:
                    break;

            }

            yield return new WaitForSeconds(0.3f);
        }
    }
    
}
