using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Graphics : NetworkBehaviour
{
    private NetworkAnimator animator;

    private string speedParam = "speed";
    private string attackParam = "attack";
    private string isAttackingParam = "IsAttacking";
    
    public void Init(NetworkAnimator anim)
    {
        animator = anim;
    }

    public void MoveAnimation(bool isMoving)
    {
        float speed = isMoving ? 1 : 0;
        
        if (animator)
            animator.Animator.SetFloat(speedParam, speed);
        else
            Debug.Log("animator is null");
    }

    public void AttackAnimation(int currentAttack)
    {
        animator.Animator.SetFloat(attackParam, currentAttack);
        animator.Animator.SetTrigger(isAttackingParam);
        AttackingClientRpc();
    }
    
    [ClientRpc]
    private void AttackingClientRpc()
    {
        animator.Animator.SetTrigger(isAttackingParam);
    }
}
