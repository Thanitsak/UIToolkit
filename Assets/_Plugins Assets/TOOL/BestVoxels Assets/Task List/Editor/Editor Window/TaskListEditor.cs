using System.Collections.Generic;
using System.Linq;
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
        private ToolbarSearchField _searchBox;
        private TextField _taskText;
        private Button _addTaskButton;
        private ScrollView _scrollViewTasks;
        private Button _clearCompletedButton;
        private Button _saveTasksButton;
        private ProgressBar _progressBar;
        private Label _notificationText;
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
            _searchBox = _container.Q<ToolbarSearchField>("SearchBox");
            _taskText = _container.Q<TextField>("TaskText");
            _addTaskButton = _container.Q<Button>("AddTaskButton");
            _scrollViewTasks = _container.Q<ScrollView>("TaskListScrollView");
            _clearCompletedButton = _container.Q<Button>("ClearCompletedButton");
            _saveTasksButton = _container.Q<Button>("SaveTasksButton");
            _progressBar = _container.Q<ProgressBar>("TaskProgressBar");
            _notificationText = _container.Q<Label>("NotificationText");
            // *Important* In large project, try and catch errors if these references can't be found before it become a big problem.


            // Specify the type
            _soObjectField.objectType = typeof(TaskListSO); // IF we want to pass in any ScriptableObject we can use ScriptableObject as a type.


            // Binding Button
            _loadTasksButton.clicked += () => { LoadTasks(); UpdateProgressBar(false); };

            _searchBox.RegisterValueChangedCallback(SearchForText);

            _taskText.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Return) // When User hit 'Enter Key'
                {
                    AddTask();
                    UpdateProgressBar(false);
                    SaveTask(false);
                }
            });
            _addTaskButton.clicked += () => { AddTask(); UpdateProgressBar(false); SaveTask(false); };

            _clearCompletedButton.clicked += () => { ClearCompletedTask(); UpdateProgressBar(false); SaveTask(false); };

            _saveTasksButton.clicked += () => SaveTask();

            // Setup
            UpdateProgressBar(false);
            _notificationText.text = "Please load a task list to continue.";
        }
        #endregion



        #region --Methods-- (Custom PRIVATE)
        private void UpdateScrollView(List<TaskData> taskData)
        {
            _scrollViewTasks.Clear();

            taskData = new List<TaskData>(taskData); // Fix 'Collection was modified' error, because when passing in _currentTasks, it will get modified when calling AddToScrollView()
            foreach (TaskData data in taskData)
            {
                if (string.IsNullOrEmpty(data.Text) || string.IsNullOrWhiteSpace(data.Text)) continue;
                
                AddToScrollView(new TaskEditor(data));
            }
        }

        private void AddToScrollView(TaskEditor task)
        {
            if (task == null)
            {
                Debug.LogWarning("the Task that passed in is null, and can't be showed on the ScrollView List");
                return;
            }

            task.Toggle.RegisterValueChangedCallback(e => UpdateProgressBar());

            _scrollViewTasks.Add(task);
        }
        #endregion



        #region --Methods-- (Subscriber)
        private void LoadTasks()
        {
            _taskListSO = _soObjectField.value as TaskListSO;
            if (_taskListSO == null)
            {
                _notificationText.text = $"Failed to load task list.";
                return;
            }

            UpdateScrollView(_taskListSO.TaskData);

            _notificationText.text = $"{_uxmlFile.name} successfully loaded.";
        }

        private void AddTask()
        {
            if (string.IsNullOrEmpty(_taskText.value) || string.IsNullOrWhiteSpace(_taskText.value))
            {
                _notificationText.text = "Can't add empty task!";
                return;
            }

            AddToScrollView(new TaskEditor(_taskText.value));

            _taskText.value = string.Empty;
            _taskText.Focus(); // Keep Cursor stay in the Text Field Box, even when we hit enter or click add button.

            _notificationText.text = "Task added successfully.";
        }

        private void SaveTask(bool showStatus=true)
        {
            // TODO : Maybe as a seperate method
            List<TaskData> taskData = new List<TaskData>();
            foreach (TaskEditor taskEditor in _scrollViewTasks.Children())
            {
                taskData.Add(new TaskData(taskEditor.Text, taskEditor.IsCompleted));
            }

            _taskListSO.ReplaceTasksWith(taskData);

            EditorUtility.SetDirty(_taskListSO);
            AssetDatabase.SaveAssetIfDirty(_taskListSO);
            AssetDatabase.Refresh();

            if (showStatus)
                _notificationText.text = "Save successful!";
        }

        private void ClearCompletedTask()
        {
            // TODO : Maybe as a seperate method
            List<TaskData> taskData = new List<TaskData>();
            foreach (TaskEditor taskEditor in _scrollViewTasks.Children())
            {
                if (taskEditor.IsCompleted) continue;

                taskData.Add(new TaskData(taskEditor.Text, taskEditor.IsCompleted));
            }

            UpdateScrollView(taskData);

            _notificationText.text = "Clear completed tasks successfully!";
        }

        private void UpdateProgressBar(bool showStatus=true)
        {
            float progressValue = 0f;
            if (_scrollViewTasks.childCount > 0)
            {
                // TODO : Maybe as a seperate method
                progressValue = (float)_scrollViewTasks.Children().Count((VisualElement e) => (e as TaskEditor).IsCompleted) / (float)_scrollViewTasks.childCount;
            }

            _progressBar.value = progressValue;
            _progressBar.title = $"{(progressValue * 100f):N0}%";

            if (showStatus)
                _notificationText.text = "Progress updated. Don't forget to saved!";
        }

        private void SearchForText(ChangeEvent<string> changeEvent)
        {
            if (changeEvent == null) return;
            string inputText = changeEvent.newValue.ToLower();

            int foundCounter = 0;
            _scrollViewTasks.Children().ToList().ForEach((VisualElement e) =>
            {
                TaskEditor t = e as TaskEditor;

                if (t.Text.ToLower().Contains(inputText) && !string.IsNullOrEmpty(inputText))
                {
                    t.Label.AddToClassList("highlight");
                    foundCounter++;
                }
                else
                    t.Label.RemoveFromClassList("highlight");
            });
            _notificationText.text = $"{foundCounter} tasks found.";
        }
        #endregion
    }
}