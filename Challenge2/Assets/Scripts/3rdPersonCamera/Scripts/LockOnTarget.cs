//----------------------------------------------
//            3rd Person Camera
// Copyright © 2015-2022 Thomas Enzenebner
//            Version 1.0.8
//         t.enzenebner@gmail.com
//----------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ThirdPersonCamera
{
    [RequireComponent(typeof(CameraController)), RequireComponent(typeof(FreeForm))]
    public class LockOnTarget : MonoBehaviour
    {
        [Header("Basic settings")]
        [Tooltip("When not null, the camera will align itself to focus the Follow Target")]
        public Targetable followTarget = null;
        [Tooltip("How fast the camera should align to the Follow Target")]
        public float rotationSpeed = 3.0f;
        [Tooltip("Applies an additional vector to the target position to tilt the camera")]
        public Vector3 tiltVector;
        [Tooltip("Ignores the resulting height difference from the direction to the target")]
        public bool normalizeHeight = false;

        [Header("Targetables sorting settings")]
        [Tooltip("A default distance value to handle targetables sorting. Increase when game world and distances are very large")]
        public float defaultDistance = 10.0f;
        [Tooltip("A weight value to handle targetables sorting. Increase to prefer angles over distance")]
        public float angleWeight = 1.0f;
        [Tooltip("A weight value to handle targetables sorting. Increase to prefer distance over angles")]
        public float distanceWeight = 1.0f;       

        [Header("Extra settings")]
        [Tooltip("Use the forward vector of the target to find the nearest Targetable. Default is the camera forward vector")]
        public bool forwardFromTarget = false;
        [Tooltip("Inform the script of using a custom input method to set the CameraInputLockOn model")]
        public bool customInput = false;
        
        [HideInInspector]
        public bool HasFollowTarget;

        public Image lockOnUI;
        public Camera mainCamera;
        public Vector3 lockOnOffset = new Vector3(0, 4, 0);

#if TPC_DEBUG
        public bool debugLockedOnTargets;
#endif

        #region Private variables
        private CameraController cameraController;
        private bool hasFreeForm;
        private FreeForm freeForm;
        private CameraInputSampling_FreeForm ffInputSampling;
        private bool hasFollow;
        private Follow followComp;
        private SortTargetables sortTargetsMethod;

        private CameraInputLockOn inputLockOn;

        public List<Targetable> targets;
        private float currentDisableTime;
        private bool defaultSmoothPivot;
        private bool prevHasFollowTarget;
#endregion

        private void Start()
        {
            currentDisableTime = 0.0f;

            cameraController = GetComponent<CameraController>();
            freeForm = GetComponent<FreeForm>();
            followComp = GetComponent<Follow>();
            ffInputSampling = GetComponent<CameraInputSampling_FreeForm>();
            sortTargetsMethod = new SortTargetables(); // init the sort method

            hasFreeForm = freeForm != null;
            hasFollow = followComp != null;

            defaultSmoothPivot = cameraController.smoothPivot;

            if (!customInput)
            {
                var lookup = GetComponent<CameraInputSampling_LockOn>();
                if (lookup == null)
                    Debug.LogError("CameraInputSampling_LockOn not found on " + transform.name + ". Consider adding it to get input sampling or enable customInput to skip this message");
            }

            targets = new List<Targetable>();
        }

        // expensive method
        // try to utilize Add/RemoveTarget
        public void UpdateTargetables()
        {
            targets = new List<Targetable>(FindObjectsOfType<Targetable>());
        }

        public void AddTarget(Targetable newTarget)
        {
            targets.Add(newTarget);
        }

        public void RemoveTarget(Targetable target)
        {
            targets.Remove(target);
        }

        public void SetTargets(List<Targetable> newTargets)
        {
            targets = newTargets;
        }

        public void ClearTargets()
        {
            targets = new List<Targetable>();
        }

        public CameraInputLockOn GetInput()
        {
            return inputLockOn;
        }

        public void UpdateInput(CameraInputLockOn newInput)
        {
            inputLockOn = newInput;
        }

        void Update()
        {            
            if (cameraController == null || cameraController.target == null)
                return; 

            int cycleIndex = inputLockOn.cycleIndex;
            bool forceCycle = inputLockOn.forceCycle;

            if (lockOnUI)
            {
                lockOnUI.gameObject.SetActive(HasFollowTarget);

                if(followTarget != null)
                {
                    lockOnUI.transform.position = followTarget.transform.position;
                    lockOnUI.transform.localPosition += lockOnOffset;
                }
            }

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null)
                {
                    RemoveTarget(targets[i]);
                }
            }

                if (inputLockOn.followingTarget)
            {
                var targetPos = cameraController.target.transform.position;
                

                if (!HasFollowTarget || forceCycle)
                {
                    // find a viable target   

                    Vector3 forward = forwardFromTarget ? cameraController.target.transform.forward : cameraController.transform.forward;

                    if (targets != null && targets.Count > 0)
                    {
                        // target acquiring by distance/angle 

                        List<TargetableWithDistance> items = new List<TargetableWithDistance>(targets.Count);
                        for (int i = 0; i < targets.Count; i++)
                        {
                            var dirToTarget = (targets[i].transform.position - targetPos);
                            var distToTarget = dirToTarget.magnitude;
                            var angleToTarget = Vector3.Angle(forward, dirToTarget);

                            // calculate normalized score for sorting
                            float score = ((Mathf.Abs(angleToTarget) / 180.0f) * angleWeight) + ((distToTarget / defaultDistance) * distanceWeight);

                            var twd = new TargetableWithDistance()
                            {
                                target = targets[i],
                                distance = distToTarget,
                                angle = angleToTarget,
                                score = score,
                                
                        };
                            
                            items.Add(twd);                            
                        }

                        items.Sort(sortTargetsMethod);

#if TPC_DEBUG
                        if (debugLockedOnTargets)
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                var twd = items[i];
                                Debug.Log(i + " " + twd.target + " " + twd.distance + " " + twd.angle + twd.score);
                            }
                        }
#endif

                        int targetIndex = Cycle(cycleIndex, items.Count);

                        if (targetIndex < items.Count)
                        {
                            HasFollowTarget = true;
                            followTarget = items[targetIndex].target;
                            
                        }
                    }
                    else
                    {
                        HasFollowTarget = false;

                        // write back updated input values
                        inputLockOn.followingTarget = false;
                    }
                }
                else
                {
                    // target acquired
               
                    if (hasFollow)
                        followComp.follow = false;
               
                    if (hasFreeForm && (freeForm.stationaryModeHorizontal == StationaryModeType.Free && freeForm.stationaryModeVertical == StationaryModeType.Free))
                    {
                        Vector3 dirToTarget = (followTarget.transform.position + followTarget.offset) - (targetPos + cameraController.offsetVector + cameraController.cameraOffsetVector);

                        if (normalizeHeight)
                            dirToTarget.y = 0;

                        cameraController.cameraRotation = Quaternion.Slerp(cameraController.cameraRotation, Quaternion.LookRotation(dirToTarget + tiltVector, Vector3.up), Time.deltaTime * rotationSpeed);                        
                    }
                    else
                    {
                        Vector3 dirToTarget = (followTarget.transform.position + followTarget.offset) - (cameraController.transform.position);

                        if (normalizeHeight)
                            dirToTarget.y = 0;

                        Quaternion toRotation = Quaternion.Inverse(cameraController.cameraRotation) * Quaternion.LookRotation(dirToTarget + tiltVector, Vector3.up);

                        //Vector3 to = toRotation * Vector3.forward;
                        //Debug.DrawLine(transform.position, transform.position + to * 10);

                        cameraController.pivotRotation = toRotation;
                    }
                }
            }
            else
            {
                // release lock on
                HasFollowTarget = false;
            }

            if (prevHasFollowTarget == HasFollowTarget) 
                return;
            
            if (HasFollowTarget)
            {
                cameraController.smoothPivot = true;
            }
            else
            {
                followTarget = null;

                if (hasFollow)
                    followComp.follow = true;
                    
                cameraController.smoothPivot = defaultSmoothPivot;

                cameraController.pivotRotation = Quaternion.Slerp(cameraController.pivotRotation, Quaternion.identity, Time.deltaTime * rotationSpeed);

                cycleIndex = 0;

                // write back updated input values
                inputLockOn.cycleIndex = cycleIndex;
            }
                
            prevHasFollowTarget = HasFollowTarget;


        }

        public static int Cycle(int direction, int max)
        {
            int index = 0;
            if (direction > 0)
            {
                for (int i =0; i < direction;i++)
                {
                    index++;

                    if (index > max)
                        index = 0;
                }
            }
            else
            {
                direction = -direction;
                for (int i = 0; i < direction; i++)
                {
                    index--;

                    if (index < 0)
                        index = max - 1;
                }
            }

            return index;
        }
    }
}