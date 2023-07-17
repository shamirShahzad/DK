using UnityEngine;
using UnityEditor;
public class EditorUnpack : Editor
{
    [MenuItem("Shortcuts/Unpack")]
    static void Unpack()
    {
        Unpack(PrefabUnpackMode.OutermostRoot);
    }
    [MenuItem("Shortcuts/UnpackCompletely")]
    static void UnpackCompletely()
    {
        Unpack(PrefabUnpackMode.Completely);
    }
    static void Unpack(PrefabUnpackMode mode)
    {
        foreach (Transform transform in Selection.transforms)
        {
            RectTransform t = transform as RectTransform;

            if (PrefabUtility.GetPrefabAssetType(transform.gameObject) != PrefabAssetType.Regular)
            {
                continue;
            }
            else
            {
                PrefabUtility.UnpackPrefabInstance(transform.gameObject, mode, InteractionMode.UserAction);
            }
        }
    }
}