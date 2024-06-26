﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.DataService
{
    public interface IUserDataService
    {
        Task<Status> Login(string email, string password);
        Task<Status> SignUp(User user);
        Task<Status> ForgotPassword(string emailId);
    }
}