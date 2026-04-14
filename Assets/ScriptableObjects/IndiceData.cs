using UnityEngine;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public enum InvestigationSlot
{
    None,
    Weapon,
    Victime,
    Culprit,
    TimeOfDeath,
    ReasonToCommit
}

/// <summary>
/// Scriptable Object that contains the datas of a clue
/// </summary>
[CreateAssetMenu(fileName = "New Indice Data", menuName = "Indice Data")]
public class IndiceData : ScriptableObject
{
    /// <summary>
    /// A matching Id with the saved notes to prevent players to scan a clue that was already scanned.
    /// </summary>
    /// 
    [Header("!!!!!!!!!!Make sure you DON'T repeat an Id amongst Indice Datas!!!!!!!!!!")]
    public int Id;
    public Sprite ItemSprite;
    public string Description;

    [Header("Linked Dialogue")]
    [SerializeField] private DialogueData dialogueOnFound;
    public DialogueData DialogueOnFound => dialogueOnFound;

    // Custom Serialization
    [TextArea(10, 20)]
    public string ExtraInformation;
    public bool CanHelpToSolve;
    [HideInInspector]
    public InvestigationSlot InvestigationSlotType;

    private void OnValidate()
    {
        if (!IsIdUnique()) throw new UnityException($"This id already exists in the database! Change the ID of the {this.name}");
    }

    /// <summary>
    /// Method to check if an IndiceData ID is unique in the assets folder.
    /// </summary>
    /// <returns></returns>
    public bool IsIdUnique()
    {
        string[] assetPaths = AssetDatabase.FindAssets($"t:{typeof(IndiceData).Name}");
        foreach (string guid in assetPaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var data = AssetDatabase.LoadAssetAtPath<IndiceData>(path);

            // Skip checking against self
            if (data != null && data != this && data.Id == this.Id)
            {
                return false;
            }
        }
        return true;
    }

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
                indice.InvestigationSlotType = (InvestigationSlot)EditorGUILayout.EnumPopup("Investigation Slot: ", indice.InvestigationSlotType);
            }
            else
            {
                indice.InvestigationSlotType = InvestigationSlot.None;
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
