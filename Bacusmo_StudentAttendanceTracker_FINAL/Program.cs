
// These are the libraries that made this code possible
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Program;
using static System.Runtime.InteropServices.JavaScript.JSType;
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class Program
{
    static List<SchoolEntity> schoolEntities = new List<SchoolEntity>();
    const string dataFilePath = "school_data.txt";
    static List<User> users = new List<User>();
    private static DateTime date;

    // These codes are what saves the data of the user after Registering a new account
    static void LoadUserData()
    {
        if (File.Exists("user_data.txt"))
        {
            string[] lines = File.ReadAllLines("user_data.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    users.Add(new Registered_User(parts[0], parts[1]));
                }
            }
        }
    }

    static void SaveUserData(User user)
    {
        File.AppendAllText("user_data.txt", $"{user.Username}:{user.Password}\n"); //This Saves the user's data in a text file so it can be resuable again 
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public static void Main(string[] args)
    {
        ShowOpeningAnimation("Welcome to Student Attendance Tracker");

        // Load data from CSV files
        LoadUserData();
        LoadClassSchedulesFromCsv();
        LoadCoursesFromTextFile(courseDataFilePath);

        // Display the main menu
        DisplayMainMenu();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    static void ShowOpeningAnimation(string message) //Shows the opening animation
    {
        Console.Clear();

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        int x = (screenWidth - message.Length) / 2;
        int y = screenHeight / 2;

        Console.SetCursorPosition(x, y);

        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(20); // Adjust the delay to control typing speed
        }

        Console.SetCursorPosition(0, screenHeight - 1);
        Console.WriteLine();
        Thread.Sleep(2000); // Display the message for 2 seconds
    }

    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    static void DisplayMainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t\t    Student Attendance Tracker");

            int menuTop = Console.WindowHeight / 2 - 3; // Center the menu
            Console.SetCursorPosition(0, menuTop);

            Console.WriteLine("\t\t\t\t\t\t1. Login");
            Console.WriteLine("\t\t\t\t\t\t2. Register");
            Console.WriteLine("\t\t\t\t\t\t3. Exit");
            int inputTop = menuTop + 3;
            Console.SetCursorPosition(0, inputTop);
            Console.Write("                               Select an option: ");

            try
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Login();
                            break;

                        case 2:
                            Register();
                            break;

                        case 3:
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\n\t\t\t\t                     Press Enter to continue...");
            Console.ReadLine();
        }
    }


    static void Login() // Method for Login
    {
        Console.Clear();
        LoadUserData();
        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        int x = (screenWidth - 26) / 2;
        int y = screenHeight / 2 - 6;

        Console.SetCursorPosition(x, y);

        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.SetCursorPosition(x, y + 2);
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        User user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user != null)
        {

            Console.Clear();
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Authentication successful.");
            Console.ReadKey();
            RunSchoolManagement();
        }
        else
        {
            Console.Clear();
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Authentication failed. Invalid username or password.");
            Console.ReadKey();
        }
    }

    static void Register() //Method for Registeration of new account
    {
        Console.Clear();

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        int x = (screenWidth - 30) / 2;
        int y = screenHeight / 2 - 6;

        Console.SetCursorPosition(x, y);

        Console.Write("Enter a new username: ");
        string username = Console.ReadLine();
        Console.SetCursorPosition(x, y + 2);
        Console.Write("Enter a new password: ");
        string password = Console.ReadLine();

        if (users.Any(u => u.Username == username))
        {
            Console.Clear();
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Username already exists. Please choose another.");
            Console.ReadKey();
        }
        else
        {
            Registered_User newUser = new Registered_User(username, password);
            users.Add(newUser);
            SaveUserData(newUser);
            Console.Clear();
            Console.SetCursorPosition(x, y);

            Console.WriteLine("Registration successful. You can now log in.");
            Console.ReadKey();
        }

    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    static void RunSchoolManagement()
    {
        LoadDataFromTextFile();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n\n\n                                   Student Attendance Tracker - School Management");

            int menuTop = Console.WindowHeight / 2 - 5;
            Console.SetCursorPosition(0, menuTop);

            Console.WriteLine("\t\t\t\t\t\t1. Semester Management"); // Moved the semester option to the top
            Console.WriteLine("\t\t\t\t\t\t2. Create a Course");
            Console.WriteLine("\t\t\t\t\t\t3. Schedule a Class");
            Console.WriteLine("\t\t\t\t\t\t4. Register a Student");
            Console.WriteLine("\t\t\t\t\t\t5. Record Attendance");
            Console.WriteLine("\t\t\t\t\t\t6. View Courses Created");
            Console.WriteLine("\t\t\t\t\t\t7. Save and Exit");
            Console.Write("\t\t\t\t\tSelect an option: ");

            int inputTop = menuTop + 8;
            Console.SetCursorPosition(0, inputTop);

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        string semesterName = CreateSemester();
                        Console.WriteLine($"                                             Semester '{semesterName}' created successfully.");
                        break;
                        SemesterManagement(); // New option for semester management
                    case 2:
                        CreateCourse();
                        break;

                    case 3:
                        ScheduleClass();
                        break;

                    case 4:
                        RegisterStudent();
                        break;

                    case 5:
                        RecordAttendance();
                        break;

                    case 6:
                        ViewInformation();
                        break;

                    case 7:
                        SaveDataToTextFile();
                        return;


                    default:
                        Console.WriteLine("\n\n\n\t\t\t\t\t\tInvalid choice. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\n\n\n\t\t\t\t\t\tInvalid input. Please enter a number.");
            }

            Console.WriteLine("\n\t\t\t\t\t\tPress Enter to continue...");
            Console.ReadLine();
        }
    }



    static void CreateCourse()
    {
        Console.Clear();

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        int x = (screenWidth - 20) / 2;
        int y = screenHeight / 2 - 6;

        Console.SetCursorPosition(x, y);

        Console.Write("Enter Course Name: ");
        string courseName = Console.ReadLine();

        Console.SetCursorPosition(x, y + 2);
        Console.Write("Enter Semester: ");
        string semester = Console.ReadLine(); // Prompt the user for the semester

        schoolEntities.Add(new Course(courseName, semester));
        Console.WriteLine("                                             \nCourse created successfully.");
    }

    static void ScheduleClass()
    {
        Console.Clear();

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        int x = (screenWidth - 19) / 2;
        int y = screenHeight / 2 - 6;

        Console.SetCursorPosition(x, y);

        Console.Write("Enter Course Name: ");
        string courseName = Console.ReadLine();
        var course = GetCourse(courseName);

        if (course != null)
        {
            Console.SetCursorPosition(x, y + 2);
            Console.Write("Enter Class Name: ");
            string className = Console.ReadLine();
            Console.SetCursorPosition(x, y + 4);
            Console.Write("Enter Start Time (e.g., mm/dd/yyyy HH:mm): ");
            string startTimeInput = Console.ReadLine();
            if (DateTime.TryParseExact(startTimeInput, "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime startTime))
            {
                Console.SetCursorPosition(x, y + 6);
                Console.Write("Enter End Time (e.g., mm/dd/yyyy HH:mm): ");
                string endTimeInput = Console.ReadLine();
                if (DateTime.TryParseExact(endTimeInput, "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime endTime))
                {
                    if (endTime > startTime)
                    {
                        course.AddClassSchedule(new ClassSchedule(className, startTime, endTime));
                        Console.WriteLine("\t\t\t\t\tClass scheduled successfully.");
                    }
                    else
                    {
                        Console.WriteLine("End Time must be later than Start Time.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"DEBUG: Invalid Start Time format. Please use the format: mm/dd/yyyy HH:mm");
            }
        }
        else
        {
            Console.WriteLine($"DEBUG: Course '{courseName}' not found.");
        }
    }





    static void RegisterStudent()
    {
        Console.Clear();

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        int x = (screenWidth - 18) / 2;
        int y = screenHeight / 2 - 6;

        Console.SetCursorPosition(x, y);

        Console.Write("Enter Course Name: ");
        string courseName = Console.ReadLine();
        var course = GetCourse(courseName);

        if (course != null)
        {
            Console.SetCursorPosition(x, y + 2);
            Console.Write("Enter Class Name: ");
            string className = Console.ReadLine();
            var classSchedule = course.GetClassSchedule(className);

            if (classSchedule != null)
            {
                Console.SetCursorPosition(x, y + 4);
                Console.Write("Enter Student Name: ");
                string studentName = Console.ReadLine();
                classSchedule.RegisterStudent(new Student(studentName));
                Console.WriteLine("\t\t\t\t\t\tStudent registered successfully.");
            }
            else
            {
                Console.WriteLine($"Class '{className}' not found in the '{courseName}' course.");
            }
        }
        else
        {
            Console.WriteLine($"Course '{courseName}' not found.");
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    static void RecordAttendance()
    {
        Console.Clear();

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        int x = (screenWidth - 16) / 2;
        int y = screenHeight / 2 - 6;

        Console.SetCursorPosition(x, y);

        Console.Write("Enter Course Name: ");
        string courseName = Console.ReadLine();
        var course = GetCourse(courseName);

        if (course != null)
        {
            Console.SetCursorPosition(x, y + 2);
            Console.Write("Enter Class Name: ");
            string className = Console.ReadLine();
            var classSchedule = course.GetClassSchedule(className);

            if (classSchedule != null)
            {
                Console.SetCursorPosition(x, y + 4);
                Console.Write("Enter Semester: ");
                string semester = Console.ReadLine(); // Prompt the user for the semester

                // Display information about the scheduled class
                Console.WriteLine($"Scheduled Class Information:");
                Console.WriteLine($"Course: {courseName}");
                Console.WriteLine($"Class: {className}");
                Console.WriteLine($"Time: {classSchedule.StartTime.ToString("MM/dd/yyyy HH:mm")} - {classSchedule.EndTime.ToString("MM/dd/yyyy HH:mm")}");
                Console.WriteLine($"Semester: {semester}");

                // Record attendance
                classSchedule.RecordAttendance(semester);
                Console.WriteLine("Attendance recorded successfully.");
            }
            else
            {
                Console.WriteLine($"Class '{className}' not found in the '{courseName}' course.");
            }
        }
        else
        {
            Console.WriteLine($"Course '{courseName}' not found.");
        }
    }



    static void SemesterManagement()
    {
        Console.Clear();

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        int x = (screenWidth - 30) / 2;
        int y = screenHeight / 2 - 6;

        Console.SetCursorPosition(x, y);

        Console.WriteLine("Semester Management");
        Console.WriteLine("\t\t\t\t\t\t1. Create a Semester");
        Console.Write("\t\t\t\t\tSelect an option: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    CreateSemester();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }

    static string CreateSemester()
    {
        Console.Clear();

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        int x = (screenWidth - 18) / 2;
        int y = screenHeight / 2 - 6;

        Console.SetCursorPosition(x, y);

        Console.Write("Enter Semester: ");
        string semesterName = Console.ReadLine();

        return semesterName;
    }



    static void ViewInformation()
    {
        Console.Clear();
        Console.WriteLine("Courses");

        foreach (var entity in schoolEntities)
        {
            entity.DisplayInfo();
            Console.WriteLine();
        }
    }

    static Course GetCourse(string courseName)
    {
        return schoolEntities.OfType<Course>().FirstOrDefault(c => c.Name == courseName);
    }



    const string courseDataFilePath = "course_data.txt";
    const string attendanceDataFilePath = "attendance_records.csv";

    static void LoadAttendanceRecordsFromCsv()
    {
        if (File.Exists(attendanceDataFilePath))
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(attendanceDataFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 5)
                {
                    // Parse the values from the CSV line
                    string courseName = parts[0];
                    string className = parts[1];
                    string studentName = parts[2];
                    string semester = parts[3];
                    string attendanceStatus = parts[4];

                    // Find or create the necessary entities
                    var course = GetOrCreateCourse(courseName);
                    var classSchedule = course.GetClassSchedule(className);
                    var student = classSchedule.Students.FirstOrDefault(s => s.Name == studentName);

                    // Create a new attendance record and add it to the class schedule
                    if (course != null && classSchedule != null && student != null)
                    {
                        var attendanceRecord = new AttendanceRecord(student, semester, attendanceStatus);
                        classSchedule.AttendanceRecords.Add(attendanceRecord);
                    }
                }
            }
        }
    }

    static void SaveAttendanceRecordsToCsv()
    {
        using (StreamWriter writer = new StreamWriter(attendanceDataFilePath))
        {
            foreach (var entity in schoolEntities.OfType<Course>())
            {
                foreach (var classSchedule in entity.ClassSchedules)
                {
                    foreach (var attendanceRecord in classSchedule.AttendanceRecords)
                    {
                        // Write attendance record data to the CSV file
                        writer.WriteLine($"{entity.Name},{classSchedule.Name},{attendanceRecord.Student.Name},{attendanceRecord.Semester},{attendanceRecord.AttendanceStatus}");
                    }
                }
            }
        }
    }


    // Modify LoadDataFromTextFile method
    static void LoadDataFromTextFile()
    {
        LoadCoursesFromTextFile(courseDataFilePath);
        LoadClassSchedulesFromCsv(); // Change this line
        LoadAttendanceRecordsFromCsv();

    }

    static void LoadCoursesFromTextFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    schoolEntities.Add(new Course(parts[0], parts[1]));
                }
            }
        }
    }

    const string classScheduleDataFilePath = "class_schedules.csv";
    static void LoadClassSchedulesFromCsv()
    {
        if (File.Exists(classScheduleDataFilePath))
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(classScheduleDataFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 4)
                {
                    // Parse the values from the CSV line
                    string courseName = parts[0];
                    string className = parts[1];
                    DateTime startTime = DateTime.ParseExact(parts[2], "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime endTime = DateTime.ParseExact(parts[3], "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);

                    // Find or create the necessary entities
                    var course = GetOrCreateCourse(courseName);

                    // Check if the class already exists in the course
                    var existingClass = course.ClassSchedules.FirstOrDefault(cs => cs.Name == className);

                    if (existingClass == null)
                    {
                        // If the class doesn't exist, add it
                        var newClass = new ClassSchedule(className, startTime, endTime);
                        course.AddClassSchedule(newClass);
                    }
                    else
                    {
                        // Update the existing class with the loaded data
                        existingClass.StartTime = startTime;
                        existingClass.EndTime = endTime;
                    }
                }
            }
        }
    }

    static Course GetOrCreateCourse(string courseName)
    {
        var existingCourse = GetCourse(courseName);

        if (existingCourse == null)
        {
            // If the course doesn't exist, create a new one
            var newCourse = new Course(courseName, "");
            schoolEntities.Add(newCourse);
            return newCourse;
        }

        return existingCourse;
    }

    // Modify SaveDataToTextFile method
    static void SaveDataToTextFile()
    {
        SaveCoursesToTextFile(courseDataFilePath);
        SaveClassSchedulesToCsv();
        SaveAttendanceRecordsToCsv();
    }

    static void SaveCoursesToTextFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var entity in schoolEntities.OfType<Course>())
            {
                writer.WriteLine($"{entity.Name}:{entity.Semester}");
            }
        }
    }

    static void SaveClassSchedulesToCsv()
    {
        using (StreamWriter writer = new StreamWriter(classScheduleDataFilePath))
        {
            foreach (var entity in schoolEntities.OfType<Course>())
            {
                foreach (var classSchedule in entity.ClassSchedules)
                {
                    // Write class schedule data to the CSV file
                    writer.WriteLine($"{entity.Name},{classSchedule.Name},{classSchedule.StartTime.ToString("MM/dd/yyyy HH:mm")},{classSchedule.EndTime.ToString("MM/dd/yyyy HH:mm")}");
                }
            }
        }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}

