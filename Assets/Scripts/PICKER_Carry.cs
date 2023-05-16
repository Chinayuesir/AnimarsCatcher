using UnityEngine;

public class PICKER_Carry : MonoBehaviour
{
    public float checkRadius = 3f;

    private bool mCanCarryObj;
    private float mHandDistance = 0.6f;
    private string mCarryObjTag = "CanCarry";
    private Vector3 mCarryObjPos = new Vector3(0f, 2.53f, 2.09f);
    private Vector3 mHandOffset = new Vector3(0f, 0.03f, 0f);
    private GameObject mLeftHandEffector;
    private GameObject mRightHandEffector;
    private GameObject mCarryObj;
    private Animator mAnimator;

    void Awake()
    {
        mAnimator = GetComponent<Animator>();
        InitHandEffectors();
    }

    void Update()
    {
        // Test
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!mCanCarryObj)
            {
                StartCarry();
            }
            else
            {
                StopCarry();
            }
        }

        if (mCanCarryObj)
        {
            GetCarryObj();
            SetCarryObjPos();
            CalculateHandEffectorPosition();
        }
    }

    #region Carry IK Controll
    public void StartCarry()
    {
        mCanCarryObj = true;
    }

    public void StopCarry()
    {
        mCanCarryObj = false;
        mCarryObj = null;
    }

    private void InitHandEffectors()
    {
        // create left hand effector
        if (mLeftHandEffector == null)
        {
            mLeftHandEffector = new GameObject(name + " LeftHandEffector");
            mLeftHandEffector.transform.parent = transform;
        }
        // create right hand effector
        if (mRightHandEffector == null)
        {
            mRightHandEffector = new GameObject(name + " RightHandEffector");
            mRightHandEffector.transform.parent = transform;
        }
    }

    private void GetCarryObj()
    {
        if (mCarryObj != null) return;

        // get objects near character
        Collider[] results = new Collider[10];
        Physics.OverlapSphereNonAlloc(transform.position, checkRadius, results);

        // find target with min distance
        float distance = float.MaxValue;
        float currentDistance;
        mCarryObj = null;
        foreach (var item in results)
        {
            // check tag
            if (item == null) break;
            if (!item.CompareTag(mCarryObjTag)) continue;
            currentDistance = Vector3.Distance(transform.position, item.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                mCarryObj = item.gameObject;
            }
        }
    }

    private void SetCarryObjPos()
    {
        if (mCarryObj == null) return;
        mCarryObj.transform.SetPositionAndRotation(transform.TransformPoint(mCarryObjPos), transform.rotation);
    }

    private void CalculateHandEffectorPosition()
    {
        if (mCarryObj == null) return;

        // calculate hand local position
        Vector3 cubeScale = mCarryObj.transform.localScale;
        Vector3 mid = mCarryObjPos - 0.5f * cubeScale.z * Vector3.forward - 0.5f * cubeScale.y * Vector3.up;
        Vector3 leftHandPos = mid - Vector3.right * mHandDistance / 2 + mHandOffset;
        Vector3 rightHandPos = mid + Vector3.right * mHandDistance / 2 + mHandOffset;

        // hand global position and rotation
        leftHandPos = transform.TransformPoint(leftHandPos);
        rightHandPos = transform.TransformPoint(rightHandPos);
        Quaternion handRotation = Quaternion.Euler(0, 0, 180) * transform.rotation;

        // set effectors
        mLeftHandEffector.transform.SetPositionAndRotation(leftHandPos, handRotation);
        mRightHandEffector.transform.SetPositionAndRotation(rightHandPos, handRotation);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (mCarryObj == null) return;

        // process left hand ik
        mAnimator.SetIKPosition(AvatarIKGoal.LeftHand, mLeftHandEffector.transform.position);
        mAnimator.SetIKRotation(AvatarIKGoal.LeftHand, mLeftHandEffector.transform.rotation);
        mAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        mAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        // process right hand ik
        mAnimator.SetIKPosition(AvatarIKGoal.RightHand, mRightHandEffector.transform.position);
        mAnimator.SetIKRotation(AvatarIKGoal.RightHand, mRightHandEffector.transform.rotation);
        mAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        mAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
    }
    #endregion
}
