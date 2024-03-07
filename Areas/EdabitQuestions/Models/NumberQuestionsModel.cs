using System.Reflection;

namespace MvcPractice.Areas.EdabitQuestions.Models
{
    public class NumberQuestionsModel
    {
        public byte QuestionId { get; set; }
        public NumberQuestion SelectedQuestion { get; set; }
        public List<int> p { get; set; }

        //Reflection function to get all the classes here
        //Classes have been named starting with 'NumberQuestion' to easily identify them for this page
        public static List<NumberQuestion> GetQuestions()
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            string ns = "MvcPractice.Areas.EdabitQuestions.Models";

            Type[]? types = assembly.GetTypes().Where(x=> String.Equals(x.Namespace, ns, StringComparison.Ordinal)).ToArray();
            List<NumberQuestion> questions = new List<NumberQuestion>();

            int typeCount = 0;
            foreach (var item in types)
            {
                if (item.Name.Contains("NumberQuestion") && !item.Name.Contains("NumberQuestionsModel") && item.Name != "NumberQuestion" && item.Name != "StringQuestion")
                {
                    questions.Add((NumberQuestion)Activator.CreateInstance(item, new object[] { typeCount,item.Name.Replace("NumberQuestion",""),null}));
                    typeCount++;
                }
            }
            return questions;
        }
        public List<NumberQuestion> QuestionList = GetQuestions();
    }
    //Base class for each question
    public abstract class NumberQuestion
    {
        public List<string> ParamNames { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public List<int>? p { get; set; }  // Parameters used to store user input
        public NumberQuestion(int questionId, string questionName, List<String> paramNames)
        {
            QuestionId = questionId;
            QuestionName = questionName;
            ParamNames = paramNames;
        }
        public abstract dynamic PerformQuestionLogic();
    }
    public class NumberQuestionTwoSum : NumberQuestion
    {
        public NumberQuestionTwoSum(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "X", "Y" }) { }        
        public override dynamic PerformQuestionLogic()
        {
            return p[0] + p[1];
        }
    }
    public class NumberQuestionMinToSec : NumberQuestion
    {
        public NumberQuestionMinToSec(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "Minute", }) { }       
        public override dynamic PerformQuestionLogic()
        {
            return p[0] * 60;
        }
    }
    public class NumberQuestionAddOne/*Change*/ : NumberQuestion
    {
        public int MyProperty { get; set; }
        public NumberQuestionAddOne/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "X" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return p[0]++;
        }
    }
    public class NumberQuestionCircuitPower/*Change*/ : NumberQuestion
    {
        public NumberQuestionCircuitPower/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "Voltage", "Current" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return p[0] * p[1];
        }
    }
    public class NumberQuestionAgeToDays/*Change*/ : NumberQuestion
    {
        public NumberQuestionAgeToDays/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "Age" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return (p[0] * 365) ;
        }
    }
    public class NumberQuestionAreaOfTriangle/*Change*/ : NumberQuestion
    {
        public NumberQuestionAreaOfTriangle/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "Base", "Height" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return (p[0] * p[1]) / 2;
        }
    }
    public class NumberQuestionRemainderOfDivision/*Change*/ : NumberQuestion
    {
        public NumberQuestionRemainderOfDivision/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "X", "Y" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return p[0] % p[1];
        }
    }
    public class NumberQuestionNumberLessThanOrEqualToZero/*Change*/ : NumberQuestion
    {
        public NumberQuestionNumberLessThanOrEqualToZero/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "X" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return p[0] <= 0;
        }
    }
    public class NumberQuestionNumberLessThanOrEqualToHundred/*Change*/ : NumberQuestion
    {
        public NumberQuestionNumberLessThanOrEqualToHundred/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "X" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return p[0] <= 100;
        }
    }
    public class NumberQuestionNumbersEqual/*Change*/ : NumberQuestion
    {
        public NumberQuestionNumbersEqual/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "X", "Y" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return p[0] == p[1];
        }
    }
    public class NumberQuestionHoursToSeconds/*Change*/ : NumberQuestion
    {
        public NumberQuestionHoursToSeconds/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "Hours" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return (p[0] * 60) * 60;
        }
    }
    public class NumberQuestionPolygonAngleSum/*Change*/ : NumberQuestion
    {
        public NumberQuestionPolygonAngleSum/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "N" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return (p[0] -2) * 180;
        }
    }
    public class NumberQuestionPerimeterRectangle : NumberQuestion
    {
        public NumberQuestionPerimeterRectangle/*Change*/(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "Length", "Width" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Change
            return (p[0] + p[1]) * 2;
        }
    }
}
