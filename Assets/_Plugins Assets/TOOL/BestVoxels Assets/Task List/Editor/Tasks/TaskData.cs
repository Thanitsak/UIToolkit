using UnityEngine;

namespace BestVoxels.TaskList
{
    [System.Serializable]
    public class TaskData
    {
        #region --Properties-- (Inspector)
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public bool IsCompleted { get; private set; }
        #endregion



        #region --Constructors-- (PUBLIC)
        public TaskData(string text, bool isCompleted)
        {
            Text = text;
            IsCompleted = isCompleted;
        }
        #endregion
    }
}