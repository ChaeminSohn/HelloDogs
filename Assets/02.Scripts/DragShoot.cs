using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragShoot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Vector2 defaultPos;
    private Vector2 releasePos;
    private Vector2 startPos;
    private Transform cameraTransform;
    private GameObject mainDog;
    private GameObject ball;
    private Rigidbody ballRb;
    public float shootPower;
    [SerializeField]
    private GameObject shootBall;
    [SerializeField]
    private GameObject defaultPosObj;
    


    void Start()
    {
        defaultPos = defaultPosObj.transform.position;
        startPos = this.transform.position;
        shootPower = 5.0f;
    }

    void Update()
    {
        if(mainDog == null)
        {
            
            mainDog = GameObject.FindGameObjectWithTag("modelObject_Script");
            
        }
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Shoot(defaultPos - releasePos);
        gameObject.SetActive(false);
        this.transform.position = startPos;
    }

    public void Shoot(Vector2 force)
    {
        ball = Instantiate(shootBall);
        ballRb = ball.GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        ball.transform.position = cameraTransform.position;
        //ballRb.velocity = new Vector3(cameraTransform.forward.x, 0 , cameraTransform.forward.z)
           // * force.y * shootPower;
        ballRb.AddForce(cameraTransform.forward * shootPower, ForceMode.VelocityChange);
        mainDog.GetComponent<DogCtrl>()?.GiveOrder("Fetch");
        InvokeRepeating("StopBall", 5.0f, 3);
    }

    void StopBall()
    {
        if(ball != null)
            ballRb.velocity = Vector3.zero;       
    }
}
