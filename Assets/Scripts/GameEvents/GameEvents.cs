using System;

public static class GameEvents
{
    public static Action<string> OnPuzzle2Completed; // puzzle 2 
    public static Action<IndiceData> OnIndiceFound; // dialogue triggers after closing the noteuipopup message
    public static Action<bool> OnInspect; // When Loupe is on 
    public static Action<bool> OnToggleDetectiveBoard; // When DetectiveBoard is on 
    public static Action<bool> OnDialogueStart; // When Dialogue is on 

}