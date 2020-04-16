using AutoMapper;
using Controllers;
using Domain.Models;
using Domain.Models.Communication;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Resources;
using System.Collections.Generic;
using Xunit;

namespace Supermarket.API.Test
{
    public class CategoriesControllerTest
    {
        #region Properties
        private readonly CategoriesController categoryController;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<ICategoryService> categoryService;
        #endregion

        public CategoriesControllerTest()
        {
            categoryService = new Mock<ICategoryService>();
            mapper = new Mock<IMapper>();
            categoryController = new CategoriesController(categoryService.Object, mapper.Object);
        }

        #region Tests

        #region GetListOfCategoriesTest
        [Fact]
        public void GetListOfCategoriesTest()
        {
            //categoryService.Setup(p => p.ListAsync()).ReturnsAsync(_categories.AsEnumerable());
            var _categoriesResources = CreateCategoryResourceListFake();

            mapper.Setup(m => m.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(It.IsAny<IEnumerable<Category>>())).Returns(_categoriesResources);

            // Act
            var okResult = categoryController.GetAllAsync();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        #endregion

        #region PostNewCategoryTest
        [Fact]
        public void PostNewCategoryTest()
        {
            var saveCategoryResource = new SaveCategoryResource() { Name = "Beer" };
            var categoryResource = new CategoryResource() { Name = "Beer" };
            var categoryResponse = new Mock<CategoryResponse>(new Category());

            mapper.Setup(m => m.Map<Category, CategoryResource>(It.IsAny<Category>())).Returns(categoryResource);
            categoryService.Setup(p => p.SaveAsync(It.IsAny<Category>())).ReturnsAsync(categoryResponse.Object);

            // Act
            var okResult = categoryController.PostAsync(saveCategoryResource);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        #endregion

        #region PostNewCategoryBadRequestTest
        [Fact]
        public void PostNewCategoryBadRequestTest()
        {
            var saveCategoryResource = new SaveCategoryResource() { Name = "Beer" };
            var categoryResource = new CategoryResource() { Name = "Beer" };
            var categoryResponse = new Mock<CategoryResponse>("Error trying to create Category");

            categoryService.Setup(p => p.SaveAsync(It.IsAny<Category>())).ReturnsAsync(categoryResponse.Object);

            // Act
            var badResult = categoryController.PostAsync(saveCategoryResource);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult.Result);
        }
        #endregion

        #region UpdateCategoryTest
        [Fact]
        public void UpdateCategoryTest()
        {
            var saveCategoryResource = new SaveCategoryResource() { Name = "Beer" };
            var categoryResource = new CategoryResource() { Name = "Beer", Id = 1 };
            var categoryResponse = new Mock<CategoryResponse>(new Category());

            categoryService.Setup(p => p.UpdateAsync(It.IsAny<int>(), It.IsAny<Category>())).ReturnsAsync(categoryResponse.Object);
            mapper.Setup(m => m.Map<Category, CategoryResource>(It.IsAny<Category>())).Returns(categoryResource);

            // Act
            var okResult = categoryController.PutAsync(1, saveCategoryResource);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        #endregion

        #region UpdateCategoryBadRequestTest
        [Fact]
        public void UpdateCategoryBadRequestTest()
        {
            var saveCategoryResource = new SaveCategoryResource() { Name = "Beer" };
            var categoryResource = new CategoryResource() { Name = "Beer" };
            var categoryResponse = new Mock<CategoryResponse>("Error trying to update Category");

            categoryService.Setup(p => p.UpdateAsync(It.IsAny<int>(), It.IsAny<Category>())).ReturnsAsync(categoryResponse.Object);

            // Act
            var badResult = categoryController.PutAsync(1, saveCategoryResource);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult.Result);
        }
        #endregion

        #region DeleteCategoryTest
        [Fact]
        public void DeleteCategoryTest()
        {
            var categoryResource = new CategoryResource() { Id = 1 };
            var categoryResponse = new Mock<CategoryResponse>(new Category());

            categoryService.Setup(p => p.DeleteAsync(It.IsAny<int>())).ReturnsAsync(categoryResponse.Object);
            mapper.Setup(m => m.Map<Category, CategoryResource>(It.IsAny<Category>())).Returns(categoryResource);

            // Act
            var okResult = categoryController.DeleteAsync(1);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        #endregion

        #region DeleteCategoryBadRequestTest
        [Fact]
        public void DeleteCategoryBadRequestTest()
        {
            var categoryResource = new CategoryResource() { Name = "Beer" };
            var categoryResponse = new Mock<CategoryResponse>("Error trying to delete Category");

            categoryService.Setup(p => p.DeleteAsync(It.IsAny<int>())).ReturnsAsync(categoryResponse.Object);

            // Act
            var badResult = categoryController.DeleteAsync(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult.Result);
        }
        #endregion

        #endregion

        #region Private Methods
        private List<CategoryResource> CreateCategoryResourceListFake()
        {
            return new List<CategoryResource>()
            {
                new CategoryResource() { Id = 1, Name = "Fruit"},
                new CategoryResource() { Id = 2, Name = "Vegetable"},
                new CategoryResource() { Id = 3, Name = "Bakery"},
                new CategoryResource() { Id = 4, Name = "Snack"},
            };
        }
        #endregion

    }
}
