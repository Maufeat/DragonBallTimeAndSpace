using UnityEngine;
using System.Collections;

public class F3DSolarCamera : MonoBehaviour
{
    Transform focus;

    F3DPlanet[] pObjects;
    int index;
    
    [Range(0.1f, 5f)]
    public float mouseSensitivity;

    [Range(20f, 200f)]
    public float zoomSpeed;

    float DefaultZoom = 100;

    Vector3 cameraRotation; 
    Vector3 offsetVector;
    float zoomLevel;

    new Transform camera;

    void Awake()
    {
        camera = GetComponent<Transform>();      
    }

    void Start()
    {            
        zoomLevel += DefaultZoom;

        index = 0;
        pObjects = FindObjectsOfType<F3DPlanet>();
        focus = pObjects[0].transform;
    }

    public Vector2 GetMouse()
    { 
        return new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
    }

    void LateUpdate()
    {

            if (Input.GetMouseButtonDown(0))
            {
                index++;
                if (index > pObjects.Length - 1)
                    index = 0;

                focus = pObjects[index].transform;
                zoomLevel = focus.localScale.x * 3;  
            }

            if (Input.GetMouseButtonDown(1))
            {
                index--;
                if (index < 0)
                    index = pObjects.Length - 1;

                focus = pObjects[index].transform;
                zoomLevel = focus.localScale.x * 3;            
            }
        
            cameraRotation += new Vector3(GetMouse().x, GetMouse().y, 0f) * mouseSensitivity;

            cameraRotation.y = Mathf.Clamp(cameraRotation.y, -85f, 85f);

            zoomLevel += Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;
            zoomLevel = Mathf.Clamp(zoomLevel, focus.localScale.x + focus.localScale.x / 3, 150f);
            offsetVector = -Vector3.forward * zoomLevel;

            offsetVector = Quaternion.AngleAxis(cameraRotation.x, Vector3.up) * offsetVector;


            Vector3 offsetSide = Vector3.Cross(offsetVector, Vector3.up).normalized;

            offsetVector = Quaternion.AngleAxis(cameraRotation.y, offsetSide) * offsetVector;

          
            camera.position = focus.position + offsetVector;          
            camera.rotation = Quaternion.LookRotation(focus.transform.position - transform.position, Vector3.up);

          
        
    }

    // Show FULLSCREEN button
#if !UNITY_EDITOR
    void OnGUI()
    {
        if (!Screen.fullScreen)
        {
            if (GUI.Button(new Rect(10, 10, 150, 100), "Fullscreen"))
            {
                Resolution[] resolutions = Screen.resolutions;
                Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
            }
        }

    }
#endif

}





