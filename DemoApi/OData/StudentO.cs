using System.Reflection;

namespace DemoApi.Models
{
    public class StudentO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool Gender { get; set; }

        public string DepartId { get; set; } = null!;

        public DateOnly? Dob { get; set; }

        public double Gpa { get; set; }
        public StudentO()
        {
            
        }

        public StudentO(Student s)
        {
            Id = s.Id;
            Name = s.Name;
            Gender = s.Gender;
            DepartId = s.DepartId;
            Dob = s.Dob;
            Gpa = s.Gpa;

        }

        public Student ToStudent()
        {
            return new Student
            {
                Id = this.Id,
                Name = this.Name,
                Gender = this.Gender,
                DepartId = this.DepartId,
                // chuyển DateOnly? → DateTime?
                Dob = this.Dob,
                Gpa = this.Gpa
            };
        }
        static public void swap(StudentO s1, Student s2)
        {
            if (s1.Id != s2.Id) return;
            s2.Name = s1.Name;
            s2.Gender = s1.Gender;
            s2.DepartId = s1.DepartId;
            s2.Dob = s1.Dob;
            s2.Gpa = s1.Gpa;
        }
    }
}
