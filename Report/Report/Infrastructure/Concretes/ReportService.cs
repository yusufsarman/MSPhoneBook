﻿using AutoMapper;
using ReportApi.Entities;
using ReportApi.Enums;
using ReportApi.Infrastructure.Interfaces;
using ReportApi.Model.ValidateObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReportApi.Infrastructure.Concretes
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Report> _reportRepository;
        private readonly IMapper _mapper;

        public ReportService(IRepository<Report> reportRepository,
            IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }
        public async Task<ReportDto> CreateReportAsync()
        {
            Report report = new Report
            {
                CreateTime = DateTime.Now,
                Status = StatusEnum.Prepararing
            };
            var data = await _reportRepository.Add(report);
            return _mapper.Map<ReportDto>(data);
        }

        public async Task<List<ReportDto>> GetListAsync()
        {
            var data = await _reportRepository.GetAll(c => c.ReportDetail);
            return _mapper.Map<List<ReportDto>>(data);
        }

        public async Task<ReportDto> GetReportByIdAsync(int reportId)
        {
            var data = await _reportRepository.GetById(reportId, c => c.ReportDetail);
            return _mapper.Map<ReportDto>(data);
        }
    }
}
