using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Report;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class ReportMapper : Profile
    {
        public ReportMapper()
        {
            CreateMap<ReportModel, Report>().ReverseMap();
        }
    }
}
