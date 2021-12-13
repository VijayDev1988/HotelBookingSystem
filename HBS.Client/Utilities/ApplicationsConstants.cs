using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBS.Client.Utilities
{
    public class ApplicationsConstants
    {
        public const string BOOKING_HISTORY_API = "/api/v1/RoomBooking/BookingHistory?mailId=";
        public const string BOOK_ROOM_API = "/api/v1/RoomBooking/BookRoom";
        public const string REGISTER_USER_API = "/api/v1/Account/register";
        public const string EXTERNAL_LOGIN_API = "/api/v1/Account/ExternalLoginSignIn";
        public const string FIND_USER_API = "/api/v1/Account/FindUser?emailId=";
        public const string CALLBACK_API = "https://localhost:44377/api/v1/Account/ExternalCallback";
        public const string AUTHENTICATE_USER_API = "/api/v1/Account/Authenticate";


        public const string JWT_TOKEN = "token";
        public const string EMAIL = "email";
        public const string PASSWORD_REGEX = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";

    }
}
