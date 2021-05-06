using PhyndAPI.DATA.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyndAPI.DATA.Repositories
{
    public class PGCommentRepository : Repository
    {
        public PGCommentRepository() : base("PhyndAPI")
        {
        }
        public IEnumerable<PGComment> GetLastYearComments(List<string> NPI, int Comments, int Page, int PageSize)
        {
            string query = "WITH cteComments AS "
                         + "(SELECT "
                         + "  ROW_NUMBER() OVER(PARTITION BY[NPI] ORDER BY[NPI]) as s_index, "
                         + "  NPI, "
                         + "  Received_Date AS CommentDateTime, "
                         + "  Survey_ID AS QuestionId, "
                         + "  RATING AS StarRating, "
                         + "  Comment "
                         + "FROM dbo.V_PG_TS_Comments "
                         + "WHERE(ISNULL(@NPI, '') = '' OR @NPI LIKE '%|' + NPI + '|%') "
                         + "  AND Received_Date BETWEEN DATEADD(YEAR, -1, getdate()) AND getdate()), "
                         + "cteLimitedComments AS "
                         + "(SELECT "
                         + "  ROW_NUMBER() OVER(ORDER BY CommentDateTime DESC) as s_index, "
                         + "  NPI, "
                         + "  CommentDateTime, "
                         + "  QuestionId, "
                         + "  StarRating, "
                         + "  Comment "
                         + "FROM cteComments "
                         + "WHERE s_index <= @Comments), "
                         + "cteTotal AS "
                         + "(SELECT COUNT(1) as TotalRecords FROM cteLimitedComments) "

                         + "SELECT "
                         + "  NPI, "
                         + "  CommentDateTime, "
                         + "  QuestionId, "
                         + "  StarRating, "
                         + "  Comment, "
                         + "  CASE WHEN ((@Page - 1) * @PageSize) + 1 > TotalRecords THEN 0 ELSE ((@Page - 1) * @PageSize) + 1 END as startRecord, "
                         + "  CASE WHEN ((@Page - 1) * @PageSize) + 1 > TotalRecords THEN 0 WHEN TotalRecords < (@Page * @PageSize) THEN TotalRecords ELSE (@Page * @PageSize) END as endRecord, "
                         + "  TotalRecords "
                         + "FROM cteLimitedComments "
                         + "  CROSS JOIN cteTotal "
                         + "WHERE s_index > ((@Page - 1) * @PageSize) "
                         + "  AND s_index <= (@Page * @PageSize) "
                         + "ORDER BY CommentDateTime DESC";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("NPI", (NPI == null || NPI.Count == 0 || (NPI.Count == 1 && NPI[0] == null)) ? "" : "|" + String.Join("|", NPI) + "|");
            parameters.Add("Comments", Comments);
            parameters.Add("Page", Page);
            parameters.Add("PageSize", PageSize);
            
            return GetListByQuery<PGComment>(query, parameters).ToList();
        }
    }
}
