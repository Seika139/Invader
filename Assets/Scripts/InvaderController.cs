using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderController : MonoBehaviour {

    enum InvaderState
    {
        Alive,
        Dead,
        Disable
    };
    InvaderState state;

    public bool OnEdge { get; private set; }
    const float edgeX = 2.4f;

    [SerializeField] InvaderManager invaderManager;

    protected void OnCreate()
    {
        OnEdge = false;
        invaderManager = GameObject.Find("InvaderManager").GetComponent<InvaderManager>();
    }

    void CheckPosition()
    {
        OnEdge = Mathf.Approximately(Mathf.Abs(transform.position.x), edgeX);
    }

    public void Move ()
    {
        Debug.Log(invaderManager);
        transform.position += invaderManager.Movement[invaderManager.movementState];
        CheckPosition();
    }
    
}
