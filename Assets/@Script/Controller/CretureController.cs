using UnityEngine;

public class CretureController : BaseController 
{
    protected Animator anim;
    protected bool isAttacking = false;
    protected Define.State _state;
    public Define.State State
    {
        get => _state;
        set
        {
            _state = value;
            ChangeAnim(value);
        }
    }
    protected override bool Init()
    {
        if(base.Init() == false) 
            return false;   


        return true;    
    }

    private void Update()
    {
        UpdateMethod();
    }
    protected virtual void UpdateMethod()
    {
        switch (State)
        {
            case Define.State.Idle:
                TryAttack();
                break;

            case Define.State.Attack:
                break; // 코루틴으로 처리되므로 여기선 대기

            case Define.State.Move:
                TryMove();
                break;
        }
    }

    protected virtual void ChangeAnim(Define.State state)
    {
        switch (state)
        {
            case Define.State.Idle:
                anim.Play("Ready");
                break;

            case Define.State.Attack:
                anim.Play("Attack");
                break;

            case Define.State.Move:
                anim.Play("Walk");
                break;
        }
    }
    protected virtual void TryAttack() { }
    protected virtual void TryMove() { }
}
