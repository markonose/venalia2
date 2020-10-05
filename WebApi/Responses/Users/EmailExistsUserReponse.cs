using System.Collections.Generic;

namespace WebApi.Responses.Users
{
    public class EmailExistsUserReponse
    {
        public EmailExistsUserReponseData Data { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class EmailExistsUserReponseData
    {
        public bool EmailExists { get; set; }
    }
}
