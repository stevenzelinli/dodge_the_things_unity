using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ProjectileLauncher))]
public class LauncherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.Pickable)]
    static void DrawGizmosSelected(ProjectileLauncher projectileLauncher, GizmoType gizmoType)
    {
        if (projectileLauncher.projectileType != null)
        {
            Gizmos.DrawSphere(
                projectileLauncher.transform.TransformPoint(projectileLauncher.spawnLocation),
                projectileLauncher.projectileType.projectileRadius
            );
        }
    }

    private void OnSceneGUI()
    {
        ProjectileLauncher projectileLauncher = target as ProjectileLauncher;
        Transform transform = projectileLauncher.transform;
        using (var cc = new EditorGUI.ChangeCheckScope())
        {
            Vector3 newOffset = transform.InverseTransformPoint(
                Handles.PositionHandle(
                    transform.TransformPoint(projectileLauncher.spawnLocation),
                    transform.rotation
                )
            );
            if (cc.changed)
            {
                Undo.RecordObject(projectileLauncher, "Spawn Change");
                projectileLauncher.spawnLocation = newOffset;
            }
        }
        Handles.BeginGUI();
        GUILayoutOption[] layoutOptions = {
            GUILayout.Width(64),
            GUILayout.Height(18)
        };

        if (GUILayout.Button("Launch", layoutOptions))
        {
            projectileLauncher.Launch(transform.forward);
        }
        Handles.EndGUI();
    }
}
