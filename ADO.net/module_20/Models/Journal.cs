namespace module_20.Models
{
    public class Journal
    {
        public Journal()
        {
            
        }
        public Journal(Student student, Lection lection)
        {
            this.student = student;
            this.lection = lection;
            mark = 0;
            presence = false;
            homework = false;
        }

        public Journal(Student student, Lection lection, int mark, bool presence, bool homework)
        {
            this.student = student;
            this.lection = lection;
            this.mark = mark;
            this.presence = presence;
            this.homework = homework;
        }

        public int mark;
        public bool presence;
        public bool homework;

        public Student student;
        public Lection lection;
    }
}