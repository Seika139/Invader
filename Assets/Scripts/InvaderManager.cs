using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderManager : MonoBehaviour
{
    public bool canMove;
    public float timeScale;

    public enum MovementState
    {
        RIGHT,
        DOWN_ON_RIGHT_END,
        LEFT,
        DOWN_ON_LEFT_END
    };

    public MovementState movementState { get; private set;}

    public float moveSpeed; // 敵が動く速さ(待ち時間の秒数)

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


    private void Start()
    {
        InvaderPlatoon = new List<InvaderController>();
        StartCoroutine(CreateInvaders());
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
                    InvaderPlatoon.Add(controller);
                }
                else if (i < 4)
                {
                    GameObject obj = Instantiate(squid, birthPosition, Quaternion.identity) as GameObject;
                    SquidController controller = obj.GetComponent<SquidController>();
                    InvaderPlatoon.Add(controller);
                }
                else
                {
                    GameObject obj = Instantiate(octopus, birthPosition, Quaternion.identity) as GameObject;
                    OctopusController controller = obj.GetComponent<OctopusController>();
                    InvaderPlatoon.Add(controller);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        movementState = MovementState.RIGHT;
        yield return null;
    }

    void ChangeMovementState()
    {
       switch (movementState)
        {
            case MovementState.RIGHT:
                foreach (InvaderController invaderConroller in InvaderPlatoon)
                {
                    if (invaderConroller.OnEdge)
                    {
                        movementState = MovementState.DOWN_ON_RIGHT_END;
                        break;
                    }
                }
                break;
            case MovementState.DOWN_ON_RIGHT_END:
                movementState = MovementState.LEFT;
                break;
            case MovementState.LEFT:
                foreach (InvaderController invaderConroller in InvaderPlatoon)
                {
                    if (invaderConroller.OnEdge)
                    {
                        movementState = MovementState.DOWN_ON_LEFT_END;
                        break;
                    }
                }
                break;
            case MovementState.DOWN_ON_LEFT_END:
                movementState = MovementState.RIGHT;
                break;
        }
    }

    IEnumerator MovePlatoon()
    {
        foreach (InvaderController invaderController in InvaderPlatoon)
        {
            invaderController.Move();
            yield return null;
            //yield return new WaitForSeconds(0.05f);
        }
        canMove = true;
        yield return null;
    }

    private void FixedUpdate()
    {
        Time.timeScale = timeScale;
        if (canMove)
        {
            canMove = false;
            ChangeMovementState();
            StartCoroutine(MovePlatoon());
        }
    }
}
