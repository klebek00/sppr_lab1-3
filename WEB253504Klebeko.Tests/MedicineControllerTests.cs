using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB253504Klebeko.UI.Services.CategoryService;
using WEB253504Klebeko.UI.Services.MedicineService;
using WEB253504Klebeko.UI.Controllers;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;



namespace WEB253504Klebeko.Tests
{
    public class MedicineControllerTests
    {
        private readonly ICategoryService _categoryService;
        private readonly IMedicineService _medicineService;
        private readonly MedicineController _medicineController;

        public MedicineControllerTests() 
        {
            _categoryService = Substitute.For<ICategoryService>();
            _medicineService = Substitute.For<IMedicineService>();
            _medicineController = new MedicineController(_medicineService, _categoryService);
        }

        [Fact]
        public async Task Index_Returns404_WhenCategoryListFails()
        {
            // Arrange
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = false,
                ErrorMessage = "Error 404"
            }));

            _medicineService.GetMedicListAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(new ResponseData<ListModel<Medicines>>
            {
                Successfull = true,
                Data = new ListModel<Medicines> { Items = new List<Medicines>() }
            }));

            // Act
            var result = await _medicineController.Index(null); 

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); 
            Assert.Equal("Error 404", notFoundResult.Value); 
        }

        [Fact]
        public async Task Index_Returns404_WhenMedicineListFails()
        {
            // Arrange
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = true,
                Data = new List<Category> { new Category { Name = "Category1" } }
            }));

            _medicineService.GetMedicListAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(new ResponseData<ListModel<Medicines>>
            {
                Successfull = false,
                ErrorMessage = "Error 404"
            }));

            // Act
            var result = await _medicineController.Index(null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error 404", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_PopulatesViewDataWithCategories_WhenSuccessful()
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Headers["X-Requested-With"].Returns((Microsoft.Extensions.Primitives.StringValues)"");

            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Returns(request);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var tempDataDictionary = new TempDataDictionary(httpContext, tempDataProvider);
            _medicineController.TempData = tempDataDictionary;

            _medicineController.ControllerContext = controllerContext;

            var categories = new List<Category> { new Category { Name = "Category1" } };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = true,
                Data = categories
            }));
            _medicineService.GetMedicListAsync(Arg.Any<string>(), Arg.Any<int>())
                .Returns(Task.FromResult(new ResponseData<ListModel<Medicines>>
                {
                    Successfull = true,
                    Data = new ListModel<Medicines>
                    {
                        Items = new List<Medicines> { new Medicines { Name = "Medicine1" } }
                    }
                }));

            // Act
            var result = await _medicineController.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); 
            var actualCategories = viewResult.ViewData["categories"] as List<Category>;
            Assert.NotNull(actualCategories);
            Assert.Equal(categories.Count, actualCategories.Count);
            Assert.Equal(categories[0].Name, actualCategories[0].Name);
        }


        [Theory]
        [InlineData(null, "Всe")]
        [InlineData("category1", "Category1")]
        public async Task Index_SetsCorrectCurrentCategory(string category, string expectedCurrentCategory)
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Headers["X-Requested-With"].Returns((Microsoft.Extensions.Primitives.StringValues)""); 

            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Returns(request);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var tempDataDictionary = new TempDataDictionary(httpContext, tempDataProvider);
            _medicineController.TempData = tempDataDictionary;

            _medicineController.ControllerContext = controllerContext;

            var categories = new List<Category>
    {
        new Category { Name = "Category1", NormalizedName = "category1" }
    };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = true,
                Data = categories
            }));
            _medicineService.GetMedicListAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(new ResponseData<ListModel<Medicines>>
            {
                Successfull = true,
                Data = new ListModel<Medicines>
                {
                    Items = new List<Medicines> { new Medicines { Name = "Medicine1" } }
                }
            }));

            // Act
            var result = await _medicineController.Index(category);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); 
            Assert.Equal(expectedCurrentCategory, viewResult.ViewData["currentCategory"]);
        }

        [Fact]
        public async Task Index_PassesMedicineListToViewModel_WhenSuccessful()
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Headers["X-Requested-With"].Returns((Microsoft.Extensions.Primitives.StringValues)""); 

            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Returns(request);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var tempDataDictionary = new TempDataDictionary(httpContext, tempDataProvider);
            _medicineController.TempData = tempDataDictionary;

            _medicineController.ControllerContext = controllerContext;

            var medicines = new List<Medicines> { new Medicines { Name = "Medicine1" } };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = true,
                Data = new List<Category> { new Category { Name = "Category1" } }
            }));
            _medicineService.GetMedicListAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(new ResponseData<ListModel<Medicines>>
            {
                Successfull = true,
                Data = new ListModel<Medicines> { Items = medicines }
            }));

            // Act
            var result = await _medicineController.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ListModel<Medicines>>(viewResult.Model); 

            Assert.Equal(medicines.Count, model.Items.Count);
            Assert.Equal(medicines[0].Name, model.Items[0].Name);
        }

    }
}
