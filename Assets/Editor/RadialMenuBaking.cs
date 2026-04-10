using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenericRadialMenu))]
public class RadialMenuBaking : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenericRadialMenu script = (GenericRadialMenu)target;

        if (GUILayout.Button("Bake Positions"))
        {
            script.BakePositions();
        }
    }
}
