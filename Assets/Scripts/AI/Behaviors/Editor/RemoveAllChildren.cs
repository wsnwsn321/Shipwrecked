using UnityEngine;
using UnityEditor;

public class RemoveAllChildren : MonoBehaviour {

    [MenuItem("GameObject/Remove All Children")]
    public static void RemoveChildren()
    {
        Transform[] selectedObjects = Selection.transforms;
        if (selectedObjects != null && selectedObjects.Length > 0)
        {
            if (EditorUtility.DisplayDialog("Remove All Children", "Do you really want to remove all children from the selected game objects?", "Remove", "Cancel"))
            {

                float numberOfChildren = 0;
                float counter = 0.0f;

                foreach (Transform currentObject in selectedObjects)
                {
                    numberOfChildren += currentObject.childCount;
                }

                foreach (Transform currentObject in selectedObjects)
                {
                    for (int i = 0; i < currentObject.childCount; i++)
                    {
                        DestroyImmediate(currentObject.GetChild(i).gameObject);
                        counter++;
                        EditorUtility.DisplayProgressBar("Removing Children", 
                            "Removing child " + (i + 1) + "/" + currentObject.childCount + " from " + currentObject.name + 
                            "\n  (" + (int)counter + "/" + (int)numberOfChildren + ")",
                            counter / numberOfChildren);
                    }
                }
                EditorUtility.ClearProgressBar();
            }

        }
    }
}
