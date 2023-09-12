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
    public class ReportApiTest
    {
        private readonly Mock<IReportService> _ReportServiceMock;
        private readonly Mock<IReportDetailService> _ReportDetailServiceMock;
        private readonly IMapper _mapper;
        private readonly ReportController _ReportController;
        private readonly Mock<IEventBus> _serviceBusMock;

        public ReportApiTest()
        {
            _ReportServiceMock = new Mock<IReportService>();
            _ReportDetailServiceMock = new Mock<IReportDetailService>();
            _serviceBusMock = new Mock<IEventBus>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _ReportController = new ReportController(_ReportServiceMock.Object, _ReportDetailServiceMock.Object, _serviceBusMock.Object, _mapper);
           

        }

        [TestMethod]
        public async Task Report_GetListAsync_Status200()
        {
            var ReportGuid = Guid.NewGuid();
            _ReportServiceMock.Setup(x => x.GetListAsync())
             .Returns(Task.FromResult(GetReportsFoo(ReportGuid)));
            
            
            var actionResult = await _ReportController.GetListAsync();

            
            var objectResult = (ObjectResult)actionResult;
            var response = (List<ReportDto>)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsInstanceOfType(response,typeof(List<ReportDto>));
        }
        [TestMethod]
        public async Task Report_GetListAsync_Status400()
        {
            var ReportGuid = Guid.NewGuid();
            _ReportServiceMock.Setup(x => x.GetListAsync()).ThrowsAsync(new Exception("Error"));


            var actionResult = await _ReportController.GetListAsync();


            var objectResult = (BadRequestObjectResult)actionResult;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
            
        }
        [TestMethod]
        public async Task Report_GetListAsync_ReturnsValidData()
        {
            var ReportGuid = Guid.NewGuid();
            var expectedData = GetReportsFoo(ReportGuid);

            _ReportServiceMock.Setup(x => x.GetListAsync())
             .Returns(Task.FromResult(expectedData));


            var actionResult = await _ReportController.GetListAsync();


            var objectResult = (OkObjectResult)actionResult;
            var response = (List<ReportDto>)objectResult.Value;
            Assert.AreEqual(expectedData, response);
        }
        [TestMethod]
        public async Task GetListAsync_HandlesNullData()
        {
             _ReportServiceMock.Setup(x => x.GetListAsync())
            .ReturnsAsync((List<ReportDto>)null);


            var actionResult = await _ReportController.GetListAsync();
            var objectResult = (OkObjectResult)actionResult;
            var response = (List<ReportDto>)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsNull(response);
        }        
        [TestMethod]
        public async Task Report_GetByIdAsync_Status200()
        {
            var ReportGuid = Guid.NewGuid();
            _ReportServiceMock.Setup(x => x.GetReportByIdAsync(It.IsAny<Guid>()))
             .Returns(Task.FromResult(GetReportFoo(ReportGuid)));


            var actionResult = await _ReportController.GetByIdAsync(ReportGuid);


            var objectResult = (ObjectResult)actionResult;
            var response = (ReportDto)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsInstanceOfType(response, typeof(ReportDto));
        }
        
        [TestMethod]
        public async Task Report_GetByIdAsync_EmptyGuidwithErrorTextStatus400()
        {
            var actionResult = await _ReportController.GetByIdAsync(Guid.Empty);


            var objectResult = (BadRequestObjectResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
            Assert.AreEqual("Id must be a valid Guid", objectResult.Value);
        }
        [TestMethod]
        public async Task Report_GetByIdAsync_HandleNullDataStatus422()
        {
            var ReportGuid = Guid.NewGuid();
            _ReportServiceMock.Setup(x => x.GetReportByIdAsync(It.IsAny<Guid>()))
             .Returns(Task.FromResult((ReportDto?)null));
            var actionResult = await _ReportController.GetByIdAsync(ReportGuid);

            var objectResult = (UnprocessableEntityResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.UnprocessableEntity);
            
        }
        [TestMethod]
        public async Task Report_GetByIdAsync_ReturnsValidData()
        {
            var ReportGuid = Guid.NewGuid();
            var expectedData = GetReportFoo(ReportGuid);

            _ReportServiceMock.Setup(x => x.GetReportByIdAsync(ReportGuid))
             .Returns(Task.FromResult(expectedData));


            var actionResult = await _ReportController.GetByIdAsync(ReportGuid);


            var objectResult = (OkObjectResult)actionResult;
            var response = (ReportDto)objectResult.Value;
            Assert.AreEqual(expectedData, response);
        }
        [TestMethod]
        public async Task Report_CreateAsync_Status201()
        {
            var ReportGuid = Guid.NewGuid();
            _ReportServiceMock.Setup(x => x.CreateReportAsync())
             .Returns(Task.FromResult(GetReportFoo(ReportGuid)));


            var actionResult = await _ReportController.CreateAsync();


            var objectResult = (ObjectResult)actionResult;
            var response = (ReportDto)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.IsInstanceOfType(response, typeof(ReportDto));
        }
        [TestMethod]
        public async Task Report_CreateAsync_Status400()
        {
            var ReportGuid = Guid.NewGuid();
            _ReportServiceMock.Setup(x => x.CreateReportAsync()).ThrowsAsync(new Exception("Error"));
             


            var actionResult = await _ReportController.CreateAsync();


            var objectResult = (BadRequestObjectResult)actionResult;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }
        [TestMethod]
        public async Task Report_CreateAsync_Status422()
        {
            var ReportGuid = Guid.NewGuid();
            _ReportServiceMock.Setup(x => x.CreateReportAsync())
             .Returns(Task.FromResult((ReportDto?)null));


            var actionResult = await _ReportController.CreateAsync();


            var objectResult = (UnprocessableEntityObjectResult)actionResult;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.UnprocessableEntity);
        }
        [TestMethod]
        public async Task Report_CreateAsync_ReturnsValidData()
        {
            var ReportGuid = Guid.NewGuid();
            var expectedData = GetReportFoo(ReportGuid);

            _ReportServiceMock.Setup(x => x.CreateReportAsync())
             .Returns(Task.FromResult(expectedData));


            var actionResult = await _ReportController.CreateAsync();


            var objectResult = (ObjectResult)actionResult;
            var response = (ReportDto)objectResult.Value;
            Assert.AreEqual(expectedData, response);
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
