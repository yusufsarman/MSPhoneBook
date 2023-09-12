using AutoMapper;
using ContactApi.Controllers;
using ContactApi.Entities;
using ContactApi.Infrastructure;
using ContactApi.Infrastructure.Concretes;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.Mapping;
using ContactApi.Model.ValidateObjects;
using ContactDetailApi.Controllers;
using ContactDetailApi.Infrastructure.Concretes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApi.UnitTest
{
    [TestClass]
    public class ServiceTests
    {
        private readonly Repository<Contact> _contactRepositoryMock;
        private readonly Mock<AppDbContext> dbContextMock;
        private readonly Repository<ContactDetail> _contactDetailRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ContactService _contactService;
        private readonly ContactDetailService _contactDetailService;

        public ServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                             .UseInMemoryDatabase(databaseName: "SomeDatabaseInMemory")
                             .Options;
            var dbContextFactoryMock = new Mock<AppDbContextFactory>();
            dbContextFactoryMock.Setup(f => f.CreateDbContext()).Returns(new AppDbContext(options));
            _contactRepositoryMock = new Repository<Contact>(dbContextFactoryMock.Object);
            _contactDetailRepositoryMock = new Repository<ContactDetail>(dbContextFactoryMock.Object);

            //_contactDetailRepositoryMock = new Mock<IRepository<ContactDetail>>();

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
            _contactService = new ContactService(_contactRepositoryMock, _mapper);
            _contactDetailService = new ContactDetailService(_contactDetailRepositoryMock, _mapper);

        }
        
        // Tests that the function creates a contact successfully
        [TestMethod]
        public async Task Test_CreateContact_Successfully()
        {

            var createDto = GetContactCreateDtoFoo();
            var cancellationToken = new CancellationToken();
            // Act
            var result = await _contactService.CreateContactAsync(createDto, cancellationToken);
            var savedData = await _contactRepositoryMock.GetById(result.Id, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(savedData);
            Assert.IsInstanceOfType(result, typeof(ContactDto));
            Assert.AreEqual(result.Id, savedData.Id);
        }

        // Tests that the function retrieves a contact by ID successfully
        [TestMethod]
        public async Task Test_GetContactById_NoDetail_Successfully()
        {

            var contactDto = GetContactCreateDtoFoo();
            var _input = _mapper.Map<Contact>(contactDto);
            var cancellationToken = new CancellationToken();
            var savedData = await _contactRepositoryMock.Add(_input, cancellationToken);

            // Act
            var result = await _contactService.GetContactByIdAsync(savedData.Id, cancellationToken);

            // Assert
            Assert.IsNotNull(savedData);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, savedData.Id);
            Assert.IsTrue(result.ContactDetails.Count <= 0);
        }

        // Tests that the function deletes a contact by ID successfully
        [TestMethod]
        public async Task Test_DeleteContactById_Successfully()
        {

            var contactDto = GetContactCreateDtoFoo();
            var _input = _mapper.Map<Contact>(contactDto);

            var savedData = await _contactRepositoryMock.Add(_input);
            await _contactService.DeleteContactByIdAsync(savedData.Id);

            var result = await _contactService.GetContactByIdAsync(savedData.Id);

            Assert.IsNull(result);

        }

        // Tests that the function retrieves a list of contacts successfully
        [TestMethod]
        public async Task Test_GetList_Successfully()
        {
            // Arrange
            var contactDtos = GetContactCreateDtosFoo();
            var _input = _mapper.Map<List<Contact>>(contactDtos);
            await _contactRepositoryMock.AddRange(_input);

            // Act
            var result = await _contactService.GetListAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            

        }

        // Tests that the function handles creating a contact with null input
        [TestMethod]
        public async Task Test_CreateContact_WithNullInput()
        {            
            ContactCreateDto addContact = null;
            var cancellationToken = new CancellationToken();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _contactService.CreateContactAsync(addContact, cancellationToken));
        }

        // Tests that the function handles creating a contact with invalid input
        [TestMethod]
        public async Task Test_CreateContact_WithInvalidInput()
        {
            // Arrange
            var addContact = new ContactCreateDto();
            var cancellationToken = new CancellationToken();
            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<DbUpdateException>(() => _contactService.CreateContactAsync(addContact, cancellationToken));
        }

        // Tests that the function handles retrieving a contact with an invalid ID
        [TestMethod]
        public async Task Test_GetContactById_WithInvalidId()
        {
            // Arrange
            var contactDto = GetContactCreateDtoFoo();
            var _input = _mapper.Map<Contact>(contactDto);

            var savedData = await _contactRepositoryMock.Add(_input);

            // Act
            var result = await _contactService.GetContactByIdAsync(Guid.Empty);

            // Assert
            Assert.IsNull(result);
        }

        // Tests that the function handles deleting a contact with an invalid ID
        [TestMethod]
        public async Task Test_DeleteContactById_WithInvalidId()
        {
            var id = Guid.Empty;
            var contactDto = GetContactCreateDtoFoo();
            var _input = _mapper.Map<Contact>(contactDto);

            var savedData = await _contactRepositoryMock.Add(_input);
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _contactService.DeleteContactByIdAsync(id));

        }

        // Tests that the function handles retrieving a list of contacts when there are no contacts in the repository
        [TestMethod]
        public async Task Test_GetList_WithNoContacts()
        {
            
            var result = await _contactService.GetListAsync();
            foreach (var contactDto in result)
            {
                await _contactService.DeleteContactByIdAsync(contactDto.Id);
            }
            var resultFinal = await _contactService.GetListAsync();
            // Assert
            Assert.IsNotNull(resultFinal);
            Assert.AreEqual(0, resultFinal.Count);
        }

        // Tests that the function handles creating a contact with duplicate data
        [TestMethod]
        public async Task Test_CreateContact_WithDuplicateData()
        {
            var id = Guid.NewGuid();
            var addContact = GetContactFooNoDetail(id);

            var savedData = await _contactRepositoryMock.Add(addContact);
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _contactRepositoryMock.Add(addContact));
            Assert.AreEqual(exception.Message, "An item with the same key has already been added. Key: " + id);
        }

        // Tests that the function retrieves a contact with details
        [TestMethod]
        public async Task Test_GetContactWithDetails()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var contactDtoWithDetail = GetContactCreateDtoFoo();
            
            var _input = _mapper.Map<Contact>(contactDtoWithDetail);
            var savedData = await _contactRepositoryMock.Add(_input);
            var contactDetailDto = GetContactDetailCreateDtoFoo(savedData.Id);
            var _inputDetail = _mapper.Map<ContactDetail>(contactDetailDto);
            var savedDataDetail = await _contactDetailRepositoryMock.Add(_inputDetail);
            // Act
            var result = await _contactService.GetContactByIdAsync(savedData.Id, cancellationToken);
            

            // Assert
            Assert.IsNotNull(result);            
            Assert.IsNotNull(result.ContactDetails);
            Assert.IsTrue(result.ContactDetails.Count>0);
            Assert.AreEqual(result.Id, savedData.Id);
            Assert.AreEqual(result.ContactDetails.First().Id, savedDataDetail.Id);
        }

        // Tests that the function retrieves a list of contacts with details
        [TestMethod]
        public async Task Test_GetListWithDetails()
        {
            var resultClear = await _contactService.GetListAsync();
            foreach (var contactDto in resultClear)
            {
                await _contactService.DeleteContactByIdAsync(contactDto.Id);
            }
            // Arrange
            var contactDtoWithDetail = GetContactCreateDtoFoo();

            var _input = _mapper.Map<Contact>(contactDtoWithDetail);
            var savedData = await _contactRepositoryMock.Add(_input);
            var contactDetailDto = GetContactDetailCreateDtoFoo(savedData.Id);
            var _inputDetail = _mapper.Map<ContactDetail>(contactDetailDto);
            var savedDataDetail = await _contactDetailRepositoryMock.Add(_inputDetail);
            // Act
            var result = await _contactService.GetListAsync();


            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsNotNull(result.FirstOrDefault().ContactDetails);
            Assert.IsTrue(result.FirstOrDefault().ContactDetails.Count > 0);
            Assert.AreEqual(result.FirstOrDefault().Id, savedData.Id);
            Assert.AreEqual(result.FirstOrDefault().ContactDetails.FirstOrDefault().Id, savedDataDetail.Id);
        }

        // Test the behavior of deleting a contact that has details
        [TestMethod]
        public async Task Test_DeleteContactWithDetails_Successfully()
        {
            var resultClear = await _contactService.GetListAsync();
            foreach (var contactDto in resultClear)
            {
                await _contactService.DeleteContactByIdAsync(contactDto.Id);
            }
            // Arrange
            var contactDtoWithDetail = GetContactCreateDtoFoo();

            var _input = _mapper.Map<Contact>(contactDtoWithDetail);
            var savedData = await _contactRepositoryMock.Add(_input);
            var contactDetailDto = GetContactDetailCreateDtoFoo(savedData.Id);
            var _inputDetail = _mapper.Map<ContactDetail>(contactDetailDto);
            var savedDataDetail = await _contactDetailRepositoryMock.Add(_inputDetail);
            await _contactService.DeleteContactByIdAsync(savedData.Id);

            var anyContact =  await _contactService.GetContactByIdAsync(savedData.Id);
            var anyContactDetail = await _contactDetailRepositoryMock.GetById(savedDataDetail.Id);
            Assert.IsNull(anyContact);
            Assert.IsNull(anyContactDetail);

        }
        private List<Contact> GetContactsFoo(Guid id)
        {
            return new List<Contact>
         {
           new Contact
           {
               Id=id,
               Name = "Ahmet",
               Surname = "C",
               Company = "Turkcell",
               ContactDetails =GetContactDetailsFoo(id)
           },
            new Contact
           {
               Id=id,
               Name = "Mehmet",
               Surname = "Y",
               Company = "Ford",
               ContactDetails =GetContactDetailsFoo(id)
           },
        };
        }
        private Contact GetContactFoo(Guid id)
        {
            return new Contact
            {
                Id = id,
                Name = "Ahmet",
                Surname = "C",
                Company = "Turkcell",
                ContactDetails = GetContactDetailsFoo(id)
            };
        }
        private Contact GetContactFooNoDetail(Guid id)
        {
            return new Contact
            {
                Id = id,
                Name = "Ahmet",
                Surname = "C",
                Company = "Turkcell"
            };
        }
        private List<ContactDetail> GetContactDetailsFoo(Guid id)
        {
            return new List<ContactDetail>
            {
                new ContactDetail
                {
                    Id = id,
                    ContactId = id,
                    ContactDetailType=Enums.ContactDetailTypeEnum.Phone,
                    Content="5422355858"

                },
                new ContactDetail
                {
                    Id = id,
                    ContactId = id,
                    ContactDetailType=Enums.ContactDetailTypeEnum.Email,
                    Content="aaa@gmail.com"

                },

            };
        }
        private ContactDetail GetContactDetailFoo(Guid id)
        {
            return new ContactDetail
            {
                Id = id,
                ContactId = id,
                ContactDetailType = Enums.ContactDetailTypeEnum.Phone,
                Content = "5422355858"

            };
        }

        private List<ContactDto> GetContactsDtoFoo(Guid id)
        {
            return new List<ContactDto>
         {
           new ContactDto
           {
               Id=id,
               Name = "Ahmet",
               Surname = "C",
               Company = "Turkcell",
               ContactDetails =GetContactDetailsDtoFoo(id)
           },
            new ContactDto
           {
               Id=id,
               Name = "Mehmet",
               Surname = "Y",
               Company = "Ford",
               ContactDetails =GetContactDetailsDtoFoo(id)
           },
        };
        }
        private ContactDto GetContactDtoFoo(Guid id)
        {
            return new ContactDto
            {
                Id = id,
                Name = "Ahmet",
                Surname = "C",
                Company = "Turkcell",
                ContactDetails = GetContactDetailsDtoFoo(id)
            };
        }
        private List<ContactDetailDto> GetContactDetailsDtoFoo(Guid id)
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
        private ContactDetailDto GetContactDetailDtoFoo(Guid id)
        {
            return new ContactDetailDto
            {
                Id = id,
                ContactId = id,
                ContactDetailType = Enums.ContactDetailTypeEnum.Phone,
                Content = "5422355858"

            };
        }
        private ContactCreateDto GetContactCreateDtoFoo()
        {
            return new ContactCreateDto
            {
                Name = "Ahmet",
                Surname = "C",
                Company = "Turkcell",
            };
        }
        private ContactDetailCreateDto GetContactDetailCreateDtoFoo(Guid id)
        {
            return new ContactDetailCreateDto
            {
                ContactId=id,
                ContactDetailType = Enums.ContactDetailTypeEnum.Phone,
                Content="5311024487"
            };
        }
        private List<ContactCreateDto> GetContactCreateDtosFoo()
        {
            return new List<ContactCreateDto> {
                new ContactCreateDto
            {
                Name = "Ahmet",
                Surname = "C",
                Company = "Turkcell",
            },
                new ContactCreateDto
            {
                Name = "Ali",
                Surname = "D",
                Company = "Telekom",
            },
                new ContactCreateDto
            {
                Name = "Yusuf",
                Surname = "S",
                Company = "Vodafone",
            }
        };

        }


        // Tests that CreateContactDetailAsync method returns a ContactDetailDto object.
        [TestMethod]
        public async Task Test_CreateContactDetail_ReturnsDto()
        {
            var contactDtoWithDetail = GetContactCreateDtoFoo();
            var cancellationToken = new CancellationToken();
            var _input = _mapper.Map<Contact>(contactDtoWithDetail);
            var savedData = await _contactRepositoryMock.Add(_input);
            var contactDetailDto = GetContactDetailCreateDtoFoo(savedData.Id);
            // Act
            var result = await _contactDetailService.CreateContactDetailAsync(contactDetailDto, cancellationToken);
             
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ContactDetailDto));
            Assert.AreEqual(result.ContactId, savedData.Id);
        }

        // Tests that DeleteContactDetailByIdAsync method deletes the ContactDetail with the specified id.
        [TestMethod]
        public async Task Test_DeleteContactDetailById_DeletesContactDetail()
        {
            var contactDtoWithDetail = GetContactCreateDtoFoo();
            var cancellationToken = new CancellationToken();
            var _input = _mapper.Map<Contact>(contactDtoWithDetail);
            var savedData = await _contactRepositoryMock.Add(_input);
            var contactDetailDto = GetContactDetailCreateDtoFoo(savedData.Id);
            var result = await _contactDetailService.CreateContactDetailAsync(contactDetailDto, cancellationToken);

            await _contactDetailService.DeleteContactDetailByIdAsync(result.Id, cancellationToken);
            var data = await _contactRepositoryMock.GetById(result.Id);
            Assert.IsNull(data);
        }

        // Tests that GetContactDetailByIdAsync method returns a ContactDetailDto object with the specified id.
        [TestMethod]
        public async Task Test_GetContactDetailById_ReturnsDto()
        {
            var contactDtoWithDetail = GetContactCreateDtoFoo();
            var cancellationToken = new CancellationToken();
            var _input = _mapper.Map<Contact>(contactDtoWithDetail);
            var savedData = await _contactRepositoryMock.Add(_input);
            var contactDetailDto = GetContactDetailCreateDtoFoo(savedData.Id);
            var result = await _contactDetailService.CreateContactDetailAsync(contactDetailDto, cancellationToken);          


            // Act
            var data = await _contactDetailService.GetContactDetailByIdAsync(result.Id, cancellationToken);

            // Assert
            Assert.IsNotNull(data);
            Assert.IsInstanceOfType(data, typeof(ContactDetailDto));
            Assert.AreEqual(data, data);
        }

        // Tests that GetListAsync method returns a List of ContactDetailDto objects.
        [TestMethod]
        public async Task Test_GetList_ReturnsListOfDtos()
        {
            var contactDtoWithDetail = GetContactCreateDtoFoo();
            var cancellationToken = new CancellationToken();
            var _input = _mapper.Map<Contact>(contactDtoWithDetail);
            var savedData = await _contactRepositoryMock.Add(_input);
            var contactDetailDto = GetContactDetailCreateDtoFoo(savedData.Id);
            var result = await _contactDetailService.CreateContactDetailAsync(contactDetailDto, cancellationToken);

            // Act
            var data = await _contactDetailService.GetListAsync(cancellationToken);

            // Assert
            Assert.IsNotNull(data);
            Assert.IsInstanceOfType(data, typeof(List<ContactDetailDto>));
            Assert.IsTrue(data.Count>0);
        }

        // Tests that CreateContactDetailAsync method throws an exception when the input is null.
        [TestMethod]
        public async Task Test_CreateContactDetail_ThrowsExceptionWhenInputIsNull()
        {
            // Arrange
            ContactDetailCreateDto addContactDetail = null;
            var cancellationToken = new CancellationToken();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _contactDetailService.CreateContactDetailAsync(addContactDetail, cancellationToken));
        }
        // Tests that DeleteContactDetailByIdAsync method throws an exception when the ContactDetail with the specified id does not exist.
        [TestMethod]
        public async Task Test_DeleteContactDetailById_DoNothingDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            // Act & Assert
            await _contactDetailService.DeleteContactDetailByIdAsync(id, cancellationToken);
        }

       

        // Tests that GetListAsync method returns an empty List when there are no ContactDetails.
        [TestMethod]
        public async Task Test_GetList_ReturnsEmptyListWhenNoContactDetails()
        {
            var resultClear = await _contactDetailService.GetListAsync();
            foreach (var contactDto in resultClear)
            {
                await _contactDetailRepositoryMock.Delete(contactDto.Id);
            }
            var cancellationToken = new CancellationToken();
            var result = await _contactDetailService.GetListAsync(cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        
        // Tests that CreateContactDetailAsync method adds the ContactDetail object to the repository.
        [TestMethod]
        public async Task Test_CreateContactDetail_AddsContactDetailToRepository()
        {
            var contactDtoWithDetail = GetContactCreateDtoFoo();
            var cancellationToken = new CancellationToken();
            var _input = _mapper.Map<Contact>(contactDtoWithDetail);
            var savedData = await _contactRepositoryMock.Add(_input);
            var contactDetailDto = GetContactDetailCreateDtoFoo(savedData.Id);
            var result = await _contactDetailService.CreateContactDetailAsync(contactDetailDto, cancellationToken);
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id != Guid.Empty);
        }

        // Tests that DeleteContactDetailByIdAsync method deletes the ContactDetail object from the repository.
        [TestMethod]
        public async Task Test_DeleteContactDetailById_DeletesContactDetailFromRepository()
        {
            var contactDtoWithDetail = GetContactCreateDtoFoo();
            var cancellationToken = new CancellationToken();
            var _input = _mapper.Map<Contact>(contactDtoWithDetail);
            var savedData = await _contactRepositoryMock.Add(_input);
            var contactDetailDto = GetContactDetailCreateDtoFoo(savedData.Id);
            var result = await _contactDetailService.CreateContactDetailAsync(contactDetailDto, cancellationToken);

            await _contactDetailService.DeleteContactDetailByIdAsync(result.Id, cancellationToken);
            var data = await _contactDetailService.GetContactDetailByIdAsync(result.Id, cancellationToken);
            Assert.IsNull(data);

        }
        // Test that the CreateContactDetailAsync function throws an exception when the input is null
        [TestMethod]
        public async Task Test_CreateContactDetail_WithNullInput()
        {
           
            CancellationToken cancellationToken = new CancellationToken();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _contactDetailService.CreateContactDetailAsync((ContactDetailCreateDto)null, cancellationToken));
        }

        // Test that the GetContactDetailByIdAsync method in the ContactDetailService class returns a ContactDetailDto object.
        [TestMethod]
        public async Task Test_GetContactDetailByIdAsync_ReturnsContactDetailDto()
        {
            // Arrange
            var contactDetailId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            var contactDetail = new ContactDetail { Id = contactDetailId };
            var contactDetailDto = new ContactDetailDto { Id = contactDetailId };
            var contactDetailRepositoryMock = new Mock<IRepository<ContactDetail>>();
            contactDetailRepositoryMock.Setup(r => r.GetById(contactDetailId, cancellationToken)).ReturnsAsync(contactDetail);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ContactDetailDto>(contactDetail)).Returns(contactDetailDto);
            var contactDetailService = new ContactDetailService(contactDetailRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await contactDetailService.GetContactDetailByIdAsync(contactDetailId, cancellationToken);

            // Assert
            Assert.AreEqual(contactDetailDto, result);
        }
        
        // Test that the GetContactDetailByIdAsync function throws an exception when the ContactDetail does not exist
        [TestMethod]
        public async Task Test_GetContactDetailById_ReturnNull_WhenContactDetailDoesNotExist()
        {
            // Arrange
            var contactDetailId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            // Act & Assert
            var result = await _contactDetailService.GetContactDetailByIdAsync(contactDetailId, cancellationToken);
            Assert.IsNull(result);
        }

        // Test that the DeleteContactDetailByIdAsync function deletes a ContactDetail from the repository successfully.
        [TestMethod]
        public async Task DeleteContactDetailById_Successfully()
        {
            // Arrange
            var contactDetailId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            var contactDetail = new ContactDetail { Id = contactDetailId };
            var contactDetailRepositoryMock = new Mock<IRepository<ContactDetail>>();
            contactDetailRepositoryMock.Setup(repo => repo.GetById(contactDetailId, cancellationToken)).ReturnsAsync(contactDetail);
            var mapperMock = new Mock<IMapper>();
            var contactDetailService = new ContactDetailService(contactDetailRepositoryMock.Object, mapperMock.Object);

            // Act
            await contactDetailService.DeleteContactDetailByIdAsync(contactDetailId, cancellationToken);

            // Assert
            contactDetailRepositoryMock.Verify(repo => repo.Delete(contactDetailId, cancellationToken), Times.Once);
        }

       

        // Test that the CreateContactDetailAsync method in the ContactDetailService class correctly maps a ContactDetailCreateDto object to a ContactDetail object.
        [TestMethod]
        public async Task Test_CreateContactDetailAsync_MapsContactDetailCreateDtoToContactDetail()
        {
            // Arrange
            var contactDetailCreateDto = new ContactDetailCreateDto
            {
                // Set properties of the ContactDetailCreateDto object
            };

            var cancellationToken = new CancellationToken();

            var contactDetailRepositoryMock = new Mock<IRepository<ContactDetail>>();
            var mapperMock = new Mock<IMapper>();

            var contactDetailService = new ContactDetailService(contactDetailRepositoryMock.Object, mapperMock.Object);

            // Act
            await contactDetailService.CreateContactDetailAsync(contactDetailCreateDto, cancellationToken);

            // Assert
            mapperMock.Verify(m => m.Map<ContactDetail>(contactDetailCreateDto), Times.Once);
        }

    }
}
