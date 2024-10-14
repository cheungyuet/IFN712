using System;
using System.Collections.Generic;
using System.Linq;

public class Movie
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Classification { get; set; }
    public int Duration { get; set; }
    public int Copies { get; set; }

    public Movie(string title, string genre, string classification, int duration, int copies)
    {
        Title = title;
        Genre = genre;
        Classification = classification;
        Duration = duration;
        Copies = copies;
    }

    public override string ToString()
    {
        return $"{Title} ({Genre}, {Classification}, {Duration} mins) - {Copies} copies available";
    }
}

public class MovieCollection
{
    private Movie[] movies;
    private int count;

    public MovieCollection()
    {
        movies = new Movie[1000]; // Initialize the array with a size of 1000
        count = 0;
    }

    public void AddMovie(Movie movie)
    {
        for (int i = 0; i < count; i++)
        {
            if (movies[i].Title == movie.Title)
            {
                movies[i].Copies += movie.Copies;
                return;
            }
        }
        movies[count++] = movie;
    }

    public void RemoveMovie(string title, int copies)
    {
        for (int i = 0; i < count; i++)
        {
            if (movies[i].Title == title)
            {
                if (movies[i].Copies > copies)
                {
                    movies[i].Copies -= copies;
                }
                else
                {
                    movies[i] = movies[--count];
                }
                return;
            }
        }
    }

    public Movie GetMovie(string title)
    {
        for (int i = 0; i < count; i++)
        {
            if (movies[i].Title == title)
            {
                return movies[i];
            }
        }
        return null;
    }

    public Movie[] GetAllMovies()
    {
        Movie[] result = new Movie[count];
        Array.Copy(movies, result, count);
        Array.Sort(result, (x, y) => x.Title.CompareTo(y.Title));
        return result;
    }
}

public class Member
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    private List<Movie> borrowedMovies;

    public Member(string firstName, string lastName, string phoneNumber, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Password = password;
        borrowedMovies = new List<Movie>();
    }

    public bool BorrowMovie(Movie movie)
    {
        if (borrowedMovies.Count >= 5 || borrowedMovies.Any(m => m.Title == movie.Title))
        {
            return false;
        }
        borrowedMovies.Add(movie);
        return true;
    }

    public bool ReturnMovie(string title)
    {
        var movie = borrowedMovies.FirstOrDefault(m => m.Title == title);
        if (movie != null)
        {
            borrowedMovies.Remove(movie);
            return true;
        }
        return false;
    }

    public List<Movie> GetBorrowedMovies()
    {
        return borrowedMovies;
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName} ({PhoneNumber})";
    }
}

public class MemberCollection
{
    private Member[] members;
    private int count;

    public MemberCollection()
    {
        members = new Member[1000]; // Initialize the array with a size of 1000
        count = 0;
    }

    public void AddMember(Member member)
    {
        for (int i = 0; i < count; i++)
        {
            if (members[i].FirstName == member.FirstName && members[i].LastName == member.LastName)
            {
                return;
            }
        }
        members[count++] = member;
    }

