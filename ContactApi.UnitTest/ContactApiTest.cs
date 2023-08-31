using AutoMapper;
using ContactApi.Controllers;
using ContactApi.Entities;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.Mapping;
using ContactApi.Model.ValidateObjects;
using ContactDetailApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContactApi.UnitTest
{
    [TestClass]
    public class ContactApiTest
    {
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly Mock<IContactDetailService> _contactDetailServiceMock;
        private readonly IMapper _mapper;
        private readonly ContactController _contactController;
        private readonly ContactDetailController _contactDetailController;

        public ContactApiTest()
        {
            _contactServiceMock = new Mock<IContactService>();
            _contactDetailServiceMock = new Mock<IContactDetailService>();

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _contactController = new ContactController(_contactServiceMock.Object);
            _contactDetailController = new ContactDetailController(_contactDetailServiceMock.Object);

        }

        [TestMethod]
        public async Task Contact_GetListAsync_Status200()
        {
            _contactServiceMock.Setup(x => x.GetListAsync())
             .Returns(Task.FromResult(GetContactFoo()));
            
            
            var actionResult = await _contactController.GetListAsync();

            
            var objectResult = (ObjectResult)actionResult;
            var response = (List<ContactDto>)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsInstanceOfType(response,typeof(List<ContactDto>));
        }

        private List<ContactDto> GetContactFoo()
        {
            return new List<ContactDto>
         {
           new ContactDto
           {
               Id=5,
               Name = "Ahmet",
               Surname = "C",
               Company = "Turkcell",
           },
            new ContactDto
           {
               Id=6,
               Name = "Mehmet",
               Surname = "Y",
               Company = "Ford",
           },
        };
        }
    }
}
