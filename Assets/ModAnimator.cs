using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModAnimator : MonoBehaviour
{

    [SerializeField] Animator animator;

    public void playReaction(bool correct)
    {
        animator.Play(correct ? "ModPositive" : "ModNegative");
    }

    public void panicState()
    {
        animator.SetFloat("Strikes", 1);
    }
}
