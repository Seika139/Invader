using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderManager : MonoBehaviour
{

    public enum MovementState
    {
        RIGHT,
        DOWN_ON_RIGHT_END,
        LEFT,
        DOWN_ON_LEFT_END
    };

    public MovementState movementState { get; private set;}

    public float waitTime;

    [SerializeField] bool canMove;

    public readonly Dictionary<MovementState, Vector3> Movement = new Dictionary<MovementState, Vector3>{
        { MovementState.RIGHT , new Vector3(0.1f,0,0) },
        { MovementState.DOWN_ON_RIGHT_END  , new Vector3(0,-0.8f,0) },
        { MovementState.LEFT , new Vector3(-0.1f,0,0) },
        { MovementState.DOWN_ON_LEFT_END, new Vector3(0,-0.8f,0) }
    };

    [SerializeField] GameObject bee;
    [SerializeField] GameObject squid;
    [SerializeField] GameObject octopus;

    List<InvaderController> InvaderPlatoon;

    [SerializeField] private bool finishCreated;

    private void Start()
    {
        finishCreated = false;
        InvaderPlatoon = new List<InvaderController>();
        IEnumerator c = CreateInvaders();
        StartCoroutine(c);
    }

    IEnumerator CreateInvaders()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                Vector3 birthPosition = new Vector3(-2.4f + j * 0.4f, 0.45f * i, 0);
                if (i < 2)
                {
                    GameObject obj = Instantiate(bee, birthPosition, Quaternion.identity) as GameObject;
                    BeeController controller = obj.GetComponent<BeeController>();
                    controller.invaderId = i * 5 + j;
                    InvaderPlatoon.Add(controller);
                }
                else if (i < 4)
                {
                    GameObject obj = Instantiate(squid, birthPosition, Quaternion.identity) as GameObject;
                    SquidController controller = obj.GetComponent<SquidController>();
                    controller.invaderId = i * 5 + j;
                    InvaderPlatoon.Add(controller);
                }
                else
                {
                    GameObject obj = Instantiate(octopus, birthPosition, Quaternion.identity) as GameObject;
                    OctopusController controller = obj.GetComponent<OctopusController>();
                    controller.invaderId = i * 5 + j;
                    InvaderPlatoon.Add(controller);
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        movementState = MovementState.RIGHT;
        yield return null;
        finishCreated = true;
    }

    void ChangeMovementState()
    {
        switch (movementState)
        {
            case MovementState.RIGHT:
                if (platoonRight >= 2.4f)
                {
                    movementState = MovementState.DOWN_ON_RIGHT_END;
                }
                break;
            case MovementState.DOWN_ON_RIGHT_END:
                movementState = MovementState.LEFT;
                break;
            case MovementState.LEFT:
                if (platoonLeft <= -2.4f)
                {
                    movementState = MovementState.DOWN_ON_LEFT_END;
                }
                break;
            case MovementState.DOWN_ON_LEFT_END:
                movementState = MovementState.RIGHT;
                break;
        }
        Debug.Log(movementState);
    }

    [SerializeField] float platoonLeft;
    [SerializeField] float platoonRight;

    IEnumerator MovePlatoon()
    {
        platoonLeft = 2.4f;
        platoonRight = -2.4f;
        foreach (InvaderController invaderController in InvaderPlatoon)
        {
            //IEnumerator coroutine = invaderController.StepOnce();
            StartCoroutine(invaderController.StepOnce());
            platoonLeft = Mathf.Min(platoonLeft, invaderController.transform.position.x);
            platoonRight = Mathf.Max(platoonRight, invaderController.transform.position.x);
            //yield return coroutine;
        }
        yield return new WaitForSeconds(waitTime); // 待ち時間をいちばんさいごのやつよりもながくする
        ChangeMovementState();
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (finishCreated & canMove)
        {
            canMove = false;
            StartCoroutine(MovePlatoon());
        }
    }
}
