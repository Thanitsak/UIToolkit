using System.Collections.Generic;
using UnityEngine;

namespace BestVoxels.TaskList
{
    [CreateAssetMenu(fileName = "Untitled Task List", menuName = "BestVoxels/Task List/New Task List", order = 0)]
    public class TaskListSO : ScriptableObject
    {
        #region --Properties-- (Inspector)
        [field: SerializeField] public List<TaskData> TaskData { get; private set; } = new List<TaskData>();
        #endregion



        #region --Methods-- (Custom PUBLIC)
        public void ReplaceTasksWith(List<TaskData> dataToAdd)
        {
            TaskData.Clear();
            TaskData = new List<TaskData>(dataToAdd); // assign without reference
        }

        public void AddTask(TaskData dataToAdd)
        {
            TaskData.Add(dataToAdd);
        }
        #endregion
    }
}