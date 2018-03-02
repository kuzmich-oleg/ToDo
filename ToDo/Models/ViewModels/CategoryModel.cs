namespace ToDo.Models.ViewModels
{
    public class CategoryModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public CategoryModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
