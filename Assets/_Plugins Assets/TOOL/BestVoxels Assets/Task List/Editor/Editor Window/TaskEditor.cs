using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace BestVoxels.TaskList
{
    public class TaskEditor : VisualElement // makes this class being treated like any other VisualElement in the UI Builder.
    {
        #region --Properties-- (Computed)
        public string Text { get => Label.text; private set => Label.text = value; }
        public bool IsCompleted { get => Toggle.value; private set => Toggle.value = value; }
        #endregion



        #region --Properties-- (Auto)
        public Label Label { get; private set; }
        public Toggle Toggle { get; private set; }
        #endregion



        #region --Constructors-- (PUBLIC)
        public TaskEditor(TaskData data) : this(data.Text, data.IsCompleted)
        {
        }

        public TaskEditor(string inputText) : this(inputText, false)
        {
        }

        public TaskEditor(string inputText, bool inputStatus)
        {
            // Another way is to use Field like in 'TaskListEditor.cs' but the field won't expose in this file since it inherits VisualElement not EditorWindow.
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Plugins Assets/TOOL/BestVoxels Assets/Task List/Editor/Editor Window/Task Editor.uxml");
            // Make grow property work on this VisualElement
            TemplateContainer uxmlImproved = visualTree.Instantiate();
            uxmlImproved.style.flexGrow = 1f;

            // Add UXML files to this VisualElement
            this.Add(uxmlImproved);


            // Find and get the first item that matches Type & Name. (if same named it will return the first, but there is another way to do if wants to get all)
            Toggle = this.Q<Toggle>();
            Label = this.Q<Label>();


            // Set up
            Text = inputText;
            IsCompleted = inputStatus;
        }
        #endregion
    }
}