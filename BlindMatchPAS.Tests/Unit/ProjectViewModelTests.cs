using BlindMatchPAS.Web.ViewModels;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BlindMatchPAS.Tests.Unit
{
    public class ProjectViewModelTests
    {
        [Fact]
        public void ProjectViewModel_IsInvalid_WhenRequiredFieldsMissing()
        {
            var model = new ProjectViewModel();
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.False(isValid);
            Assert.NotEmpty(results);
        }
    }
}