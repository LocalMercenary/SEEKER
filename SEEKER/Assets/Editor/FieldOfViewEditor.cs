using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(EnemyAi))]

public class NewBehaviourScript : Editor
{

    private void OnSceneGUI()
    {
        EnemyAi fov = (EnemyAi)target;
        
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }

        if (fov.playerRef != null)
        {
            Handles.color = Color.cyan; // Change color for better visibility
            Handles.DrawWireArc(fov.playerRef.transform.position, Vector3.up, Vector3.forward, 360, fov.playerRadius);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
