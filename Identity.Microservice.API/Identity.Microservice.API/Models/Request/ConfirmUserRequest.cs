﻿namespace Identity.Microservice.API.Models.Request
{
    public class ConfirmUserRequest()
    {
        public string Email { get; set; }
        public string Password{ get; set; }
        public string Code { get; set; }
    }
}
