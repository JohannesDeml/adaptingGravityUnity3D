using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GravityHandle))]
public class GravityHandleEditor : Editor
{
    private GravityHandle handle;
    private void OnSceneGUI()
    {
        handle = (GravityHandle) target;
        Handles.color = Color.green;
        Handles.Slider(handle.transform.position, handle.transform.forward, handle.transform.localScale.z, Handles.ArrowCap, 1f);
    }
}
