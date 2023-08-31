using AutoMapper;
using ContactApi.Controllers;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.Mapping;
using ContactDetailApi.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApi.UnitTest
{
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
    }
}
