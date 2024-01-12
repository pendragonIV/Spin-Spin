using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpinType
{
    Positive,
    Negative
}

public enum MoveDirection
{
    None,
    Horizontal,
    Vertical,
    LeftDiagonal,
    RightDiagonal
}

public class Spin : MonoBehaviour
{
    private GameObject _firstLinkedSpin;
    private GameObject _secondLinkedSpin;

    [SerializeField]
    private ParticleSystem _chargedEffect;
    [SerializeField]
    private Animator _spinAnimator;

    [SerializeField]
    private SpinType _spinType;
    [SerializeField]
    private MoveDirection _moveDirection;
    [SerializeField]
    private float _spinMoveSpeed;
    [SerializeField]
    private Vector2 _spinDefaultPosition;
    [SerializeField]
    private float _spinMoveRange;
    private bool _isSpinCanMove;

    private void Start()
    {
        _spinDefaultPosition = transform.position;
        _isSpinCanMove = true;
    }

    private void FixedUpdate()
    {
        if(IsNoneSpinLinked() && _isSpinCanMove)
        {
            SpinMoving();
        }
    }

    public void StaticSpin()
    {
        _isSpinCanMove = false;
    }

    private void SpinMoving()
    {
        if(transform.position.x > _spinDefaultPosition.x + _spinMoveRange || transform.position.y > _spinDefaultPosition.y + _spinMoveRange)
        {
            _spinMoveSpeed = -_spinMoveSpeed;
        }else if(transform.position.x < _spinDefaultPosition.x - _spinMoveRange || transform.position.y < _spinDefaultPosition.y - _spinMoveRange)
        {
            _spinMoveSpeed = -_spinMoveSpeed;
        }
        Vector2 moveDir = GetSpinMoveDiretion();
        transform.position += (Vector3)moveDir * Time.deltaTime;
    }

    public bool IsNoneSpinLinked()
    {
        if(_firstLinkedSpin == null && _secondLinkedSpin == null)
        {
            return true;
        }
        return false;
    }

    public bool IsSpinLinkedTo(GameObject spin)
    {
        if (spin == _firstLinkedSpin || spin == _secondLinkedSpin)
        {
            return true;
        }
        return false;
    }

    public bool IsSpinCanLinkTo()
    {
        if(_firstLinkedSpin == null || _secondLinkedSpin == null)
        {
            return true;
        }
        return false;
    }

    public bool IsSameSpinType(SpinType spinType)
    {
        if(spinType == _spinType)
        {
            return true;
        }
        return false;
    }

    public SpinType GetSpinType()
    {
        return _spinType;
    }

    public bool LinkSpin(GameObject spin)
    {
        if (_firstLinkedSpin == null)
        {
            _firstLinkedSpin = spin;
            SpinFullyChargedEffect();
            return true;
        }
        else if (_secondLinkedSpin == null)
        {
            _secondLinkedSpin = spin;
            SpinFullyChargedEffect();
            return true;
        }
        return false;
    }

    public void SpinFullyChargedEffect()
    {
        if(_firstLinkedSpin != null && _secondLinkedSpin != null)
        {
            _chargedEffect.Play();
            if(_spinType == SpinType.Positive)
            {
                _spinAnimator.Play("(+)IdleCharged");
            }
            else
            {
                _spinAnimator.Play("(-)IdleCharged");
            }
        }
    }

    public bool IsSpinFullyCharge()
    {
        if(_firstLinkedSpin != null && _secondLinkedSpin != null)
        {
            return true;
        }
        return false;
    }

    public Vector2 GetSpinMoveDiretion()
    {
        switch (_moveDirection)
        {
            case MoveDirection.Horizontal:
                return new Vector2(_spinMoveSpeed, 0);
            case MoveDirection.Vertical:
                return new Vector2(0, _spinMoveSpeed);
            case MoveDirection.LeftDiagonal:
                return new Vector2(_spinMoveSpeed, _spinMoveSpeed);
            case MoveDirection.RightDiagonal:
                return new Vector2(-_spinMoveSpeed, _spinMoveSpeed);
            default:
                return Vector2.zero;
        }
    }
}
