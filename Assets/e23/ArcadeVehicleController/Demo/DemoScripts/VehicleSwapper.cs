using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace e23.VehicleController.Demo
{
    public class VehicleSwapper : MonoBehaviour
    {
        public event Action onVehicleSwapped;

        [SerializeField] private List<SwapData> vehiclesToSwap = null;
        [SerializeField] private GameObject aiCamera = null;
        [SerializeField] private GameObject helicopterControls = null;
        [SerializeField] private int startVehicle = 0;

        private int activeVehicle = 0;

        public VehicleBehaviour ActiveVehicle => vehiclesToSwap[activeVehicle].vehicleBehaviour;

        private void Awake() 
        {
            for (int i = 0; i < vehiclesToSwap.Count; i++)
            {
                Transform vehcileTransform = vehiclesToSwap[i].input.transform;
                vehiclesToSwap[i].ResetPos = vehcileTransform.position;
            }
        }

        private void Start() 
        {
            if (aiCamera != null) { aiCamera.SetActive(false); }
            
            for (int i = 0; i < vehiclesToSwap.Count; i++)
            {
                ToggleCameraType(vehiclesToSwap[i]);
            }
            
            SwapVehicle(startVehicle);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                vehiclesToSwap[activeVehicle].isFollowCam = !vehiclesToSwap[activeVehicle].isFollowCam;
                ToggleCameraType(vehiclesToSwap[activeVehicle]);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                aiCamera.SetActive(!aiCamera.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetVehiclePosition();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void SwapVehicle(int index)
        {
            for (int i = 0; i < vehiclesToSwap.Count; i++)
            {
                if (i == index)
                {
                    activeVehicle = index;
                    SetDataState(vehiclesToSwap[i], true);

                    onVehicleSwapped?.Invoke();
                }
                else
                {
                    SetDataState(vehiclesToSwap[i], false);
                }
            }

            if (helicopterControls != null) { helicopterControls.SetActive(vehiclesToSwap[activeVehicle].isHelicopter); }
        }

        private void SetDataState(SwapData swapData, bool active)
        {
            swapData.cameraParent.SetActive(active);
            swapData.input.enabled = active;
        }

        private void ToggleCameraType(SwapData swapData)
        {
            if (swapData.isFollowCam == false)
            {
                swapData.topDownCamera.gameObject.SetActive(true);
                swapData.followCamera.gameObject.SetActive(false);
            }
            else
            {
                swapData.topDownCamera.gameObject.SetActive(false);
                swapData.followCamera.gameObject.SetActive(true);
            }
        }

        private void ResetVehiclePosition()
        {
            vehiclesToSwap[activeVehicle].vehicleBehaviour.SetPosition(vehiclesToSwap[activeVehicle].ResetPos, Quaternion.identity);
        }

        [System.Serializable]
        public class SwapData
        {
            public Examples.ExampleInput input;
            public bool isFollowCam = false;
            public GameObject cameraParent;
            public CinemachineVirtualCamera topDownCamera;
            public CinemachineVirtualCamera followCamera;
            public bool isHelicopter = false;

            private Vector3 startingPosition;
            public VehicleBehaviour vehicleBehaviour { get { return input.GetComponent<VehicleBehaviour>(); } }
            public Vector3 ResetPos { get { return startingPosition; } set { startingPosition = value; } }
        }
    }
}