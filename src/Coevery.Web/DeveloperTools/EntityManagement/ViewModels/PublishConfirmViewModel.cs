using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Coevery.ContentManagement.ViewModels;
using Coevery.Environment.Extensions.Models;

namespace Coevery.DeveloperTools.EntityManagement.ViewModels {
    public class PublishConfirmViewModel {
        public PublishConfirmViewModel()
        {
            Modules = new List<ExtensionDescriptor>();
        }

        /// <summary>
        /// The technical name of the field
        /// </summary>
        [Required]
        public string ModuleId { get; set; }

        /// <summary>
        /// The display name of the field
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        public IEnumerable<ExtensionDescriptor> Modules { get; set; }
    }
}