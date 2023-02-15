using System.Collections.Generic;
using UnityEngine;

namespace BestVoxels.TaskList
{
    [CreateAssetMenu(fileName = "Untitled Task List", menuName = "BestVoxels/Task List/New Task List", order = 0)]
    public class TaskListSO : ScriptableObject
    {
        #region --Properties-- (Inspector)
        // JUST TEMP AFTER, Remove [SerializeField]  &  Make it Public like a normal Auto Properties
        [field: SerializeField] public List<Task> Tasks { get; private set; } = new List<Task>();
        #endregion



        #region --Methods-- (Custom PUBLIC)
        public void ReplaceTasksWith(List<Task> tasksToAdd)
        {
            Tasks.Clear();
            Tasks = new List<Task>(tasksToAdd); // assign without reference
        }

        public void AddTask(Task taskToAdd)
        {
            Tasks.Add(taskToAdd);
        }
        #endregion
    }
}