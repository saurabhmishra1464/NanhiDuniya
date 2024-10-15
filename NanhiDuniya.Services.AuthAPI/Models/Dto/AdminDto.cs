namespace NanhiDuniya.Services.AuthAPI.Models.Dto
{
    public class AdminDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public bool Status { get; set; } 
    }

}
