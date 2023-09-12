using AutoMapper;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReportApi.Controllers;
using ReportApi.Infrastructure.Interfaces;
using ReportApi.IntegrationEvents.Events;
using ReportApi.IntegrationEvents.Handlers;
using ReportApi.Mapping;
using ReportApi.Model.ValidateObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using YamlDotNet.Core.Tokens;

namespace ReportApi.UnitTest
{
    [TestClass]
    public class IntegrationEventHandlerTests
    {
        private readonly Mock<IReportService> _ReportServiceMock;
        private readonly Mock<IReportDetailService> _ReportDetailServiceMock;
        private readonly IMapper _mapper;
        private readonly ReportController _ReportController;
        private readonly Mock<IEventBus> _serviceBusMock;

        public IntegrationEventHandlerTests()
        {
            _ReportServiceMock = new Mock<IReportService>();
            _ReportDetailServiceMock = new Mock<IReportDetailService>();
            _serviceBusMock = new Mock<IEventBus>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _ReportController = new ReportController(_ReportServiceMock.Object, _ReportDetailServiceMock.Object, _serviceBusMock.Object, _mapper);
           

        }

        [TestMethod]
        public async Task Handle_ValidEvent_CallsServiceMethods()
        {
            // Arrange
            var reportId = Guid.NewGuid(); // Create a sample report ID
            var expectedData = GetReportFoo(reportId);

            _ReportServiceMock.Setup(x => x.GetReportByIdAsync(reportId))
             .Returns(Task.FromResult(expectedData));

            var integrationEvent = new ReportCreatedIntegrationEvent
            {
                ReportId = reportId,
                Details = new List<ReportCreatedIntegrationEvent.ReportDetailDto>
            {
                new ReportCreatedIntegrationEvent.ReportDetailDto { Location = "Location1", PhoneNumberCount = 10, ContactCount = 20 },
                new ReportCreatedIntegrationEvent.ReportDetailDto { Location = "Location2", PhoneNumberCount = 5, ContactCount = 15 }
            }
            };
            var loggerMock = new Mock<ILogger<ReportCreatedIntegrationEvent>>();

           
            var handler = new ReportCreatedIntegrationEventHandler(
                _ReportDetailServiceMock.Object,
                _ReportServiceMock.Object,
                loggerMock.Object
            );

            // Act
            await handler.Handle(integrationEvent);

            // Assert
            
            _ReportServiceMock.Verify(service => service.GetReportByIdAsync(reportId), Times.Once);
            _ReportServiceMock.Verify(service => service.ReportCompletedAsync(reportId), Times.Once);
            _ReportDetailServiceMock.Verify(service => service.CreateReportDetailsAsync(It.IsAny<List<ReportDetailCreateDto>>()), Times.Once);

            
        }

        private List<ReportDto> GetReportsFoo(Guid id)
        {
            return new List<ReportDto>
         {
           new ReportDto
           {
               Id=id,
               Status =Enums.StatusEnum.Completed,
               CreateTime = DateTime.Now,
               CompletionTime = DateTime.Now,
               ReportDetail=new List<ReportDetailDto>
               {
                   new ReportDetailDto
                   {
                       Id = Guid.NewGuid(),
                       EmailCount=1,
                       Location ="Ankara",
                       PhoneCount=1,
                       ReportId=id,
                   },
                   new ReportDetailDto
                   {
                       Id = Guid.NewGuid(),
                       EmailCount=3,
                       Location ="İstanbul",
                       PhoneCount=4,
                       ReportId=id,
                   }
               }              
           },
           new ReportDto
           {
               Id=id,
               Status =Enums.StatusEnum.Prepararing,
               CreateTime = DateTime.Now,
               CompletionTime = null,
               ReportDetail=new List<ReportDetailDto>()
               
           }
        };
        }
        private ReportDto GetReportFoo(Guid id)
        {
            return new ReportDto
            {
                Id = id,
                Status = Enums.StatusEnum.Completed,
                CreateTime = DateTime.Now,
                CompletionTime = DateTime.Now,
                ReportDetail = new List<ReportDetailDto>
               {
                   new ReportDetailDto
                   {
                       Id = Guid.NewGuid(),
                       EmailCount=1,
                       Location ="Ankara",
                       PhoneCount=1,
                       ReportId=id,
                   },
                   new ReportDetailDto
                   {
                       Id = Guid.NewGuid(),
                       EmailCount=3,
                       Location ="İstanbul",
                       PhoneCount=4,
                       ReportId=id,
                   }
               }
            };
        }
       
    }
}
