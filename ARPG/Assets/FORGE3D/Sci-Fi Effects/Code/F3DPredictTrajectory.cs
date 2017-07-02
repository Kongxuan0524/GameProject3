/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;

public class F3DPredictTrajectory : MonoBehaviour
{

	public static Vector3 Predict(Vector3 sPos, Vector3 tPos, Vector3 tLastPos, float pSpeed)
    {
        // Target velocity
        Vector3 tVel = (tPos - tLastPos) / Time.deltaTime;
        
        // Time to reach the target
        float flyTime = GetProjFlightTime(tPos - sPos, tVel, pSpeed);

        if (flyTime > 0)
            return tPos + flyTime * tVel;
        else
            return tPos;
    }

    static float GetProjFlightTime(Vector3 dist, Vector3 tVel, float pSpeed)
    {
        float a = Vector3.Dot(tVel, tVel) - pSpeed * pSpeed;
        float b = 2.0f * Vector3.Dot(tVel, dist);
        float c = Vector3.Dot(dist, dist);

        float det = b * b - 4 * a * c;

        if (det > 0)
            return 2 * c / (Mathf.Sqrt(det) - b);
        else
            return -1;
    }
}
