using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    float distance;
    Rigidbody myRB;
    MeshRenderer render;
    public int normalCollisionCount = 1;
    public float moveLimit = .3f;
    public float collisionMoveFactor = .01f;
    public float addHeightWhenClicked = 0.0f;
    public bool freezeRotationOnDrag = true;
    public Camera cam;
    private Rigidbody myRigidbody;
    private Transform myTransform;
    private bool canMove = false;
    private float yPos;
    private bool gravitySetting;
    private bool freezeRotationSetting;
    private float sqrMoveLimit;
    private int collisionCount = 0;
    private Transform camTransform;
    bool isOver;

    private void Start()
    {
        render = GetComponentInChildren<MeshRenderer>();
        myRigidbody = GetComponent<Rigidbody>();
        myTransform = transform;
        if (!cam)
        {
            cam = Camera.main;
        }
        if (!cam)
        {
            Debug.LogError("Can't find camera tagged MainCamera");
            return;
        }
        camTransform = cam.transform;
        sqrMoveLimit = moveLimit * moveLimit;
    }

    
    
    void FixedUpdate()
    {
        if (transform.position.y <= 0.1f)
        {
            myTransform.position = new Vector3(myTransform.position.x, 0.3f, myTransform.position.z);
        }
        if (Input.GetMouseButtonDown(1) && isOver)
        {
            canMove = true;
            myTransform.Translate(Vector3.up * addHeightWhenClicked);
            gravitySetting = myRigidbody.useGravity;
            freezeRotationSetting = myRigidbody.freezeRotation;
            myRigidbody.useGravity = false;
            myRigidbody.freezeRotation = freezeRotationOnDrag;
            yPos = myTransform.position.y;
        }
        if(Input.GetMouseButtonUp(1) && isOver)
        {
            canMove = false;
            myRigidbody.useGravity = gravitySetting;
            myRigidbody.freezeRotation = freezeRotationSetting;
            if (!myRigidbody.useGravity)
            {
                Vector3 pos1 = myTransform.position;
                pos1.y = yPos - addHeightWhenClicked;
                myTransform.position = pos1;
            }
        }
        distance = cam.transform.localPosition.z;

        if (!canMove || !isOver)
        {
            return;
        }
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;

        Vector3 pos = myTransform.position;
        pos.y = yPos;
        myTransform.position = pos;

        Vector3 mousePos = Input.mousePosition;
        Vector3 move = cam.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, camTransform.position.y - myTransform.position.y)) - myTransform.position;
        move.y = 0.0f;
        if (collisionCount >= normalCollisionCount)
        {
            move = move.normalized * collisionMoveFactor;
        }
        else if (move.sqrMagnitude > sqrMoveLimit)
        {
            move = move.normalized * collisionMoveFactor;
        }

        myRigidbody.MovePosition(myRigidbody.position + move);
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -distance - 0.3f);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        myRigidbody.MovePosition(objPosition);

    }

    private void OnMouseDown()
    {
        myRigidbody.useGravity = false;
        myRigidbody.freezeRotation = true;
    }

    private void OnMouseUp()
    {
        myRigidbody.useGravity = true;
        myRigidbody.freezeRotation = false;
    }

    void OnMouseEnter()
    {
        render.material.color = Color.green;
        isOver = true;
    }

    void OnMouseExit()
    {
        render.material.color = Color.white;
        isOver = false;
    }
    void OnCollisionEnter()
    {
        collisionCount++;
    }

    void OnCollisionExit()
    {
        collisionCount--;
    }
}
