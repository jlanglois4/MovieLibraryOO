using System;
using MovieLibraryOO.Context;
using MovieLibraryOO.DataModels;

namespace MovieLibraryOO.Services
{
    public class UserService
    {
        private MovieContext _db;
        public UserService(MovieContext db)
        {
            this._db = db;
        }

        public void AddUser()
        {
            bool validChoice = false;
            long age = 0;
            string gender = null;
            string zipCode = null;
            do
            {
                try
                {
                    Console.WriteLine("Enter your age: ");
                    age = Convert.ToInt32(Console.ReadLine());
                    if (age != 0)
                    {
                        validChoice = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number greater than 0.");
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            } while (!validChoice);


            do
            {
                try
                {
                    validChoice = false;
                    Console.WriteLine("Enter your gender: (M/F)");
                    gender = Console.ReadLine();

                    if (gender != "M" && gender != "F")
                    {
                        Console.WriteLine("Please enter a valid gender.");
                    }
                    else
                    {
                        validChoice = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid gender.");
                }
            } while (!validChoice);

            do
            {
                try
                {
                    validChoice = false;
                    Console.WriteLine("Enter your ZIP code: ");
                    zipCode = Console.ReadLine();


                    int canBeInt = Convert.ToInt32(zipCode);
                    if (zipCode.Length != 5 && zipCode != "" && canBeInt / 1 == canBeInt)
                    {
                        Console.WriteLine("Please enter a valid ZIP code.");
                    }
                    else
                    {
                        validChoice = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid ZIP code.");
                }
            } while (!validChoice);
            

            OccupationService occupationService = new OccupationService(_db);
            occupationService.SetOccupation();
            Occupation occupation = occupationService.Occupation;

            User user = null;
            if (age != 0 && gender != null && zipCode.Length == 5)
            {
                user = new User {Age = age, Gender = gender, ZipCode = zipCode, Occupation = occupation};
            }
            else
            {
                Console.WriteLine("Error adding user.");
            }

            if (user != null)
            {
                _db.AddUser(user);
                Console.WriteLine("User added: \n" +
                                  $"{user.Display()}");
            }
            else
            {
                Console.WriteLine("Error adding user.");
            }
        }
        
        
    }
}