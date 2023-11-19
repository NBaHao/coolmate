using AutoMapper;
using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoolMate.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryDTO>> getAllCategory()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            var categoryResponses = BuildCategoryTree(categories, null);
            return categoryResponses;
        }

        public async Task<bool> addCategory(AddCategoryDTO addCategoryDTO)
        {
            var parentCate = await _categoryRepository.GetByIdAsync(addCategoryDTO.ParentCategoryId);
            if (parentCate == null) return false;
            await _categoryRepository.createCategoryAsync(_mapper.Map<ProductCategory>(addCategoryDTO));
            return true;
        }

        public async Task<bool> updateCategory(UpdateCategoryDTO updateCategoryDTO)
        {
            var category = await _categoryRepository.GetByIdAsync(updateCategoryDTO.Id);
            if (category == null) return false;
            category.CategoryName = updateCategoryDTO.CategoryName;
            await _categoryRepository.updateCategoryAsync(category);
            return true;
        }
        public async Task<bool> deleteCategory(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null) return false;
            await _categoryRepository.DeleteCategoryAsync(category);
            return true;
        }
        private List<CategoryDTO> BuildCategoryTree(IEnumerable<ProductCategory> categories, int? parentId)
        {
            var categoryDTOs = new List<CategoryDTO>();

            var filteredCategories = categories.Where(c => c.ParentCategoryId == parentId);

            foreach (var category in filteredCategories)
            {
                var categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    Children = BuildCategoryTree(categories, category.Id)
                };

                categoryDTOs.Add(categoryDTO);
            }

            return categoryDTOs;
        }
    }
}
