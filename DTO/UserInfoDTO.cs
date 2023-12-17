using CoolMate.Models;
using System.Xml.Linq;

namespace CoolMate.DTO
{
    public class UserInfoDTO
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }

        public UserInfoDTO(string name, string username, string email, string phoneNumber, string birthday, string gender, int? weight, int? height)
        {
            Name = name;
            Username = username;
            Email = email;
            PhoneNumber = phoneNumber;
            Birthday = birthday;
            Gender = gender;
            Weight = weight;
            Height = height;
        }

        public UserInfoDTO() { }
        public UserInfoDTO(SiteUser siteUser)
        {
            Name = siteUser.Name;
            Username = siteUser.UserName;
            Email = siteUser.Email;
            PhoneNumber = siteUser.PhoneNumber;
            Birthday = siteUser.Birthday;
            Gender = siteUser.Gender;
            Weight = siteUser.Weight;
            Height = siteUser.Height;
        }
    }
}
