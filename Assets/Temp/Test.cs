using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements; // for accessing components in the UI Builder Library that are 'Generic' and available to in-game UI.
using UnityEditor.UIElements; // for accessing components in the UI Builder Library that are 'Editor' specific.

public class Test : EditorWindow
{
    #region --Fields-- (In Class)
    private VisualElement _container;
    #endregion



    #region --Methods-- (Annotation)
    // check 'About Create Editor as Window' for more details on creating Window
    [MenuItem("Testing/Test Window")]
    private static void ShowTestWindow()
    {
        Test window = GetWindow<Test>(); // Show Window, its type is this class itself
        window.titleContent = new GUIContent("Test Floating Window"); // Set Window name
        window.minSize = new Vector2(500, 500); // Set Window Minimum Size
    }
    #endregion



    #region --Methods-- (Built In)
    private void CreateGUI()
    {
        _container = rootVisualElement; // the root of this Window Editor

        // Read the UXML file (its data is saved as something called Visual Tree Asset)
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Temp/Test.uxml");
        // Add UXML to this Window 
        _container.Add(visualTree.Instantiate());

        // Read the USS file (its data is saved as something called Style Sheet)
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Temp/uss test.uss");
        // Add USS to this Window
        _container.styleSheets.Add(styleSheet);
    }
    #endregion
}