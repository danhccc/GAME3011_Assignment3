using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDataBase
{
    public static Item[] Items { get; private set; }

    // Grab all assets inside this folder and sort it into array
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => Items = Resources.LoadAll<Item>("Items/");
}
