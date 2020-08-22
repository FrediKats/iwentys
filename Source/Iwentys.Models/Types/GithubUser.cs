namespace Iwentys.Models.Types
{
    public class GithubUser
    {
        public GithubUser(string name, string avatarUrl, string bio, string company) : this()
        {
            Name = name;
            AvatarUrl = avatarUrl;
            Bio = bio;
            Company = company;
        }

        private GithubUser()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
        public string Company { get; set; }
    }
}