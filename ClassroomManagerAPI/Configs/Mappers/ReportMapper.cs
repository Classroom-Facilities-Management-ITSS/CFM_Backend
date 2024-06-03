using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Report;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class ReportMapper : Profile
    {
        public ReportMapper()
        {
            CreateMap<ReportModel, Report>().ReverseMap().IgnoreAllNonExisting();
            CreateMap<AddReportModel, Report>().ReverseMap().IgnoreAllNonExisting();
            CreateMap<UpdateReportModel, Report>().ReverseMap().IgnoreAllNonExisting();
        }
    }
}
