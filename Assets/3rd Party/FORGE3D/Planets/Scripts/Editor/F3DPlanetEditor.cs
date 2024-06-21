using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(F3DPlanet))]
public class F3DPlanetEditor : Editor
{
    [MenuItem("FORGE3D/Planets/Add F3DPlanet")]
    static void AddSunScript()
    {
        Selection.activeGameObject.AddComponent<F3DPlanet>();
    }

    [MenuItem("FORGE3D/Planets/Add F3DPlanet", true, 0)]
    static bool ValidateSunScript()
    {
        if (Selection.activeGameObject != null && !Selection.activeGameObject.GetComponent<F3DPlanet>())
            return true;
        else return false;
    }

    public override void OnInspectorGUI()
    {
        F3DPlanet myTarget = (F3DPlanet)target;

        EditorGUILayout.BeginVertical(); // BEGIN

        //
        GUIStyle smallFont = new GUIStyle();
        smallFont.fontSize = 9;
        smallFont.wordWrap = true;

        smallFont.normal.textColor = new Color(0.7f, 0.7f, 0.7f);

        GUIStyle headerFont = new GUIStyle();
        headerFont.fontSize = 11;
        headerFont.fontStyle = FontStyle.Bold;
        headerFont.normal.textColor = new Color(0.75f, 0.75f, 0.75f);

        GUIStyle subHeaderFont = new GUIStyle();
        subHeaderFont.fontSize = 10;
        subHeaderFont.fontStyle = FontStyle.Bold;
        subHeaderFont.margin = new RectOffset(1, 0, 0, 0);
        subHeaderFont.padding = new RectOffset(1, 0, 3, 0);
        subHeaderFont.normal.textColor = new Color(0.70f, 0.70f, 0.70f);
        //

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Planet settings:", headerFont);

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginVertical("Box");
        myTarget.OrbitPoint = (Transform)EditorGUILayout.ObjectField("Orbit around point:", myTarget.OrbitPoint, typeof(Transform), true);
        EditorGUILayout.EndVertical();

        myTarget.OrbitRate = EditorGUILayout.FloatField("Orbit rate:", myTarget.OrbitRate);
        myTarget.RotationRate = EditorGUILayout.FloatField("Local rotation:", myTarget.RotationRate);

        myTarget.ShowOrbit = EditorGUILayout.Toggle("Show trajectory", myTarget.ShowOrbit);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical(); // END

        EditorUtility.SetDirty(myTarget);
    }

    void OnSceneGUI()
    {
        F3DPlanet myTarget = (F3DPlanet)target;

        if (myTarget.OrbitPoint && myTarget.ShowOrbit)
        {
            float distToOrbitPoint = Vector3.Distance(myTarget.transform.position, myTarget.OrbitPoint.position);
            Handles.Disc(Quaternion.identity, myTarget.OrbitPoint.position, myTarget.transform.up, distToOrbitPoint, false, 1);
        }       
    }
}
