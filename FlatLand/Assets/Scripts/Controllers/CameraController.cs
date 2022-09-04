using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.5f;
    [Range(0.1f, 90f)]
    public float perspectiveMinValue = 0.1f;
    [Range(0.2f, 179.9f)]
    public float perspectiveMaxValue = 179.9f;
    [Space(20)]
    public float orthoZoomSpeed = 0.5f;
    public bool isOrtho = false;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            Debug.LogError("CameraController: Failed to get camera.");
    }

    private void Update()
    {
        if(Input.touchCount == 2)
        {
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);

            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

            float prevTouchDeltaMag = (touchOnePrevPos - touchTwoPrevPos).magnitude;
            float touchDeltaMag = (touchOne.position - touchTwo.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            isOrtho = cam.orthographic;
            if(cam.orthographic)
            {
                cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
            }
            else
            {
                cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, perspectiveMinValue, perspectiveMaxValue);
            }
        }
        else if(Application.isEditor)
        {
            cam.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * perspectiveZoomSpeed * -100f;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, perspectiveMinValue, perspectiveMaxValue);
        }
    }
}