    public void RemoveMember(string firstName, string lastName)
    {
        for (int i = 0; i < count; i++)
        {
            if (members[i].FirstName == firstName && members[i].LastName == lastName)
            {
                members[i] = members[--count];
                return;
            }
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

    public Member[] GetAllMembers()
    {
        Member[] result = new Member[count];
        Array.Copy(members, result, count);
        return result;
    }
}

class Program
{
    static MovieCollection movieCollection = new MovieCollection();
    static MemberCollection memberCollection = new MemberCollection();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Welcome to the Community Library");
            Console.WriteLine("1. Staff Login");
            Console.WriteLine("2. Member Login");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    StaffLogin();
                    break;
                case "2":
                    MemberLogin();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void StaffLogin()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        if (username == "staff" && password == "today123")
        {
            StaffMenu();
        }
        else
        {
            Console.WriteLine("Invalid credentials.");
        }
    }

    static void StaffMenu()
    {
        while (true)
        {
            Console.WriteLine("Staff Menu");
            Console.WriteLine("1. Add Movie");
            Console.WriteLine("2. Remove Movie");
            Console.WriteLine("3. Register Member");
            Console.WriteLine("4. Remove Member");
            Console.WriteLine("5. Find Member's Phone Number");
            Console.WriteLine("6. Find Members Renting a Movie");
            Console.WriteLine("7. Logout");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddMovie();
                    break;
                case "2":
                    RemoveMovie();
                    break;
                case "3":
                    RegisterMember();
                    break;
                case "4":
                    RemoveMember();
                    break;
                case "5":
                    FindMemberPhoneNumber();
                    break;
                case "6":
                    FindMembersRentingMovie();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void AddMovie()
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();
        Console.Write("Enter genre: ");
        string genre = Console.ReadLine();
        Console.Write("Enter classification: ");
        string classification = Console.ReadLine();
        Console.Write("Enter duration (in minutes): ");
        int duration = int.Parse(Console.ReadLine());
        Console.Write("Enter number of copies: ");
        int copies = int.Parse(Console.ReadLine());

        Movie movie = new Movie(title, genre, classification, duration, copies);
        movieCollection.AddMovie(movie);
        Console.WriteLine("Movie added successfully.");
    }

    static void RemoveMovie()
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();
        Console.Write("Enter number of copies to remove: ");
        int copies = int.Parse(Console.ReadLine());

        movieCollection.RemoveMovie(title, copies);
        Console.WriteLine("Movie removed successfully.");
    }

    static void RegisterMember()
    {
        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter phone number: ");
        string phoneNumber = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        Member member = new Member(firstName, lastName, phoneNumber, password);
        memberCollection.AddMember(member);
        Console.WriteLine("Member registered successfully.");
    }

    static void RemoveMember()
    {
        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine();

        Member member = memberCollection.GetMember(firstName, lastName);
        if (member != null && member.GetBorrowedMovies().Count == 0)
        {
            memberCollection.RemoveMember(firstName, lastName);
            Console.WriteLine("Member removed successfully.");
        }
        else
        {
            Console.WriteLine("Member cannot be removed. They may have borrowed movies.");
        }
    }

    static void FindMemberPhoneNumber()
    {
        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine();

        Member member = memberCollection.GetMember(firstName, lastName);
        if (member != null)
        {
            Console.WriteLine($"Phone number: {member.PhoneNumber}");
        }
        else
        {
            Console.WriteLine("Member not found.");
        }
    }

    static void FindMembersRentingMovie()
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();

        Movie movie = movieCollection.GetMovie(title);
        if (movie != null)
        {
            Console.WriteLine("Members currently renting this movie:");
            foreach (var member in memberCollection.GetAllMembers())
            {
                if (member.GetBorrowedMovies().Any(m => m.Title == title))
                {
                    Console.WriteLine(member);
                }
            }
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    static void MemberLogin()
    {
        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        Member member = memberCollection.GetMember(firstName, lastName);
        if (member != null && member.Password == password)
        {
            MemberMenu(member);
        }
        else
        {
            Console.WriteLine("Invalid credentials.");
        }
    }

    static void MemberMenu(Member member)
    {
        while (true)
        {
            Console.WriteLine("Member Menu");
            Console.WriteLine("1. Display All Movies");
            Console.WriteLine("2. Display Movie Information");
            Console.WriteLine("3. Borrow Movie");
            Console.WriteLine("4. Return Movie");
            Console.WriteLine("5. List Borrowed Movies");
            Console.WriteLine("6. Logout");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    DisplayAllMovies();
                    break;
                case "2":
                    DisplayMovieInformation();
                    break;
                case "3":
                    BorrowMovie(member);
                    break;
                case "4":
                    ReturnMovie(member);
                    break;
                case "5":
                    ListBorrowedMovies(member);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void DisplayAllMovies()
    {
        var movies = movieCollection.GetAllMovies();
        foreach (var movie in movies)
        {
            Console.WriteLine(movie);
        }
    }

    static void DisplayMovieInformation()
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();

        Movie movie = movieCollection.GetMovie(title);
        if (movie != null)
        {
            Console.WriteLine(movie);
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    static void BorrowMovie(Member member)
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();

        Movie movie = movieCollection.GetMovie(title);
        if (movie != null && movie.Copies > 0)
        {
            if (member.BorrowMovie(movie))
            {
                movie.Copies--;
                Console.WriteLine("Movie borrowed successfully.");
            }
            else
            {
                Console.WriteLine("Cannot borrow this movie. You may have reached the limit or already borrowed this title.");
            }
        }
        else
        {
            Console.WriteLine("Movie not available.");
        }
    }

    static void ReturnMovie(Member member)
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();

        if (member.ReturnMovie(title))
        {
            Movie movie = movieCollection.GetMovie(title);
            if (movie != null)
            {
                movie.Copies++;
                Console.WriteLine("Movie returned successfully.");
            }
        }
        else
        {
            Console.WriteLine("You have not borrowed this movie.");
        }
    }

    static void ListBorrowedMovies(Member member)
    {
        var borrowedMovies = member.GetBorrowedMovies();
        if (borrowedMovies.Count > 0)
        {
            Console.WriteLine("Borrowed Movies:");
            foreach (var movie in borrowedMovies)
            {
                Console.WriteLine(movie);
            }
        }
        else
        {
            Console.WriteLine("No movies borrowed.");
        }
    }
}