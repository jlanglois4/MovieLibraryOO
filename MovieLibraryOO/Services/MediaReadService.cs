using System;
using System.Collections.Generic;

namespace MovieLibraryOO
{
    public class MediaReadService
    {
        public void ListMedia(List<string> list)
        {
            if (list.Count != 0)
            {
                int startPoint = 0;
                int perPage = 0;
                Console.WriteLine("Enter the starting point for movies.");
                while (true)
                {
                    try
                    {
                        startPoint = Convert.ToInt32(Console.ReadLine());
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Enter a valid number.");
                    }
                }
                
                Console.WriteLine("Enter how many movies you would like to see page.");
                while (true)
                {
                    try
                    {
                        perPage = Convert.ToInt32(Console.ReadLine());
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Enter a valid number.");
                    }
                    
                }
                
                while (list.Count != startPoint)
                {
                    if (startPoint < (list.Count - perPage))
                    {
                        for (int i = startPoint; i < (startPoint + perPage); i++)
                        {
                            Console.WriteLine(list[i]);
                        }
                        startPoint += perPage;
                        
                        Console.WriteLine("Enter 1 to exit. Enter anything else to continue.");
                        var lineRead = Console.ReadLine();
                        if (lineRead.Equals("1"))
                        {
                            startPoint = list.Count;
                            Console.WriteLine("Exit.");
                        }
                        else
                        {
                            Console.WriteLine("Continue.");
                        }
                    }
                    else
                    {
                        perPage = (list.Count - startPoint);
                        for (int i = 0; i < perPage; i++)
                        {
                            Console.WriteLine(list[i + startPoint]);
                        }

                        startPoint = list.Count;
                    }
                }
            }
        }
    }
}