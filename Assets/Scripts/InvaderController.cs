using System;
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

    public int invaderId; // 敵の番号
    [SerializeField] InvaderManager invaderManager;

    protected void OnCreate()
    {
        invaderManager = GameObject.Find("InvaderManager").GetComponent<InvaderManager>();
    }

    public IEnumerator StepOnce()
    {
        float stopTime = invaderManager.waitTime * invaderId / 3 / 55;  //waitTimeをフルに使うと微妙に見える
        yield return new WaitForSeconds(stopTime);
        transform.position += invaderManager.Movement[invaderManager.movementState];
    }
}
