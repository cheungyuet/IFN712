
    using System;
    using System.Diagnostics.Metrics;
    using System.Collections.Generic;

    namespace Claude_AI
    {

        class Program
        {
            static MovieCollection movieCollection = new MovieCollection();
            static MemberCollection memberCollection = new MemberCollection(100);

            static void Main(string[] args)
            {
                InitializeData();

                while (true)
                {
                    Console.WriteLine("Welcome to the Community Library");
                    Console.WriteLine("1. Staff Login");
                    Console.WriteLine("2. Member Login");
                    Console.WriteLine("3. Exit");
                    Console.Write("Enter your choice: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            if (StaffLogin())
                            {
                                StaffMenu();
                            }
                            break;
                        case "2":
                            Member loggedInMember = MemberLogin();
                            if (loggedInMember != null)
                            {
                                MemberMenu(loggedInMember);
                            }
                            break;
                        case "3":
                            Console.WriteLine("Thank you for using the Community Library. Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }

            static void InitializeData()
            {
                // Add some initial movies and members for testing
                movieCollection.Add("The Shawshank", new Movie("The Shawshank", "Drama", "M15+", 142, 5));
                movieCollection.Add("The Godfather", new Movie("The Godfather", "Drama", "MA15+", 175, 3));
                movieCollection.Add("The Dark Knight", new Movie("The Dark Knight", "Action", "M15+", 152, 4));

                memberCollection.Add(new Member("John", "H", "1234567890", "1234"));
                memberCollection.Add(new Member("Tim", "S", "0987654321", "5678"));
            }

            static bool StaffLogin()
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                if (username == "staff" && password == "today123")
                {
                    Console.WriteLine("Staff login successful.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid username or password.");
                    return false;
                }
            }

            static Member MemberLogin()
            {
                Console.Write("Enter first name: ");
                string firstName = Console.ReadLine();
                Console.Write("Enter last name: ");
                string lastName = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                Member member = memberCollection.Find(firstName, lastName);
                if (member != null && member.Password == password)
                {
                    Console.WriteLine("Member login successful.");
                    return member;
                }
                else
                {
                    Console.WriteLine("Invalid member information or password.");
                    return null;
                }
            }

            static void StaffMenu()
            {
                while (true)
                {
                    Console.WriteLine("\nStaff Menu");
                    Console.WriteLine("1. Add a new movie DVD");
                    Console.WriteLine("2. Remove a movie DVD");
                    Console.WriteLine("3. Register a new member");
                    Console.WriteLine("4. Remove a member");
                    Console.WriteLine("5. Find a member's phone number");
                    Console.WriteLine("6. Find members who are currently renting a movie");
                    Console.WriteLine("7. Return to main menu");
                    Console.Write("Enter your choice: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddMovieDVD();
                            break;
                        case "2":
                            RemoveMovieDVD();
                            break;
                        case "3":
                            RegisterNewMember();
                            break;
                        case "4":
                            RemoveMember();
                            break;
                        case "5":
                            FindMemberPhone();
                            break;
                        case "6":
                            FindMembersRentingMovie();
                            break;
                        case "7":
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            static void RemoveMovieDVD()
            {
                Console.Write("Enter the title of the movie to remove: ");
                string title = Console.ReadLine();
                Movie movie = movieCollection.Get(title);

                if (movie != null)
                {
                    Console.Write($"Enter the number of copies to remove (current copies: {movie.AvailableCopies}): ");
                    int copiesToRemove = int.Parse(Console.ReadLine());

                    if (copiesToRemove <= movie.AvailableCopies)
                    {
                        movie.AvailableCopies -= copiesToRemove;
                        if (movie.AvailableCopies == 0)
                        {
                            movieCollection.Remove(title);
                            Console.WriteLine("All copies removed. Movie deleted from the system.");
                        }
                        else
                        {
                            Console.WriteLine($"Removed {copiesToRemove} copies. Remaining copies: {movie.AvailableCopies}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Trying to remove more copies than available.");
                    }
                }
                else
                {
                    Console.WriteLine("Movie not found in the system.");
                }
            }

            static void RegisterNewMember()
            {
                Console.Write("Enter first name: ");
                string firstName = Console.ReadLine();
                Console.Write("Enter last name: ");
                string lastName = Console.ReadLine();
                Console.Write("Enter contact number: ");
                string contactNumber = Console.ReadLine();
                Console.Write("Enter 4-digit password: ");
                string password = Console.ReadLine();

                if (password.Length != 4 || !int.TryParse(password, out _))
                {
                    Console.WriteLine("Error: Password must be a 4-digit number.");
                    return;
                }

                if (memberCollection.Find(firstName, lastName) == null)
                {
                    Member newMember = new Member(firstName, lastName, contactNumber, password);
                    memberCollection.Add(newMember);
                    Console.WriteLine("New member registered successfully.");
                }
                else
                {
                    Console.WriteLine("Error: Member already exists.");
                }
            }

            static void RemoveMember()
            {
                Console.Write("Enter first name of the member to remove: ");
                string firstName = Console.ReadLine();
                Console.Write("Enter last name of the member to remove: ");
                string lastName = Console.ReadLine();

                Member member = memberCollection.Find(firstName, lastName);
                if (member != null)
                {
                    if (member.GetBorrowedMovies().Count == 0)
                    {
                        memberCollection.Remove(firstName, lastName);
                        Console.WriteLine("Member removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Error: Member still has borrowed movies. Please return all movies before removing the member.");
                    }
                }
                else
                {
                    Console.WriteLine("Member not found.");
                }
            }

            static void FindMemberPhone()
            {
                Console.Write("Enter first name: ");
                string firstName = Console.ReadLine();
                Console.Write("Enter last name: ");
                string lastName = Console.ReadLine();

                Member member = memberCollection.Find(firstName, lastName);
                if (member != null)
                {
                    Console.WriteLine($"Contact number for {firstName} {lastName}: {member.ContactNumber}");
                }
                else
                {
                    Console.WriteLine("Member not found.");
                }
            }

            static void FindMembersRentingMovie()
            {
                Console.Write("Enter the title of the movie: ");
                string title = Console.ReadLine();

                Movie movie = movieCollection.Get(title);
                if (movie != null)
                {
                    bool found = false;
                    foreach (Member member in memberCollection.GetAllMembers())
                    {
                        if (member.GetBorrowedMovies().Exists(m => m.Title == title))
                        {
                            Console.WriteLine($"{member.FirstName} {member.LastName} is currently renting this movie.");
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        Console.WriteLine("No members are currently renting this movie.");
                    }
                }
                else
                {
                    Console.WriteLine("Movie not found in the system.");
                }
            }

            static void BorrowMovie(Member member)
            {
                Console.Write("Enter the title of the movie to borrow: ");
                string title = Console.ReadLine();

                Movie movie = movieCollection.Get(title);
                if (movie != null)
                {
                    if (movie.AvailableCopies > 0)
                    {
                        if (member.BorrowMovie(movie))
                        {
                            movie.AvailableCopies--;
                            Console.WriteLine($"Successfully borrowed '{title}'.");
                        }
                        else
                        {
                            Console.WriteLine("Error: You have reached the maximum number of borrowed movies or already borrowed this title.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: No copies available for this movie.");
                    }
                }
                else
                {
                    Console.WriteLine("Movie not found in the system.");
                }
            }

            static void ReturnMovie(Member member)
            {
                Console.Write("Enter the title of the movie to return: ");
                string title = Console.ReadLine();

                if (member.ReturnMovie(title))
                {
                    Movie movie = movieCollection.Get(title);
                    if (movie != null)
                    {
                        movie.AvailableCopies++;
                        Console.WriteLine($"Successfully returned '{title}'.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: You haven't borrowed this movie.");
                }
            }

            static void ListBorrowedMovies(Member member)
            {
                List<Movie> borrowedMovies = member.GetBorrowedMovies();
                if (borrowedMovies.Count > 0)
                {
                    Console.WriteLine("Currently borrowed movies:");
                    foreach (Movie movie in borrowedMovies)
                    {
                        Console.WriteLine(movie);
                    }
                }
                else
                {
                    Console.WriteLine("You haven't borrowed any movies.");
                }
            }

            static void DisplayMovieInfo()
            {
                Console.Write("Enter the title of the movie: ");
                string title = Console.ReadLine();

                Movie movie = movieCollection.Get(title);
                if (movie != null)
                {
                    Console.WriteLine(movie);
                }
                else
                {
                    Console.WriteLine("Movie not found in the system.");
                }
            }

            static void MemberMenu(Member member)
            {
                while (true)
                {
                    Console.WriteLine($"\nWelcome, {member.FirstName} {member.LastName}");
                    Console.WriteLine("1. Display all movies");
                    Console.WriteLine("2. Borrow a movie DVD");
                    Console.WriteLine("3. Return a movie DVD");
                    Console.WriteLine("4. List current borrowed movie DVDs");
                    Console.WriteLine("5. Display movie information");
                    Console.WriteLine("6. Return to main menu");
                    Console.Write("Enter your choice: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            DisplayAllMovies();
                            break;
                        case "2":
                            BorrowMovie(member);
                            break;
                        case "3":
                            ReturnMovie(member);
                            break;
                        case "4":
                            ListBorrowedMovies(member);
                            break;
                        case "5":
                            DisplayMovieInfo();
                            break;
                        case "6":
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }

            // Implement the methods for each menu option here
            // For example:

            static void AddMovieDVD()
            {
                Console.Write("Enter movie title: ");
                string title = Console.ReadLine();

                Movie existingMovie = movieCollection.Get(title);
                if (existingMovie != null)
                {
                    Console.Write("Enter number of new copies: ");
                    int newCopies = int.Parse(Console.ReadLine());
                    existingMovie.AvailableCopies += newCopies;
                    Console.WriteLine("Copies added successfully.");
                }
                else
                {
                    Console.Write("Enter genre: ");
                    string genre = Console.ReadLine();
                    Console.Write("Enter classification: ");
                    string classification = Console.ReadLine();
                    Console.Write("Enter duration (minutes): ");
                    int duration = int.Parse(Console.ReadLine());
                    Console.Write("Enter number of copies: ");
                    int copies = int.Parse(Console.ReadLine());

                    Movie newMovie = new Movie(title, genre, classification, duration, copies);
                    movieCollection.Add(title, newMovie);
                    Console.WriteLine("New movie added successfully.");
                }
            }

            // Implement other methods similarly...

            static void DisplayAllMovies()
            {
                Movie[] allMovies = movieCollection.GetAllMovies();
                foreach (Movie movie in allMovies)
                {
                    Console.WriteLine(movie);
                }
            }
        }

        public class MovieCollection
        {
            private const int MaxSize = 1000;
            private Movie[] movies;
            private string[] keys;
            private int count;

            public MovieCollection()
            {
                movies = new Movie[MaxSize];
                keys = new string[MaxSize];
                count = 0;
            }

            private int Hash(string key)
            {
                int total = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    total += (int)key[i];
                }
                return total % MaxSize;
            }

            public void Add(string key, Movie movie)
            {
                int index = Hash(key);
                while (keys[index] != null && keys[index] != key)
                {
                    index = (index + 1) % MaxSize;
                }
                if (keys[index] == null)
                {
                    keys[index] = key;
                    movies[index] = movie;
                    count++;
                }
            }

            public Movie Get(string key)
            {
                int index = Hash(key);
                while (keys[index] != null)
                {
                    if (keys[index] == key)
                    {
                        return movies[index];
                    }
                    index = (index + 1) % MaxSize;
                }
                return null;
            }

            public void Remove(string key)
            {
                int index = Hash(key);
                while (keys[index] != null)
                {
                    if (keys[index] == key)
                    {
                        keys[index] = null;
                        movies[index] = null;
                        count--;
                        return;
                    }
                    index = (index + 1) % MaxSize;
                }
            }

            public int Count => count;

            public Movie[] GetAllMovies()
            {
                Movie[] allMovies = new Movie[count];
                int j = 0;
                for (int i = 0; i < MaxSize; i++)
                {
                    if (movies[i] != null)
                    {
                        allMovies[j++] = movies[i];
                    }
                }
                Array.Sort(allMovies, (a, b) => string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase));
                return allMovies;
            }
        }
        public class Movie
        {
            public string Title { get; set; }
            public string Genre { get; set; }
            public string Classification { get; set; }
            public int Duration { get; set; }
            public int AvailableCopies { get; set; }

            public Movie(string title, string genre, string classification, int duration, int availableCopies)
            {
                Title = title;
                Genre = genre;
                Classification = classification;
                Duration = duration;
                AvailableCopies = availableCopies;
            }

            public override string ToString()
            {
                return $"Title: {Title}, Genre: {Genre}, Classification: {Classification}, Duration: {Duration} minutes, Available Copies: {AvailableCopies}";
            }
        }
        public class MemberCollection
        {
            private Member[] members;
            private int count;

            public MemberCollection(int capacity)
            {
                members = new Member[capacity];
                count = 0;
            }

            public void Add(Member member)
            {
                if (count < members.Length)
                {
                    members[count++] = member;
                }
            }

            public Member Find(string firstName, string lastName)
            {
                for (int i = 0; i < count; i++)
                {
                    if (members[i].FirstName == firstName && members[i].LastName == lastName)
                    {
                        return members[i];
                    }
                }
                return null;
            }

            public bool Remove(string firstName, string lastName)
            {
                for (int i = 0; i < count; i++)
                {
                    if (members[i].FirstName == firstName && members[i].LastName == lastName)
                    {
                        members[i] = members[--count];
                        members[count] = null;
                        return true;
                    }
                }
                return false;
            }

            public Member[] GetAllMembers()
            {
                Member[] allMembers = new Member[count];
                Array.Copy(members, allMembers, count);
                return allMembers;
            }
        }
        public class Member
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ContactNumber { get; set; }
            public string Password { get; set; }
            private List<Movie> borrowedMovies;

            public Member(string firstName, string lastName, string contactNumber, string password)
            {
                FirstName = firstName;
                LastName = lastName;
                ContactNumber = contactNumber;
                Password = password;
                borrowedMovies = new List<Movie>();
            }

            public bool BorrowMovie(Movie movie)
            {
                if (borrowedMovies.Count < 5 && !borrowedMovies.Exists(m => m.Title == movie.Title))
                {
                    borrowedMovies.Add(movie);
                    return true;
                }
                return false;
            }

            public bool ReturnMovie(string movieTitle)
            {
                Movie movie = borrowedMovies.Find(m => m.Title == movieTitle);
                if (movie != null)
                {
                    borrowedMovies.Remove(movie);
                    return true;
                }
                return false;
            }

            public List<Movie> GetBorrowedMovies()
            {
                return new List<Movie>(borrowedMovies);
            }

            public override string ToString()
            {
                return $"{FirstName} {LastName}, Contact: {ContactNumber}";
            }
        }
    }