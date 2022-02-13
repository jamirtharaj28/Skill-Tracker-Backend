using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Profile.Application.Contracts;
using Profile.Domain.Entities;
using SkillTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Profile.Application.Features.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, string>
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProfileCommandHandler> _logger;

        public UpdateProfileCommandHandler(ISkillRepository skillRepo, IMapper mapper, ILogger<UpdateProfileCommandHandler> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _skillRepository = skillRepo;
        }


        public async Task<string> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var existingSkills = await _skillRepository.GetItems(request.EmpId);

            List<Task> tasks = new List<Task>();

            existingSkills.ForEach(s =>
            {
                tasks.Add(_skillRepository.DeleteAsync(s.EmpId, s.SkillId));
            });

            Task.WaitAll(tasks.ToArray());

            var skillentitiess = request.Skills.Select(s => _mapper.Map<SkillEntity>(s)).ToList();
            skillentitiess.ForEach(s => s.EmpId = request.EmpId);

            await _skillRepository.AddRangeAsync(skillentitiess);

            _logger.LogInformation($"Profile {request.EmpId} is successfully updated.");

            return request.EmpId;
        }
    }
}
