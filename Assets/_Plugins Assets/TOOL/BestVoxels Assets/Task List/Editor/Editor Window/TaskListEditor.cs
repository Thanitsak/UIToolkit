using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements; // for accessing components in the UI Builder Library that are 'Generic' and available to in-game UI.
using UnityEditor.UIElements; // for accessing components in the UI Builder Library that are 'Editor' specific.

namespace BestVoxels.TaskList
{
    public class TaskListEditor : EditorWindow
    {
        #region --Fields-- (Inspector)
        [SerializeField] private VisualTreeAsset _uxmlFile; // this data is saved as something called Visual Tree Asset
        [SerializeField] private StyleSheet _ussFile; // this data is saved as something called Style Sheet
        #endregion



        #region --Fields-- (In Class)
        private VisualElement _container;
        private TaskListSO _taskListSO;

        private ObjectField _soObjectField;
        private Button _loadTasksButton;
        private TextField _taskText;
        private Button _addTaskButton;
        private ScrollView _taskListScrollView;
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

            // Add UXML & USS files to this Window 
            _container.Add(_uxmlFile.Instantiate()); // Another Way Read the UXML file 'VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Plugins Assets/TOOL/BestVoxels Assets/Task List/Editor/Editor Window/Task List Editor.uxml");'
            _container.styleSheets.Add(_ussFile); // Another Way Read the USS file 'StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Plugins Assets/TOOL/BestVoxels Assets/Task List/Editor/Editor Window/Task List Editor.uss");'


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

        private void SaveTask(string task)
        {
            _taskListSO.AddTask(task);

            EditorUtility.SetDirty(_taskListSO);
            AssetDatabase.SaveAssetIfDirty(_taskListSO);
            AssetDatabase.Refresh();
        }
        #endregion



        #region --Methods-- (Subscriber)
        private void LoadTasks()
        {
            _taskListSO = _soObjectField.value as TaskListSO;
            if (_taskListSO == null) return;

            _taskListScrollView.Clear();
            foreach (string each in _taskListSO.Tasks)
            {
                if (string.IsNullOrEmpty(each) || string.IsNullOrWhiteSpace(each)) continue;

                _taskListScrollView.Add(CreateTask(each));
            }
        }

        private void AddTask()
        {
            if (string.IsNullOrEmpty(_taskText.value) || string.IsNullOrWhiteSpace(_taskText.value)) return;

            _taskListScrollView.Add(CreateTask(_taskText.value));
            SaveTask(_taskText.value);

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