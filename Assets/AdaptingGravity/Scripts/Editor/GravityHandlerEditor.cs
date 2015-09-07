// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GravityHandlerEditor.cs" company="Johannes Deml">
//   Copyright (c) 2015 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptingGravity.Editor
{
    using UnityEngine;
    using System.Collections;
    using AdaptingGravity.Physics.Gravity;
    using UnityEditor;

    /// <summary>
    /// This editor script extends the tweaking functionality for the gravity handler in the editor.
    /// It adds the possibility to add gravity handles <see cref="GravityHandle"/>
    /// </summary>
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
                GameObject newHandle = CreateNewGravityHandle();
                GravityHandle gravityHandle = newHandle.GetComponent<GravityHandle>();
                gravityHandle.Parent = gravityHandler;
                gravityHandler.handles.Add(gravityHandle);
                EditorUtility.SetDirty(gravityHandler);
                EditorUtility.SetDirty(newHandle);
                //Jump to the newly created object in the editor, this way it can be edited right away
                Selection.activeGameObject = newHandle;
            }
        }

        /// <summary>
        /// Creates a new gameobject from a prefab which will be used as a new handle.
        /// The game object gets the name "Handle" + the number of handles that handle list has stored
        /// </summary>
        /// <returns>A gravity handle game object</returns>
        private GameObject CreateNewGravityHandle()
        {
            int newHandleID = gravityHandler.handles.Count;
            GameObject newHandle = (GameObject)Instantiate(gravityHandler.GravityHandlePrefab, gravityHandler.GravityHandles.transform.position,
                gravityHandler.GravityHandles.transform.rotation);
            newHandle.transform.SetParent(gravityHandler.GravityHandles.transform, true);
            newHandle.name = "Handle" + newHandleID;
            return newHandle;
        }
    }


}
