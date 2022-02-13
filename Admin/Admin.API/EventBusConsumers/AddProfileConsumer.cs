using Admin.Application.Features.Commands.CacheProfile;
using AutoMapper;
using EventBus.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Admin.API.EventBusConsumers
{
    public class AddProfileConsumer : IConsumer<AddProfileEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<AddProfileConsumer> _logger;

        public AddProfileConsumer(IMediator mediator, IMapper mapper, ILogger<AddProfileConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AddProfileEvent> context)
        {
            var command = JsonConvert.DeserializeObject<CacheProfileCommand>(context.Message.Data);
            await _mediator.Send(command);

            //var escommand = JsonConvert.DeserializeObject<ESCacheProfileCommand>(context.Message.Data);
            //_ = _mediator.Send(escommand);
        }
    }
}
