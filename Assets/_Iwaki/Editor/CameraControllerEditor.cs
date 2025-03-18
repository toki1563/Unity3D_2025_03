using UnityEditor;
using UnityEngine;

/// <summary>
/// カメラターゲットを一括で指定する用のエディタ拡張
/// </summary>
[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("カメラのPriorityやアクティブ状態はこのクラスによって上書きされます", MessageType.None);

        EditorGUILayout.Space(10);

        base.OnInspectorGUI();

        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("プレイヤーをここに投下してカメラのターゲットを一括設定");

        var obj = EditorGUILayout.ObjectField(null, typeof(Transform), true);
        if (obj is Transform transform)
        {
            var cameraController = target as CameraController;

            cameraController.SetTarget(transform);
        }
    }
}
