using static Gemini.Program;

namespace Gemini
{
class Program
    {
        static void Main(string[] args)
        {
            MovieCollection movieCollection = new MovieCollection();
            MemberCollection memberCollection = new MemberCollection();
            // Test cases


            while (true)
            {
                Console.WriteLine("Welcome to the Community Library Movie DVD Management System");
                Console.WriteLine("1. Login as Staff");
                Console.WriteLine("2. Login as Member");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        if (StaffLogin())
                        {
                            StaffMenu(movieCollection, memberCollection);
                        }
                        break;
                    case 2:
                        Member member = MemberLogin(memberCollection);
                        if (member != null)
                        {
                            MemberMenu(movieCollection, memberCollection, member);
                        }
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void StaffMenu(MovieCollection movieCollection, MemberCollection memberCollection)
        {
            bool continueLoop = true;

            do
            {
                Console.WriteLine("Staff Menu");
                Console.WriteLine("1. Add Movie DVD");
                Console.WriteLine("2. Remove Movie DVD");
                Console.WriteLine("3. Register Member");
                Console.WriteLine("4. Remove Member");
                Console.WriteLine("5. Find Member");
                Console.WriteLine("6. Find Members Renting Movie");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddMovieDVD(movieCollection);
                        break;
                    case 2:
                        RemoveMovieDVD(movieCollection);
                        break;
                    case 3:
                        RegisterMember(memberCollection);
                        break;
                    case 4:
                        RemoveMember(memberCollection);
                        break;
                    case 5:
                        FindMember(memberCollection);
                        break;
                    case 6:
                        FindMembersRentingMovie(memberCollection);
                        break;
                    case 7:
                        return; // Exit the staff menu
                    case 0:
                        continueLoop = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            } while (continueLoop);
        }

        static void MemberMenu(MovieCollection movieCollection, MemberCollection memberCollection, Member currentMember)
        {
            while (true)
            {
                Console.WriteLine("Member Menu");
                Console.WriteLine("1. Display All Movies");
                Console.WriteLine("2. Display Movie Information");
                Console.WriteLine("3. Borrow Movie");
                Console.WriteLine("4. Return Movie");
                Console.WriteLine("5. List Borrowed Movies");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        DisplayAllMovies(movieCollection);
                        break;
                    case 2:
                        DisplayMovieInformation(movieCollection);
                        break;
                    case 3:
                        BorrowMovie(movieCollection, memberCollection, currentMember);        
                        break;
                    case 4:
                        ReturnMovie(movieCollection, memberCollection, currentMember);
                        break;
                    case 5:
                        ListBorrowedMovies(currentMember);
                        break;
                    case 6:
                        return; // Exit the member menu
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static bool StaffLogin()
        {
            Console.Write("Enter staff username: ");
            string username = Console.ReadLine();
            Console.Write("Enter staff password: ");
            string password = Console.ReadLine();

            // Replace with actual staff credentials (e.g., using a database)
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

        static Member MemberLogin(MemberCollection memberCollection)
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            Member member = memberCollection.GetMember(firstName, lastName);
            if (member == null || member.Password != password)
            {
                Console.WriteLine("Invalid login credentials.");
                return null;
            }

            Console.WriteLine("Member login successful.");
            return member;
        }

        static void AddMovieDVD(MovieCollection movieCollection)
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine();

            // Check if movie already exists
            if (movieCollection.GetMovie(title) != null)
            {
                Console.WriteLine("Movie already exists.");
                return;
            }

            Console.Write("Enter genre: ");
            string genre = Console.ReadLine();
            Console.Write("Enter classification: ");
            string classification = Console.ReadLine();
            Console.Write("Enter duration: ");
            int duration = int.Parse(Console.ReadLine());
            Console.Write("Enter number of copies: ");
            int copies = int.Parse(Console.ReadLine());

            Movie movie = new Movie(title, genre, classification, duration, copies);
            movieCollection.AddMovie(movie);

            Console.WriteLine("Movie added successfully.");
        }
        static void RemoveMovieDVD(MovieCollection movieCollection)
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine();

            Movie movie = movieCollection.GetMovie(title);
            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
                return;
            }

            Console.Write("Enter number of copies to remove: ");
            int copiesToRemove = int.Parse(Console.ReadLine());

            if (copiesToRemove > movie.AvailableCopies)
            {
                Console.WriteLine("Cannot remove more copies than available.");
                return;
            }

            movie.AvailableCopies -= copiesToRemove;
            if (movie.AvailableCopies == 0)
            {
                movieCollection.RemoveMovie(title);
                Console.WriteLine("All copies removed.");
            }
            else
            {
                Console.WriteLine("Copies removed successfully.");
            }
        }
        static void RegisterMember(MemberCollection memberCollection)
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter password: ");
        
            string password = Console.ReadLine();


            // Check if member already exists
            if (memberCollection.GetMember(firstName, lastName) != null)
            {
                Console.WriteLine("Member already exists.");
                return;
            }

            Member member = new Member(firstName, lastName, password);
            memberCollection.AddMember(member);

            Console.WriteLine("Member registered successfully.");
        }
        static void RemoveMember(MemberCollection memberCollection)
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();


