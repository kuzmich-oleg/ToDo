using System.Collections.Generic;
using System.Linq;

namespace ToDo.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
        public int NextPriority
        {
            get { return _nextPriority; }
            set { _nextPriority = value; }
        }

        private static int _nextPriority = 0;
    }
}
