using UnityEngine;

namespace AnimarsCatcher
{
    public class UIBlueprintPointer
    {
        public Transform PositionPointer;
        public float radius = 70f;
        public float displayRadius = 20f;

        public Transform Player;
        public Transform BlueprintTrans;

        private Vector3 mCenter;

        public UIBlueprintPointer(Transform pointer, Transform player, Transform blueprint)
        {
            PositionPointer = pointer;
            Player = player;
            BlueprintTrans = blueprint;

            mCenter = pointer.position;
        }

        public void Update()
        {
            if (BlueprintTrans == null) return;

            
            float angle = Vector3.SignedAngle(Vector3.forward, Vector3.ProjectOnPlane(BlueprintTrans.position - Player.position, Vector3.up), Vector3.up);
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.back) * Vector3.up;
            Vector3 pos;

            float distance = Vector3.Distance(BlueprintTrans.position, Player.position);
            if (distance < displayRadius)
            {
                pos = mCenter + dir.normalized * distance / displayRadius * radius;
            }
            else
            {
                pos = mCenter + dir.normalized * radius;
            }

            PositionPointer.position = pos;
        }
    }
}

