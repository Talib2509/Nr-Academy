using AutoMapper;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyCORE.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Mappings
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDTO, AppUser>();
        }
    }
}
