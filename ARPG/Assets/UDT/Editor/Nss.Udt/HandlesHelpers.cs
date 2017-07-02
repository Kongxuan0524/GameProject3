using UnityEngine;
using UnityEditor;

namespace Nss.Udt.Common {
    public class HandlesHelpers {
        /// <summary>
        /// Draws a handle label only if its within the visible sceneview camera
        /// </summary>
        /// <param name="position"></param>
        /// <param name="threshold"></param>
        /// <param name="label"></param>
        /// <param name="style"></param>
        public static void VisibleLabel(Vector3 position, float threshold, string label, GUIStyle style) {
            if (IsVisible(position, threshold * 2f)) {
                Handles.Label(position, label, style);
            }
        }

        /// <summary>
        /// Draws a position handle in the scene view if the handle size is below the threshold size
        /// </summary>
        /// <param name="position">target position vector</param>
        /// <returns>updated position vector</returns>
        public static Vector3 VisiblePositionHandle(Vector3 position, float threshold) {
            if (IsVisible(position, threshold)) {
                if (HandleUtility.GetHandleSize(position) < threshold) {
                    position = Handles.PositionHandle(position, Quaternion.identity);
                }
            }

            return position;
        }

        /// <summary>
        /// Determines whether the specified target is visible.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns>
        ///   <c>true</c> if the specified target is visible; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsVisible(Vector3 target, float threshold) {
            try {
                Vector3 cameraInverse = Camera.current.WorldToScreenPoint(target);

                return ((cameraInverse.x >= 0.0f && cameraInverse.x <= Camera.current.pixelWidth) &&
                    (cameraInverse.y >= 0 && cameraInverse.y <= Camera.current.pixelHeight) &&
                    (cameraInverse.z > 0 && cameraInverse.z < threshold));
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Rotates a point around another as a pivot by a rotation
        /// </summary>
        /// <param name="point">Source point</param>
        /// <param name="pivot">Pivot point</param>
        /// <param name="rotation">Rotation amount</param>
        /// <returns>New position</returns>
        public static Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion rotation) {
            return rotation * (point - pivot) + pivot;
        }
    }
}