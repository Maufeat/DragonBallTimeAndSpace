using UnityEngine;
using System.Collections;

public class F3DSunController : MonoBehaviour {

    float mouseX, mouseY;
    Vector3 offsetVector;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update ()
    {
        mouseX += Input.GetAxis("Mouse X") * 3;
        mouseY += Input.GetAxis("Mouse Y") * 3;

        offsetVector = Quaternion.AngleAxis(mouseX, Vector3.up) * (Vector3.forward + Vector3.up) * 3;

        Vector3 offsetSide = Vector3.Cross(offsetVector, Vector3.up).normalized;

        offsetVector = Quaternion.AngleAxis(mouseY, offsetSide) * offsetVector;

        transform.position = offsetVector;

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
