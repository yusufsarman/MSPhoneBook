using AutoMapper;
using ContactApi.Controllers;
using ContactApi.Enums;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.IntegrationEvents.EventHandlers;
using ContactApi.IntegrationEvents.Events;
using ContactApi.Mapping;
using ContactApi.Model.ValidateObjects;
using ContactDetailApi.Controllers;
using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApi.UnitTest
{
    [TestClass]
    public class IntegrationEventHandlerTests
    {
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly Mock<IContactDetailService> _contactDetailServiceMock;
        private readonly IMapper _mapper;
        private readonly ContactController _contactController;
        private readonly ContactDetailController _contactDetailController;
        private readonly Mock<IEventBus> _eventBusMock;

        public IntegrationEventHandlerTests()
        {
            _contactServiceMock = new Mock<IContactService>();
            _contactDetailServiceMock = new Mock<IContactDetailService>();
            _eventBusMock=new Mock<IEventBus>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _contactController = new ContactController(_contactServiceMock.Object);
            _contactDetailController = new ContactDetailController(_contactDetailServiceMock.Object);

        }

        [TestMethod]
        public async Task Handle_ValidEvent_PublishesEventAndLogs()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            var integrationEvent = new ReportStartedIntegrationEvent
            {
                ReportId = id
            };
            _contactDetailServiceMock.Setup(x => x.GetListAsync(cancellationToken))
            .ReturnsAsync(GetContactDetailsFoo(id));
            _eventBusMock.Setup(bus => bus.Publish(It.IsAny<ReportCreatedIntegrationEvent>())).Verifiable();           
            
            var loggerMock = new Mock<ILogger<ReportStartedIntegrationEvent>>();

           
            var handler = new ReportStartedIntegrationEventHandler(
                _contactDetailServiceMock.Object,
                _eventBusMock.Object,
                loggerMock.Object
            );
           
            await handler.Handle(integrationEvent);

           
            _eventBusMock.Verify(bus => bus.Publish(It.IsAny<ReportCreatedIntegrationEvent>()), Times.Once);

           
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
                new ContactDetailDto
                {
                    Id = id,
                    ContactId = id,
                    ContactDetailType=Enums.ContactDetailTypeEnum.Location,
                    Content="Ankara"

                },

            };
        }
    }
}
