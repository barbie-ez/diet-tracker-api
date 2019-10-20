﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightLossTracker.Api.Helpers;
using WeightLossTracker.DataStore.DTOs;
using WeightLossTracker.DataStore.DTOs.Content;
using WeightLossTracker.DataStore.DTOs.Creation;
using WeightLossTracker.DataStore.Entitties;

namespace WeightLossTracker.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WeightHistories, WeightHistoriesDto>()
                .ForMember(dest => dest.Weight,
                                        opt => opt.MapFrom(src => src.Weight));
            CreateMap<UserProfileModel, UserProfileDto>()
                .ForMember(dest => dest.Name,
                                        opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age,
                                        opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()))
                .ForMember(dest => dest.WeightHistories, 
                                        opt => opt.MapFrom(src => src.WeightHistories))
                .ForMember(dest => dest.Weight,
                                        opt => opt.MapFrom(src => src.CurrentWeight));
            CreateMap<FoodModel, FoodDto>();
            CreateMap<MealCategoriesModel, MealCategoriesDto>();
            CreateMap<MealCategoriesCreationDto, MealCategoriesDto>();
            CreateMap<WeightHistories, WeightHistoriesDto>();
            CreateMap<DietTrackerModel, DietEntryDto>()
                .ForMember(dest => dest.Food,
                                        opt => opt.MapFrom(src => src.Food))
                .ForMember(dest => dest.MealCategories,
                                        opt => opt.MapFrom(src => src.MealCategories));
            CreateMap<DietEntryDto, DietTrackerModel> ()
                .ForMember(dest => dest.Food,
                                        opt => opt.MapFrom(src => src.Food))
                .ForMember(dest => dest.MealCategories,
                                        opt => opt.MapFrom(src => src.MealCategories));
        }
    }
}
