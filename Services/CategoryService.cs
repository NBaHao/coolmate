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
            return categories.Select(c => _mapper.Map<CategoryDTO>(c));
        }

        public async Task<bool> addCategory(AddCategoryDTO addCategoryDTO)
        {
            if (addCategoryDTO.ParentCategoryId != 0)
            {
                var parentCate = await _categoryRepository.GetByIdAsync(addCategoryDTO.ParentCategoryId);
                if (parentCate == null) return false;
            }
            ProductCategory productCategory = _mapper.Map<ProductCategory>(addCategoryDTO);
            if (productCategory.ParentCategoryId == 0) 
                productCategory.ParentCategoryId = null;
            productCategory.Slug = ConvertToSlug(productCategory.CategoryName);
            await _categoryRepository.createCategoryAsync(productCategory);
            return true;
        }

        public async Task<bool> updateCategory(UpdateCategoryDTO updateCategoryDTO)
        {
            var category = await _categoryRepository.GetByIdAsync(updateCategoryDTO.Id);
            if (category == null) return false;
            category.CategoryName = updateCategoryDTO.CategoryName;
            category.Slug = ConvertToSlug(updateCategoryDTO.CategoryName);
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

            string slug = stringBuilder.ToString().ToLower().Replace(' ', '-');

            return slug;
        }
    }
}
