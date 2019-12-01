namespace Darwin.Data.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Net.Mail;

    public class User : TableEntity
    {
        public User()
        {

        }

        public User(Guid userId, string userName, string firstName, string lastName, string emailAddress, string profilePhotoUri)
        {
            this.PartitionKey = userId.ToString("N");
            this.RowKey = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
            this.ProfilePhotoUrl = profilePhotoUri;
        }

        public string UserId => this.PartitionKey;

        public string UserName => this.RowKey;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string ProfilePhotoUrl { get; set; }
    }
}
