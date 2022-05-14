using MultiLanguages.Views;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class FruitModels
    {
        [Display(Name = "水果名稱", ResourceType = typeof(FruitML))]
        public string Name { get; set; }

        [Display(Name = "數量", ResourceType = typeof(FruitML))]
        public int Count { get; set; }
    }
}
