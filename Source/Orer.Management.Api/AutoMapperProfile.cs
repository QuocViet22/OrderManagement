﻿using AutoMapper;
using OrderManagement.Entities.Entities;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;

namespace OrerManagement.Api
{
    /// <summary>
    /// Using for auto map object from database to DTO
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMapping_Employee_ResEmployeeInfoDto();
            CreateMapping_ReqOrderInfoDto_Order();
            CreateMapping_ReqOrderLogInfoDto_OrderLog();
        }

        /// <summary>
        /// Create mapping profile for DB Employee to model EmployeeInfoDto
        /// </summary>
        private void CreateMapping_Employee_ResEmployeeInfoDto()
        {
            CreateMap<Employee, ResEmployeeInfoDto>()
                .ForMember(x => x.Id, options => options.MapFrom(source => source.Id))
                .ForMember(x => x.Name, options => options.MapFrom(source => source.Name))
                .ForMember(x => x.PhoneNumber, options => options.MapFrom(source => source.PhoneNumber));
        }

        /// <summary>
        /// Create mapping profile for ReqOrderInfoDto model to DB Order
        /// </summary>
        private void CreateMapping_ReqOrderInfoDto_Order()
        {
            CreateMap<ReqOrderInfoDto, Order>()
                .ForMember(x => x.CustomerName, options => options.MapFrom(source => source.CustomerName))
                .ForMember(x => x.PhoneNumber, options => options.MapFrom(source => source.PhoneNumber))
                .ForMember(x => x.Address, options => options.MapFrom(source => source.Address))
                .ForMember(x => x.JobTitle, options => options.MapFrom(source => source.JobTitle))
                .ForMember(x => x.JobDescription, options => options.MapFrom(source => source.JobDescription))
                .ForMember(x => x.Signature, options => options.MapFrom(source => source.Signature));
        }

        /// <summary>
        /// Create mapping profile for ReqOrderLogInfoDto model to DB OrderLog
        /// </summary>
        private void CreateMapping_ReqOrderLogInfoDto_OrderLog()
        {
            CreateMap<ReqOrderLogInfoDto, OrderLog>()
                .ForMember(x => x.Content, options => options.MapFrom(source => source.Content))
                .ForMember(x => x.CreateBy, options => options.MapFrom(source => source.CreateBy))
                .ForMember(x => x.CreatedOn, options => options.MapFrom(source => source.CreatedOn));
        }
    }
}
