using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    public class Player : MonoBehaviour
    {
        public float MoveSpeed = 20f;

        public float ControlRadiusMin = 0f;
        public float ControlRadiusMax = 5f;

        public GameObject TargetPosGO;

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

        // Group Behaviour
        private Dictionary<Transform, int> mIndex = new();

        // Smoke
        public GameObject FXSmoke;

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
            AssignAniToCarry();
            AssignAniToShoot();
            
            mCurrentRadius = Mathf.Lerp(mCurrentRadius, mRightMouseButton ? ControlRadiusMax : ControlRadiusMin,
                Time.deltaTime * 10f);
            TargetPosGO.transform.position = GetMouseWorldPos();
            TargetPosGO.transform.Find("Cylinder").localScale = Vector3.one * (2 * mCurrentRadius);
        }

        private void FixedUpdate()
        {
            RobotMove();
            SetDestinations();
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

            SetSmoke(speed);
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
                    if (!mPickerAniList.Contains(pickerAni))
                    {
                        mPickerAniList.Add(pickerAni);
                        pickerAni.IsFollow = true;
                        mIndex.Add(pickerAni.transform, mIndex.Count);
                    }
                }else if (hitColliders[i].CompareTag("BLASTER_Ani"))
                {
                    var blasterAni = hitColliders[i].GetComponent<BLASTER_Ani>();
                    if (!mBlasterAniList.Contains(blasterAni))
                    {
                        mBlasterAniList.Add(blasterAni);
                        blasterAni.IsFollow = true;
                        mIndex.Add(blasterAni.transform, mIndex.Count);
                    }
                }
            }
        }

        private void AssignAniToCarry()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 50f))
                {
                    if (hit.collider.CompareTag("PickableItem"))
                    {
                        var pickerAni = ChooseOnePickerAni();
                        if (pickerAni != null)
                        {
                            pickerAni.IsPick = true;
                            pickerAni.PickableItem = hit.collider.gameObject.GetComponent<PickableItem>();
                        }
                    }
                }
            }
        }

        private PICKER_Ani ChooseOnePickerAni()
        {
            foreach (var pickerAni in mPickerAniList)
            {
                if (!pickerAni.IsPick)
                {
                    return pickerAni;
                }
            }

            return null;
        }

        private void AssignAniToShoot()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit,50f))
                {
                    if (hit.collider.CompareTag("FragileItem"))
                    {
                        var blasterAni = ChooseOneBlasterAni();
                        if (blasterAni != null)
                        {
                            blasterAni.IsShoot = true;
                            blasterAni.FragileItem = hit.collider.gameObject.GetComponent<FragileItem>();
                        }
                    }
                }
            }
        }
        
        private BLASTER_Ani ChooseOneBlasterAni()
        {
            foreach (var blasterAni in mBlasterAniList)
            {
                if (!blasterAni.IsShoot)
                {
                    return blasterAni;
                }
            }
            return null;
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
        
        private void SetDestinations()
        {
            mPickerAniList.ForEach(item => item.Destination = FollowUtility.RectArrange(transform, mIndex[item.transform]));
            mBlasterAniList.ForEach(item => item.Destination = FollowUtility.RectArrange(transform, mIndex[item.transform]));
        }

        private void SetSmoke(Vector3 speed)
        {
            FXSmoke.SetActive(speed.sqrMagnitude > 0f);
            FXSmoke.transform.forward = -speed;
        }
    }
}

