using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyndAPI.DATA.Models
{
    public class PGComment
    {
        public string NPI { get; set; }
        public DateTime CommentDateTime { get; set; }
        public string QuestionId { get; set; }
        public string StarRating { get; set; }
        public string Comment { get; set; }
        public int startRecord { get; set; }
        public int endRecord { get; set; }
        public int TotalRecords { get; set; }
    }
}
