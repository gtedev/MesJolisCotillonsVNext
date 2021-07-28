namespace MesJolisCotillons.Contracts.ViewModels.Product
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
        }

        public CategoryViewModel(int categoryId, string name, string displayName)
        {
            this.CategoryId = categoryId;
            this.Name = name;
            this.DisplayName = displayName;
        }

        public int CategoryId { get; private set; }

        public string Name { get; private set; }

        public string DisplayName { get; private set; }
    }
}
