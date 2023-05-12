using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class RotatePlayerUsingTouch : MonoBehaviour
    {
        PlayerControls inputActions;
        float leftAndRightAngle;
        [SerializeField]
        float speed = 20;
        Vector2 moveDelta;
        float horizontal;

        private void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();

                inputActions.RotateControls.DeltaX.performed += inputActions => moveDelta = inputActions.ReadValue<Vector2>();
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void Update()
        {
            horizontal = -moveDelta.x;

            leftAndRightAngle += horizontal * speed * Time.deltaTime;

            Vector3 rotation = Vector3.zero;
            rotation.y = leftAndRightAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;

        }
    }
    
}
