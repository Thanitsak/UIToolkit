using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements; // for accessing components in the UI Builder Library that are 'Generic' and available to in-game UI.
using UnityEditor.UIElements; // for accessing components in the UI Builder Library that are 'Editor' specific.

namespace BestVoxels.TaskList.EditorWindow
{
    public class TaskListEditor : UnityEditor.EditorWindow
    {
        #region --Fields-- (In Class)
        private VisualElement _container;
        #endregion



        #region --Fields-- (Constant)
        private const string _path = "Assets/_Plugins Assets/TOOL/BestVoxels Assets/Task List/Editor/Editor Window/";
        #endregion



        #region --Methods-- (Annotation)
        // check 'About Create Editor as Window' for more details on creating Window
        [MenuItem("BestVoxels/Task List")]
        private static void ShowEditorWindow()
        {
            TaskListEditor window = GetWindow<TaskListEditor>(); // Show Window, its type is this class itself which should contain 'CreateGUI()' so it can populate GUI stuff on it.
            window.titleContent = new GUIContent("Task List Window"); // Set Window name
            window.minSize = new Vector2(500, 500); // Set Window Minimum Size
        }
        #endregion



        #region --Methods-- (Built In)
        private void CreateGUI()
        {
            Debug.Log("TaskListEditor.cs");
            _container = rootVisualElement; // the root of this Window Editor

            // Read the UXML file (its data is saved as something called Visual Tree Asset)
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_path + "Task List Editor.uxml");
            // Add UXML to this Window 
            _container.Add(visualTree.Instantiate());

            // Read the USS file (its data is saved as something called Style Sheet)
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(_path + "Task List Editor.uss");
            // Add USS to this Window
            _container.styleSheets.Add(styleSheet);
        }
        #endregion
    }
}