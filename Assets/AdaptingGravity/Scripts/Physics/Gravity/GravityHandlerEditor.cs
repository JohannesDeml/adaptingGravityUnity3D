using UnityEngine;
using System.Collections;
using AdaptingGravity.Physics.Gravity;
using UnityEditor;

[CustomEditor(typeof(GravityHandler))]
public class GravityHandlerEditor : Editor
{
    private GravityHandler gravityHandler;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        gravityHandler = (GravityHandler)target;

        if (GUILayout.Button("Add Handle"))
        {
            int newHandleID = gravityHandler.handles.Count;
            GameObject newHandle = (GameObject)Instantiate(gravityHandler.GravityHandlePrefab, gravityHandler.GravityHandles.transform.position,
                gravityHandler.GravityHandles.transform.rotation);
            newHandle.transform.SetParent(gravityHandler.GravityHandles.transform, true);
            newHandle.name = "Handle" + newHandleID;
            GravityHandle gravityHandle = newHandle.GetComponent<GravityHandle>();
            gravityHandle.Parent = gravityHandler;
            gravityHandler.handles.Add(gravityHandle);
            EditorUtility.SetDirty(gravityHandler);
            EditorUtility.SetDirty(newHandle);
            Selection.activeGameObject = newHandle;
        }
    }
}
