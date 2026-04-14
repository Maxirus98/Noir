using System;

public static class GameEvents
{
    public static Action<string> OnPuzzle2Completed; // puzzle 2 
    public static Action<IndiceData> OnIndiceFound; // dialogue triggers after closing the noteuipopup message
}