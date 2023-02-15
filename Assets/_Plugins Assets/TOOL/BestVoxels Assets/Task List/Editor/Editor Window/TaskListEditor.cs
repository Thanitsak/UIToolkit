using System.Collections.Generic;
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
        private List<Task> _scrollViewTasks = new List<Task>();

        private VisualElement _container;
        private TaskListSO _taskListSO;

        private ObjectField _soObjectField;
        private Button _loadTasksButton;
        private TextField _taskText;
        private Button _addTaskButton;
        private ScrollView _taskListScrollView;
        private Button _saveTasksButton;
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

            // Make grow property work on Editor Window
            TemplateContainer uxmlImproved = _uxmlFile.Instantiate();
            uxmlImproved.style.flexGrow = 1f;

            // Add UXML & USS files to this Window
            _container.Add(uxmlImproved); // Another Way Read the UXML file 'VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Plugins Assets/TOOL/BestVoxels Assets/Task List/Editor/Editor Window/Task List Editor.uxml");'
            _container.styleSheets.Add(_ussFile); // Another Way Read the USS file 'StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Plugins Assets/TOOL/BestVoxels Assets/Task List/Editor/Editor Window/Task List Editor.uss");'


            // Find and get the first item that matches Type & Name. (if same named it will return the first, but there is another way to do if wants to get all)
            _soObjectField = _container.Q<ObjectField>("SoObjectField");
            _loadTasksButton = _container.Q<Button>("LoadTasksButton");
            _taskText = _container.Q<TextField>("TaskText");
            _addTaskButton = _container.Q<Button>("AddTaskButton");
            _taskListScrollView = _container.Q<ScrollView>("TaskListScrollView");
            _saveTasksButton = _container.Q<Button>("SaveTasksButton");
            // *Important* In large project, try and catch errors if these references can't be found before it become a big problem.


            // Specify the type
            _soObjectField.objectType = typeof(TaskListSO); // IF we want to pass in any ScriptableObject we can use ScriptableObject as a type.


            // Binding Button
            _loadTasksButton.clicked += LoadTasks;

            _taskText.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Return) // When User hit 'Enter Key'
                {
                    AddTask();
                    SaveTask();
                }
            });
            _addTaskButton.clicked += () => { AddTask(); SaveTask(); };

            _saveTasksButton.clicked += SaveTask;
        }
        #endregion



        #region --Methods-- (Custom PRIVATE)
        private void AddToScrollView(Task task)
        {
            Toggle toggle = new Toggle();
            toggle.text = task.name;
            _taskListScrollView.Add(toggle);

            _scrollViewTasks.Add(task);
        }

        private void AddToScrollView(string taskName)
        {
            Task task = new Task(taskName);

            AddToScrollView(task);
        }
        #endregion



        #region --Methods-- (Subscriber)
        private void LoadTasks()
        {
            _taskListSO = _soObjectField.value as TaskListSO;
            if (_taskListSO == null) return;

            _scrollViewTasks.Clear(); // Clear Old Data
            _taskListScrollView.Clear();
            foreach (Task each in _taskListSO.Tasks)
            {
                if (string.IsNullOrEmpty(each.name) || string.IsNullOrWhiteSpace(each.name)) continue;

                AddToScrollView(each);
            }
        }

        private void AddTask()
        {
            if (string.IsNullOrEmpty(_taskText.value) || string.IsNullOrWhiteSpace(_taskText.value)) return;

            AddToScrollView(_taskText.value);

            _taskText.value = string.Empty;
            _taskText.Focus(); // Keep Cursor stay in the Text Field Box, even when we hit enter or click add button.
        }

        private void SaveTask()
        {
            for (int i = 0; i < _taskListScrollView.childCount; i++) //foreach (Toggle each in _taskListScrollView.Children())
            {
                Toggle eachToggle = _taskListScrollView.ElementAt(i) as Toggle;
                if (eachToggle == null) continue;

                _scrollViewTasks[i].isCompleted = eachToggle.value;
            }

            _taskListSO.ReplaceTasksWith(_scrollViewTasks);

            EditorUtility.SetDirty(_taskListSO);
            AssetDatabase.SaveAssetIfDirty(_taskListSO);
            AssetDatabase.Refresh();
        }
        #endregion
    }
}