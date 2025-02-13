#if UNITY_EDITOR
using UnityEditor;
#endif
using NUnit.Framework;
using UnityEngine;
using System.Linq;

public class PlayerItemTests
{
    private static readonly string[] ValidTypes = { "Common", "Rare", "Epic", "Legendary" };

    private PlayerItem[] LoadAllPlayerItems()
    {
#if UNITY_EDITOR
        return AssetDatabase.FindAssets("t:PlayerItem")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<PlayerItem>)
            .ToArray();
#else
        return Resources.LoadAll<PlayerItem>("");
#endif
    }

    [Test]
    public void PlayerItems_ValidType()
    {
        var items = LoadAllPlayerItems();
        Assert.Greater(items.Length, 0, "No PlayerItems found!");

        foreach (var item in items)
        {
            Assert.Contains(item.Type, ValidTypes.ToList(), $"Invalid Type: {item.Type} in {item.name}");
        }
    }

    [Test]
    public void PlayerItems_ValidNameFormat()
    {
        var items = LoadAllPlayerItems();
        foreach (var item in items)
        {
            Assert.AreEqual($"PlayerItem_{item.Type}", item.name, $"Invalid Name: {item.name}, expected PlayerItem_{item.Type}");
        }
    }

    [Test]
    public void PlayerItems_ValidPriceRange()
    {
        var items = LoadAllPlayerItems();
        foreach (var item in items)
        {
            Assert.IsTrue(item.Price >= 1 && item.Price <= 1000, $"Invalid Price: {item.Price} in {item.name}");
        }
    }
}
