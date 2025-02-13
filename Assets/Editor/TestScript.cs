using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class TestScript
{
    [MenuItem("Testing/Load and Validate")]
    public static void LoadAndValidate()
    {
        const string objectsFolder = "Assets/Items";
        var filePaths = Directory.GetFiles(objectsFolder, "*.asset");
        var files = filePaths.Select(path => AssetDatabase.LoadAssetAtPath<PlayerItem>(path)).ToList();

        Debug.Log($"Files loaded:\n{string.Join("\n", files)}");
        
        // Validation
        foreach (var item in files)
        {
            if (item == null)
            {
                Debug.LogWarning("Loaded asset is null. Skipping validation.");
                continue;
            }

            ValidateItem(item);
        }
    }

    private static void ValidateItem(PlayerItem item)
    {
        // Validate Type
        string[] validTypes = { "Common", "Rare", "Epic", "Legendary" };
        if (!validTypes.Contains(item.Type))
        {
            Debug.LogError($"Invalid Type for {item.name}: {item.Type}. Must be one of {string.Join(", ", validTypes)}.");
        }
        
        // Validate Name format
        if (!item.name.StartsWith("PlayerItem_"))
        {
            Debug.LogError($"Invalid name for {item.name}: Name must start with 'PlayerItem_'.");
        }
        else
        {
            string expectedName = "PlayerItem_" + item.Type;
            if (item.name != expectedName)
            {
                Debug.LogError($"Invalid name for {item.name}: Name must be 'PlayerItem_{item.Type}' (got {item.name}).");
            }
        }

        // Validate Price
        if (item.Price < 1 || item.Price > 1000)
        {
            Debug.LogError($"Invalid Price for {item.name}: Price must be between 1 and 1000 (got {item.Price}).");
        }
    }
}
