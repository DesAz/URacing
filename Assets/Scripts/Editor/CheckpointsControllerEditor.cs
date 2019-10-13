#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using URacing.Checkpoints;

namespace URacing
{
    [CustomEditor(typeof(CheckpointsController))]
    public class CheckpointsControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Find checkpoints"))
            {
                var checkpointsController = (CheckpointsController)target;
                var checkpoints = checkpointsController.GetComponentsInChildren<Checkpoint>();

                checkpointsController.UpdateCheckpoints(checkpoints);

            }
        }
    }
}
#endif