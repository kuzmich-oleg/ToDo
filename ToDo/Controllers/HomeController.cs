using Microsoft.AspNetCore.Mvc;
using ToDo.Models;
using ToDo.Models.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Controllers
{
    public class HomeController : Controller
    {
        private ToDoContext _context;
        private IndexViewModel _viewModel;

        public HomeController(ToDoContext context)
        {
            _context = context;            
            _viewModel = new IndexViewModel { Categories = _context.Categories, Statuses = _context.Statuses };

            SwapPriorities();
        }

        private void SwapPriorities()
        {
            int start = 1;
            foreach (Task task in _context.Tasks.OrderBy(t => t.Priority))
                task.Priority = start++;
            _context.SaveChanges();

            _viewModel.NextPriority = start;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.Select(c => new CategoryModel(c.CategoryId, c.Name)).ToList();
            categories.Insert(0, new CategoryModel(0, "All"));

            return View(categories);
        }

        public IActionResult GetList(int categoryId)
        {
            var tasks = _context.Tasks.Include(t => t.Category)
                .Include(t => t.Status)
                .OrderBy(t => t.Priority);

            if (categoryId > 0)
                _viewModel.Tasks = tasks.Where(t => t.CategoryId == categoryId);
            else
                _viewModel.Tasks = tasks;

            return PartialView("TaskList", _viewModel);
        }

        [HttpPut]
        public IActionResult Add(string name, int priority, int status, int category, int selectedCategory)
        {
            _viewModel.NextPriority++;

            _context.Tasks.Add(new Task { Name = name, Priority = priority, StatusId = status, CategoryId = category });
            _context.SaveChanges();

            return RedirectToAction("GetList", new { categoryId = selectedCategory });
        }

        [HttpDelete]
        public IActionResult Delete(int taskId, int selectedCategory)
        {
            Task task = _context.Tasks.First(t => t.TaskId == taskId);

            if (task.Priority != _viewModel.NextPriority - 1)
                SwapPriorities();

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return RedirectToAction("GetList", new { categoryId = selectedCategory });
        }

        [HttpPost]
        public IActionResult ChangeStatus(int taskId, int statusId, int selectedCategory)
        {
            Task task = _context.Tasks.First(t => t.TaskId == taskId);

            task.StatusId = statusId;
            _context.Tasks.Update(task);
            _context.SaveChanges();

            return RedirectToAction("GetList", new { categoryId = selectedCategory });
        }

        [HttpPost]
        public IActionResult ChangePriority(int taskId, int priority, string direction, int selectedCategory)
        {
            Task taskToMove = _context.Tasks.First(t => t.TaskId == taskId);
            int priorityDirection = priority + ((direction == "up") ? -1 : 1);
            Task taskNext = _context.Tasks.First(t => t.Priority == priorityDirection);

            taskNext.Priority = priority;
            taskToMove.Priority = priorityDirection;

            _context.UpdateRange(taskToMove, taskNext);
            _context.SaveChanges();

            return RedirectToAction("GetList", new { categoryId = selectedCategory });
        }
    }
}
