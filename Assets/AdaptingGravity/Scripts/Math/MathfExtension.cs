// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathfExtension.cs" company="Johannes Deml">
//   Copyright (c) 2015 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptingGravity.Math
{
    using UnityEngine;
    using System.Collections;

    public static class MathfExtension
    {
        public static float ClampAngle(this float angle, float minAngle, float maxAngle)
        {
            //Move all angles in the space of  -180, 180
            if (angle < 90f || angle > 270f)
            {
                if (angle > 180f)
                {
                    angle -= 360f;
                }
                if (maxAngle > 180f)
                {
                    maxAngle -= 360f;
                }
                if (minAngle > 180f)
                {
                    minAngle -= 360f;
                }
            }

            //Clamp angle
            if (angle > maxAngle)
            {
                angle = maxAngle;
            } else if (angle < minAngle)
            {
                angle = minAngle;
            }

            //Convert back to normal angle format
            if (angle < 0f)
            {
                angle += 360f;
            }

            return angle;
        }
    }
}
