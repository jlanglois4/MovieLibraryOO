using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CsvHelper;
using MovieLibraryOO.Context;
using MovieLibraryOO.DataModels;

namespace MovieLibraryOO.Services
{
    public class OccupationService
    {
        private MovieContext _db;
        public Occupation Occupation { get; private set; }
        private int _selection;

        public OccupationService(MovieContext db)
        {
            this._db = db;
        }


        public string GetOccupationName()
        {
            string occupationName;
            bool validChoice = false;
            do
            {
                Console.WriteLine("Select your occupation: ");

                List<string> occupationList = new List<string>();
                foreach (var occupation in _db.Occupations)
                {
                    occupationList.Add(occupation.Name);
                }
            
                occupationList.Sort();

                int id = 1;
                for (int i = 0; i < occupationList.Count; i++)
                {
                    try
                    {
                        Console.Write("{1}. {0,-16} ", occupationList[i++], id++);
                        Console.WriteLine("{1}. {0, -17}", occupationList[i], id++);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine();
                        // ignored
                    }
                }

                _selection = Convert.ToInt32(Console.ReadLine());

                occupationName = occupationList[_selection - 1];

                if (_selection > occupationList.Count + 1)
                {
                    Console.WriteLine("Enter a valid option.");
                }
                else
                {
                    validChoice = true;
                }
            
            } while (!validChoice);

            return occupationName;
        }

        private int GetSelection()
        {
            return _selection;
        }
        public void SetOccupation()
        {
            
            
            Occupation newOccupation = new Occupation();

            
            newOccupation.Name = GetOccupationName();
            newOccupation.Id = GetSelection();

            Occupation = newOccupation;
            bool exists = false;
            foreach (var occupation in _db.Occupations)
            {
                if (occupation.Id == newOccupation.Id && occupation.Name == newOccupation.Name)
                {
                    Occupation = occupation;
                    break;
                }
            }
        }
    }
}