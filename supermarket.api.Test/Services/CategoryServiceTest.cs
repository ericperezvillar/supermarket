using Domain.Models;
using Domain.Models.Communication;
using Domain.Repositories;
using Domain.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Supermarket.API.Test
{
    public class CategoryServiceTest
    {
        #region Properties
        private readonly CategoryService categoryService;
        private readonly Mock<IUnitOfWork> unitOfwork;
        private readonly Mock<ICategoryRepository> categoryRepository;
        #endregion

        public CategoryServiceTest()
        {
            categoryRepository = new Mock<ICategoryRepository>();
            unitOfwork = new Mock<IUnitOfWork>();
            categoryService = new CategoryService(categoryRepository.Object, unitOfwork.Object);
        }

        #region Tests

        #region GetListOfCategoriesTest
        [Fact]
        public void GetListOfCategoriesTest()
        {
            var _categories = CreateCategoryListFake();

            categoryRepository.Setup(p => p.ListAsync()).ReturnsAsync(_categories);

            // Act
            var okResult = categoryService.ListAsync();

            // Assert
            var list = Assert.IsAssignableFrom<List<Category>>(okResult.Result);
            Assert.True(list.Count == 4 );
        }
        #endregion

        #region PostNewCategoryTest
        [Fact]
        public void PostNewCategoryTest()
        {
            categoryRepository.Setup(p => p.AddAsync(It.IsAny<Category>()));
        
            // Act
            var okResult = categoryService.SaveAsync(new Category() { Name = "Beer" });

            // Assert
            var result = Assert.IsAssignableFrom<CategoryResponse>(okResult.Result);
            Assert.True(result.Success);
        }
        #endregion

        #region PostNewCategoryUnsuccessTest
        [Fact]
        public void PostNewCategoryUnsuccessTest()
        {
            var category = new Category() { Name = "Beer" };

            categoryRepository.Setup(p => p.AddAsync(It.IsAny<Category>()));
            categoryRepository.Setup(p => p.FindCategoryByName(It.IsAny<string>())).ReturnsAsync(category);

            // Act
            var okResult = categoryService.SaveAsync(category);

            // Assert
            var result = Assert.IsAssignableFrom<CategoryResponse>(okResult.Result);
            Assert.False(result.Success);
        }
        #endregion

        #region UpdateCategoryTest
        [Fact]
        public void UpdateCategoryTest()
        {
            categoryRepository.Setup(p => p.Update(It.IsAny<Category>()));
            categoryRepository.Setup(p => p.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(new Category() { Name = "Snack" });

            // Act
            var okResult = categoryService.UpdateAsync(1, new Category() { Name = "Beer" });

            // Assert
            var result = Assert.IsAssignableFrom<CategoryResponse>(okResult.Result);
            Assert.True(result.Success);
            Assert.Equal("Beer", result.Resource.Name);
        }
        #endregion

        #region UpdateCategoryUnsuccessTest
        [Fact]
        public void UpdateCategoryUnsuccessTest()
        {
            categoryRepository.Setup(p => p.Update(It.IsAny<Category>()));
            categoryRepository.Setup(p => p.FindByIdAsync(It.IsAny<int>()));

            // Act
            var okResult = categoryService.UpdateAsync(1, new Category() { Name = "Beer", Id = 1 });

            // Assert
            var result = Assert.IsAssignableFrom<CategoryResponse>(okResult.Result);
            Assert.False(result.Success);
        }
        #endregion

        #region DeleteCategoryTest
        [Fact]
        public void DeleteCategoryTest()
        {
            categoryRepository.Setup(p => p.Update(It.IsAny<Category>()));
            categoryRepository.Setup(p => p.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(new Category() { Name = "Snack" });

            // Act
            var okResult = categoryService.DeleteAsync(1);

            // Assert
            var result = Assert.IsAssignableFrom<CategoryResponse>(okResult.Result);
            Assert.True(result.Success);
        }
        #endregion

        #region DeleteCategoryTest
        [Fact]
        public void DeleteCategoryUnsuccessTest()
        {
            categoryRepository.Setup(p => p.Update(It.IsAny<Category>()));
            categoryRepository.Setup(p => p.FindByIdAsync(It.IsAny<int>()));

            // Act
            var okResult = categoryService.DeleteAsync(1);

            // Assert
            var result = Assert.IsAssignableFrom<CategoryResponse>(okResult.Result);
            Assert.False(result.Success);
        }
        #endregion

        #endregion

        #region Private Methods
        private List<Category> CreateCategoryListFake()
        {
            return new List<Category>()
            {
                new Category() { Id = 1, Name = "Fruit"},
                new Category() { Id = 2, Name = "Vegetable"},
                new Category() { Id = 3, Name = "Bakery"},
                new Category() { Id = 4, Name = "Snack"},
            };
        }
        #endregion

    }
}
