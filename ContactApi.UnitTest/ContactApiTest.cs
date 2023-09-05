using AutoMapper;
using ContactApi.Controllers;
using ContactApi.Entities;
using ContactApi.Enums;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.Mapping;
using ContactApi.Model.ValidateObjects;
using ContactDetailApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        #region ContactTests
        [TestMethod]
        public async Task Contact_GetListAsync_Status200()
        {
            var contactGuid = Guid.NewGuid();
            _contactServiceMock.Setup(x => x.GetListAsync())
             .Returns(Task.FromResult(GetContactsFoo(contactGuid)));


            var actionResult = await _contactController.GetListAsync();


            var objectResult = (ObjectResult)actionResult;
            var response = (List<ContactDto>)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsInstanceOfType(response, typeof(List<ContactDto>));
        }
        [TestMethod]
        public async Task Contact_GetListAsync_Status400()
        {
            var contactGuid = Guid.NewGuid();
            _contactServiceMock.Setup(x => x.GetListAsync()).ThrowsAsync(new Exception("Error"));

            var actionResult = await _contactController.GetListAsync();
            var objectResult = (BadRequestObjectResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);

        }
        [TestMethod]
        public async Task Contact_GetListAsync_ReturnsValidData()
        {
            var ReportGuid = Guid.NewGuid();
            var expectedData = GetContactsFoo(ReportGuid);

            _contactServiceMock.Setup(x => x.GetListAsync())
             .Returns(Task.FromResult(expectedData));


            var actionResult = await _contactController.GetListAsync();


            var objectResult = (OkObjectResult)actionResult;
            var response = (List<ContactDto>)objectResult.Value;
            Assert.AreEqual(expectedData, response);
        }
        [TestMethod]
        public async Task Contact_GetListAsync_HandlesNullData()
        {
            _contactServiceMock.Setup(x => x.GetListAsync())
           .ReturnsAsync((List<ContactDto>)null);


            var actionResult = await _contactController.GetListAsync();
            var objectResult = (OkObjectResult)actionResult;
            var response = (List<ContactDto>)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsNull(response);
        }


        [TestMethod]
        public async Task Contact_GetByIdAsync_Status200()
        {
            Guid contactId = Guid.NewGuid();
            _contactServiceMock.Setup(x => x.GetContactByIdAsync(contactId))
             .Returns(Task.FromResult(GetContactFoo(contactId)));


            var actionResult = await _contactController.GetByIdAsync(contactId);


            var objectResult = (ObjectResult)actionResult;
            var response = (ContactDto)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(ContactDto));
            Assert.AreEqual(response.Id, contactId);
        }

        [TestMethod]
        public async Task Contact_GetByIdAsync_EmptyGuidwithErrorTextStatus400()
        {
            var actionResult = await _contactController.GetByIdAsync(Guid.Empty);


            var objectResult = (BadRequestObjectResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
            Assert.AreEqual("Id must be a valid Guid", objectResult.Value);
        }
        [TestMethod]
        public async Task Contact_GetByIdAsync_HandleNullDataStatus422()
        {
            var ReportGuid = Guid.NewGuid();
            _contactServiceMock.Setup(x => x.GetContactByIdAsync(It.IsAny<Guid>()))
             .Returns(Task.FromResult((ContactDto?)null));
            var actionResult = await _contactController.GetByIdAsync(ReportGuid);

            var objectResult = (UnprocessableEntityResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.UnprocessableEntity);

        }
        [TestMethod]
        public async Task Contact_GetByIdAsync_ReturnsValidData()
        {
            var contactGuid = Guid.NewGuid();
            var expectedData = GetContactFoo(contactGuid);

            _contactServiceMock.Setup(x => x.GetContactByIdAsync(contactGuid))
             .Returns(Task.FromResult(expectedData));


            var actionResult = await _contactController.GetByIdAsync(contactGuid);


            var objectResult = (OkObjectResult)actionResult;
            var response = (ContactDto)objectResult.Value;
            Assert.AreEqual(expectedData, response);
        }
        [TestMethod]
        public async Task Contact_CreateAsync_ModelIsValid()
        {
            var model = new ContactCreateDto
            {
                Name = "111111111111111111111111111111",
                Surname = "lastname",
                Company = "company"
            };
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

            // Assert
            Assert.IsTrue(isValid, "Model validation should succeed.");
            Assert.AreEqual(0, results.Count, "No validation errors should be present.");

        }
        [TestMethod]
        public async Task Contact_CreateAsync_ModelIsNotValidStatus400()
        {
            string name = "Ahmet";
            for (int i = 0; i < 15; i++)
            {
                name += name;
            }
            var model = new ContactCreateDto
            {
                Name = name,
                Surname = "lastname",
                Company = "company"
            };
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

            // Assert
            Assert.IsFalse(isValid, "Model validation should succeed.");
            Assert.AreEqual(1, results.Count, "Validation Error count is expected value ");

        }
        [TestMethod]
        public async Task Contact_CreateAsync_Status201()
        {
            var contactId = Guid.NewGuid();
            var contactInfo = GetContactCreateFoo();
            _contactServiceMock.Setup(x => x.CreateContactAsync(contactInfo))
             .Returns(Task.FromResult(GetContactFoo(contactId)));


            var actionResult = await _contactController.CreateAsync(contactInfo);


            var objectResult = (ObjectResult)actionResult;
            var response = (ContactDto)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(ContactDto));
            Assert.AreEqual(response.Id, contactId);
        }
        [TestMethod]
        public async Task Contact_CreateAsync_ExceptionThrowStatus400()
        {
            var contactId = Guid.NewGuid();
            var contactInfo = GetContactCreateFoo();
            _contactServiceMock.Setup(x => x.CreateContactAsync(contactInfo)).ThrowsAsync(new Exception("Error"));


            var actionResult = await _contactController.CreateAsync(contactInfo);


            var objectResult = (BadRequestObjectResult)actionResult;
            Assert.AreEqual("Error", objectResult.Value);
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }
        [TestMethod]
        public async Task Contact_CreateAsync_Status422()
        {
            var contactId = Guid.NewGuid();
            var contactInfo = GetContactCreateFoo();
            _contactServiceMock.Setup(x => x.CreateContactAsync(contactInfo)).Returns(Task.FromResult((ContactDto?)null));



            var actionResult = await _contactController.CreateAsync(contactInfo);


            var objectResult = (UnprocessableEntityResult)actionResult;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.UnprocessableEntity);
        }

        [TestMethod]
        public async Task Contact_DeleteContactByIdAsync_Status204()
        {
            var contactId = Guid.NewGuid();

            _contactServiceMock.Setup(x => x.DeleteContactByIdAsync(contactId))
             .Returns(Task.FromResult(true));


            var actionResult = await _contactController.DeleteAsync(contactId);
            var objectResult = (NoContentResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.NoContent);
        }
        [TestMethod]
        public async Task Contact_DeleteContactByIdAsync_Status400()
        {

            var actionResult = await _contactController.DeleteAsync(Guid.Empty);


            var objectResult = (BadRequestObjectResult)actionResult;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }
        [TestMethod]
        public async Task Contact_DeleteContactByIdAsync_ValidIdStatus400()
        {

            var actionResult = await _contactController.DeleteAsync(Guid.Empty);


            var objectResult = (BadRequestObjectResult)actionResult;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
            Assert.AreEqual("Id must be a valid Guid", objectResult.Value);
        }
        private List<ContactDto> GetContactsFoo(Guid id)
        {
            return new List<ContactDto>
         {
           new ContactDto
           {
               Id=id,
               Name = "Ahmet",
               Surname = "C",
               Company = "Turkcell",
               ContactDetails =GetContactDetailsFoo(id)
           },
            new ContactDto
           {
               Id=id,
               Name = "Mehmet",
               Surname = "Y",
               Company = "Ford",
               ContactDetails =GetContactDetailsFoo(id)
           },
        };
        }
        private ContactDto GetContactFoo(Guid id)
        {
            return new ContactDto
            {
                Id = id,
                Name = "Ahmet",
                Surname = "C",
                Company = "Turkcell",
                ContactDetails = GetContactDetailsFoo(id)
            };
        }
        private List<ContactDetailDto> GetContactDetailsFoo(Guid id)
        {
            return new List<ContactDetailDto>
            {
                new ContactDetailDto
                {
                    Id = id,
                    ContactId = id,
                    ContactDetailType=Enums.ContactDetailTypeEnum.Phone,
                    Content="5422355858"

                },
                new ContactDetailDto
                {
                    Id = id,
                    ContactId = id,
                    ContactDetailType=Enums.ContactDetailTypeEnum.Email,
                    Content="aaa@gmail.com"

                },

            };
        }
        private ContactDetailDto GetContactDetailFoo(Guid id)
        {
            return new ContactDetailDto
            {
                Id = id,
                ContactId = id,
                ContactDetailType = Enums.ContactDetailTypeEnum.Phone,
                Content = "5422355858"

            };
        }
        private ContactCreateDto GetContactCreateFoo()
        {
            return new ContactCreateDto
            {
                Name = "Ahmet",
                Surname = "C",
                Company = "Turkcell",
            };
        }
        #endregion

        #region ContactDetailTests
        [TestMethod]
        public async Task ContactDetail_GetListAsync_Status200()
        {
            var contactGuid = Guid.NewGuid();
            _contactDetailServiceMock.Setup(x => x.GetListAsync())
             .Returns(Task.FromResult(GetContactDetailsFoo(contactGuid)));


            var actionResult = await _contactDetailController.GetListAsync();


            var objectResult = (ObjectResult)actionResult;
            var response = (List<ContactDetailDto>)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsInstanceOfType(response, typeof(List<ContactDetailDto>));
        }
        [TestMethod]
        public async Task ContactDetail_GetListAsync_Status400()
        {
            var contactDetailGuid = Guid.NewGuid();
            _contactDetailServiceMock.Setup(x => x.GetListAsync()).ThrowsAsync(new Exception("Error"));

            var actionResult = await _contactDetailController.GetListAsync();
            var objectResult = (BadRequestObjectResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);

        }
        [TestMethod]
        public async Task ContactDetail_GetListAsync_ReturnsValidData()
        {
            var ReportGuid = Guid.NewGuid();
            var expectedData = GetContactDetailsFoo(ReportGuid);

            _contactDetailServiceMock.Setup(x => x.GetListAsync())
             .Returns(Task.FromResult(expectedData));


            var actionResult = await _contactDetailController.GetListAsync();


            var objectResult = (OkObjectResult)actionResult;
            var response = (List<ContactDetailDto>)objectResult.Value;
            Assert.AreEqual(expectedData, response);
        }
        [TestMethod]
        public async Task ContactDetail_GetListAsync_HandlesNullData()
        {
            _contactDetailServiceMock.Setup(x => x.GetListAsync())
           .ReturnsAsync((List<ContactDetailDto>)null);


            var actionResult = await _contactDetailController.GetListAsync();
            var objectResult = (OkObjectResult)actionResult;
            var response = (List<ContactDetailDto>)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsNull(response);
        }


        [TestMethod]
        public async Task ContactDetail_GetByIdAsync_Status200()
        {
            Guid contactId = Guid.NewGuid();
            _contactDetailServiceMock.Setup(x => x.GetContactDetailByIdAsync(contactId))
             .Returns(Task.FromResult(GetContactDetailFoo(contactId)));


            var actionResult = await _contactDetailController.GetByIdAsync(contactId);


            var objectResult = (ObjectResult)actionResult;
            var response = (ContactDetailDto)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(ContactDetailDto));
            Assert.AreEqual(response.Id, contactId);
        }

        [TestMethod]
        public async Task ContactDetail_GetByIdAsync_EmptyGuidwithErrorTextStatus400()
        {
            var actionResult = await _contactDetailController.GetByIdAsync(Guid.Empty);


            var objectResult = (BadRequestObjectResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
            Assert.AreEqual("Id must be a valid Guid", objectResult.Value);
        }
        [TestMethod]
        public async Task ContactDetail_GetByIdAsync_HandleNullDataStatus404()
        {
            var ContactGuid = Guid.NewGuid();
            _contactDetailServiceMock.Setup(x => x.GetContactDetailByIdAsync(It.IsAny<Guid>()))
             .Returns(Task.FromResult((ContactDetailDto?)null));
            var actionResult = await _contactDetailController.GetByIdAsync(ContactGuid);

            var objectResult = (NotFoundResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.NotFound);

        }
        [TestMethod]
        public async Task ContactDetail_GetByIdAsync_ReturnsValidData()
        {
            var contactGuid = Guid.NewGuid();
            var expectedData = GetContactDetailFoo(contactGuid);

            _contactDetailServiceMock.Setup(x => x.GetContactDetailByIdAsync(contactGuid))
             .Returns(Task.FromResult(expectedData));


            var actionResult = await _contactDetailController.GetByIdAsync(contactGuid);


            var objectResult = (OkObjectResult)actionResult;
            var response = (ContactDetailDto)objectResult.Value;
            Assert.AreEqual(expectedData, response);
        }
        [TestMethod]
        public async Task ContactDetail_CreateAsync_ModelIsValid()
        {
            var model = new ContactDetailCreateDto()
            {
                ContactId = Guid.NewGuid(),
                ContactDetailType = ContactDetailTypeEnum.Email,
                Content = "aaa"
            };
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

            // Assert
            Assert.IsTrue(isValid, "Model validation should succeed.");
            Assert.AreEqual(0, results.Count, "No validation errors should be present.");

        }
        [TestMethod]
        public async Task ContactDetail_CreateAsync_ModelIsNotValidStatus400()
        {
            var model = new ContactDetailCreateDto()
            {
                ContactId = Guid.NewGuid(),
                ContactDetailType = ContactDetailTypeEnum.Email,
                Content = null
            };
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

            // Assert
            Assert.IsFalse(isValid, "Model validation should succeed.");
            Assert.AreEqual(1, results.Count, "Content is required.");

        }
        [TestMethod]
        public async Task ContactDetail_CreateAsync_Status201()
        {
            var contactDetailId = Guid.NewGuid();
            var model = new ContactDetailCreateDto()
            {
                ContactId = Guid.NewGuid(),
                ContactDetailType = ContactDetailTypeEnum.Email,
                Content = "aaa"
            };
            _contactDetailServiceMock.Setup(x => x.CreateContactDetailAsync(model))
             .Returns(Task.FromResult(GetContactDetailFoo(contactDetailId)));


            var actionResult = await _contactDetailController.CreateAsync(model);


            var objectResult = (ObjectResult)actionResult;
            var response = (ContactDetailDto)objectResult.Value;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(ContactDetailDto));
            Assert.AreEqual(response.Id, contactDetailId);
        }
        [TestMethod]
        public async Task ContactDetail_CreateAsync_ExceptionThrowStatus400()
        {
            var contactDetailId = Guid.NewGuid();
            var model = new ContactDetailCreateDto()
            {
                ContactId = Guid.NewGuid(),
                ContactDetailType = ContactDetailTypeEnum.Email,
                Content = "aaa"
            };
            _contactDetailServiceMock.Setup(x => x.CreateContactDetailAsync(model)).ThrowsAsync(new Exception("Error"));


            var actionResult = await _contactDetailController.CreateAsync(model);


            var objectResult = (BadRequestObjectResult)actionResult;
            Assert.AreEqual("Error", objectResult.Value);
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }
        [TestMethod]
        public async Task ContactDetail_CreateAsync_Status422()
        {
            var contactDetailId = Guid.NewGuid();
            var model = new ContactDetailCreateDto()
            {
                ContactId = Guid.NewGuid(),
                ContactDetailType = ContactDetailTypeEnum.Email,
                Content = "aaa"
            };
            _contactDetailServiceMock.Setup(x => x.CreateContactDetailAsync(model)).Returns(Task.FromResult((ContactDetailDto?)null));



            var actionResult = await _contactDetailController.CreateAsync(model);


            var objectResult = (UnprocessableEntityResult)actionResult;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.UnprocessableEntity);
        }

        [TestMethod]
        public async Task ContactDetail_DeleteContactDetailByIdAsync_Status204()
        {
            var contactId = Guid.NewGuid();

            _contactDetailServiceMock.Setup(x => x.DeleteContactDetailByIdAsync(contactId))
             .Returns(Task.FromResult(true));


            var actionResult = await _contactDetailController.DeleteAsync(contactId);
            var objectResult = (NoContentResult)actionResult;

            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.NoContent);
        }
        [TestMethod]
        public async Task ContactDetail_DeleteContactDetailByIdAsync_Status400()
        {

            var actionResult = await _contactDetailController.DeleteAsync(Guid.Empty);


            var objectResult = (BadRequestObjectResult)actionResult;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }
        [TestMethod]
        public async Task ContactDetail_DeleteContactDetailByIdAsync_ValidIdStatus400()
        {

            var actionResult = await _contactDetailController.DeleteAsync(Guid.Empty);


            var objectResult = (BadRequestObjectResult)actionResult;
            Assert.AreEqual(objectResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
            Assert.AreEqual("Id must be a valid Guid", objectResult.Value);
        }
        #endregion
    }
}
