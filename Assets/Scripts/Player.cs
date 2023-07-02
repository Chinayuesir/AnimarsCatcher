using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    public class Player : MonoBehaviour
    {
        public Animator PICKER_Ani;
        public Animator BLASTER_Ani;
        public float MoveSpeed = 20f;

        public float ControlRadiusMin = 0f;
        public float ControlRadiusMax = 5f;

        private float mCurrentRadius;
        private Vector3 mTargetPos;
        private bool mRightMouseButton;

        private List<PICKER_Ani> mPickerAniList = new List<PICKER_Ani>();
        private List<BLASTER_Ani> mBlasterAniList = new List<BLASTER_Ani>();
        private float mAniSpeed = 2f;

        //Components
        private Rigidbody mRigidbody;
        private CharacterController mCharacterController;
        
        //MainCamera
        private Camera mMainCamera;
        

        private void Awake()
        {
            mRigidbody = GetComponent<Rigidbody>();
            mCharacterController = GetComponent<CharacterController>();
            mMainCamera=Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                mRightMouseButton = true;
                mTargetPos = GetMouseWorldPos();
                GetControlAnis();
            }
            else
            {
                mRightMouseButton = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var targetPos = GetMouseWorldPos();
                foreach (var pickerAni in mPickerAniList)
                {
                   
                }

                foreach (var blasterAni in mBlasterAniList)
                {
                    blasterAni.SetMoveTargetPos(targetPos);
                }
            }

            mCurrentRadius = Mathf.Lerp(mCurrentRadius, mRightMouseButton ? ControlRadiusMax : ControlRadiusMin,
                Time.deltaTime * 10f);
        }

        private void FixedUpdate()
        {
            RobotMove();
        }

        private void RobotMove()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float y = mMainCamera.transform.rotation.eulerAngles.y;
            Vector3 targetDirection = new Vector3(h, 0, v);
            targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;

            if (targetDirection != Vector3.zero)
                transform.forward = Vector3.Lerp(transform.forward, targetDirection, 10f * Time.deltaTime);
            var speed = targetDirection * MoveSpeed;
            //mRigidbody.velocity = speed;
            mCharacterController.SimpleMove(speed);
        }

        private void GetControlAnis()
        {
            Collider[] hitColliders = new Collider[50];
            int numColliders = Physics.OverlapSphereNonAlloc(mTargetPos, mCurrentRadius, hitColliders);
            for (int i = 0; i < numColliders; i++)
            {
                if (hitColliders[i].CompareTag("PICKER_Ani"))
                {
                    var pickerAni = hitColliders[i].GetComponent<PICKER_Ani>();
                    mPickerAniList.Add(pickerAni);
                    pickerAni.IsFollow = true;
                }else if (hitColliders[i].CompareTag("BLASTER_Ani"))
                {
                    mBlasterAniList.Add(hitColliders[i].GetComponent<BLASTER_Ani>());
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(mTargetPos,mCurrentRadius);
        }

        private Vector3 GetMouseWorldPos()
        {
            Ray ray = mMainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 200))
            {
                return hit.point;
            }
            return Vector3.zero;
        }

        //Test Code for Animation
        private void AnimationSystemTest()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PICKER_Ani.SetBool("LeftMouseDown",true);
                BLASTER_Ani.SetBool("LeftMouseDown",true);
            }

            if (Input.GetMouseButton(0))
            {
                mAniSpeed = Mathf.Clamp(mAniSpeed + Time.deltaTime * 5, 2, 5);
                PICKER_Ani.SetFloat("AniSpeed",mAniSpeed);
                BLASTER_Ani.SetFloat("AniSpeed",mAniSpeed);
            }

            if (Input.GetMouseButton(1))
            {
                mAniSpeed = Mathf.Clamp(mAniSpeed - Time.deltaTime * 5, 2, 5);
                PICKER_Ani.SetFloat("AniSpeed",mAniSpeed);
                BLASTER_Ani.SetFloat("AniSpeed",mAniSpeed);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                BLASTER_Ani.SetTrigger("Shoot");
            }
        }
    }
}

