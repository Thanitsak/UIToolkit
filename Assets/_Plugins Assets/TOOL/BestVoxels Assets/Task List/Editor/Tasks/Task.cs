namespace BestVoxels.TaskList
{
    [System.Serializable]
    public class Task
    {
        #region --Fields-- (Inspector)
        public string name;
        public bool isCompleted;
        #endregion



        #region --Constructors-- (PUBLIC)
        public Task(string name)
        {
            this.name = name;
            this.isCompleted = false;
        }

        public Task(string name, bool status)
        {
            this.name = name;
            this.isCompleted = status;
        }
        #endregion
    }
}