            Member member = memberCollection.GetMember(firstName, lastName);
            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return;
            }

            if (member.BorrowedMovies.Count > 0)
            {
                Console.WriteLine("Member must return all borrowed movies first.");
                return;
            }

            memberCollection.RemoveMember(member);
            Console.WriteLine("Member removed successfully.");
        }
        static void FindMember(MemberCollection memberCollection)
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();


            Member member = memberCollection.GetMember(firstName, lastName);
            if (member == null)
            {
                Console.WriteLine("Member not found.");
            }
            else
            {
                Console.WriteLine("Contact phone number: " + member.ContactPhoneNumber);
            }
        }
    
        static void FindMembersRentingMovie(MemberCollection memberCollection)
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine();

            // Find members who have borrowed the movie
            List<Member> members = memberCollection.GetMembersWithBorrowedMovie(title);

            if (members.Count == 0)
            {
                Console.WriteLine("No members are currently renting this movie.");
            }
            else
            {
                Console.WriteLine("Members renting this movie:");
                foreach (Member member in members)
                {
                    Console.WriteLine(member.FirstName + " " + member.LastName);
                }
            }
        }
        static void DisplayAllMovies(MovieCollection movieCollection)
        {
            Console.WriteLine("All Movies:");
            foreach (Movie movie in movieCollection.GetAllMovies())
            {
                Console.WriteLine($"Title: {movie.Title}");
                Console.WriteLine($"Genre: {movie.Genre}");
                Console.WriteLine($"Classification: {movie.Classification}");
                Console.WriteLine($"Duration:  { movie.Duration} minutes");
                Console.WriteLine($"Available Copies: {movie.AvailableCopies}");

                Console.WriteLine();
            }
        }
        static void DisplayMovieInformation(MovieCollection movieCollection)
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine();

            Movie movie = movieCollection.GetMovie(title);
            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
            }
            else
            {
                Console.WriteLine($"Title: {movie.Title}");
                Console.WriteLine($"Genre: {movie.Genre}");
                Console.WriteLine($"Classification: {movie.Classification}");
                Console.WriteLine($"Duration: { movie.Duration} minutes");
                Console.WriteLine($"Available Copies: {movie.AvailableCopies}");

            }
        }
        static void BorrowMovie(MovieCollection movieCollection, MemberCollection memberCollection, Member member)
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine();

            Movie movie = movieCollection.GetMovie(title);
            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
                return;
            }

            if (movie.AvailableCopies == 0)
            {
                Console.WriteLine("Movie is currently unavailable.");
                return;
            }

            // Check if member has reached borrowing limit
            if (member.BorrowedMovies.Count >= 5)
            {
                Console.WriteLine("You have reached your borrowing limit.");
                return;
            }

            // Check if member is already borrowing the movie
            if (member.BorrowedMovies.Contains(title))
            {
                Console.WriteLine("You are already borrowing this movie.");
                return;
            }

            member.BorrowedMovies.Add(title);
            movie.AvailableCopies--;
            Console.WriteLine("Movie borrowed successfully.");
        }
        static void ReturnMovie(MovieCollection movieCollection, MemberCollection memberCollection, Member member)
        {
            Console.Write("Enter movie title: ");
            string title = Console.ReadLine();

            Movie movie = movieCollection.GetMovie(title);
            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
                return;
            }

            if (!member.BorrowedMovies.Contains(title))
            {
                Console.WriteLine("You are not currently borrowing this movie.");
                return;
            }

            member.BorrowedMovies.Remove(title);
            movie.AvailableCopies++;
            Console.WriteLine("Movie returned successfully.");
        }
        static void ListBorrowedMovies(Member member)
        {
            if (member.BorrowedMovies.Count == 0)
            {
                Console.WriteLine("You are not currently borrowing any movies.");
            }
            else
            {
                Console.WriteLine("Your borrowed movies:");
                foreach (string title in member.BorrowedMovies)
                {
                    Console.WriteLine(title);
                }
            }
        }


        public class Movie
        {
            public string Title { get; set; }
            public string Genre { get; set; }
            public string Classification { get; set; }
            public int Duration
            {
                get;
                set;
            }
            public int AvailableCopies { get; set; }

            public
         Movie(string title, string genre, string classification, int duration, int availableCopies)
            {
                Title = title;
                Genre = genre;
                Classification = classification;
                Duration = duration;
                AvailableCopies = availableCopies;
            }
        }
        public class MovieCollection
        {
            private Movie[] movies = new Movie[1000];

            public void AddMovie(Movie movie)
            {
                int index = GetHashCode(movie.Title) % movies.Length;
                while (movies[index] != null && movies[index].Title != movie.Title)
                {
                    index = (index + 1) % movies.Length;
                }
                movies[index] = movie;
            }

            public Movie GetMovie(string title)
            {
                int index = GetHashCode(title) % movies.Length;
                while (movies[index] != null && movies[index].Title != title)
                {
                    index = (index + 1) % movies.Length;
                }
                return movies[index];
            }

            public void RemoveMovie(string title)
            {
                int index = GetHashCode(title) % movies.Length;
                while (movies[index] != null && movies[index].Title != title)
                {
                    index = (index + 1) % movies.Length;
                }
                if (movies[index] != null)
                {
                    movies[index] = null;
                }
            }

            private int GetHashCode(string key)
            {
                // Implement a simple hash function
                int hash = 0;
                foreach (char c in key)
                {
                    hash += c;
                }
                return hash;
            }
            public List<Movie> GetAllMovies()
            {
                // 取得所有電影的列表
                List<Movie> allMovies = new List<Movie>();
                foreach (Movie movie in movies)
                {
                    if (movie != null)
                    {
                        allMovies.Add(movie);
                    }
                }
                return allMovies;
            }
        }
        public class Member
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            public
         List<string> BorrowedMovies
            { get; set; } = new List<string>();

            public Member(string firstName, string lastName, string password)
            {
                FirstName = firstName;
                LastName = lastName;
                Password = password;
            }
            public string ContactPhoneNumber { get; set; }
        }
        public class MemberCollection
        {
            private Member[] members = new Member[100];
            private int count = 0;

            public void AddMember(Member member)
            {
                if (count < members.Length)
                {
                    members[count++] = member;
                }
            }

            public Member GetMember(string firstName, string lastName)
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

            public void RemoveMember(Member member)
            {
                for (int i = 0; i < count; i++)
                {
                    if (members[i] == member)
                    {
                        members[i] = members[count - 1];
                        count--;
                        break;
                    }
                }
            }
            public List<Member> GetMembersWithBorrowedMovie(string title)
            {
                List<Member> result = new List<Member>();
                for (int i = 0; i < count; i++)
                {
                    if (members[i].BorrowedMovies.Contains(title))
                    {
                        result.Add(members[i]);
                    }
                }
                return result;
            }
        }
    }

}