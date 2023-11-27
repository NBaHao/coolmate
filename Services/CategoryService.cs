using AutoMapper;
using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

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
            ProductCategory productCategory = _mapper.Map<ProductCategory>(addCategoryDTO);
            productCategory.Slug = ConvertToSlug(productCategory.CategoryName);
            await _categoryRepository.createCategoryAsync(productCategory);
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
        static string ConvertToSlug(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string slug = stringBuilder.ToString().ToLower().Replace(' ', '_');

            return slug;
        }
    }
}
