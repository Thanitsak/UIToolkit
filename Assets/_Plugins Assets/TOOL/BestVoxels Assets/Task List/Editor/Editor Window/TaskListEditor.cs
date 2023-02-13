using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements; // for accessing components in the UI Builder Library that are 'Generic' and available to in-game UI.
using UnityEditor.UIElements; // for accessing components in the UI Builder Library that are 'Editor' specific.

namespace BestVoxels.TaskList
{
    public class TaskListEditor : EditorWindow
    {
        #region --Fields-- (In Class)
        private VisualElement _container;

        private ObjectField _soObjectField;
        private Button _loadTasksButton;
        private TextField _taskText;
        private Button _addTaskButton;
        private ScrollView _taskListScrollView;
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
            _container = rootVisualElement; // the root of this Window Editor

            // Read the UXML file (its data is saved as something called Visual Tree Asset)
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_path + "Task List Editor.uxml");
            // Add UXML to this Window 
            _container.Add(visualTree.Instantiate());

            // Read the USS file (its data is saved as something called Style Sheet)
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(_path + "Task List Editor.uss");
            // Add USS to this Window
            _container.styleSheets.Add(styleSheet);


            // Find and get the first item that matches Type & Name. (if same named it will return the first, but there is another way to do if wants to get all)
            _soObjectField = _container.Q<ObjectField>("SoObjectField");
            _loadTasksButton = _container.Q<Button>("LoadTasksButton");
            _taskText = _container.Q<TextField>("TaskText");
            _addTaskButton = _container.Q<Button>("AddTaskButton");
            _taskListScrollView = _container.Q<ScrollView>("TaskListScrollView");
            // *Important* In large project, try and catch errors if these references can't be found before it become a big problem.


            // Specify the type
            _soObjectField.objectType = typeof(TaskListSO); // IF we want to pass in any ScriptableObject we can use ScriptableObject as a type.


            // Binding Button
            _loadTasksButton.clicked += LoadTasks;

            _taskText.RegisterCallback<KeyDownEvent>(AddTask); // When User hit 'Enter Key'
            _addTaskButton.clicked += AddTask;
        }
        #endregion



        #region --Methods-- (Custom PRIVATE)
        private Toggle CreateTask(string text)
        {
            Toggle task = new Toggle();
            task.text = text;

            return task;
        }
        #endregion



        #region --Methods-- (Subscriber)
        private void LoadTasks()
        {
            TaskListSO taskListSO = _soObjectField.value as TaskListSO;
            if (taskListSO == null) return;

            _taskListScrollView.Clear();
            foreach (string each in taskListSO.Tasks)
            {
                if (string.IsNullOrEmpty(each) || string.IsNullOrWhiteSpace(each)) continue;

                _taskListScrollView.Add(CreateTask(each));
            }
        }

        private void AddTask()
        {
            if (string.IsNullOrEmpty(_taskText.value) || string.IsNullOrWhiteSpace(_taskText.value)) return;

            _taskListScrollView.Add(CreateTask(_taskText.text));

            _taskText.value = string.Empty;
            _taskText.Focus(); // Keep Cursor stay in the Text Field Box, even when we hit enter or click add button.
        }

        private void AddTask(KeyDownEvent e)
        {
            if (e.keyCode == KeyCode.Return)
                AddTask();
        }
        #endregion
    }
}