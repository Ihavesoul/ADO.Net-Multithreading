using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using module_20.BusinessLogic.Helpers;
using module_20.DataAccessLayer;
using module_20.Helpers;
using module_20.Models;

namespace module_20.BusinessLogic
{
    public class UniversityLogic
    {
        private readonly ILogger _logger;
        private readonly IConfig _config;

        private readonly StudentDal _studentDal;
        private readonly LectionDal _lectionDal;
        private readonly JournalDal _journalDal;
        private readonly LectorDal _lectorDal;

        public UniversityLogic() { }
        public UniversityLogic(ILogger logger, IConfig config)
        {
            this._logger = logger;
            this._config = config;
            this._studentDal = new StudentDal(logger,config);
            this._lectionDal = new LectionDal(logger,config);
            this._journalDal = new JournalDal(logger,config);
            this._lectorDal = new LectorDal(logger,config);
        }


        public bool AddStudent(Student student)
        {
            _studentDal.InsertStudent(student.firstName, student.secondName, student.email, student.phoneNumber);
            return true;
        }

        public bool AddLection(Lection lection)
        {
            _lectionDal.InserLection(lection.lector.lectorId.id, lection.name, lection.date);
            return true;
        }

        public bool AddLector(Lector lector)
        {
            _lectorDal.InsertLector(lector.firstName, lector.secondName, lector.email);
            return true;
        }

        public bool AddJournal(Journal journal)
        {
            _journalDal.InsertStudentAndLection(journal.student.studentId.id, journal.lection.lectionId.id,
                journal.mark, journal.presence, journal.homework);
            return true;
        }

        public virtual void SetMarkAndPresence(int mark, bool presence, Journal journal, ISender sender)
        {
            
            //TODO:CheckStudentHomework && Check presence
            if (journal.StudentHomeworkCheck() && (mark <= 5 && mark >= 0))
            {
                _journalDal.UpdateStudentAndLection(journal.student.studentId.id, journal.lection.lectionId.id, mark,
                    presence, journal.homework);
                journal.student.UpdateAverageMark(_journalDal.GetStudentAndLectionsWithStudentMark(), sender);
                journal.student.UpdateVisiting(_journalDal.GetStudentAndLectionsWithStudentMark(), sender);

            }

        }
    }
}
