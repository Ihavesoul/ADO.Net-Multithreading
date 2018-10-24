using System;
using System.Collections.Generic;
using System.Linq;
using module_20.Helpers;
using module_20.Models;

namespace module_20.BusinessLogic.Helpers
{
    public static class StudentHelper
    {
        private const int UnacceptableAverageMark = 4;
        private const int UnacceptableVisting = 3;
        private const string UniversityNumber = "45643"; //random university phone number
        private const string LowMarkMessage = "Your score is too low";
        private const string LowPresicenceMessage = "You missed a lot of classes";

        public static bool StudentHomeworkCheck(this Journal journal) =>
            //TODO : Create custom expection for say user "Your journal is empty"
            journal != null && !journal.homework;

        public static int CalculateAverageMark(this List<Journal> journal)
        {
            ExceptionHelper.CheckCollection(journal);
            var sumMark = 0;
            if (journal.Count == 0)
            {
                return 0;
            }
            foreach (var student in journal)
            {
                sumMark += student.mark;
            }
            return sumMark/journal.Count;
        }

        public static void UpdateAverageMark(this Student student,List<Journal> journal, ISender sender)
        {
            ExceptionHelper.CheckCollection(journal);
            var resultList = journal.Where(x => x.student.Equals(student)).ToList();
            if ((student.averageMark = CalculateAverageMark(resultList)) < UnacceptableAverageMark)
            {
                sender.SendMessage(student.phoneNumber,UniversityNumber,LowMarkMessage);
            }
        }

        public static int CalculateVisiting(this List<Journal> journal)
        {
            ExceptionHelper.CheckCollection(journal);
            int sumVisiting = 0;
            foreach (var student in journal)
            {
                sumVisiting += Convert.ToInt32(student.presence);
            }

            return journal.Count - sumVisiting;
        }

        public static void UpdateVisiting(this Student student, List<Journal> journal,ISender sender)
        {
            ExceptionHelper.CheckCollection(journal);
            var resultList = journal.Where(x => x.student.Equals(student)).ToList();
            if ((student.visiting = CalculateVisiting(resultList)) < UnacceptableVisting)
            {
                sender.SendMessage(student.email, journal.First()?.lection.lector.email,LowPresicenceMessage);
            }
        }
    }
}
