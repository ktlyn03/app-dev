using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Maui.Storage;
using System.ComponentModel;

namespace StudentRegistrationSystem
{
    public class StudentDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public StudentDatabase(string dbFileName)
        {
            string folderPath = FileSystem.AppDataDirectory;
            string dbPath = Path.Combine(folderPath, dbFileName);
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Student>().Wait();
        }

        public class Student : INotifyPropertyChanged
        {
            private string _firstName;
            private string _lastName;

            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            public string MiddleName { get; set; }

            [NotNull]
            public string FirstName
            {
                get => _firstName;
                set
                {
                    if (_firstName != value)
                    {
                        _firstName = value;
                        OnPropertyChanged(nameof(FirstName));
                        OnPropertyChanged(nameof(FullName)); // Notify FullName when FirstName changes
                    }
                }
            }

            [NotNull]
            public string LastName
            {
                get => _lastName;
                set
                {
                    if (_lastName != value)
                    {
                        _lastName = value;
                        OnPropertyChanged(nameof(LastName));
                        OnPropertyChanged(nameof(FullName)); // Notify FullName when LastName changes
                    }
                }
            }

            [NotNull, Unique]
            public string Email { get; set; }

            public string PhoneNumber { get; set; }

            [NotNull]
            public DateTime RegistrationDate { get; set; }

            public long LRN { get; set; }

            public string Course { get; set; }

            // Parent Information
            public string ParentName { get; set; }
            public int ParentAge { get; set; }
            public string ParentContact { get; set; }

            // Computed FullName property
            public string FullName => string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName)
                ? "Unknown"
                : $"{FirstName} {LastName}";

            // INotifyPropertyChanged implementation
            public event PropertyChangedEventHandler PropertyChanged;

            // Notify property change
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Methods for database operations
        public Task<int> InsertStudentAsync(Student student)
        {
            return _database.InsertAsync(student);
        }

        public Task<List<Student>> GetStudentsAsync()
        {
            return _database.Table<Student>().ToListAsync();
        }

        public Task<Student> GetStudentAsync(int id)
        {
            return _database.Table<Student>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateStudentAsync(Student student)
        {
            return _database.UpdateAsync(student);
        }

        public Task<int> DeleteStudentAsync(Student student)
        {
            return _database.DeleteAsync(student);
        }

        public Task<Student> GetStudentByEmailAsync(string email)
        {
            return _database.Table<Student>().Where(i => i.Email == email).FirstOrDefaultAsync();
        }

        public Task<Student> GetStudentByLrnAsync(long lrn)
        {
            return _database.Table<Student>().Where(i => i.LRN == lrn).FirstOrDefaultAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var existingStudent = await GetStudentByEmailAsync(email);
            return existingStudent == null;
        }

        public async Task<bool> IsLrnUniqueAsync(long lrn)
        {
            var existingStudent = await GetStudentByLrnAsync(lrn);
            return existingStudent == null;
        }

        public async Task<int> UpdateStudentCourseAsync(Student student)
        {
            return await _database.UpdateAsync(student);
        }

        public async Task DeleteAllStudentsAsync()
        {
            await _database.DeleteAllAsync<Student>();
        }
    }
}
