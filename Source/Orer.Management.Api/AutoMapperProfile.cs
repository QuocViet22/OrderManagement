using AutoMapper;
using OrderManagement.Entities.Entities;
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
    }
}
