using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.AuthDTO
{
    public class TestDTO
    {
        public class TestCreateDto
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public int CourseId { get; set; }
        }

       
        public class TestUpdateDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int CourseId { get; set; }
        }
        public class TestItemDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int CourseId { get; set; }
        }
    }
}
