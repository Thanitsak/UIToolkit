using System.Collections.Generic;
using UnityEngine;

namespace BestVoxels.TaskList.Tasks
{
    [CreateAssetMenu(fileName = "Untitled Task List", menuName = "BestVoxels/Create Task List", order = 100)]
    public class TaskListSO : ScriptableObject
    {
        #region --Properties-- (Inspector)
        // JUST TEMP AFTER, Remove [SerializeField]  &  Make it Public like a normal Auto Properties
        [field: SerializeField] public List<string> Tasks { get; private set; } = new List<string>();
        #endregion



        #region --Methods-- (Custom PUBLIC)
        public bool Save(List<string> savedTasks)
        {
            Tasks.Clear();
            Tasks = savedTasks;

            return true;
        }
        #endregion
    }
}