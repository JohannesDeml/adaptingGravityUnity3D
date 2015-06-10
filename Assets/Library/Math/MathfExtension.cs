using UnityEngine;
using System.Collections;

namespace Deml.Math
{
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
