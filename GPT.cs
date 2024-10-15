using System;

namespace CommunityLibraryDVDManagement
    {
        // Enum for Movie Genres
        public enum Genre
        {
            Drama,
            Adventure,
            Family,
            Action,
            SciFi,
            Comedy,
            Animated,
            Thriller,
            Other
        }

        // Enum for Movie Classification
        public enum Classification
        {
            G,
            PG,
            M15Plus,
            MA15Plus
        }

        // Movie Class
        public class Movie
        {
            public string Title { get; set; }
            public Genre Genre { get; set; }
            public Classification Classification { get; set; }
            public int Duration { get; set; } // in minutes
            public int NumberOfCopies { get; set; }

            public Movie(string title, Genre genre, Classification classification, int duration, int numberOfCopies)
            {
                Title = title;
                Genre = genre;
                Classification = classification;
                Duration = duration;
                NumberOfCopies = numberOfCopies;
            }

            public override string ToString()
            {
                return $"Title: {Title}, Genre: {Genre}, Classification: {Classification}, Duration: {Duration} minutes, Copies Available: {NumberOfCopies}";
            }
        }

        // Node class for linked list in hash table
        public class MovieNode
        {
            public string Key { get; set; }
            public Movie Value { get; set; }
            public MovieNode Next { get; set; }

            public MovieNode(string key, Movie value)
            {
                Key = key;
                Value = value;
                Next = null;
            }
        }

        // MovieCollection Class - Simple Hash Table Implementation
        public class MovieCollection
        {
            private MovieNode[] table;
            private int size;

            public MovieCollection(int size = 1031) // 1031 is a prime number near 1000
            {
                this.size = size;
                table = new MovieNode[size];
            }

            private int GetHash(string key)
            {
                int hash = 0;
                foreach (char c in key)
                {
                    hash = (hash * 31 + c) % size;
                }
                return hash;
            }

            public bool AddMovie(Movie movie)
            {
                if (FindMovie(movie.Title) != null)
                {
                    // Movie already exists, update number of copies
                    FindMovie(movie.Title).NumberOfCopies += movie.NumberOfCopies;
                    return false; // Indicate that it's an existing movie
                }

                int index = GetHash(movie.Title);
                MovieNode newNode = new MovieNode(movie.Title, movie);

                if (table[index] == null)
                {
                    table[index] = newNode;
                }
                else
                {
                    MovieNode current = table[index];
                    while (current.Next != null)
                    {
                        current = current.Next;
                    }
                    current.Next = newNode;
                }
                return true; // New movie added
            }

            public bool RemoveMovie(string title, int numberToRemove)
            {
                int index = GetHash(title);
                MovieNode current = table[index];
                MovieNode prev = null;

                while (current != null)
                {
                    if (current.Key.Equals(title, StringComparison.OrdinalIgnoreCase))
                    {
                        if (current.Value.NumberOfCopies < numberToRemove)
                        {
                            return false; // Not enough copies to remove
                        }
                        else if (current.Value.NumberOfCopies == numberToRemove)
                        {
                            // Remove the node from the linked list
                            if (prev == null)
                            {
                                table[index] = current.Next;
                            }
                            else
                            {
                                prev.Next = current.Next;
                            }
                        }
                        else
                        {
                            current.Value.NumberOfCopies -= numberToRemove;
                        }
                        return true; // Removal successful
                    }
                    prev = current;
                    current = current.Next;
                }
                return false; // Movie not found
            }

            public Movie FindMovie(string title)
            {
                int index = GetHash(title);
                MovieNode current = table[index];
                while (current != null)
                {
                    if (current.Key.Equals(title, StringComparison.OrdinalIgnoreCase))
                    {
                        return current.Value;
                    }
                    current = current.Next;
                }
                return null; // Not found
            }

            public void DisplayAllMovies()
            {
                // Collect all movies
                Movie[] movies = new Movie[1000];
                int count = 0;
                for (int i = 0; i < size; i++)
                {
                    MovieNode current = table[i];
                    while (current != null)
                    {
                        movies[count++] = current.Value;
                        current = current.Next;
                    }
                }

                // Sort movies by title (simple bubble sort for demonstration)
                for (int i = 0; i < count - 1; i++)
                {
                    for (int j = 0; j < count - i - 1; j++)
                    {
                        if (string.Compare(movies[j].Title, movies[j + 1].Title, StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            Movie temp = movies[j];
                            movies[j] = movies[j + 1];
                            movies[j + 1] = temp;
                        }
                    }
                }

                // Display sorted movies
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine(movies[i].ToString());
                }
            }

            public bool IsEmpty()
            {
                for (int i = 0; i < size; i++)
                {
                    if (table[i] != null)
                        return false;
                }
                return true;
            }
        }

        // Member Class
        public class Member
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public string[] BorrowedMovies { get; set; }

            public Member(string firstName, string lastName, string phoneNumber, string password)
            {
                FirstName = firstName;
                LastName = lastName;
                PhoneNumber = phoneNumber;
                Password = password;
                BorrowedMovies = new string[5];
            }

            public string FullName()
            {
                return $"{FirstName} {LastName}";
            }

            public bool BorrowMovie(string title)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (BorrowedMovies[i] == null)
                    {
                        BorrowedMovies[i] = title;
                        return true;
                    }
                    else if (BorrowedMovies[i].Equals(title, StringComparison.OrdinalIgnoreCase))
                    {
                        return false; // Already borrowed this movie
                    }
                }
                return false; // Borrow limit reached
            }

            public bool ReturnMovie(string title)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (BorrowedMovies[i] != null && BorrowedMovies[i].Equals(title, StringComparison.OrdinalIgnoreCase))
                    {
                        BorrowedMovies[i] = null;
                        return true;
                    }
                }
                return false; // Movie not found in borrowed list
            }

            public bool HasBorrowed(string title)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (BorrowedMovies[i] != null && BorrowedMovies[i].Equals(title, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }

            public int BorrowedCount()
            {
                int count = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (BorrowedMovies[i] != null)
                        count++;
                }
                return count;
            }

            public void ListBorrowedMovies()
            {
                Console.WriteLine($"Borrowed Movies for {FullName()}:");
                for (int i = 0; i < 5; i++)
                {
                    if (BorrowedMovies[i] != null)
                    {
                        Console.WriteLine($"- {BorrowedMovies[i]}");
                    }
                }
            }
        }

        // MemberCollection Class
        public class MemberCollection
        {
            private Member[] members;
            private int count;
            private int size;

            public MemberCollection(int size = 1000)
            {
                this.size = size;
                members = new Member[size];
                count = 0;
            }

            public bool AddMember(Member member)
            {
                if (FindMember(member.FirstName, member.LastName) != null)
                {
                    return false; // Member already exists
                }
                if (count >= size)
                {
                    return false; // Collection full
                }
                members[count++] = member;
                return true;
            }

            public bool RemoveMember(string firstName, string lastName)
            {
                int index = -1;
                for (int i = 0; i < count; i++)
                {
                    if (members[i].FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                        members[i].LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    return false; // Member not found
                }

                // Check if member has borrowed movies
                if (members[index].BorrowedCount() > 0)
                {
                    return false; // Cannot remove member with borrowed movies
                }

                // Remove member by shifting
                for (int i = index; i < count - 1; i++)
                {
                    members[i] = members[i + 1];
                }
                members[count - 1] = null;
                count--;
                return true;
            }

            public Member FindMember(string firstName, string lastName)
            {
                for (int i = 0; i < count; i++)
                {
                    if (members[i].FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                        members[i].LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                    {
                        return members[i];
                    }
                }
                return null;
            }

            public Member FindMemberByFullName(string fullName)
            {
                for (int i = 0; i < count; i++)
                {
                    if (members[i].FullName().Equals(fullName, StringComparison.OrdinalIgnoreCase))
                    {
                        return members[i];
                    }
                }
                return null;
            }

            public void ListMembersBorrowingMovie(string title)
            {
                bool found = false;
                for (int i = 0; i < count; i++)
                {
                    if (members[i].HasBorrowed(title))
                    {
                        Console.WriteLine(members[i].FullName());
                        found = true;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("No members are currently borrowing this movie.");
                }
            }

            public void ListAllMembers()
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"Name: {members[i].FullName()}, Phone: {members[i].PhoneNumber}");
                }
            }
        }

        // Program Class
        class Program
        {
            static MovieCollection movieCollection = new MovieCollection();
            static MemberCollection memberCollection = new MemberCollection();

            static void Main(string[] args)
            {
            // Adding sample movies to the collection
            movieCollection.AddMovie(new Movie("Inception", Genre.SciFi, Classification.M15Plus, 148, 3));
            movieCollection.AddMovie(new Movie("Toy Story", Genre.Animated, Classification.G, 81, 4));
            movieCollection.AddMovie(new Movie("The Godfather", Genre.Drama, Classification.M15Plus, 175, 2));


            // Adding sample members to the collection
            memberCollection.AddMember(new Member("Candy", "C", "1029384756", "9000"));
            memberCollection.AddMember(new Member("Alice", "A", "1234567890", "1234"));
            memberCollection.AddMember(new Member("Bob", "B", "0987654321", "5678"));
            bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine("=== Community Library DVD Management System ===");
                    Console.WriteLine("1. Staff Login");
                    Console.WriteLine("2. Member Login");
                    Console.WriteLine("3. Exit");
                    Console.Write("Select an option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            StaffLogin();
                            break;
                        case "2":
                            MemberLogin();
                            break;
                        case "3":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Press Enter to continue.");
                            Console.ReadLine();
                            break;
                    }
                }
            }

            // Staff Login
            static void StaffLogin()
            {
                Console.Clear();
                Console.WriteLine("=== Staff Login ===");
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = ReadPassword();

                if (username.Equals("staff", StringComparison.OrdinalIgnoreCase) && password == "today123")
                {
                    StaffMenu();
                }
                else
                {
                    Console.WriteLine("Invalid credentials. Press Enter to return to main menu.");
                    Console.ReadLine();
                }
            }

            // Member Login
            static void MemberLogin()
            {
                Console.Clear();
                Console.WriteLine("=== Member Login ===");
                Console.Write("First Name: ");
                string firstName = Console.ReadLine();
                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();
                Console.Write("Password: ");
                string password = ReadPassword();

                Member member = memberCollection.FindMember(firstName, lastName);
                if (member != null && member.Password == password)
                {
                    MemberMenu(member);
                }
                else
                {
                    Console.WriteLine("Invalid credentials. Press Enter to return to main menu.");
                    Console.ReadLine();
                }
            }

            // Staff Menu
            static void StaffMenu()
            {
                bool logout = false;
                while (!logout)
                {
                    Console.Clear();
                    Console.WriteLine("=== Staff Menu ===");
                    Console.WriteLine("1. Add Movie DVD");
                    Console.WriteLine("2. Remove Movie DVD");
                    Console.WriteLine("3. Register New Member");
                    Console.WriteLine("4. Remove Member");
                    Console.WriteLine("5. Find Member's Contact Number");
                    Console.WriteLine("6. Find Members Borrowing a Movie");
                    Console.WriteLine("7. Logout");
                    Console.Write("Select an option: ");
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
                            FindMemberContact();
                            break;
                        case "6":
                            FindMembersBorrowingMovie();
                            break;
                        case "7":
                            logout = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Press Enter to continue.");
                            Console.ReadLine();
                            break;
                    }
                }
            }

            // Member Menu
            static void MemberMenu(Member member)
            {
                bool logout = false;
                while (!logout)
                {
                    Console.Clear();
                    Console.WriteLine($"=== Member Menu ({member.FullName()}) ===");
                    Console.WriteLine("1. Display All Movies");
                    Console.WriteLine("2. Display Movie Information");
                    Console.WriteLine("3. Borrow Movie DVD");
                    Console.WriteLine("4. Return Movie DVD");
                    Console.WriteLine("5. List Borrowed Movies");
                    Console.WriteLine("6. Logout");
                    Console.Write("Select an option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            DisplayAllMovies();
                            break;
                        case "2":
                            DisplayMovieInformation();
                            break;
                        case "3":
                            BorrowMovieDVD(member);
                            break;
                        case "4":
                            ReturnMovieDVD(member);
                            break;
                        case "5":
                            member.ListBorrowedMovies();
                            Console.WriteLine("Press Enter to continue.");
                            Console.ReadLine();
                            break;
                        case "6":
                            logout = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Press Enter to continue.");
                            Console.ReadLine();
                            break;
                    }
                }
            }

            // Add Movie DVD
            static void AddMovieDVD()
            {
                Console.Clear();
                Console.WriteLine("=== Add Movie DVD ===");
                Console.Write("Enter Movie Title: ");
                string title = Console.ReadLine();

                Console.WriteLine("Select Genre:");
                foreach (var g in Enum.GetValues(typeof(Genre)))
                {
                    Console.WriteLine($"{(int)g} - {g}");
                }
                Console.Write("Enter Genre number: ");
                Genre genre;
                if (!Enum.TryParse(Console.ReadLine(), out genre))
                {
                    Console.WriteLine("Invalid genre. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("Select Classification:");
                foreach (var c in Enum.GetValues(typeof(Classification)))
                {
                    Console.WriteLine($"{(int)c} - {c}");
                }
                Console.Write("Enter Classification number: ");
                Classification classification;
                if (!Enum.TryParse(Console.ReadLine(), out classification))
                {
                    Console.WriteLine("Invalid classification. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                Console.Write("Enter Duration in minutes: ");
                int duration;
                if (!int.TryParse(Console.ReadLine(), out duration) || duration <= 0)
                {
                    Console.WriteLine("Invalid duration. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                Console.Write("Enter Number of Copies to Add: ");
                int copies;
                if (!int.TryParse(Console.ReadLine(), out copies) || copies <= 0)
                {
                    Console.WriteLine("Invalid number of copies. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                Movie movie = new Movie(title, genre, classification, duration, copies);
                bool isNew = movieCollection.AddMovie(movie);
                if (isNew)
                {
                    Console.WriteLine("New movie added successfully.");
                }
                else
                {
                    Console.WriteLine("Existing movie updated with additional copies.");
                }
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Remove Movie DVD
            static void RemoveMovieDVD()
            {
                Console.Clear();
                Console.WriteLine("=== Remove Movie DVD ===");
                Console.Write("Enter Movie Title to Remove: ");
                string title = Console.ReadLine();

                Movie movie = movieCollection.FindMovie(title);
                if (movie == null)
                {
                    Console.WriteLine("Movie not found. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                Console.Write($"Enter number of copies to remove (Available: {movie.NumberOfCopies}): ");
                int copies;
                if (!int.TryParse(Console.ReadLine(), out copies) || copies <= 0)
                {
                    Console.WriteLine("Invalid number of copies. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                if (copies > movie.NumberOfCopies)
                {
                    Console.WriteLine("Cannot remove more copies than available. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                bool success = movieCollection.RemoveMovie(title, copies);
                if (success)
                {
                    Console.WriteLine("Movie copies removed successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to remove movie copies.");
                }
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Register New Member
            static void RegisterNewMember()
            {
                Console.Clear();
                Console.WriteLine("=== Register New Member ===");
                Console.Write("First Name: ");
                string firstName = Console.ReadLine();
                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();
                Console.Write("Phone Number: ");
                string phoneNumber = Console.ReadLine();
                Console.Write("Set 4-digit Password: ");
                string password = ReadPassword();

                if (password.Length != 4 || !int.TryParse(password, out _))
                {
                    Console.WriteLine("Password must be a 4-digit number. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                Member newMember = new Member(firstName, lastName, phoneNumber, password);
                bool added = memberCollection.AddMember(newMember);
                if (added)
                {
                    Console.WriteLine("Member registered successfully.");
                }
                else
                {
                    Console.WriteLine("Member already exists or collection full.");
                }
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Remove Member
            static void RemoveMember()
            {
                Console.Clear();
                Console.WriteLine("=== Remove Member ===");
                Console.Write("Enter Member's First Name: ");
                string firstName = Console.ReadLine();
                Console.Write("Enter Member's Last Name: ");
                string lastName = Console.ReadLine();

                Member member = memberCollection.FindMember(firstName, lastName);
                if (member == null)
                {
                    Console.WriteLine("Member not found. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                if (member.BorrowedCount() > 0)
                {
                    Console.WriteLine("Member has borrowed DVDs. They must return all DVDs before removal.");
                    Console.WriteLine("Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                bool removed = memberCollection.RemoveMember(firstName, lastName);
                if (removed)
                {
                    Console.WriteLine("Member removed successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to remove member.");
                }
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Find Member's Contact Number
            static void FindMemberContact()
            {
                Console.Clear();
                Console.WriteLine("=== Find Member's Contact Number ===");
                Console.Write("Enter Member's Full Name: ");
                string fullName = Console.ReadLine();

                Member member = memberCollection.FindMemberByFullName(fullName);
                if (member != null)
                {
                    Console.WriteLine($"Phone Number: {member.PhoneNumber}");
                }
                else
                {
                    Console.WriteLine("Member not found.");
                }
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Find Members Borrowing a Movie
            static void FindMembersBorrowingMovie()
            {
                Console.Clear();
                Console.WriteLine("=== Find Members Borrowing a Movie ===");
                Console.Write("Enter Movie Title: ");
                string title = Console.ReadLine();

                Movie movie = movieCollection.FindMovie(title);
                if (movie == null)
                {
                    Console.WriteLine("Movie not found.");
                    Console.WriteLine("Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine($"Members currently borrowing \"{title}\":");
                memberCollection.ListMembersBorrowingMovie(title);
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Display All Movies
            static void DisplayAllMovies()
            {
                Console.Clear();
                Console.WriteLine("=== All Movies in Library ===");
                if (movieCollection.IsEmpty())
                {
                    Console.WriteLine("No movies in the library.");
                }
                else
                {
                    movieCollection.DisplayAllMovies();
                }
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Display Movie Information
            static void DisplayMovieInformation()
            {
                Console.Clear();
                Console.WriteLine("=== Display Movie Information ===");
                Console.Write("Enter Movie Title: ");
                string title = Console.ReadLine();

                Movie movie = movieCollection.FindMovie(title);
                if (movie != null)
                {
                    Console.WriteLine(movie.ToString());
                }
                else
                {
                    Console.WriteLine("Movie not found.");
                }
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Borrow Movie DVD
            static void BorrowMovieDVD(Member member)
            {
                Console.Clear();
                Console.WriteLine("=== Borrow Movie DVD ===");
                if (member.BorrowedCount() >= 5)
                {
                    Console.WriteLine("Borrow limit reached. Return a movie before borrowing another.");
                    Console.WriteLine("Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                Console.Write("Enter Movie Title to Borrow: ");
                string title = Console.ReadLine();

                Movie movie = movieCollection.FindMovie(title);
                if (movie == null)
                {
                    Console.WriteLine("Movie not found.");
                    Console.WriteLine("Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                if (movie.NumberOfCopies <= 0)
                {
                    Console.WriteLine("No copies available for borrowing.");
                    Console.WriteLine("Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                if (member.HasBorrowed(title))
                {
                    Console.WriteLine("You have already borrowed this movie.");
                    Console.WriteLine("Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                bool borrowed = member.BorrowMovie(title);
                if (borrowed)
                {
                    movie.NumberOfCopies--;
                    Console.WriteLine("Movie borrowed successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to borrow movie. Possibly reached borrow limit.");
                }
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Return Movie DVD
            static void ReturnMovieDVD(Member member)
            {
                Console.Clear();
                Console.WriteLine("=== Return Movie DVD ===");
                Console.Write("Enter Movie Title to Return: ");
                string title = Console.ReadLine();

                if (!member.HasBorrowed(title))
                {
                    Console.WriteLine("You have not borrowed this movie.");
                    Console.WriteLine("Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                bool returned = member.ReturnMovie(title);
                if (returned)
                {
                    Movie movie = movieCollection.FindMovie(title);
                    if (movie != null)
                    {
                        movie.NumberOfCopies++;
                    }
                    else
                    {
                        // If movie was removed from collection, add it back with one copy
                        Movie newMovie = new Movie(title, Genre.Other, Classification.G, 0, 1);
                        movieCollection.AddMovie(newMovie);
                    }
                    Console.WriteLine("Movie returned successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to return movie.");
                }
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }

            // Utility method to read password without echoing
            static string ReadPassword()
            {
                string password = "";
                ConsoleKeyInfo info = Console.ReadKey(true);
                while (info.Key != ConsoleKey.Enter)
                {
                    if (info.Key == ConsoleKey.Backspace)
                    {
                        if (password.Length > 0)
                        {
                            password = password.Substring(0, password.Length - 1);
                            Console.Write("\b \b");
                        }
                    }
                    else
                    {
                        password += info.KeyChar;
                        Console.Write("*");
                    }
                    info = Console.ReadKey(true);
                }
                Console.WriteLine();
                return password;
            }
        }
    }
