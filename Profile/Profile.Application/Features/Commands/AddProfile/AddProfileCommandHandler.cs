using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Profile.Application.Contracts;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using SkillTracker.Entities;
using System.Linq;
using EventBus.Messaging.Events;
using Profile.Domain.Entities;

namespace Profile.Application.Features.Commands.AddProfile
{
    public class AddProfileCommandHandler : IRequestHandler<AddProfileCommand, string>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddProfileCommandHandler> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public AddProfileCommandHandler(
            IMapper mapper,
            ILogger<AddProfileCommandHandler> logger,
            IPublishEndpoint publishEndpoint,
            IProfileRepository profileRepository
         )
        {
            _mapper = mapper;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _profileRepository = profileRepository;
        }

        public async Task<string> Handle(AddProfileCommand request, CancellationToken cancellationToken)
        {
            var userId1 = await SaveProfileInfo(request);
            var userId = await SavePersonalInfo(request);
            await SaveSkills(request);

            _logger.LogInformation($"Profile {request.EmpId} is successfully created.");

            var eventMessage = new AddProfileEvent
            {
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(request)
            };

            //await _publishEndpoint.Publish<AddProfileEvent>(eventMessage);

            return userId;
        } 

        private async Task<string> SaveProfileInfo(AddProfileCommand request)
        {
            var profileInfo = _mapper.Map<ProfileEntity>(request);
            profileInfo.UserId = $"user{profileInfo.EmpId.ToUpper().Replace("CTS", "")}";
            profileInfo.CreatedDate = System.DateTime.UtcNow;
            profileInfo.LastModifiedDate = System.DateTime.UtcNow;

            await _profileRepository.AddAsync(profileInfo);

            return profileInfo.UserId;
        }

        private async Task<string> SavePersonalInfo(AddProfileCommand request)
        {
            var personalInfo = _mapper.Map<PersonalInfoEntity>(request);
            personalInfo.UserId = $"user{personalInfo.EmpId.ToUpper().Replace("CTS", "")}";
            personalInfo.CreatedDate = System.DateTime.UtcNow;
            personalInfo.LastModifiedDate = System.DateTime.UtcNow;

            return personalInfo.UserId;
        }

        private async Task SaveSkills(AddProfileCommand request)
        {
            var skills = request.Skills.Select(s => _mapper.Map<SkillEntity>(s)).ToList();
            skills.ForEach(s =>
            {
                s.EmpId = request.EmpId;
                s.CreatedDate = System.DateTime.UtcNow;
                s.LastModifiedDate = System.DateTime.UtcNow;
            });
        }
    }
}
