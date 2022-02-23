
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

namespace jokerispunk
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class ComfortRampCollision : UdonSharpBehaviour
    {
        public Collider flat, ramp;
        private VRCPlayerApi lp;
        private LayerMask colliderMask;
        private bool moveVert, moveHoriz;
        private bool moving, movingLast;

        private void Start()
        {
            lp = Networking.LocalPlayer;

            // filter the ground-check raycast for only the layer containing the ramp collider
            int rampLayer = ramp.gameObject.layer;
            colliderMask = 1 << rampLayer;
        }

        private void Update()
        {
            bool grounded = lp.IsPlayerGrounded();
            moving = (moveVert || moveHoriz || !grounded);

            // on move
            if (moving && !movingLast)
                _DisableComfort();

            // on stop
            if (!moving && movingLast)
            {
                // has player stopped on the ramp? if so, switch to flat
                RaycastHit rh;
                Vector3 lpPos = lp.GetPosition();
                Vector3 gndRayOrigin = lpPos;
                gndRayOrigin.y += 0.2f;
                Physics.Raycast(gndRayOrigin, Vector3.down, out rh, 1f, colliderMask);

                if (rh.collider == ramp)
                    _EnableComfort(lpPos);
            }

            movingLast = moving;
        }

        public void _EnableComfort(Vector3 lpPos)
        {
            ramp.enabled = false;
            flat.transform.position = lpPos;
            flat.enabled = true;
        }

        public void _DisableComfort()
        {
            ramp.enabled = true;
            flat.enabled = false;
        }

        public override void InputMoveVertical(float value, UdonInputEventArgs args)
        {
            moveVert = value != 0f;
        }

        public override void InputMoveHorizontal(float value, UdonInputEventArgs args)
        {
            moveHoriz = value != 0f;
        }

        // disable the feature entirely by deactivating this GameObject
        private void OnDisable()
        {
            _DisableComfort();
        }

        // avoids a narrow issue where the player respawn is located on the ramp itself
        public override void OnPlayerRespawn(VRCPlayerApi player)
        {
            if (player.isLocal)
                _DisableComfort();
        }
    }
}
