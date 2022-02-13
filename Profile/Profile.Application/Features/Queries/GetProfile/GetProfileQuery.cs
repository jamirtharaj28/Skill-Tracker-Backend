using MediatR;
using Profile.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Application.Features.Queries.GetProfile
{
    public class GetProfileQuery: IRequest<ProfileVM>
    {
        public string EmpId { get; set; }
    }
}
