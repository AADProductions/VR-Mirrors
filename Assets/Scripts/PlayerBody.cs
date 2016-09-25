using UnityEngine;
using System.Collections;

public class PlayerBody : MonoBehaviour {

	public Transform Pivot;
	public Transform Head;
	public Transform RightHandTarget;
	public Transform LeftHandTarget;
	public Transform HeadTarget;
	public Transform LookTarget;
	public Transform LeftFootTarget;
	public Transform RightFootTarget;
	Animator animator;
	Vector3 leftFootPos;
	Vector3 rightFootPos;
	Vector3 leftFootRot;
	Vector3 rightFootRot;
//	Vector3 rot;

	void Start () {
		animator = GetComponent <Animator> ();
	}

	void Update () {
		leftFootPos = LeftFootTarget.position;
		leftFootPos.y = 0f;
		rightFootPos = RightFootTarget.position;
		rightFootPos.y = 0f;

		leftFootRot = LeftFootTarget.eulerAngles;
		leftFootRot.x = 0f;
		leftFootRot.z = 0f;

		rightFootRot = RightFootTarget.eulerAngles;
		rightFootRot.x = 0f;
		rightFootRot.z = 0f;

		Pivot.position = HeadTarget.position;
        //rot = HeadTarget.eulerAngles;
        //rot.x = 0f;
        //rot.z = 0f;
        //Pivot.eulerAngles = rot;//Vector3.Lerp (Vector3.zero, rot, 0.5f);
        Pivot.eulerAngles = new Vector3(Pivot.eulerAngles.x, HeadTarget.eulerAngles.y, Pivot.eulerAngles.z);
    }

    void LateUpdate()
    {
        Head.eulerAngles = new Vector3(HeadTarget.eulerAngles.z, HeadTarget.eulerAngles.y + 270, -(HeadTarget.eulerAngles.x + 90));
    }

    void OnAnimatorIK()
	{
		animator.SetIKPosition (AvatarIKGoal.RightHand, RightHandTarget.position);
		animator.SetIKRotation (AvatarIKGoal.RightHand, RightHandTarget.rotation);
		animator.SetIKPositionWeight (AvatarIKGoal.RightHand, 1f);
		animator.SetIKRotationWeight (AvatarIKGoal.RightHand, 1f);

		animator.SetIKPosition (AvatarIKGoal.LeftHand, LeftHandTarget.position);
		animator.SetIKRotation (AvatarIKGoal.LeftHand, LeftHandTarget.rotation);
		animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1f);
		animator.SetIKRotationWeight (AvatarIKGoal.LeftHand, 1f);

		animator.SetIKPosition (AvatarIKGoal.RightFoot, rightFootPos);
		animator.SetIKRotation (AvatarIKGoal.RightFoot, Quaternion.Euler (rightFootRot));
		animator.SetIKPositionWeight (AvatarIKGoal.RightFoot, 1f);
		animator.SetIKRotationWeight (AvatarIKGoal.RightFoot, 1f);
		
		animator.SetIKPosition (AvatarIKGoal.LeftFoot, leftFootPos);
		animator.SetIKRotation (AvatarIKGoal.LeftFoot, Quaternion.Euler (leftFootRot));
		animator.SetIKPositionWeight (AvatarIKGoal.LeftFoot, 1f);
		animator.SetIKRotationWeight (AvatarIKGoal.LeftFoot, 1f);

		//animator.SetLookAtPosition (LookTarget.position);
		//animator.SetLookAtWeight (1f);
	}
}
