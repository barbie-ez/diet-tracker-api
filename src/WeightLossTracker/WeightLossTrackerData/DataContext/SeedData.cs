﻿using System;
using System.Linq;
using System.Threading.Tasks;
using WeightLossTrackerData.Constants;
using WeightLossTrackerData.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace WeightLossTrackerData.DataContext
{
    public class SeedData
    {
        private static UserManager<UserProfileModel> _manager;
        private static ApplicationDbContext _context;
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            _manager = (UserManager<UserProfileModel>)serviceProvider.GetService(typeof(UserManager<UserProfileModel>));

            _context = (ApplicationDbContext)serviceProvider.GetService(typeof(ApplicationDbContext));
            InitializeVMS().GetAwaiter().GetResult();


        }

        private static async Task InitializeVMS()
        {
            if (!_context.Roles.Any())
            {
                try
                {
                    _context.Roles.Add(new UserRoleModel(AppRoles.Administrator));
                    _context.Roles.Add(new UserRoleModel(AppRoles.Member));

                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }


            if (!_context.Users.Any())
            {

                try
                {
                    UserProfileModel newUser = new UserProfileModel();
                    newUser.Email = newUser.UserName = "admisnistrator@gmail.com";
                    newUser.FirstName = newUser.UserName = "Barbara";
                    newUser.LastName = newUser.UserName = "Ezomo";
                    newUser.Height = 170.0F;
                    newUser.CurrentWeight = 89.3F;
                    newUser.DateOfBirth = new DateTimeOffset(DateTime.Now);
                    newUser.PhoneNumber = "07038875015";

                    var result = _manager.CreateAsync(newUser, "Password@1").GetAwaiter().GetResult();
                    var token = _manager.GenerateEmailConfirmationTokenAsync(newUser).GetAwaiter().GetResult();
                    var confirmEmail = _manager.ConfirmEmailAsync(newUser, token).GetAwaiter().GetResult();
                    if (confirmEmail.Succeeded)
                    {
                        if (result.Succeeded)
                        {
                            var newResult = _manager.AddToRoleAsync(newUser, AppRoles.Administrator).GetAwaiter().GetResult();
                            if (newResult.Succeeded)
                            {
                                Console.WriteLine("User created Successfully");
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }



            }

            try
            {
                UserProfileModel newUser = new UserProfileModel();
                newUser.Email = newUser.UserName = "member@gmail.com";
                newUser.PhoneNumber = "07038875015";
                newUser.FirstName = newUser.UserName = "Barbara";
                newUser.LastName = newUser.UserName = "Ezomo";
                newUser.Height = 170.0F;
                newUser.CurrentWeight = 89.3F;
                newUser.DateOfBirth = new DateTimeOffset(DateTime.Now);
                var result = _manager.CreateAsync(newUser, "Password@1").GetAwaiter().GetResult();
                var token = _manager.GenerateEmailConfirmationTokenAsync(newUser).GetAwaiter().GetResult();
                var confirmEmail = _manager.ConfirmEmailAsync(newUser, token).GetAwaiter().GetResult();
                if (confirmEmail.Succeeded)
                {
                    if (result.Succeeded)
                    {
                        var newResult = _manager.AddToRoleAsync(newUser, AppRoles.Member).GetAwaiter().GetResult();
                        if (newResult.Succeeded)
                        {
                            Console.WriteLine("User created Successfully");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }



        }
    }
}
