using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Profile.Application.Contracts;
using Profile.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Profile.Application.Features.Queries.GetProfile
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileVM>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetProfileQueryHandler> _logger;

        public GetProfileQueryHandler(IMapper mapper, ILogger<GetProfileQueryHandler> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public Task<ProfileVM> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
