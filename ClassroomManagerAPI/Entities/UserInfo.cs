namespace ClassroomManagerAPI.Entities
{
	public class UserInfo : BaseEntity
	{
        public string FirstName { get; set; }
		public string LastName { get; set; }
        public string FullName {  get; private set; }
		public string Dob { get; set; }
		public string Avatar { get; set; }

		public UserInfo(string firstName, string lastName)
		{
			FirstName = firstName;
			LastName = lastName;
			FullName = FirstName + " " + LastName;
		}

    }
}
