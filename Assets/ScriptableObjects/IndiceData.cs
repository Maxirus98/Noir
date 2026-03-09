using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Indice Data", menuName = "Indice Data")]
public class IndiceData : ScriptableObject
{
    private enum InvestigationSlot
    {
        None,
        Weapon,
        Victime,
        Culprit,
        TimeOfDeath,
        ReasonToCommit
    }

    public Sprite ItemSprite;
    public string Description;

    // Custom Serialization
    [TextArea(10, 20)]
    public string ExtraInformation;
    public bool CanHelpToSolve;
    private InvestigationSlot investigationSlotType;

#if UNITY_EDITOR
    [CustomEditor(typeof(IndiceData))]
    public class IndiceEditor: Editor
    {
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            IndiceData indice = (IndiceData) target;
            EditorGUILayout.Space();

            if (indice.CanHelpToSolve)
            {
                indice.investigationSlotType = (InvestigationSlot)EditorGUILayout.EnumPopup("Investigation Slot: ", indice.investigationSlotType);
            }
            else
            {
                indice.investigationSlotType = InvestigationSlot.None;
            }

            // Reload the inspector if changed
            if (GUI.changed)
            {
                EditorUtility.SetDirty(indice);
            }
        }
    }
#endif
}
