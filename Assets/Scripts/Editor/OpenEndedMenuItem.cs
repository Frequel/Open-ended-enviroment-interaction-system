using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OpenEndedMenuItem : Editor
{
    // Add a menu item to create custom GameObjects.
    // Priority 1 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/Open Ended Objects/Default(Empty)", false)]
    static void CreateInitializerGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Open Ended Object (Default)");
        go.AddComponent<Initializer>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/Open Ended Objects/Game Manager", false)]
    static void CreateGameManagerGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("GameManager");
        go.AddComponent<GameManager>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/Open Ended Objects/Shaking Manager", false)]
    static void CreateShakingManagerGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("ShakingManager");
        go.AddComponent<shakingManager>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}