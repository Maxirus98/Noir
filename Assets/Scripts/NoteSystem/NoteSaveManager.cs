using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Note
{
    public int Id;
    public string SpritePath;
    public string Description;
    public string ExtraInformation;
    public bool CanHelpToSolve;

    public InvestigationSlot InvestigationSlotType;
}

/// <summary>
/// Note wrapper that contains a public field of type List to be able to serialize a list without installing additionnal packages.
/// Serializable data needs to stay public for the json Unity package
/// </summary>
[System.Serializable]
public class NotesListWrapper
{
    public List<Note> notes;

    public NotesListWrapper(List<Note> notes) { 
        this.notes = notes;    
    }
}

public static class NoteSaveManager
{
    private static string savePath;

    public static void SaveNote(IndiceData indiceData)
    {
        var noteListWrapper = GetSavedNotes();
        var notes = noteListWrapper.notes;

        // Guard clause if the id already exists in the list.
        if (notes.Exists(n => n.Id == indiceData.Id)) return;

        notes.Add(new Note
        {
            Id = indiceData.Id,
            SpritePath = AssetDatabase.GetAssetPath(indiceData.ItemSprite),
            Description = indiceData.Description,
            ExtraInformation = indiceData.ExtraInformation,
            CanHelpToSolve = indiceData.CanHelpToSolve,
            InvestigationSlotType = indiceData.InvestigationSlotType
        });

        // Reassigner la liste de note au wrapper.
        noteListWrapper.notes = notes;

        // Building savePath
        savePath = Path.Combine(Application.persistentDataPath, "notes.json");
        string jsonNotes = JsonUtility.ToJson(noteListWrapper, true);

        Debug.Log(jsonNotes);

        // Write the JSON string to a file
        File.WriteAllText(savePath, jsonNotes);
    }

    public static NotesListWrapper GetSavedNotes()
    {
        string json = "";
        savePath = Path.Combine(Application.persistentDataPath, "notes.json");
        try
        {
            // Read the JSON string from the file
            json = File.ReadAllText(savePath);
        }
        catch (IOException) {
            // Create new file if not found
            json = "{}";
            File.WriteAllText(savePath, json);
        }
        

        // Convert the JSON string back to a PlayerDataManager object
        return JsonUtility.FromJson<NotesListWrapper>(json);
    }
}
