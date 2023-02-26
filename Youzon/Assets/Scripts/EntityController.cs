using System;
using UnityEngine;

public class EntityController : MonoBehaviour, IComparable<EntityController>
{
    private int[] d_x = { 0, 0, 1, -1 };
    private int[] d_y = { -1, 1, 0, 0 };

    public agentAction[] Actions { get; private set; }
    public float Result { get; private set; }
    public int DisPoint { get; private set; }
    public float moveCoolTime = 1;
    public float moveSpeed = 1;
    private int _curState;
    private float _lastMove;
    private Rigidbody _rigid;

    private float _StartTime;

    public void Start()
    {
        _StartTime = Time.time;
        _rigid = GetComponent<Rigidbody>();
        _curState = 0;
        _lastMove = Time.time;
        Result = -1;
        DisPoint = -1;
    }

    public void SetAction(agentAction[] actions)
    { 
        Actions = actions;
    }
    private void Update()
    {
        if (Result != -1) return;
            _rigid.transform.Translate(
            new Vector3(
                d_x[(int)Actions[_curState]] * moveSpeed,0,d_y[(int)Actions[_curState]] * moveSpeed)
            * Time.deltaTime);
        if (!(_lastMove + moveCoolTime < Time.time)) return;
        if (_curState + 1 < Actions.Length)
        {
            _curState++;
            _lastMove = Time.time;
        }
        else if (DisPoint == -1) DisPoint = (int)(-transform.position.x + 15);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Result = Time.time - _StartTime;
            DisPoint = (int)(-transform.position.x + 15);
        }
    }

    public int CompareTo(EntityController other)
    {
        if (this.Result == other.Result) return -this.DisPoint.CompareTo(other.DisPoint);
        else if (this.Result == -1) return 1;
        else if (other.Result == -1) return -1;
        else return this.Result.CompareTo(other.Result);
    }
}