public abstract class SchoolEntity : ISchoolEntity
{
    public string Name { get; set; }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Name: {Name}");
    }
}

public class Course : SchoolEntity, ISchoolEntity
{
    public List<ClassSchedule> ClassSchedules { get; set; }
    public string Semester { get; set; }

    public Course(string name, string semester)
    {
        Name = name;
        Semester = semester;
        ClassSchedules = new List<ClassSchedule>();
    }

    public void AddClassSchedule(ClassSchedule classSchedule)
    {
        ClassSchedules.Add(classSchedule);
        classSchedule.ParentEntity = this;
    }

    public ClassSchedule GetClassSchedule(string className)
    {
        return ClassSchedules.FirstOrDefault(cs => cs.Name == className);
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Course Name: {Name}");
    }
}

public interface ISchoolEntity
{
    void DisplayInfo();
}


public class ClassSchedule : SchoolEntity, ISchoolEntity
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<Student> Students { get; set; }
    public List<AttendanceRecord> AttendanceRecords { get; set; }

    public ClassSchedule(string name, DateTime startTime, DateTime endTime)
    {
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
        Students = new List<Student>();
        AttendanceRecords = new List<AttendanceRecord>();
    }

    public IEnumerable<AttendanceRecord> GetStudentAttendanceRecords(Student student, string semester, DateTime date)
    {
        // Get the attendance records for the specified student, semester, and date
        return student.AttendanceRecords
            .Where(record => record.Semester == semester && record.ClassSchedule == this && record.Date.Date == date.Date);
    }



    public void RegisterStudent(Student student)
    {
        Students.Add(student);
    }

    public void RecordAttendance(string semester)
    {
        Console.WriteLine($"\nRecording attendance for '{Name}' in '{ParentEntity.Name}':");

        // Create a list to store attendance information
        List<string> attendanceInfoList = new List<string>();

        foreach (var student in Students)
        {
            Console.Write($"Is {student.Name} (P)resent/(L)ate/(A)bsent?: ");
            var input = Console.ReadLine();

            string attendanceStatus;

            if (input.Equals("P", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"{student.Name} is Present.");
                attendanceStatus = "Present";
            }
            else if (input.Equals("L", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"{student.Name} is Late.");
                attendanceStatus = "Late";
            }
            else if (input.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"{student.Name} is Absent.");
                attendanceStatus = "Absent";
            }
            else
            {
                Console.WriteLine($"{student.Name} has an invalid attendance status.");
                continue; // Skip to the next iteration if the input is invalid
            }

            // Record student attendance
            RecordStudentAttendance(student, semester, attendanceStatus);

            // Add attendance information to the list
            attendanceInfoList.Add($"{student.Name},{attendanceStatus}");
        }

        // Save attendance information to a file
        SaveAttendanceToFile(attendanceInfoList);

        Console.WriteLine("\nAttendance recorded for all students.");
    }

    private void SaveAttendanceToFile(List<string> attendanceInfoList)
    {
        string filePath = $"attendance_{ParentEntity.Name}_{Name}.csv";

        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Add a header with the date
                writer.WriteLine($"[Date]\t\t\t{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                writer.WriteLine("Student\t\t\tAttendance");

                // Write attendance information to the file
                foreach (var attendanceInfo in attendanceInfoList)
                {
                    writer.WriteLine(attendanceInfo);
                }

                writer.WriteLine(); // Add an empty line for better separation
            }

            Console.WriteLine($"Attendance information saved to {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving attendance information: {ex.Message}");
        }
    }



    private void SaveAttendanceToCsv()
    {
        string csvFilePath = $"attendance_{ParentEntity.Name}_{Name}.csv";

        using (StreamWriter writer = new StreamWriter(csvFilePath))
        {
            foreach (var record in AttendanceRecords)
            {
                writer.WriteLine($"{record.Student.Name},{record.AttendanceStatus},{record.Date:MM/dd/yyyy}");
            }
        }

        Console.WriteLine($"Attendance information saved to {csvFilePath}");
    }

    public void RecordStudentAttendance(Student student, string semester, string attendanceStatus)
    {
        // Check if the attendance record already exists 
        var existingRecord = AttendanceRecords
            .FirstOrDefault(record => record.Student == student && record.Semester == semester);

        if (existingRecord != null)
        {
            // Update the existing record
            existingRecord.AttendanceStatus = attendanceStatus;
        }
        else
        {
            // Create a new attendance record with the current date
            AttendanceRecords.Add(new AttendanceRecord(student, semester, attendanceStatus)
            {
                Date = DateTime.Now.Date // Set the date to the current date
            });
        }
    }

    public Course ParentEntity { get; set; }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Class Name: {Name}");
    }
}

public class Student : SchoolEntity, ISchoolEntity
{
    public List<AttendanceRecord> AttendanceRecords { get; set; }

    public Student(string name)
    {
        Name = name;
        AttendanceRecords = new List<AttendanceRecord>();
    }
}

public class AttendanceRecord
{
    public Student Student { get; }
    public string Semester { get; }
    public string AttendanceStatus { get; set; }
    public ClassSchedule ClassSchedule { get; internal set; }
    public DateTime Date { get; set; }

    public AttendanceRecord(Student student, string semester, string attendanceStatus)
    {
        Student = student;
        Semester = semester;
        AttendanceStatus = attendanceStatus;
    }
}
public class User
{
    public string Username { get; set; }
    public string Password { get; set; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}

public class Registered_User : User //Created a derived class to inherit for the User
{
    public Registered_User(string username, string password) : base(username, password) { }
}