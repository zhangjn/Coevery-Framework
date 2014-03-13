using System.ComponentModel.DataAnnotations;

namespace Coevery.DeveloperTools.Projections.ViewModels {
    public class AdminCreateViewModel {
        [Required, StringLength(1024)]
        public string Name { get; set; }

    }
}