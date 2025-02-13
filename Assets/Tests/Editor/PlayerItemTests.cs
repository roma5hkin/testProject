using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class PlayerItemTests
{
    private const string AssetsFolder = "Assets/Items"; // Папка с ассетами

    [Test]
    public void PlayerItems_Should_Have_Valid_Properties()
    {
        PlayerItem[] items = GetPlayerItems();
        List<string> errors = new List<string>();

        foreach (var item in items)
        {
            string assetPath = AssetDatabase.GetAssetPath(item);

            // Проверка типа
            string[] validTypes = { "Common", "Rare", "Epic", "Legendary" };
            if (!validTypes.Contains(item.Type))
                errors.Add($"[Asset: {assetPath}] Invalid Type: {item.Type}");

            // Проверка имени
            string expectedName = "PlayerItem_" + item.Type;
            if (item.name != expectedName)
                errors.Add($"[Asset: {assetPath}] Invalid name: Expected 'PlayerItem_{item.Type}', but got '{item.name}'");

            // Проверка цены
            if (item.Price < 1 || item.Price > 1000)
                errors.Add($"[Asset: {assetPath}] Invalid Price: Expected between 1 and 1000, but got {item.Price}");
        }

        // Если есть ошибки — кидаем Assert.Fail()
        if (errors.Count > 0)
        {
            Assert.Fail(string.Join("\n", errors));
        }
    }

    private PlayerItem[] GetPlayerItems()
    {
        string[] guids = AssetDatabase.FindAssets("t:PlayerItem", new[] { AssetsFolder });
        return guids
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(path => AssetDatabase.LoadAssetAtPath<PlayerItem>(path))
            .Where(item => item != null)
            .ToArray();
    }
}
