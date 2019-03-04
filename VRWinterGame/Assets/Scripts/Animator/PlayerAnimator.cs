using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {

        if (Intel.sharedInstance.ExerciseState == 0 && Intel.sharedInstance.Calibrated == true)
        {
            animator.SetBool("isActive", true);
            animator.SetBool("isNotActive", true);     

        } else if (Intel.sharedInstance.ExerciseState == 2)
        {
            animator.SetBool("isActive", false);
            animator.SetBool("isNotActive", false);
        }
    }
}
